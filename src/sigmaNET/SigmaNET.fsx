#r "nuget: DynamicObj"
#r "nuget: Newtonsoft.Json, 12.0.3"
#r "nuget: Giraffe.ViewEngine, 1.4.0"
#r "./bin/Debug/net6.0/sigmaNET.dll"

open System
open sigmaNET
open Newtonsoft.Json

let node1 = Node.Init("Thomas",X = 0, Y = 10, Size = 10, Label = "A" )
let node2 = Node.Init("Eric",X = 10, Y = 0, Size = 50, Label = "B" )
let edge1 = Edge.Init("Thomas","Eric", Size = 10)

let graphData = GraphData()
let _ = graphData.addNode(node1)
let _ = graphData.addNode(node2)
let _ = graphData.addEdge(edge1)

// let node_tmp = Node.Init("Eric" )

// let ntsettings = new JsonSerializerSettings()
// //let _ = ntsettings.PreserveReferencesHandling <- PreserveReferencesHandling.Objects
// let _ = ntsettings.ReferenceLoopHandling <- ReferenceLoopHandling.Ignore//ReferenceLoopHandling.Serialize

// JsonConvert.SerializeObject (edge1,ntsettings,)



HTML.show(graphData)