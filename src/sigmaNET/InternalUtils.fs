namespace sigmaNET

module internal InternalUtils =

    open System.Reflection
    open System.IO

    let getFullSigmaJS () =
        let assembly = Assembly.GetExecutingAssembly()
        let resourceName = "sigmaNET.sigma.min.js"
        use stream = assembly.GetManifestResourceStream(resourceName)
        use reader = new StreamReader(stream)
        reader.ReadToEnd()

    let getFullGraphologyJS () =
        let assembly = Assembly.GetExecutingAssembly()
        let resourceName = "graphology.umd.min.js"
        use stream = assembly.GetManifestResourceStream(resourceName)
        use reader = new StreamReader(stream)
        reader.ReadToEnd()
