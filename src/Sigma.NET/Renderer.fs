namespace Sigma.NET

// https://www.sigmajs.org/docs/interfaces/settings.Settings.html

open DynamicObj
open System
open Newtonsoft.Json

// CssLength Units
type CssLength =
| PX of int
| Percent of int 
with static member serialize (v:CssLength) =
        match v with
        | PX px -> sprintf "%ipx" px
        | Percent p -> sprintf "%i%%" p



module Render =

    type Settings() =
        inherit DynamicObj ()
   
        static member Init(
            ?HideEdgesOnMove : bool,
            ?HideLabelsOnMove : bool,
            ?RenderLabels : bool,
            ?RenderEdgeLabels : bool,
            ?EnableEdgeClickEvents : bool,
            ?EnableEdgeWheelEvents : bool,
            ?EnableEdgeHoverEvents : bool,
            ?DefaultNodeColor : string,
            ?DefaultNodeType : string,
            ?DefaultEdgeColor : string,
            ?DefaultEdgeType : string,    
            ?LabelFont : string,
            ?LabelSize : int,
            ?LabelWeight : string,
            ?LabelColor : string,
            ?EdgeLabelFont : string,
            ?EdgeLabelSize : int,
            ?EdgeLabelWeight : string,
            ?EdgeLabelColor : string,
            ?StagePadding : int,
            ?LabelDensity : int,
            ?LabelGridCellSize : int,
            ?LabelRenderedSizeThreshold : int,
            ?NodeReducer : string,
            ?EdgeReducer : string,
            ?ZIndex : bool,
            ?MinCameraRatio : int,
            ?MaxCameraRatio : int,
            ?LabelRenderer : string,
            ?HoverRenderer : string,
            ?EdgeLabelRenderer : string
        ) =
            Settings()
            |> Settings.Style
                (
                    ?HideEdgesOnMove = HideEdgesOnMove,
                    ?HideLabelsOnMove = HideLabelsOnMove,  
                    ?RenderLabels = RenderLabels,
                    ?RenderEdgeLabels = RenderEdgeLabels,
                    ?EnableEdgeClickEvents = EnableEdgeClickEvents,
                    ?EnableEdgeWheelEvents = EnableEdgeWheelEvents,
                    ?EnableEdgeHoverEvents = EnableEdgeHoverEvents,
                    ?DefaultNodeColor = DefaultNodeColor,
                    ?DefaultNodeType = DefaultNodeType,
                    ?DefaultEdgeColor = DefaultEdgeColor,
                    ?DefaultEdgeType = DefaultEdgeType,
                    ?LabelFont = LabelFont,
                    ?LabelSize = LabelSize,
                    ?LabelWeight = LabelWeight,
                    ?LabelColor = LabelColor,
                    ?EdgeLabelFont = EdgeLabelFont,
                    ?EdgeLabelSize = EdgeLabelSize,
                    ?EdgeLabelWeight = EdgeLabelWeight,
                    ?EdgeLabelColor = EdgeLabelColor,
                    ?StagePadding = StagePadding,
                    ?LabelDensity = LabelDensity,
                    ?LabelGridCellSize = LabelGridCellSize,
                    ?LabelRenderedSizeThreshold = LabelRenderedSizeThreshold,
                    ?NodeReducer = NodeReducer,
                    ?EdgeReducer = EdgeReducer,
                    ?ZIndex = ZIndex,
                    ?MinCameraRatio = MinCameraRatio,
                    ?MaxCameraRatio = MaxCameraRatio,
                    ?LabelRenderer = LabelRenderer,
                    ?HoverRenderer = HoverRenderer,
                    ?EdgeLabelRenderer = EdgeLabelRenderer
                )
        static member Style
            ( 
                ?HideEdgesOnMove,
                ?HideLabelsOnMove,
                ?RenderLabels,
                ?RenderEdgeLabels,
                ?EnableEdgeClickEvents,
                ?EnableEdgeWheelEvents,
                ?EnableEdgeHoverEvents,
                ?DefaultNodeColor,
                ?DefaultNodeType,
                ?DefaultEdgeColor,
                ?DefaultEdgeType,
                ?LabelFont,
                ?LabelSize,
                ?LabelWeight,
                ?LabelColor,
                ?EdgeLabelFont,
                ?EdgeLabelSize,
                ?EdgeLabelWeight,
                ?EdgeLabelColor,
                ?StagePadding,
                ?LabelDensity,
                ?LabelGridCellSize,
                ?LabelRenderedSizeThreshold,
                ?NodeReducer,
                ?EdgeReducer,
                ?ZIndex,
                ?MinCameraRatio,
                ?MaxCameraRatio,
                ?LabelRenderer,
                ?HoverRenderer,
                ?EdgeLabelRenderer
            ) =
                (fun (settings:Settings) -> 
                    HideEdgesOnMove |> DynObj.setValueOpt settings "hideEdgesOnMove"
                    HideLabelsOnMove |> DynObj.setValueOpt settings "hideLabelsOnMove"
                    RenderLabels |> DynObj.setValueOpt settings "renderLabels"
                    RenderEdgeLabels |> DynObj.setValueOpt settings "renderEdgeLabels"
                    EnableEdgeClickEvents |> DynObj.setValueOpt settings "enableEdgeClickEvents"
                    EnableEdgeWheelEvents |> DynObj.setValueOpt settings "enableEdgeWheelEvents"
                    EnableEdgeHoverEvents |> DynObj.setValueOpt settings "enableEdgeHoverEvents"
                    DefaultNodeColor |> DynObj.setValueOpt settings "defaultNodeColor"
                    DefaultNodeType |> DynObj.setValueOpt settings "defaultNodeType"
                    DefaultEdgeColor |> DynObj.setValueOpt settings "defaultEdgeColor"
                    DefaultEdgeType |> DynObj.setValueOpt settings "defaultEdgeType"
                    LabelFont |> DynObj.setValueOpt settings "labelFont"
                    LabelSize |> DynObj.setValueOpt settings "labelSize"
                    LabelWeight |> DynObj.setValueOpt settings "labelWeight"
                    LabelColor |> DynObj.setValueOpt settings "labelColor"
                    EdgeLabelFont |> DynObj.setValueOpt settings "edgeLabelFont"
                    EdgeLabelSize |> DynObj.setValueOpt settings "edgeLabelSize"
                    EdgeLabelWeight |> DynObj.setValueOpt settings "edgeLabelWeight"
                    EdgeLabelColor |> DynObj.setValueOpt settings "edgeLabelColor"
                    StagePadding |> DynObj.setValueOpt settings "stagePadding"
                    LabelDensity |> DynObj.setValueOpt settings "labelDensity"
                    LabelGridCellSize |> DynObj.setValueOpt settings "labelGridCellSize"
                    LabelRenderedSizeThreshold |> DynObj.setValueOpt settings "labelRenderedSizeThreshold"
                    NodeReducer |> DynObj.setValueOpt settings "nodeReducer"
                    EdgeReducer |> DynObj.setValueOpt settings "edgeReducer"
                    ZIndex |> DynObj.setValueOpt settings "zIndex"
                    MinCameraRatio |> DynObj.setValueOpt settings "minCameraRatio"
                    MaxCameraRatio |> DynObj.setValueOpt settings "maxCameraRatio"
                    LabelRenderer |> DynObj.setValueOpt settings "labelRenderer"
                    HoverRenderer |> DynObj.setValueOpt settings "hoverRenderer"
                    EdgeLabelRenderer |> DynObj.setValueOpt settings "edgeLabelRenderer"
                    
                    // out ->
                    settings
                )
            
