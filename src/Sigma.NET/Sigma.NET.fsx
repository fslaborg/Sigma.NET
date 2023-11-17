#r "nuget: DynamicObj"
#r "nuget: Newtonsoft.Json, 12.0.3"
#r "nuget: Giraffe.ViewEngine, 1.4.0"
#r "./bin/Debug/net6.0/Sigma.NET.dll"

#r "nuget: Graphoscope, 0.4.0"
open Sigma.NET

open System
open System.IO
open Newtonsoft.Json
open Graphoscope

open Graphoscope.RandomModels

let colors = [|"#F2FFE9";"#A6CF98";"#557C55";"#FA7070"|]

let N = 50
let p = 0.005
let rnd = new Random()
let random_number = rnd.Next(1, 51)
//let myGilbertGraph = Gilbert.initDirectedFGraph N p
let myBollobasRiordan = RandomModels.BollobasRiordan.initDirectedFGraph  N 0.5 0.1 0.4 0.6 0.4 FGraph.empty

Graph.empty()
|> Graph.withNodes[
    for node in myBollobasRiordan do
        yield (Node.Init(key=string node.Key, Size=rnd.Next(1, 18), Color=colors.[rnd.Next(0, 4)]))

]
|> Graph.withEdges [
    for node in myBollobasRiordan do
        let pred,t,_ = node.Value
        for kv in pred do
            yield (Edge.Init(source=string kv.Key, target=string t,Size=rnd.Next(1, 3)) )

]

// |> Graph.withForceAtlas2(Iterations=100,Settings=FA2Settings.Init(AdjustSizes=true,Gravity=5),GetEdgeWeight="size")
// |> Graph.withNoverlap(50)
|> Graph.withCircularLayout()
|> Graph.show()


// let jsonTxt = File.ReadAllText("D:/Source/sigmaNET/src/sigmaNET/data.json")
// let data = JsonConvert.DeserializeObject<GraphData>(jsonTxt)


// Graph.empty()
// |> Graph.withNodes data.Nodes
// |> Graph.withEdges data.Edges
// |> Graph.withForceAtlas2(Iterations=100,Settings=FA2Settings.Init(AdjustSizes=true,Gravity=10))
// |> Graph.show()




// SigmaSetting:
//     settings: {
//     sideMargin: 10,
//     minEdgeSize: 2,
//     maxEdgeSize: 2,
//     minNodeSize: 3,
//     maxNodeSize: 14,
//     labelThreshold: 2,
//     labelAlignment: 'center',
//     nodesPowRatio: 1.3,
//     edgesPowRatio: 1,
//     autoResize: true,
//     autoRescale: true,
//     labelSizeRatio: 20,
