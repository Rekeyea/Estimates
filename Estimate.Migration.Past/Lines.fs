namespace Estimate.Migration.Past

open System.Text.RegularExpressions
open System.IO
open System.Linq

module Lines = 
    
    let rec private deleteEmptyLines (text:string) = 
        let rLines = "\r\n((\r\n)+)(.*)"
        let mat = Regex.Match(text, rLines)
        if Regex.IsMatch(text,rLines) then deleteEmptyLines (Regex.Replace(text,rLines,"\r\n"+mat.Groups.[3].Value)) else text
        
    let private deleteTextComment (commentRegex:string)(text:string) =
        Regex.Replace(text, commentRegex, "")
        |>deleteEmptyLines 
        
    let rec private deleteTextComments (comments:string list)(text:string)=
        match comments with
            |[] -> text
            | head :: tail -> deleteTextComments tail (deleteTextComment head text) 

    let readLines (comments:string list)(file:string)=
        file
        |>File.ReadAllText
        |>(deleteTextComments comments)
        |>(fun x -> if x.Last().Equals('\n') then x.Split('\n').Length else x.Split('\n').Length+1)
        |>decimal



