namespace Estimate.Migration.Past

open System.IO

module Weight = 
    
    let weightFile (file:string)=
        file
        |>FileInfo
        |>fun x -> (decimal x.Length)/(decimal 1024)
        
        



