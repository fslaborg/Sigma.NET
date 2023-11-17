namespace Sigma.NET

module StyleParam =

    [<RequireQualifiedAccess>]
    type EdgeType = 
        | Curve
        | Line
        | Arrow
        | CurvedArrow
        static member toString =
            function
            | Curve -> "curve"
            | Line -> "line"
            | Arrow -> "arrow"
            | CurvedArrow -> "curvedArrow"

    [<RequireQualifiedAccess>]
    type NodeType = 
        | Circle
        | Triangle
        | Square
        | Pentagon
        | Star
        | Hexagon
        | Heart
        | Cloud
        | Custom of string
        static member toString =
            function
            | Circle -> "circle"
            | Triangle -> "triangle"
            | Square -> "square"
            | Pentagon -> "pentagon"
            | Star -> "star"
            | Hexagon -> "hexagon"
            | Heart -> "heart"
            | Cloud -> "cloud"
            | Custom str -> str            
            