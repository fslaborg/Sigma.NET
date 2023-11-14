namespace sigmaNET

open System

//module internal InternalUtils =
module InternalUtils =

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
        let resourceName = "sigmaNET.graphology.umd.min.js"
        use stream = assembly.GetManifestResourceStream(resourceName)
        use reader = new StreamReader(stream)
        reader.ReadToEnd()
    
    let getFullGraphologyLibraryJS () =
        let assembly = Assembly.GetExecutingAssembly()
        let resourceName = "sigmaNET.graphology-library.min.js"
        use stream = assembly.GetManifestResourceStream(resourceName)
        use reader = new StreamReader(stream)
        reader.ReadToEnd()

    let getFullPathSigmaNETLibraryJS () =
        let home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        $"{home}/.nuget/packages/sigmanet/0.0.0-dev/content/sigmaNET.js"
        