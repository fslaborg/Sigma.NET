namespace Sigma.NET

open DynamicObj
open System
open Newtonsoft.Json

  

//{key: 'Thomas', attributes: { x: 0,  y: 10, size: 1, label: 'A'}},
// type: "image", image: "./user.svg", color: RED

type Attributes() =
    inherit DynamicObj ()
   
    static member Init 
        (
            ?X     : #IConvertible,
            ?Y     : #IConvertible,
            ?Size  : #IConvertible,
            ?Label : string,
            ?Color : string,
            ?StyleType : StyleParam.EdgeType

        ) =    
            Attributes()
            |> Attributes.Style
                (
                    ?X = X,
                    ?Y = Y,
                    ?Size = Size,
                    ?Label = Label,
                    ?Color = Color,
                    ?StyleType =  StyleType
                )

    // Applies updates to Attributes()
    static member Style
        (    
            ?X,
            ?Y,
            ?Size,
            ?Label,
            ?Color,
            ?StyleType

        ) =
            (fun (attributes:Attributes) -> 

                X         |> DynObj.setValueOpt attributes "x"
                Y         |> DynObj.setValueOpt attributes "y"
                Size      |> DynObj.setValueOpt attributes "size"
                Label     |> DynObj.setValueOpt attributes "label"
                Color     |> DynObj.setValueOpt attributes "color"
                StyleType |> DynObj.setValueOptBy attributes "type" StyleParam.EdgeType.toString
                
                // out ->
                attributes
            )



type Edge() =
    inherit DynamicObj ()
   
    static member Init 
        (            
            source : string,
            target : string,
            ?Key   : string,
            ?Size  : #IConvertible,
            ?Label : string,
            ?Color : string,
            ?StyleType : StyleParam.EdgeType
            
        ) =    
            Edge()
            |> Edge.Style
                (                    
                    source = source,
                    target = target,
                    ?Key = Key,
                    ?Size = Size,
                    ?Label = Label,
                    ?Color = Color,
                    ?StyleType =  StyleType
                )

    // Applies updates to Edge() 
    static member Style
        (    
            source,
            target,
            ?Key,
            ?Size,
            ?Label,
            ?Color,
            ?StyleType

        ) =
            (fun (edge:Edge) -> 

                let attributes = 
                    match edge.TryGetTypedValue<Attributes>("attributes") with
                    | None -> Attributes()
                    | Some a -> a
                    |> Attributes.Style
                        (
                            ?Size = Size,
                            ?Label = Label,
                            ?Color = Color,
                            ?StyleType =  StyleType
                        )
                
                source     |> DynObj.setValue    edge "source"
                target     |> DynObj.setValue    edge "target"
                Key        |> DynObj.setValueOpt edge "key"
                attributes |> DynObj.setValue    edge "attributes"
                
                // out ->
                edge
            )


type Node() =
    inherit DynamicObj ()
   
    static member Init 
        (
            key    : string,
            ?X     : #IConvertible,
            ?Y     : #IConvertible,
            ?Size  : #IConvertible,
            ?Label : string,
            ?Color : string
            // ?StyleType : string
        ) =    
            Node()
            |> Node.Style
                (
                    key = key,
                    ?X  = X,
                    ?Y  = Y,
                    ?Size = Size,
                    ?Label = Label,
                    ?Color = Color
                    // ?StyleType =  StyleType
                )

    // Applies updates to Data()
    static member Style
        (    
            key,
            ?X,
            ?Y,
            ?Size,
            ?Label,
            ?Color
            // ?StyleType

        ) =
            (fun (node:Node) -> 

                let attributes = 
                    match node.TryGetTypedValue<Attributes>("attributes") with
                    | None -> Attributes()
                    | Some a -> a
                    |> Attributes.Style
                        (
                            ?X  = X,
                            ?Y  = Y,
                            ?Size = Size,
                            ?Label = Label,
                            ?Color = Color
                            // ?StyleType =  StyleType
                        )

                key        |> DynObj.setValue node "key"
                attributes |> DynObj.setValue node "attributes"
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
            
