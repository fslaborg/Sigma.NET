namespace sigmaNET


open DynamicObj
open DynamicObj.Operators
open System.Runtime.InteropServices

open Giraffe.ViewEngine

/// Contains mutable global default values.
///
/// Changing these values will apply the default values to all consecutive Graph generations.
module Defaults =

    /// The default width of the graph container in generated html files. Default: 600 (px)
    let mutable DefaultWidth = CssLength.Percent 100

    /// The default height of the graph container in generated html files. Default: 600 (px)
    let mutable DefaultHeight = CssLength.PX 900


    let mutable DefaultDisplayOptions = DisplayOptions.initDefault()

    /// reset global defaults to the initial values
    let reset () =
        DefaultWidth <- CssLength.Percent 100
        DefaultHeight <- CssLength.PX 600
        DefaultDisplayOptions <- DisplayOptions.initDefault()