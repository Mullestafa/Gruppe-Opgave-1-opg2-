#r "nuget:DIKU.Canvas, 1.0"

open Canvas

type vec = float * float

let vecMull (f:float, (v1:float, v2:float):vec) : vec =
    (v1*f,v2*f)

let vecRotate (r:float, (v1,v2):vec) : vec =
    (v1*cos(r)-v2*sin(r),v1*sin(r)+v2*cos(r))
printf "%A" (vecRotate((System.Math.PI),(3.0,0.0)))