namespace Sigma.NET.Interactive

open Sigma.NET

module Formatters = 
    
    /// Converts a Cytoscape type to it's HTML representation to show in a notebook environment.
    let toInteractiveHTML (graph:SigmaGraph): string = 
        let sigma = Sigma.NET.InternalUtils.getUriJS()

        graph
        |> HTML.toGraphHTML(
            DisplayOpts = DisplayOptions.init(
                SigmaJSRef = JSlibReference.Require sigma
                )
            )