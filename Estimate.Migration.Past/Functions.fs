namespace Estimate.Migration.Past

open System
open System.IO
open System.Linq
open Estimate.Types
open System.Text.RegularExpressions

module Functions =


    let rec private deleteEmptyLines (text:string) = 
        let rLines = "\r\n((\r\n)+)(.*)"
        let mat = Regex.Match(text, rLines)
        if Regex.IsMatch(text,rLines) then deleteEmptyLines (Regex.Replace(text,rLines,"\r\n"+mat.Groups.[3].Value)) else text
        
    let private deleteTextComment (text:string) (commentRegex:string)=
        Regex.Replace(text, commentRegex, "")
        |>deleteEmptyLines 
        
    let rec private deleteTextComments (text:string) (comments:string list)=
        match comments with
            |[] -> text
            | head :: tail -> deleteTextComments (deleteTextComment text head) tail
        
    let rec private linesInFolder folderName (extensions: string list) (comments: string list) =
        let currentDirLines = 
            Directory.EnumerateFiles(folderName)
            |>Seq.where(fun x -> extensions.Contains(Path.GetExtension(x)))
            |>Seq.map(fun x-> File.ReadAllText(x))
            |>Seq.map(fun x-> deleteTextComments x comments)
            |>Seq.map(fun x -> if x.Last().Equals('\n') then x.Split('\n').Length else x.Split('\n').Length+1)
            |>Seq.sum
        let childDirLines = 
            folderName
            |>Directory.EnumerateDirectories
            |>Seq.map(fun x -> linesInFolder x extensions comments)
            |>Seq.sum
        currentDirLines+childDirLines

    let estimateProjectsInFolder (directory:string) (baseLines:int) (baseHours:decimal) (extensions: string list) (comments: string list)  =
        directory
        |>Directory.EnumerateDirectories
        |>Seq.map(fun x -> (Path.GetFileName(x),linesInFolder x extensions comments))
        |>Seq.map(fun (x,y) -> { ProjectName=x; Lines=y;EstimateDuration=(decimal)y*baseHours/(decimal)baseLines } )
