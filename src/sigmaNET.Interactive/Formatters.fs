namespace sigmaNET.Interactive

open sigmaNET

module Formatters = 
    
    /// Converts a Cytoscape type to it's HTML representation to show in a notebook environment.
    let toInteractiveHTML (graph:SigmaGraph): string = 
        graph
        |> HTML.toGraphHTML(
            //DisplayOpts = DisplayOptions.init(JSlibReference = JSlibReference. $"https://cdnjs.cloudflare.com/ajax/libs/cytoscape/{Globals.CYTOSCAPEJS_VERSION}/cytoscape.min")
            DisplayOpts = DisplayOptions.initDefault()
        )