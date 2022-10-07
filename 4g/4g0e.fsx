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

/// <summary>
/// given a source and a destination, returns
/// a list of candidate next positions that brings
/// robot closer to destination
/// </summary>
/// <param name=scs> source point </param>
/// <param name=tg> destination point </param>
/// <returns> list of adjacent positions that are closer to destination </returns>
let candidates (src:pos) (tg:pos) : (pos list) =
    let x, y = src
    let adjacent =[(x,y-1); (x+1,y); (x, y+1); (x-1,y)]
    let adjacentCandidates : pos list = List.filter (fun i -> (dist i tg) < (dist src tg)) adjacent // all relevant positions
    let candidateSum : pos = List.fold (fun (a,b) (c,d) -> (a+c-x,b+d-y) ) (0,0) adjacentCandidates // sum of move commands

    if  fst candidateSum <> 0 && snd candidateSum <> 0 then
        let diagonalPosition = (x + fst candidateSum, y + snd candidateSum) // add src to candidateSum to get diagonal position
        diagonalPosition::adjacentCandidates
    else
        adjacentCandidates
//printfn "candidatesDiag: %A" (candidates (0,0) (2,4))

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

/// <summary>
/// Creates a canvas and draws an appropriate grid with lines
/// representing the routes.
/// </summary>
/// <param name=canvasSize> tuple representing size of canvas in pixels (x,y) </param>
/// <param name=start> starting point for routes </param>
/// <param name=target> destination point </param>
/// <returns> Returns unit, but draws canvas </returns>
let drawRoutes (canvasSize:pos) (start:pos) (target:pos) =
    
    let (w, h) = canvasSize
    let C = create w h
    let drawableX = w*2/3
    let drawableY = h*2/3
    let firstX = w / 2 - drawableX / 2
    let firstY = h / 2 - drawableY / 2

    let xPoints = max (fst start) (fst target) + 1
    let yPoints = max (snd start) (snd target) + 1
    let xResolution = drawableX / xPoints
    let yResolution = drawableY / yPoints

    let xCoords = List.map (fun e -> e * xResolution + firstX) [0..xPoints]
    let yCoords = List.map (fun e -> h - (e * yResolution + firstY)) [0..yPoints]
    let coords = List.map (fun a -> List.map ( fun b -> (a,b)) yCoords) xCoords

/// <summary>
/// Draws a circle on canvas
/// </summary>
/// <param name=angle> 
/// where on circle to start drawing
/// (dummy value used for recursion). Input 0.0 for normal circle 
/// </param>
/// <param name=point> center of circle </param>
/// <param name=rad> radius of circle </param>
/// <returns> Returns unit, but draws a circle </returns>
    let rec drawCircle (angle:float) (point:(int*int)) (rad:float) =
        let xZero = float (fst point)
        let yZero = float (snd point)
        let nextangle: float = angle + 0.1
        if nextangle < (2.0 * System.Math.PI) then
            let a: int = int  (xZero + rad * cos angle)
            let b: int = int (yZero + rad * sin angle)
            let c: int = int (xZero + rad * cos nextangle)
            let d: int = int (yZero + rad * sin nextangle)
            setLine C black (a, b) (c, d)
            drawCircle (nextangle) point rad
        else // tegn det sidste stykke for at slutte cirkelen (da vi itererer funktionen med rationelle tal (0.1))
            let a: int = int (xZero + rad * cos angle)
            let b: int = int (yZero + rad * sin angle)
            let c: int = int (xZero + rad * cos (2.0 * System.Math.PI))
            let d: int = int (yZero + rad * sin (2.0 * System.Math.PI))
            setLine C black (a, b) (c, d)

    /// <summary>
    /// Draws routes on canvas
    /// </summary>
    /// <param name=theRoute> 
    /// A pos list lists, representing routes consisting of positions
    /// </param>
    /// <returns> Returns unit, but draws a collection of routes </returns>
    let drawLines theRoute =
        let drawLine acc elm = 
            setLine C black acc elm
            elm

        for (i:pos list) in theRoute do
            List.fold drawLine i.Head i |> ignore

    // Draw an appropriate amount of circles onto canvas 
    List.map (fun a -> List.map (fun b -> drawCircle 0.0 b 4.) a ) coords |> ignore
    // Draw routes onto canvas
    let theRoute = routes start target
    let actualRoute = List.map (fun a -> List.map (fun b -> coords[fst b][snd b] ) a )  theRoute
    drawLines actualRoute

    do show C "Routes"


drawRoutes (600, 400) (3,4) (1,1)


