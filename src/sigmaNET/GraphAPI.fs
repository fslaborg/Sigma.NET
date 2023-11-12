namespace sigmaNET

open System.Runtime.InteropServices



[<AutoOpen>]
module Graph = 
    let node key = Node.Init(key = key) 
    let edge source target = Edge.Init(source = source,target = target) 
    


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


    [<CompiledName("Show")>] 
    static member show() (graph:SigmaGraph) = 
        HTML.show(graph)
