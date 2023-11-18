namespace Sigma.NET 


open DynamicObj
    
type SigmaGraph() = 
    inherit DynamicObj ()

    let tmpGraphData  = new GraphData()
    let tmpLayout     = Layout.Random (RandomOptions())
    let tmpSetting    = Render.Settings()
    let tmpWidgets    = 
        let tmp = ResizeArray<string>()
        tmp.Add("")
        tmp
    let tmpWidth      = Defaults.DefaultWidth 
    let tmpHeight     = Defaults.DefaultHeight

    member this.AddNode (node:Node) = 
        tmpGraphData.addNode (node) 

    member this.AddEdge (edge:Edge) = 
        tmpGraphData.addEdge(edge) 
    
    member this.GetWidgetsAsString () =
        tmpWidgets |> Seq.reduce (fun acc x -> acc + x + " ")

    member val GraphData  = tmpGraphData  with get,set
    member val Layout     = tmpLayout  with get,set
    member val Settings   = tmpSetting  with get,set  
    member val Widgets    = tmpWidgets  with get,set

    member val Width      = tmpWidth  with get,set
    member val Height     = tmpHeight  with get,set


        
