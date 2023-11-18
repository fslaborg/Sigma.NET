namespace Sigma.NET

open DynamicObj
open System
open Newtonsoft.Json

  

//{key: 'Thomas', attributes: { x: 0,  y: 10, size: 1, label: 'A'}},
// type: "image", image: "./user.svg", color: RED

// DisplayData = (for notes and edges) 
  //label: string | null;
  //size: number;
  //color: string;
  //hidden: boolean;
  //forceLabel: boolean;
  //zIndex: number;
  //type: string;

type DisplayData() =
    inherit DynamicObj ()
   
    static member Init 
        (           
            ?Label      : string,
            ?Size       : #IConvertible,
            ?Color      : string,
            ?Hidden     : bool,
            ?ForceLabel : bool,
            ?ZIndex     : int,
            ?StyleType  : string,
            ?X          : #IConvertible,
            ?Y          : #IConvertible

        ) =    
            DisplayData()
            |> DisplayData.Style
                (
                    ?Label = Label,
                    ?Size = Size,                    
                    ?Color = Color,
                    ?Hidden     = Hidden,
                    ?ForceLabel = ForceLabel,
                    ?ZIndex     = ZIndex,
                    ?StyleType =  StyleType,
                    ?X = X,
                    ?Y = Y
                )

    // Applies updates to Attributes()
    static member Style
        (    
            ?Label,
            ?Size,
            ?Color,
            ?Hidden,
            ?ForceLabel,
            ?ZIndex,
            ?StyleType,
            ?X,
            ?Y
        ) =
            (fun (displayData:DisplayData) -> 

                Label      |> DynObj.setValueOpt displayData "label"
                Size       |> DynObj.setValueOpt displayData "size"                
                Color      |> DynObj.setValueOpt displayData "color"
                Hidden     |> DynObj.setValueOpt displayData "hidden"
                ForceLabel |> DynObj.setValueOpt displayData "forceLabel"
                ZIndex     |> DynObj.setValueOpt displayData "zIndex"
                StyleType  |> DynObj.setValueOpt displayData "type" 
                X          |> DynObj.setValueOpt displayData "x"
                Y          |> DynObj.setValueOpt displayData "y"                
                // out ->
                displayData
            )



type Edge() =
    inherit DynamicObj ()
   
    static member Init 
        (            
            source : string,
            target : string,
            ?Key   : string,
            ?DisplayData : DisplayData
            
        ) =    
            Edge()
            |> Edge.Style
                (                    
                    source = source,
                    target = target,
                    ?Key = Key,
                    ?DisplayData = DisplayData
                )

    // Applies updates to Edge() 
    static member Style
        (    
            source,
            target,
            ?Key,
            ?DisplayData

        ) =
            (fun (edge:Edge) -> 
                
                source      |> DynObj.setValue    edge "source"
                target      |> DynObj.setValue    edge "target"
                Key         |> DynObj.setValueOpt edge "key"
                DisplayData |> DynObj.setValueOpt    edge "attributes"
                
                // out ->
                edge
            )

type Node() =
    inherit DynamicObj ()
   
    static member Init 
        (
            key    : string,
            ?DisplayData : DisplayData
        ) =    
            Node()
            |> Node.Style
                (
                    key = key,
                    ?DisplayData = DisplayData

                )

    // Applies updates to Data()
    static member Style
        (    
            key,
            ?DisplayData

        ) =
            (fun (node:Node) -> 

                key         |> DynObj.setValue node "key"
                DisplayData |> DynObj.setValueOpt node "attributes"
                // out ->
                node
            )

type GraphData() =
    let nodes = ResizeArray()  
    let edges = ResizeArray()
    
    [<JsonProperty("nodes")>]
    member _.Nodes = nodes
    [<JsonProperty("edges")>]
    member _.Edges = edges

    member _.addNode(node:Node) =
        nodes.Add(node)

    member _.addEdge(edge:Edge) =
        edges.Add(edge)
            
