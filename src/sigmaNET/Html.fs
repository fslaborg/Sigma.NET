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
            containerId: string,
            sigmaReference: JSlibReference,
            graphologyReference: JSlibReference
        ) =
        match sigmaReference with
        | _ ->
            script
                [ _type "text/javascript" ]
                [
                    rawText (
                        Globals.SCRIPT_TEMPLATE
                            .Replace("[CONTAINERID]",containerId)
                            .Replace("[GRAPHDATA]", graphData)
                    )
                ]


    static member Doc(
        graphHTML: XmlNode list, 
        sigmaReference: JSlibReference, 
        graphologyReference: JSlibReference, 
        ?AdditionalHeadTags
    ) =
        let additionalHeadTags =
            defaultArg AdditionalHeadTags []

        let graphologyScriptRef =
            match graphologyReference with
            | CDN cdn -> script [ _src cdn ] []
            | Full ->
                script
                    [ _type "text/javascript" ]
                    [
                        rawText (InternalUtils.getFullGraphologyJS ())
                    ]
            | NoReference -> rawText ""
            //| Require _ -> rawText ""

        let sigmaScriptRef =
            match sigmaReference with
            | CDN cdn -> script [ _src cdn ] []
            | Full ->
                script
                    [ _type "text/javascript" ]
                    [
                        rawText (InternalUtils.getFullSigmaJS ())
                    ]
            | NoReference -> rawText ""
            //| Require _ -> rawText ""

        html
            []
            [
                head
                    []
                    [
                        graphologyScriptRef
                        sigmaScriptRef
                        yield! additionalHeadTags
                    ]
                body [] [ yield! graphHTML]
            ]

    static member CreateGraphHTML
        (
            graphData: string,
            divId: string,
            sigmaScriptRef: JSlibReference,
            graphologyScriptRef: JSlibReference,
            ?Width: int,
            ?Height: int
        ) =
        let width, height = 
            Width |> Option.defaultValue Defaults.DefaultWidth,
            Height |> Option.defaultValue Defaults.DefaultHeight

        let graphScript =
            HTML.CreateGraphScript(
                graphData = graphData,
                containerId = divId,
                sigmaReference = sigmaScriptRef,
                graphologyReference = graphologyScriptRef
            )

        [
            div
                [ _id divId; _style $"width:{width}px; height:{height}px"]
                [
                    comment "Cytoscape graph will be drawn inside this DIV"
                ]
            graphScript
        ]

    /// Converts a CyGraph to it HTML representation. The div layer has a default size of 600 if not specified otherwise.
    static member toGraphHTMLNodes (
        ?DisplayOpts: DisplayOptions
    ) =
        fun (graphData:GraphData) -> 

            let displayOptions = defaultArg DisplayOpts Defaults.DefaultDisplayOptions
            let sigmaReference = displayOptions |> DisplayOptions.getSigmaReference
            let graphologyReference = displayOptions |> DisplayOptions.getGraphologyReference

            let guid = Guid.NewGuid().ToString()
            let id   = sprintf "e%s" <| Guid.NewGuid().ToString().Replace("-","").Substring(0,10)

            let ntsettings = new JsonSerializerSettings()
            let _ = ntsettings.ReferenceLoopHandling <- ReferenceLoopHandling.Ignore             

            let jsonGraph = JsonConvert.SerializeObject (graphData,ntsettings)

            HTML.CreateGraphHTML(
                graphData = jsonGraph,
                divId = id,
                sigmaScriptRef = sigmaReference,
                graphologyScriptRef = graphologyReference
            )

    static member toGraphHTML(
        ?DisplayOpts: DisplayOptions
    ) =
        fun (graphData:GraphData) -> 
            graphData
            |> HTML.toGraphHTMLNodes(?DisplayOpts = DisplayOpts)
            |> RenderView.AsString.htmlNodes

    /// Converts a Graph to it HTML representation and embeds it into a html page.
    static member toEmbeddedHTML (
        ?DisplayOpts: DisplayOptions
    ) =
        fun (graphData:GraphData) ->
            let displayOptions = defaultArg DisplayOpts Defaults.DefaultDisplayOptions
            let sigmaReference = DisplayOptions.getSigmaReference displayOptions
            let graphologyReference = DisplayOptions.getGraphologyReference displayOptions
            HTML.Doc(
                graphHTML = (HTML.toGraphHTMLNodes(DisplayOpts = displayOptions) graphData),
                sigmaReference = sigmaReference,
                graphologyReference = graphologyReference
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


    static member show (graphData:GraphData, ?DisplayOpts: DisplayOptions) = 
        let guid = Guid.NewGuid().ToString()
        let html = HTML.toEmbeddedHTML(?DisplayOpts = DisplayOpts) graphData
        let tempPath = Path.GetTempPath()
        let file = sprintf "%s.html" guid
        let path = Path.Combine(tempPath, file)
        File.WriteAllText(path, html)
        path |> HTML.openOsSpecificFile
