namespace sigmaNET.Interactive

open sigmaNET

module Formatters = 
    
    /// Converts a Cytoscape type to it's HTML representation to show in a notebook environment.
    let toInteractiveHTML (graph:SigmaGraph): string = 
        let sigma = sigmaNET.InternalUtils.getUriJS()

        graph
        |> HTML.toGraphHTML(
            DisplayOpts = DisplayOptions.init(
                SigmaJSRef = JSlibReference.Require sigma
                )
            )