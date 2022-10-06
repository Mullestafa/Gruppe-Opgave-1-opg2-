#r "nuget:DIKU.Canvas, 1.0"

open Canvas




type pos = (int*int)

/// <summary>
/// given two positions, returns the distance squared
/// </summary>
/// <param name=p1> source point </param>
/// <param name=(x2,y2)> destination point </param>
/// <returns> squared distance between points </returns>
let dist (p1:pos) (p2:pos) : int =
    let xDelta = (fst p2) - (fst p1)
    let yDelta = (snd p2) - (snd p1)
    xDelta * xDelta +  yDelta * yDelta
printfn "distance: %A" (dist (0,0) (2,4))

/// <summary>
/// given a source and a destination, returns
/// a list of candidate next positions that brings
/// robot closer to destination
/// </summary>
/// <param name=scs> source point </param>
/// <param name=tg> destination point </param>
/// <returns> list of adjacent positions that are closer to destination </returns>
(*
let candidates (src:pos) (tg:pos) : (pos list) =
    let x, y = src
    let adjacent =[(x,y-1); (x+1,y); (x, y+1); (x-1,y)]
    List.filter (fun i -> (dist i tg) < (dist src tg)) adjacent
printfn "%A" (candidates (0,0) (2,4))
*)
let candidates (src:pos) (tg:pos) : (pos list) =
    let x, y = src
    let adjacentElements : pos list = [(1,0);(-1,0);(0,1);(0,-1)]
    let adjacent : pos list =  List.map (fun adj -> (fst adj + x, snd adj + y)) adjacentElements // all positions adjacent to src

    let adjacentCandidates : pos list = List.filter (fun i -> (dist i tg) < (dist src tg)) adjacent // all relevant positions
    let candidateSum : pos = List.fold (fun (a,b) (c,d) -> (a+c-x,b+d-y) ) (0,0) adjacentCandidates // sum of move commands

    if  fst candidateSum <> 0 && snd candidateSum <> 0 then
        let diagonalPosition = (x + fst candidateSum, y + snd candidateSum) // add src to candidateSum to get diagonal position
        diagonalPosition::adjacentCandidates
    else
        adjacentCandidates
printfn "candidatesDiag: %A" (candidates (0,0) (2,4))

/// <summary>
/// given a source and a destination, returns
/// a list of lists containing every shortest path
/// to destination
/// </summary>
/// <param name=scs> source point </param>
/// <param name=tg> destination point </param>
/// <returns> list of lists with shortest paths </returns>
let rec routes (src:pos) (tg:pos) : pos list list =
    match src with
        a when a = tg -> [[src]]
        | _ -> 

            let shortestLists (allPaths:pos list list) =
                let minimumLength = List.min (List.map (fun l -> List.length l) allPaths)
                List.filter (fun list -> List.length list = minimumLength) allPaths
                

            //candidates src tg
            candidates src tg
            |> List.map (fun e -> routes e tg)
            |> List.concat
            |> List.map (fun e -> src::e)
            |> shortestLists 
        
let w = 600
let h = 400  
let C = create w h
let drawableX = w*2/3
let drawableY = h*2/3
let firstX = w / 2 - drawableX / 2
let firstY = h / 2 - drawableY / 2

let start = (0,0)
let target = (3,4)
let xPoints = abs ((fst start) - (fst target))  
let yPoints = abs ((snd start) - (snd target)) 
let xResolution = drawableX / xPoints
let yResolution = drawableY / yPoints

let xCoords = List.map (fun e -> e * xResolution + firstX) [0..xPoints]
let yCoords = List.map (fun e -> e * yResolution + firstY) [0..yPoints]
let coords = List.map (fun a -> List.map ( fun b -> (a,b)) yCoords) xCoords
printfn "%A" coords


let rec drawCircle (angle : float) (point:(int*int)) =
    let xZero = float (fst point)
    let yZero = float (snd point)
    let rad = 4.
    let nextangle: float = angle + 0.1
    if nextangle < (2.0 * System.Math.PI) then
        let a: int = int  (xZero + rad * cos angle)
        let b: int = int (yZero + rad * sin angle)
        let c: int = int (xZero + rad * cos nextangle)
        let d: int = int (yZero + rad * sin nextangle)
        setLine C black (a, b) (c, d)
        drawCircle (nextangle) point
    else // tegn det sidste stykke for at slutte cirkelen (da vi itererer funktionen med rationelle tal (0.1))
        let a: int = int (xZero + rad * cos angle)
        let b: int = int (yZero + rad * sin angle)
        let c: int = int (xZero + rad * cos (2.0 * System.Math.PI))
        let d: int = int (yZero + rad * sin (2.0 * System.Math.PI))
        setLine C black (a, b) (c, d)


List.map (fun a -> List.map (fun b -> drawCircle 0.0 b) a ) coords

let theRoute = routes start target
printfn "%A" theRoute

let drawLines theRoute : unit =
    let drawLine acc elm = 
        setLine C black acc elm
        elm
    
    for (i:pos list) in theRoute do
        List.fold drawLine i.Head i
        |> ignore

let actualRoute = List.map (fun a -> List.map (fun b -> coords[fst b][snd b] ) a )  theRoute

drawLines actualRoute
printfn "%A" actualRoute
do show C "hejsa"

(*
let 
let thePaths = routes (0,0) (3,3)
let buildTurtleCommands =
*)



