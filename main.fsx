#r "nuget:DIKU.Canvas, 1.0"

open Canvas

type vec = float * float

type state = float

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

/// <summary>
/// Given a c canvas, col color, vec vector, and pos vector
/// defines a line on c wih color col, corrosponding to vec orignaing in pos
/// </summary>
/// <param name="c">Canvas</param>
/// <param name="col">color</param>
/// <param name="vec">vecor</param>
/// <param name="pos">origin</param>
/// <returns>unit</returns>
let setVector (c:canvas) (Col) (vec:vec) (pos:vec) =
  let start = toInt pos
  let tempFloatVector = vecSum vec pos
  let destination = toInt(tempFloatVector)
  do setLine c black start destination

/// <summary>
/// Given parameters heih and width, both of type int
/// returns a canvas with 36 equally spaced spokes drawn in a circle originating from the center
/// </summary>
/// <param name="height">height</param>
/// <param name="width">width</param>
/// <returns>Canvas</returns>
let draw (height:int) (width:int) (s:state) =
  let C = create (width:int) (height:int)
  let v = (300.0,0.0)
  let center= (float(height/2), float(width/2))
  let spokes = 36.0

  let rec drawSpokes (amount:float) =
    match amount with
      | n when 1.0 <= n ->
        setVector C black (vecRotate (s + amount*2.0*System.Math.PI/36.0) v) center
        drawSpokes (amount - 1.0)
      | _ -> ()
  do drawSpokes spokes
  C



let react (s:state) (k:key) : state option =
    let stepsize = 0.01 // How far spokes should rotate for each keypress
    match getKey k with
        | LeftArrow ->
            Some (s - stepsize)
        | RightArrow ->
            Some (s + stepsize)
        |_ -> None


do runApp "hejsa" 600 600 draw react 0.0