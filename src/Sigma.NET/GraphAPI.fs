namespace Sigma.NET

open System.Runtime.InteropServices
open System
open DynamicObj

// https://www.bsimard.com/2018/04/25/graph-viz-with-sigmajs.html


[<AutoOpen>]
type VisGraphElement() = 
    static member node key = Node.Init(key = key) 

    static member withNodeData(
            ?Label      : string,
            ?Size       : #IConvertible,
            ?Color      : string,
            ?Hidden     : bool,
            ?ForceLabel : bool,
            ?ZIndex     : int,
            ?StyleType  : StyleParam.NodeType,
            ?X          : #IConvertible,
            ?Y          : #IConvertible
    ) =
        fun (node:Node) -> 
            let styleType = Option.map StyleParam.NodeType.toString StyleType
            let displayData = 
                    match node.TryGetTypedValue<DisplayData>("attributes") with
                    | None -> DisplayData()
                    | Some a -> a
                    |> DisplayData.Style
                        (?Label=Label,?Size=Size,?Color=Color,?Hidden=Hidden,
                         ?ForceLabel=ForceLabel,?ZIndex=ZIndex,?StyleType=styleType,?X=X,?Y=Y)
            displayData |> DynObj.setValue node "attributes"
            node
    
    static member edge source target = Edge.Init(source = source,target = target) 
    
    static member withEdgeData(
            ?Label      : string,
            ?Size       : #IConvertible,
            ?Color      : string,
            ?Hidden     : bool,
            ?ForceLabel : bool,
            ?ZIndex     : int,
            ?StyleType  : StyleParam.EdgeType,
            ?X          : #IConvertible,
            ?Y          : #IConvertible
    ) =
        fun (edge:Edge) -> 
            let styleType = Option.map StyleParam.EdgeType.toString StyleType
            let displayData = 
                    match edge.TryGetTypedValue<DisplayData>("attributes") with
                    | None -> DisplayData()
                    | Some a -> a
                    |> DisplayData.Style
                        (?Label=Label,?Size=Size,?Color=Color,?Hidden=Hidden,
                         ?ForceLabel=ForceLabel,?ZIndex=ZIndex,?StyleType=styleType,?X=X,?Y=Y)
            displayData |> DynObj.setValue edge "attributes"
            edge


// Module to manipulate and sytely a graph
type Graph() =
    
    [<CompiledName("Empty")>]
    static member empty () = SigmaGraph()

    /// Creates a graph with a single node
    [<CompiledName("WithNode")>]
    static member withNode (node:Node) (graph:SigmaGraph) = 
        graph.AddNode(node)
        graph       

    [<CompiledName("WithNodes")>]
    static member withNodes (nodes:Node seq) (graph:SigmaGraph) = 
        nodes |> Seq.iter (fun node -> graph.AddNode node) 
        graph

    [<CompiledName("WithEdge")>]
    static member withEdge (edge:Edge) (graph:SigmaGraph) = 
        graph.AddEdge(edge)
        graph       

    [<CompiledName("WithEdges")>]
    static member withEdges (edges:Edge seq) (graph:SigmaGraph) = 
        edges |> Seq.iter (fun edge -> graph.AddEdge edge) 
        graph

    [<CompiledName("WithRandomLayout")>] 
    static member withRandomLayout(
        [<Optional; DefaultParameterValue(null)>] ?Scale,
        [<Optional; DefaultParameterValue(null)>] ?Center,
        [<Optional; DefaultParameterValue(null)>] ?Dimensions    
        ) = 
            fun (graph:SigmaGraph) -> 
                graph.Layout <- Layout.Random (RandomOptions.Init(?Dimensions=Dimensions,?Center=Center,?Scale=Scale))
                graph   

    [<CompiledName("WithCircularLayout")>]
    static member withCircularLayout(
        [<Optional; DefaultParameterValue(null)>] ?Scale,
        [<Optional; DefaultParameterValue(null)>] ?Center
        ) = 
            fun (graph:SigmaGraph) -> 
                graph.Layout <- Layout.Circular (CircularOptions.Init(?Scale=Scale,?Center=Center))
                graph   
 


    [<CompiledName("WithForceAtlas2")>]
    static member withForceAtlas2(
        [<Optional; DefaultParameterValue(null)>] ?Iterations, 
        [<Optional; DefaultParameterValue(null)>] ?Settings,
        [<Optional; DefaultParameterValue(null)>] ?GetEdgeWeight) = 
        fun (graph:SigmaGraph) ->
            graph.Layout <- Layout.FA2 (FA2Options.Init(?Iterations=Iterations,?GetEdgeWeight=GetEdgeWeight,?Settings=Settings))
            graph

    [<CompiledName("WithNoverlap")>]
    static member withNoverlap(
        [<Optional; DefaultParameterValue(null)>] ?MaxIterations,
        [<Optional; DefaultParameterValue(null)>] ?Settings) = 
        fun (graph:SigmaGraph) ->
            graph.Layout <- Layout.Noverlap (NoverlapOptions.Init(?MaxIterations = MaxIterations,?Settings=Settings))
            graph



    /// Sets the size of a canvas (in pixels)
    [<CompiledName("WithSize")>]
    static member withSize
        (
            [<Optional; DefaultParameterValue(null)>] ?Width: CssLength,
            [<Optional; DefaultParameterValue(null)>] ?Height: CssLength
        ) =

        fun (graph:SigmaGraph) ->
            graph.Width <- Option.defaultValue Defaults.DefaultWidth Width
            graph.Height <- Option.defaultValue Defaults.DefaultHeight Height
            
            graph

    /// Sets the Renderer settings
    [<CompiledName("WithRenderer")>]
    static member withRenderer(settings:Render.Settings) = 
        fun (graph:SigmaGraph) ->
            graph.Settings <- settings
            graph

    [<CompiledName("Show")>] 
    static member show() (graph:SigmaGraph) = 
        HTML.show(graph)
