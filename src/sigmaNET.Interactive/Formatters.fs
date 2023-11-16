namespace sigmaNET.Interactive

open sigmaNET

module Formatters = 
    
    /// Converts a Cytoscape type to it's HTML representation to show in a notebook environment.
    let toInteractiveHTML (graph:SigmaGraph): string = 
        let sigma = sigmaNET.InternalUtils.getFullPathSigmaJS()
        let graphology = sigmaNET.InternalUtils.getFullPathGraphologyJS()
        let graphologyLib = sigmaNET.InternalUtils.getFullPathGraphology_LibJS()

        graph
        |> HTML.toGraphHTML(
            DisplayOpts = DisplayOptions.init(
                SigmaJSRef = JSlibReference.Require sigma,
                GraphologyJSRef = JSlibReference.Require graphology,
                Graphology_LibraryJSRef = JSlibReference.Require graphologyLib
                )
            )