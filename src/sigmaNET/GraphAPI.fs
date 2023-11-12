namespace sigmaNET



[<AutoOpen>]
module Graph = 
    let node key = Node.Init(key = key) 
    let edge source target = Edge.Init(source = source,target = target) 
    


// Module to manipulate and sytely a graph
type Graph() =
    
    static member empty () = SigmaGraph()

    static member withNode (node:Node) (graph:SigmaGraph) = 
        graph.AddNode(node)
        graph       

    static member withNodes (nodes:Node seq) (graph:SigmaGraph) = 
        nodes |> Seq.iter (fun node -> graph.AddNode node) 
        graph

    static member withEdge (edge:Edge) (graph:SigmaGraph) = 
        graph.AddEdge(edge)
        graph       

    static member withEdges (edges:Edge seq) (graph:SigmaGraph) = 
        edges |> Seq.iter (fun edge -> graph.AddEdge edge) 
        graph

    //Random Layout
    static member withRandomLayout(?Dimensions,?Center, ?Scale) = 
            fun (graph:SigmaGraph) -> 
                graph.Layout <- Layout.Random (RandomOptions.Init(?Dimensions=Dimensions,?Center=Center,?Scale=Scale))
                graph   

    static member withForceAtlas2(?Iterations, ?GetEdgeWeight, ?FA2Settings) = 
        fun (graph:SigmaGraph) ->
            graph.Layout <- Layout.FA2 (FA2Options.Init(?Iterations=Iterations,?GetEdgeWeight=GetEdgeWeight,?Settings=FA2Settings))
            graph

    static member show() (graph:SigmaGraph) = 
        HTML.show(graph)
