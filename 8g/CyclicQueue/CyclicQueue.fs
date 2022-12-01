module CyclicQueue

type Value = int

let mutable queue : Value option[] = [||] 
let mutable first : int option = None
let mutable last : int option = None
  
let create (n:int) : unit =
    try
        queue <- Array.zeroCreate n
    with _ -> 
        eprintfn "Please pass positive integer as argument"

let iterate i:int option =
    match i with
    Some x when x = (queue.Length - 1) -> Some 0
    | Some (x: int) -> Some (x+1)
    | None -> None

let enqueue (e:Value) : bool =
    let isfull: bool =
        match last with
        Some x when iterate (Some x) = first -> true
        | _ -> false

    match last with
    None -> 
        match queue with
            [||] -> false
            | _ ->
                queue[0] <- Some e
                last <- Some 0
                first <- Some 0
                true
    | _ ->
        if isfull then
            false
        else
            last <- (iterate last)
            queue[last.Value] <- Some e
            true


let dequeue () : Value option =
    match first with
    Some x -> 
        let result = queue[x]
        if first = last then
            last <- None
            first <- None
            result
        else
            first <- (iterate first)
            result
    | None -> None   

let isEmpty () : bool =
    if first = None then
        true
    else
        false


let length () : int =
    match last with
    None -> 0
    | Some x -> 
        if last > first then
            last.Value - first.Value + 1
        else
            (x+1) + (queue.Length-first.Value+1)

let classicQue : Value option[] =
    match last with
    None -> [||]
    | Some x -> 
        if last > first then
            queue[last.Value..first.Value]
        else
            Array.append queue[0..last.Value] queue[first.Value..(queue.Length-1)]

let toString () : string =
    let filteredArr: Value option[] = classicQue
    let stringArr: string[] = Array.map (fun (i:Value option) -> string(i.Value)) filteredArr
    Array.fold (fun (acc: string) (x: string) -> acc + "," + x) "" stringArr
 
