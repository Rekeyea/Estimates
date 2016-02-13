namespace Estimate.Types

type public LineOfCodeEstimate = 
    {
        ProjectName:string;
        Lines:int;
        EstimateDuration:decimal;
    }
