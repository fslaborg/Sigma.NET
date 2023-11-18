namespace Sigma.NET

module StyleParam =

    [<RequireQualifiedAccess>]
    type EdgeType = 
        | Line
        | Arrow        
        //| Curve                
        //| CurvedArrow
        //| Dashed
        //| Dotted
        //| Parallel
        //| Tapered
        | Custom of string
        static member toString =
            function            
            | Line -> "line"
            | Arrow -> "arrow"
            //| Curve -> "curve"
            //| CurvedArrow -> "curvedArrow"
            //| Dashed -> "dashed"
            //| Dotted -> "dotted"
            //| Parallel -> "parallel"
            //| Tapered -> "tapered"
            | Custom str -> str 

    [<RequireQualifiedAccess>]
    type NodeType = 
        | Circle
        //| Triangle
        //| Square
        //| Pentagon
        //| Star
        //| Hexagon
        //| Heart
        //| Cloud
        | Custom of string
        static member toString =
            function
            | Circle -> "circle"
            //| Triangle -> "triangle"
            //| Square -> "square"
            //| Pentagon -> "pentagon"
            //| Star -> "star"
            //| Hexagon -> "hexagon"
            //| Heart -> "heart"
            //| Cloud -> "cloud"
            | Custom str -> str            
            