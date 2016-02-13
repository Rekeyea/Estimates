// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Estimate.Types
open Estimate.Migration.Past
open System

[<EntryPoint>]
let main argv = 
    
    let baseLines = int(argv.[0])
    let baseHours = decimal(argv.[1])
    let extensions = List.ofArray (argv.[2].Split(','))
    let comments = List.ofArray(argv.[3].Split(','))
    let currentDir = System.IO.Directory.GetCurrentDirectory()
    let  res = Functions.estimateProjectsInFolder currentDir baseLines baseHours extensions comments

    let u = 
        res
        |>Seq.map(fun x -> String.Format("{0},{1},{2}",x.ProjectName,x.Lines,x.EstimateDuration))

    let aux = Seq.ofList ["Project Name, Lines of Code, Estimate Duration"]
    let v = Seq.append aux u
    
    let fileName = System.IO.Path.GetFileName(currentDir)
    System.IO.File.WriteAllLines(fileName+".csv",Array.ofSeq(v))

    printfn "%A" v
    let final = Console.ReadLine()
    0 // return an integer exit code
