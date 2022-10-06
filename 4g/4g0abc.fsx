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
//printfn "%A" (dist (0,0) (2,2))

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
    List.filter (fun i -> (dist i tg) < (dist src tg)) adjacent
//printfn "%A" (candidates (0,0) (2,2))

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
            /// <summary>
            /// Given a list of paths, find the
            /// shortest length of paths
            /// and return those of the same length
            /// </summary>
            /// <param name=scs> source point </param>
            /// <param name=tg> destination point </param>
            /// <returns> list of lists with shortest paths </returns>
            let shortestLists (allPaths:pos list list) =
                let minimumLength = List.min (List.map (fun l -> List.length l) allPaths)
                List.filter (fun list -> List.length list = minimumLength) allPaths
                

            //candidates src tg
            candidates src tg
            |> List.map (fun e -> routes e tg)
            |> List.concat
            |> List.map (fun e -> src::e)

printfn "%A" (routes (1,1) (3,3))

