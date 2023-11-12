#r "nuget: DynamicObj"
#r "nuget: Newtonsoft.Json, 12.0.3"
#r "nuget: Giraffe.ViewEngine, 1.4.0"
#r "./bin/Debug/net6.0/sigmaNET.dll"

open System
open sigmaNET
open Newtonsoft.Json

open Newtonsoft.Json.Linq
open System.IO

let jsonTxt = File.ReadAllText("D:/Source/sigmaNET/src/sigmaNET/data.json")
let data = JsonConvert.DeserializeObject<GraphData>(jsonTxt)


Graph.empty()
|> Graph.withNodes data.Nodes
|> Graph.withEdges data.Edges
|> Graph.show()



