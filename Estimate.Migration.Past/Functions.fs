namespace Estimate.Migration.Past

open System
open System.IO
open System.Linq
open Estimate.Types
open System.Text.RegularExpressions
open Lines
open Weight


module Functions =

    let rec private calculateInFolder (folderName:string)(extensions:string list)(mapper: string -> decimal) = 
        let currentDirCalc = 
            Directory.EnumerateFiles(folderName)
            |>Seq.where(fun x -> extensions.Contains(Path.GetExtension(x)))
            |>Seq.map mapper
            |>Seq.sum
        let childDirCalc = 
            folderName
            |>Directory.EnumerateDirectories
            |>Seq.map(fun x -> calculateInFolder x extensions mapper)
            |>Seq.sum
        currentDirCalc+childDirCalc
        
        
    let rec private linesInFolder (comments: string list) folderName (extensions: string list)  =
        calculateInFolder folderName extensions (readLines comments)

    let rec private weightInFolder folderName (extensions: string list) =
        calculateInFolder folderName extensions weightFile

    let private estimateProjectsInFolder (directory:string) (baseValue:decimal) (baseHours:decimal) (extensions: string list) (calculate:string->string list->decimal) =
        directory
        |>Directory.EnumerateDirectories
        |>Seq.map(fun x -> (Path.GetFileName(x),calculate x extensions))
        |>Seq.map(fun (x,y) -> { ProjectName=x; EstimateValue=y;EstimateDuration=(decimal)y*baseHours/baseValue } )

    let estimateProjectInFolderByLine(directory:string) (baseValue:decimal) (baseHours:decimal) (extensions: string list) (comments: string list)  =
        estimateProjectsInFolder directory baseValue baseHours extensions (linesInFolder comments)

    let estimateProjectInFolderByWeight(directory:string) (baseValue:decimal) (baseHours:decimal) (extensions: string list)=
        estimateProjectsInFolder directory baseValue baseHours extensions weightInFolder

    
