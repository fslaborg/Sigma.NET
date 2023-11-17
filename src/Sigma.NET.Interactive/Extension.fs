namespace Sigma.NET.Interactive

open System
open System.Threading.Tasks
open Microsoft.DotNet.Interactive
open Microsoft.DotNet.Interactive.Formatting
open Sigma.NET

type FormatterKernelExtension() =

    interface IKernelExtension with
        member _.OnLoadAsync _ =

            Formatter.Register<SigmaGraph>(
                Action<_, _>(fun graph (writer: IO.TextWriter) -> writer.Write(Formatters.toInteractiveHTML graph)), // use a function here that converts the sigma graph type to the html to show in the notebook (from the `Formatters module`)
                "text/html"
            )

            Task.CompletedTask