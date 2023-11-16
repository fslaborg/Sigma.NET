namespace sigmaNET

open System

//module internal InternalUtils =
module InternalUtils =
    
    type JSRefGroup ={
      Sigma         : string
      Graphology    : string
      GraphologyLib : string
    } 

    open System.Reflection
    open System.IO

    let readFromManifestResource (resourceName:string) =
        let assembly = Assembly.GetExecutingAssembly()
        use stream = assembly.GetManifestResourceStream(resourceName)
        use reader = new StreamReader(stream)
        reader.ReadToEnd()

    let getSourceCodeJS () =

        {
            Sigma = readFromManifestResource "sigmaNET.sigma.min.js";
            Graphology = readFromManifestResource "sigmaNET.graphology.umd.min.js";
            GraphologyLib = readFromManifestResource "sigmaNET.graphology-library.min.js"
        }

    let getUriJS () =
        {
            Sigma = $"https://cdnjs.cloudflare.com/ajax/libs/sigma.js/{Globals.SIGMAJS_VERSION}/sigma.min.js";
            Graphology = $"https://cdnjs.cloudflare.com/ajax/libs/graphology/{Globals.GRAPHOLOGY_VERSION}/graphology.umd.min.js";
            GraphologyLib = $"https://cdn.jsdelivr.net/npm/graphology-library@{Globals.GRAPHOLOGY_LIB_VERSION}/dist/graphology-library.min.js" ;
        }

    let getNugetPathJS () =
        //Assembly.GetExecutingAssembly().GetName().Version.ToString()
        let home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Replace("\\", "/")
        {
            Sigma = $"{home}/.nuget/packages/sigmanet/{Globals.NUGET_VERSION}/content/sigma.min.js"
            Graphology = $"{home}/.nuget/packages/sigmanet/{Globals.NUGET_VERSION}/content/graphology.umd.min.js"
            GraphologyLib = $"{home}/.nuget/packages/sigmanet/{Globals.NUGET_VERSION}/content/graphology-library.min.js" ;
        }

        