namespace sigmaNET


open DynamicObj
    
type SigmaGraph() = 
    inherit DynamicObj ()

    let tmpGraphData  = new GraphData()
    let tmpLayout     = Layout.Random (RandomOptions())

    member this.AddNode (node:Node) = 
        tmpGraphData.addNode (node) 

    member this.AddEdge (edge:Edge) = 
        tmpGraphData.addEdge(edge) 


    member val GraphData  = tmpGraphData  with get,set
    member val Layout     = tmpLayout  with get,set


        
