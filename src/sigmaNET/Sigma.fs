namespace sigmaNET


open DynamicObj
    
type SigmaGraph() = 
    inherit DynamicObj ()

    let tmpGraphData  = new GraphData()

    member this.AddNode (node:Node) = 
        tmpGraphData.addNode (node) 

    member this.AddEdge (edge:Edge) = 
        tmpGraphData.addEdge(edge) 


    member val GraphData  = tmpGraphData  with get,set 

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

    static member show() (graph:SigmaGraph) = 
        HTML.show(graph.GraphData)
        
