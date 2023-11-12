namespace sigmaNET


open DynamicObj
    
type SigmaGraph() = 
    inherit DynamicObj ()

    let tmpGraphData  = new GraphData()
    let tmpLayout     = Layout.Random (RandomOptions())
    let tmpWidth      = Defaults.DefaultWidth 
    let tmpHeight     = Defaults.DefaultHeight

    member this.AddNode (node:Node) = 
        tmpGraphData.addNode (node) 

    member this.AddEdge (edge:Edge) = 
        tmpGraphData.addEdge(edge) 


    member val GraphData  = tmpGraphData  with get,set
    member val Layout     = tmpLayout  with get,set
    member val Width      = tmpWidth  with get,set
    member val Height     = tmpHeight  with get,set


        
