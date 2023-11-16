namespace sigmaNET

open DynamicObj
open System.Runtime.InteropServices
open Giraffe.ViewEngine

// adapted from Plotly.NET (https://github.com/plotly/Plotly.NET/blob/dev/src/Plotly.NET/DisplayOptions/DisplayOptions.fs)

///Sets how the javascript library is referenced in the head of html docs.
type JSlibReference =
    | Local of InternalUtils.JSRefGroup 
    /// The url for a script tag that references the javascript library CDN
    | CDN of InternalUtils.JSRefGroup
    /// Full javascript library source code (~100KB) is included in the output. HTML files generated with this option are fully self-contained and can be used offline
    | Full of InternalUtils.JSRefGroup
    ///// Use requirejs to reference javascript library from a url
    | Require of InternalUtils.JSRefGroup
    //include no javascript library script at all. This can be helpfull when embedding the output into a document that already references the javascript library.
    | NoReference

type DisplayOptions() =
    inherit DynamicObj()

    /// <summary>
    /// Returns a new DisplayOptions object with the given styles
    /// </summary>
    /// <param name="AdditionalHeadTags">Additional tags that will be included in the document's head </param>
    /// <param name="Description">HTML tags that appear below the chart in HTML docs</param>
    /// <param name="SigmaJSReference">Sets how sigma.js is referenced in the head of html docs. When CDN, a script tag that references a CDN is included in the output. When Full, a script tag containing the cytoscape.js source code (~400KB) is included in the output. HTML files generated with this option are fully self-contained and can be used offline</param>
    /// <param name="GraphologyJSReference">Sets how graphology.js is referenced in the head of html docs. When CDN, a script tag that references a CDN is included in the output. When Full, a script tag containing the cytoscape.js source code (~400KB) is included in the output. HTML files generated with this option are fully self-contained and can be used offline</param>
    static member init
        (
            [<Optional; DefaultParameterValue(null)>] ?AdditionalHeadTags: XmlNode list,
            [<Optional; DefaultParameterValue(null)>] ?Description: XmlNode list,
            [<Optional; DefaultParameterValue(null)>] ?SigmaJSRef: JSlibReference
        ) =
        DisplayOptions()
        |> DisplayOptions.style (
            ?AdditionalHeadTags = AdditionalHeadTags,
            ?Description = Description,
            ?SigmaJSRef = SigmaJSRef
        )

    /// <summary>
    /// Returns a function sthat applies the given styles to a DisplayOptions object
    /// </summary>
    /// <param name="AdditionalHeadTags">Additional tags that will be included in the document's head </param>
    /// <param name="Description">HTML tags that appear below the chart in HTML docs</param>
    /// <param name="CytoscapeJSReference">Sets how cytoscape.js is referenced in the head of html docs. When CDN, a script tag that references a CDN is included in the output. When Full, a script tag containing the cytoscape.js source code (~400KB) is included in the output. HTML files generated with this option are fully self-contained and can be used offline</param>
    static member style
        (
            [<Optional; DefaultParameterValue(null)>] ?AdditionalHeadTags: XmlNode list,
            [<Optional; DefaultParameterValue(null)>] ?Description: XmlNode list,
            [<Optional; DefaultParameterValue(null)>] ?SigmaJSRef: JSlibReference
        ) =
        (fun (displayOpts: DisplayOptions) ->

            AdditionalHeadTags |> DynObj.setValueOpt displayOpts "AdditionalHeadTags"
            Description |> DynObj.setValueOpt displayOpts "Description"
            SigmaJSRef |> DynObj.setValueOpt displayOpts "SigmaJSRef"

            displayOpts)

    /// <summary>
    /// Returns a DisplayOptions Object with the cdn set to Globals.SIGMAJS_VERSION and Globals.GRAPHOLOGY_VERSION
    /// </summary>
    static member initCDNOnly() =
        DisplayOptions()
        |> DisplayOptions.style (
            SigmaJSRef               = CDN (InternalUtils.getUriJS())
        )

    /// <summary>
    /// Returns a DisplayOptions Object with the cdn set to Globals.SIGMAJS_VERSION and additional head tags 
    /// </summary>
    static member initDefault() =
        DisplayOptions.init (
            SigmaJSRef = CDN (InternalUtils.getUriJS ()),
            AdditionalHeadTags =
                [
                    title [] [ str "sigmaNET Datavisualization" ]
                    meta [ _charset "UTF-8" ]
                    meta
                        [
                            _name "description"
                            _content "A sigma.js graph generated with sigmaNET"
                        ]
                    link
                        [
                            _id "favicon"
                            _rel "shortcut icon"
                            _type "image/png"
                            _href $"data:image/png;base64,{Globals.LOGO_BASE64}"
                        ]
                ]
        )

    static member setAdditionalHeadTags(additionalHeadTags: XmlNode list) =
        (fun (displayOpts: DisplayOptions) ->
            additionalHeadTags |> DynObj.setValue displayOpts "AdditionalHeadTags"
            displayOpts)

    static member tryGetAdditionalHeadTags(displayOpts: DisplayOptions) =
        displayOpts.TryGetTypedValue<XmlNode list>("AdditionalHeadTags")

    static member getAdditionalHeadTags(displayOpts: DisplayOptions) =
        displayOpts |> DisplayOptions.tryGetAdditionalHeadTags |> Option.defaultValue []

    static member addAdditionalHeadTags(additionalHeadTags: XmlNode list) =
        (fun (displayOpts: DisplayOptions) ->
            displayOpts
            |> DisplayOptions.setAdditionalHeadTags (
                List.append (DisplayOptions.getAdditionalHeadTags displayOpts) additionalHeadTags
            ))

    static member setDescription(description: XmlNode list) =
        (fun (displayOpts: DisplayOptions) ->
            description |> DynObj.setValue displayOpts "Description"
            displayOpts)

    static member tryGetDescription(displayOpts: DisplayOptions) =
        displayOpts.TryGetTypedValue<XmlNode list>("Description")

    static member getDescription(displayOpts: DisplayOptions) =
        displayOpts |> DisplayOptions.tryGetDescription |> Option.defaultValue []

    static member addDescription(description: XmlNode list) =
        (fun (displayOpts: DisplayOptions) ->
            displayOpts
            |> DisplayOptions.setDescription (List.append (DisplayOptions.getDescription displayOpts) description))

    static member setSigmaReference(sigmaJSReference: JSlibReference) =
        (fun (displayOpts: DisplayOptions) ->
            sigmaJSReference |> DynObj.setValue displayOpts "SigmaJSRef"
            displayOpts)

    static member tryGetSigmaReference(displayOpts: DisplayOptions) =
        displayOpts.TryGetTypedValue<JSlibReference>("SigmaJSRef")


    static member getSigmaReference(displayOpts: DisplayOptions) =
        displayOpts |> DisplayOptions.tryGetSigmaReference |> Option.defaultValue (JSlibReference.NoReference)

  
