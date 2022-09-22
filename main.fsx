#r "nuget:DIKU.Canvas, 1.0"

open Canvas

type vec = float * float

let vecSum (x1:float, y1:float) (x2:float, y2:float) : vec = 
    (x1 + x2, y1 + y2)

/// <summary>
/// given two vectors, return their sum
/// </summary>
/// <param name=(x1,y1)> first vector </param>
/// <param name=(x2,y2)> second vector </param>
/// <returns> sum of the two vectors </returns>
let vecSum (x1:float, y1:float) (x2:float, y2:float) = 
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

/// <summary>
/// Given parameters v, of float*float
/// returns new tuple of type int*int
/// </summary>
/// <param name="v">vector</param>
/// <returns>New tuple in int.</returns>
let toInt (v1:float, v2:float):vec : int*int =
  (int (System.Math.Round(v1)), int (System.Math.Round(v2)))


let setVector (c:canvas) (Col) (vec:vec) (pos:vec) =
  let start = toInt pos
  let newFloatVector = vecSum vec pos
  let destination = toInt(newFloatVector)
  do setLine c black start destination

let draw (height:int) (width:int) =
  let C = create (width:int) (height:int)
  let v = (200.0,0.0)
  let c = (float(height/2), float(width/2))
  for i in 1.0 .. 36.0 do
    do setVector C black (vecRotate (i*2.0*System.Math.PI/36.0) v) c
  C
let Can = draw 600 600
show Can "hejsa"

