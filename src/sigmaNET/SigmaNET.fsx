#r "nuget: DynamicObj"
#r "nuget: Newtonsoft.Json, 12.0.3"
#r "nuget: Giraffe.ViewEngine, 1.4.0"
#r "./bin/Debug/net6.0/sigmaNET.dll"

#r "nuget: Graphoscope, 0.4.0"
open Graphoscope

open System
open System.IO
open Newtonsoft.Json
open sigmaNET

open Graphoscope.RandomModels


let N = 900
let p = 0.005
let rnd = new Random()
let random_number = rnd.Next(1, 51)
let myGilbertGraph = Gilbert.initDirectedFGraph N p

Graph.empty()
|> Graph.withNodes[
    for node in myGilbertGraph do
        yield (Node.Init(key=string node.Key, Size=rnd.Next(1, 21)))

]
|> Graph.withEdges [
    for node in myGilbertGraph do
        let pred,t,_ = node.Value
        for kv in pred do
            yield (Edge.Init(source=string kv.Key, target=string t,Size=rnd.Next(1, 6)) )

]
//|> Graph.withForceAtlas2(Iterations=100,Settings=FA2Settings.Init(AdjustSizes=true,Gravity=5),GetEdgeWeight="size")
|> Graph.withNoverlap(50)
|> Graph.show()


// let jsonTxt = File.ReadAllText("D:/Source/sigmaNET/src/sigmaNET/data.json")
// let data = JsonConvert.DeserializeObject<GraphData>(jsonTxt)


// Graph.empty()
// |> Graph.withNodes data.Nodes
// |> Graph.withEdges data.Edges
// |> Graph.withForceAtlas2(Iterations=100,Settings=FA2Settings.Init(AdjustSizes=true,Gravity=10))
// |> Graph.show()




