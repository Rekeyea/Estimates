// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Estimate.Types
open Estimate.Migration.Past
open System

[<EntryPoint>]
let main argv = 
    let  res = Functions.estimateProjectsInFolder (System.IO.Directory.GetCurrentDirectory()) 1 ((decimal)1) [".txt"] [@"\/\*(.|\r\n)*\*\/"]
    printfn "%A" res
    let final = Console.ReadLine()
    0 // return an integer exit code
