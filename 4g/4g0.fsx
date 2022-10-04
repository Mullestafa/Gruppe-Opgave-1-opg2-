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

let rec routes (src:pos) (tg:pos) : pos list list =
    printfn "%A" src
    match src with
    | (1,1) -> [[tg]]
    | _ ->
        let cand = candidates src tg
        printfn "%A" cand
        List.concat (List.map (fun (i:pos) -> List.map (fun (j:pos) -> [i] @ List.concat (routes j tg)) cand) [src])
//        List.concat (List.map (fun (i:pos) -> (List.map (fun (j:pos) -> [[i] @ (List.concat (routes j tg))]) (cand))) [src])
        


printfn "%A" (routes (3,3) (1,1))
*)
