// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Estimate.Types
open Estimate.Migration.Past
open System

[<EntryPoint>]
let main argv = 
    
    let baseValue = decimal(argv.[0])
    let baseHours = decimal(argv.[1])
    let extensions = List.ofArray (argv.[2].Split(','))
    let comments = List.ofArray(argv.[3].Split(','))
    let func = argv.[4]

    let currentDir = System.IO.Directory.GetCurrentDirectory()
    
    let fileName = System.IO.Path.GetFileName(currentDir)

    let a = 
        match func.ToLower() with
            |"lines"->Some (Functions.estimateProjectInFolderByLine currentDir baseValue baseHours extensions comments)
            |"weights"->Some (Functions.estimateProjectInFolderByWeight currentDir baseValue baseHours extensions)
            | _ -> None
    let aux = 
        match func.ToLower() with
            |"lines"-> (Seq.ofList ["Project Name, Lines of Code, Estimate Duration"])
            |"weights"-> (Seq.ofList ["Project Name, Weight of File (KB), Estimate Duration"])
            | _ -> Seq.ofList []

    let v = 
        match a with
            | Some b -> 
                b
                |>Seq.map(fun x -> String.Format("{0},{1},{2}",x.ProjectName,x.EstimateValue,x.EstimateDuration))
                |>(Seq.append aux)
            | None -> aux

    System.IO.File.WriteAllLines(fileName+".csv",Array.ofSeq(v))
    printfn "%A" v
    let final = Console.ReadLine()
    0 // return an integer exit code 
