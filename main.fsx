#r "nuget:DIKU.Canvas, 1.0"

open Canvas

type vec = float * float






let vecSum (x1:float, y1:float) (x2:float, y2:float) : vec = 
    (x1 + x2, y1 + y2)

/// <summary>
/// Given parameters f, and v solve for fv
/// when v=(x,y) then ouput (f*x,f*y)
/// </summary>
/// <param name="f">coefficient</param>
/// <param name="v">vector</param>
/// <returns>New vector f*v.</returns>
let vecMull (f:float) (v1:float, v2:float): vec =
    (v1*f,v2*f)

/// <summary>
/// Given parameters r:float, and v:vec rotate v by f radians
/// </summary>
/// <param name="r">radians clockwise</param>
/// <param name="v">vector</param>
/// <returns>New vector rotated r radians clockwise.</returns>
let vecRotate (r:float) (v1,v2) : vec =
    (v1*cos(r)-v2*sin(r),v1*sin(r)+v2*cos(r))

let toInt ((v1:float, v2:float):vec) : int*int =