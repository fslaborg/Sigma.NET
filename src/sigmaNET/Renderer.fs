namespace sigmaNET


// CssLength Units
type CssLength =
| PX of int
| Percent of int 
with static member serialize (v:CssLength) =
        match v with
        | PX px -> sprintf "%ipx" px
        | Percent p -> sprintf "%i%%" p

//module Renderer

