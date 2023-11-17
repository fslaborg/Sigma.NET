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
