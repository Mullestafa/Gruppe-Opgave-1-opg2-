#r "nuget:DIKU.Canvas, 1.0"

open Canvas

type vec = float * float

let vecMull (f:float, (v1:float, v2:float):vec) : vec =
    (v1*f,v2*f)

printf "%A" (vecMull(2,(3,4)))
