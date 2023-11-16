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

    let getFullPathSigmaJS () =
        //Assembly.GetExecutingAssembly().GetName().Version.ToString()
        let home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Replace("\\", "/")
        $"https://cdnjs.cloudflare.com/ajax/libs/sigma.js/{Globals.SIGMAJS_VERSION}/sigma.min.js"
        // $"{home}/.nuget/packages/sigmanet/0.0.0-dev/content/sigma.min.js"

    let getFullPathGraphologyJS () =
        //Assembly.GetExecutingAssembly().GetName().Version.ToString()
        let home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Replace("\\", "/")
        $"https://cdnjs.cloudflare.com/ajax/libs/graphology/{Globals.SIGMAJS_VERSION}/graphology.esm.min.js"
        // $"{home}/.nuget/packages/sigmanet/0.0.0-dev/content/graphology.umd.min.js"

    let getFullPathGraphology_LibJS () =
        //Assembly.GetExecutingAssembly().GetName().Version.ToString()
        let home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Replace("\\", "/")
        $"https://cdnjs.cloudflare.com/ajax/libs/sigma.js/{Globals.SIGMAJS_VERSION}/sigma.min.js"
        // $"{home}/.nuget/packages/sigmanet/0.0.0-dev/content/graphology-library.min.js"

        