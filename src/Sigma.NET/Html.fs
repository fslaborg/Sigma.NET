namespace Sigma.NET

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
            settings: string,
            containerId: string,
            widgets: string,
            sigmaJSRef: JSlibReference
        ) =
        match sigmaJSRef with
        | Require gjs ->
            script
                [ _type "text/javascript" ]
                [
                    rawText (
                        Globals.REQUIREJS_SCRIPT_TEMPLATE
                                  //'[GRAPHOLOGY_JS]',
                                  //'[SIGMA_JS]',
                                  //'[GRAPHOLOGY-LIB_JS]',
                            .Replace("[GRAPHOLOGY_JS]", gjs.Graphology)
                            .Replace("[SIGMA_JS]", gjs.Sigma)
                            .Replace("[GRAPHOLOGY-LIB_JS]", gjs.GraphologyLib)
                            .Replace("[CONTAINERID]",containerId)
                            .Replace("[LAYOUT]",layout)
                            .Replace("[SETTINGS]",settings)
                            .Replace("[WIDGETS]",widgets)
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
                            .Replace("[SETTINGS]",settings)
                            .Replace("[WIDGETS]",widgets)
                            .Replace("[GRAPHDATA]", graphData)
                    )
                ]


    static member Doc(
        graphHTML: XmlNode list, 
        sigmaJSRef: JSlibReference,
        ?AdditionalHeadTags
    ) =
        let additionalHeadTags =
            defaultArg AdditionalHeadTags []

        let sigmaScriptRefs =
            match sigmaJSRef with
            | Local gjs -> [
                                script [ _src gjs.Graphology ] []
                                script [ _src gjs.Sigma ] []
                                script [ _src gjs.GraphologyLib ] []
                            
                           ]
            | CDN gjs -> [
                                script [ _src gjs.Graphology ] []
                                script [ _src gjs.Sigma ] []
                                script [ _src gjs.GraphologyLib ] []
                            
                           ]
            | Full gjs -> [
                                script [ _type "text/javascript" ] [ rawText gjs.Graphology ]
                                script [ _type "text/javascript" ] [ rawText gjs.Sigma ]
                                script [ _type "text/javascript" ] [ rawText gjs.GraphologyLib ]
                            ]
            | NoReference -> [rawText ""]
            | Require gjs -> [
                                script [ _src gjs.Graphology ] []
                                script [ _src gjs.Sigma ] []
                                script [ _src gjs.GraphologyLib ] []
                           ]


        html
            []
            [
                head
                    []
                    [
                        yield! sigmaScriptRefs
                        yield! additionalHeadTags
                    ]
                body [] [ yield! graphHTML]
            ]

    static member CreateGraphHTML
        (
            graphData: string,
            layout : string,
            settings: string,
            divId: string,
            widgets: string,
            sigmaJSRef: JSlibReference,
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
                settings = settings,
                containerId = divId,
                widgets = widgets,
                sigmaJSRef = sigmaJSRef
            )

        [
            
            div [ _id "graph" ] // TODO: maybe remove this div?
                [
                    div
                        [ _id divId; _style (sprintf "width: %s; height: %s" (CssLength.serialize width) (CssLength.serialize height) )]
                        [
                            rawText "&nbsp"
                            comment "Sigma.NET graph will be drawn inside this DIV"
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

            let guid = Guid.NewGuid().ToString()
            let id   = sprintf "e%s" <| Guid.NewGuid().ToString().Replace("-","").Substring(0,10)

            let ntsettings = new JsonSerializerSettings()
            let _ = ntsettings.ReferenceLoopHandling <- ReferenceLoopHandling.Ignore             

            let jsonGraph = JsonConvert.SerializeObject (graph.GraphData,ntsettings)

            let layout = Layout.serialize graph.Layout  
            
            let settings = JsonConvert.SerializeObject (graph.Settings,ntsettings)

            HTML.CreateGraphHTML(
                graphData = jsonGraph,
                layout = layout,
                settings = settings,
                divId = id,
                widgets = graph.GetWidgetsAsString(),
                sigmaJSRef = sigmaReference,
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
            HTML.Doc(
                graphHTML = (HTML.toGraphHTMLNodes(DisplayOpts = displayOptions) graph),
                sigmaJSRef = sigmaReference
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
