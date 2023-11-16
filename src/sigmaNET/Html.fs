namespace sigmaNET

open System
open System.IO
open Newtonsoft.Json
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

//open Cytoscape.NET.CytoscapeModel
open Giraffe.ViewEngine


/// HTML template for Cytoscape
type HTML =
 

    static member CreateGraphScript
        (
            graphData: string,
            layout : string,
            containerId: string,
            sigmaJSRef: JSlibReference,
            graphologyJSRef: JSlibReference,
            graphology_LibraryJSRef: JSlibReference
        ) =
        match sigmaJSRef,graphologyJSRef,graphology_LibraryJSRef with
        | Require s,Require g, Require gl ->
            script
                [ _type "text/javascript" ]
                [
                    rawText (
                        Globals.REQUIREJS_SCRIPT_TEMPLATE
                                  //'[GRAPHOLOGY_JS]',
                                  //'[SIGMA_JS]',
                                  //'[GRAPHOLOGY-LIB_JS]',
                            .Replace("[GRAPHOLOGY_JS]", g)
                            .Replace("[SIGMA_JS]", s)
                            .Replace("[GRAPHOLOGY-LIB_JS]", gl)
                            .Replace("[CONTAINERID]",containerId)
                            .Replace("[LAYOUT]",layout)
                            .Replace("[GRAPHDATA]", graphData)
                    )
                ]
        | _ ->
            script
                [ _type "text/javascript" ]
                [
                    rawText (
                        Globals.SCRIPT_TEMPLATE
                            .Replace("[CONTAINERID]",containerId)
                            .Replace("[LAYOUT]",layout)
                            .Replace("[GRAPHDATA]", graphData)
                    )
                ]


    static member Doc(
        graphHTML: XmlNode list, 
        sigmaJSRef: JSlibReference,
        graphologyJSRef: JSlibReference,
        graphology_LibraryJSRef: JSlibReference,
        ?AdditionalHeadTags
    ) =
        let additionalHeadTags =
            defaultArg AdditionalHeadTags []

        let graphologyScriptRef =
            match graphologyJSRef with
            | Local fdp -> script [ _src fdp ] []
            | CDN cdn -> script [ _src cdn ] []
            | Full ->
                script
                    [ _type "text/javascript" ]
                    [
                        rawText (InternalUtils.getFullSigmaJS ())
                    ]
            | NoReference -> rawText ""
            | Require _ -> rawText ""

        let graphologyLibScriptRef =
            match graphology_LibraryJSRef with
            | Local fdp -> script [ _src fdp ] []
            | CDN cdn -> script [ _src cdn ] []
            | Full ->
                script
                    [ _type "text/javascript" ]
                    [
                        rawText (InternalUtils.getFullSigmaJS ())
                    ]
            | NoReference -> rawText ""
            | Require _ -> rawText ""

        let sigmaScriptRef =
            match sigmaJSRef with
            | Local fdp -> script [ _src fdp ] []
            | CDN cdn -> script [ _src cdn ] []
            | Full ->
                script
                    [ _type "text/javascript" ]
                    [
                        rawText (InternalUtils.getFullSigmaJS ())
                    ]
            | NoReference -> rawText ""
            | Require _ -> rawText ""


        html
            []
            [
                head
                    []
                    [
                        graphologyScriptRef
                        graphologyLibScriptRef
                        sigmaScriptRef
                        yield! additionalHeadTags
                    ]
                body [] [ yield! graphHTML]
            ]

    static member CreateGraphHTML
        (
            graphData: string,
            layout : string,
            divId: string,
            sigmaJSRef: JSlibReference,
            graphologyJSRef: JSlibReference,
            graphology_LibraryJSRef: JSlibReference,
            ?Width: CssLength,
            ?Height: CssLength
        ) =
        let width, height = 
            Width |> Option.defaultValue Defaults.DefaultWidth,
            Height |> Option.defaultValue Defaults.DefaultHeight

        let graphScript =
            HTML.CreateGraphScript(
                graphData = graphData,
                layout = layout,
                containerId = divId,
                sigmaJSRef = sigmaJSRef,
                graphologyJSRef = graphologyJSRef,
                graphology_LibraryJSRef = graphology_LibraryJSRef
            )

        [
            
            div [ _id "graph" ] // TODO: maybe remove this div?
                [
                    div
                        [ _id divId; _style (sprintf "width: %s; height: %s" (CssLength.serialize width) (CssLength.serialize height) )]
                        [
                            rawText "&nbsp"
                            comment "SigmaNET graph will be drawn inside this DIV"
                        ]
                    graphScript
                ]
        ]

    /// Converts a CyGraph to it HTML representation. The div layer has a default size of 600 if not specified otherwise.
    static member toGraphHTMLNodes (
        ?DisplayOpts: DisplayOptions
    ) =
        fun (graph:SigmaGraph) -> 

            let displayOptions = defaultArg DisplayOpts Defaults.DefaultDisplayOptions
            let sigmaReference = displayOptions |> DisplayOptions.getSigmaReference
            let graphologyReference = displayOptions |> DisplayOptions.getGraphologyReference
            let graphologyLibReference = displayOptions |> DisplayOptions.getGraphologyLibReference

            let guid = Guid.NewGuid().ToString()
            let id   = sprintf "e%s" <| Guid.NewGuid().ToString().Replace("-","").Substring(0,10)

            let ntsettings = new JsonSerializerSettings()
            let _ = ntsettings.ReferenceLoopHandling <- ReferenceLoopHandling.Ignore             

            let jsonGraph = JsonConvert.SerializeObject (graph.GraphData,ntsettings)

            let layout = Layout.serialize graph.Layout                

            HTML.CreateGraphHTML(
                graphData = jsonGraph,
                layout = layout,
                divId = id,
                sigmaJSRef = sigmaReference,
                graphologyJSRef = graphologyReference,
                graphology_LibraryJSRef = graphologyLibReference,
                // Maybe we should use the DisplayOptions width and height here?
                Width = graph.Width,
                Height = graph.Height
            )

    static member toGraphHTML(
        ?DisplayOpts: DisplayOptions
    ) =
        fun (graph:SigmaGraph) -> 
            graph
            |> HTML.toGraphHTMLNodes(?DisplayOpts = DisplayOpts)
            |> RenderView.AsString.htmlNodes

    /// Converts a Graph to it HTML representation and embeds it into a html page.
    static member toEmbeddedHTML (
        ?DisplayOpts: DisplayOptions
    ) =
        fun (graph:SigmaGraph) ->
            let displayOptions = defaultArg DisplayOpts Defaults.DefaultDisplayOptions
            let sigmaReference = DisplayOptions.getSigmaReference displayOptions
            let graphologyReference = DisplayOptions.getGraphologyReference displayOptions
            let graphologyLibReference = DisplayOptions.getGraphologyLibReference displayOptions
            HTML.Doc(
                graphHTML = (HTML.toGraphHTMLNodes(DisplayOpts = displayOptions) graph),
                sigmaJSRef = sigmaReference,
                graphologyJSRef = graphologyReference,
                graphology_LibraryJSRef = graphologyLibReference
            )
            |> RenderView.AsString.htmlDocument


    ///Choose process to open plots with depending on OS. Thanks to @zyzhu for hinting at a solution (https://github.com/plotly/Plotly.NET/issues/31)
    static member internal openOsSpecificFile path =
        if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then
            let psi = System.Diagnostics.ProcessStartInfo(FileName = path, UseShellExecute = true)
            System.Diagnostics.Process.Start(psi) |> ignore
        elif RuntimeInformation.IsOSPlatform(OSPlatform.Linux) then
            System.Diagnostics.Process.Start("xdg-open", path) |> ignore
        elif RuntimeInformation.IsOSPlatform(OSPlatform.OSX) then
            System.Diagnostics.Process.Start("open", path) |> ignore
        else
            invalidOp "Not supported OS platform"


    static member show (graph:SigmaGraph, ?DisplayOpts: DisplayOptions) = 
        let guid = Guid.NewGuid().ToString()
        let html = HTML.toEmbeddedHTML(?DisplayOpts = DisplayOpts) graph
        let tempPath = Path.GetTempPath()
        let file = sprintf "%s.html" guid
        let path = Path.Combine(tempPath, file)
        File.WriteAllText(path, html)
        path |> HTML.openOsSpecificFile
