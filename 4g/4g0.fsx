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
    let initList = [src]
    List.map (fun i -> (List.map (fun j -> ([j] @ [i])) (candidates (i.Head) tg))) initList


printfn "%A" (routes (3,3) (1,1))