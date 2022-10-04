type pos = (int*int)

let dist (p1:pos) (p2:pos) : int =
    ((fst p2)-(fst p1))*((fst p2)-(fst p1)) + ((snd p2)-(snd p1))*((snd p2)-(snd p1))
//printfn "%A" (dist (0,0) (2,2))

let candidates (src:pos) (tg:pos) : (pos list) =
    let x, y = src
    let adjecent =[(x,y-1); (x+1,y); (x, y+1); (x-1,y)]
    List.filter (fun i -> (dist i tg) < (dist src tg)) adjecent
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