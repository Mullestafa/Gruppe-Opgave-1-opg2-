module Game
open System


type pos = int*int
type value = Red | Green | Blue | Yellow | Black
type piece = value * pos
type state = piece list

let fromValue (v:value) : Canvas.color=
    match v with
    | Red -> Canvas.red
    | Green -> Canvas.green
    | Blue -> Canvas.blue
    | Yellow -> Canvas.yellow
    | Black -> Canvas.black


let nextColor (v:value) : value =
    match v with
    | Red -> Green
    | Green -> Blue
    | Blue -> Yellow
    | Yellow -> Black
    | Black -> Black
    //let colorDict = dict[Red, 2; Green, 4; Blue, 8; Yellow, 16; Black, 32]
    //ideen er at finde værdien til value, at gange med 2 og så finde farven til værdien

let rec filter (n:int) (s:state) : state =
    match s with
        | ele::rest ->
            let (_,(x,_)) = ele
            match (x=n) with
            | true -> [ele] @ (filter n rest)
            | _ -> filter n rest
        | _ -> []


let rec shiftUp (s:state) : state =
    // I think this is done.

    // This function should shift as far as possible UP
    // And merge when necessary
    // here is some pseoudocode
    (*
    for each column (using filterfunction):
        sort by row
        for every tile:
            if color matches with adjecent tile:
                replace the two tiles with one tile of (nextColor,(this tile position))
        len = length of list
        move tiles to positions 0 to len
    *)
    let rec mergeTiles (s:state) : state =
        match s with
            | (col1,pos1)::(col2,pos2)::rest when col1=col2 ->
                let col3 = nextColor col1
                (col3,pos1)::(mergeTiles rest)
            | _ -> s
    let rec shoveTiles (n:int) (s:state)  : state =
        match s with
            |(col,(x,y))::rest -> (col,(x,n))::(shoveTiles (n+1) rest)
            |_ -> []

    let rec buildList (column:int) (s:state) : state =
        match column with
            n when n > 2 ->
                []
            | _ -> 
                let thisColumn = s  |> filter column 
                                    |> List.sortBy (fun (_,(_,y)) -> y) 
                                    |> mergeTiles 
                                    |> shoveTiles 0
                thisColumn @ buildList (column+1) s
    buildList 0 s 



let flipUD (s:state) : state = 
    List.map (fun (col,(x,y)) -> (col,(x,2-y))) s
    //(i,j) -> (2 -i,j), e.g.

let rec transpose (s:state) : state =
    List.map (fun (col,(x,y)) -> (col,(y,x))) s
    // Håber det er ok at jeg bare kopierede flipUD funktionen herned. Det er simplere
    (*
    let rec loopThroughState (s:state) : state =
        match s with
        | (value,(x,y))::rest -> [(value,(y,x))] @ (loopThroughState rest)
        | [] -> []
    loopThroughState s
    *)



let empty (s:state) : (pos list) = 
    let posList = List.map (fun i -> snd i) s
    let refList = List.concat (List.map (fun i -> List.map (fun j -> (i,j)) [0..2]) [0..2])
    List.except posList refList

let addRandom (color:value) (s:state) : state option =
    let emptySlots = empty s
    if emptySlots.Length > 0 then
        printfn "emptySlots = %A" emptySlots
        let rnd = Random().Next(0, emptySlots.Length)
        printfn "rnd = %A" rnd
        Some ([(color, emptySlots[rnd])] @ s)
    else None



// UNITTEST WOOOO
let unitTest ((testList:state), (expShiftUptest:state), (expFlipUDtest:state), (expTransposetest:state), (expEmptytest:pos list)) (name:string) : bool = 
    let testList = testList
    let testSet = Set.ofList testList

    let shiftUptest= (testList |> shiftUp |> Set.ofList = (Set.ofList expShiftUptest))
    printfn "shiftUp delivers expected results: %A" shiftUptest

    let flipUDtest= (testList |> flipUD |> Set.ofList = (Set.ofList expFlipUDtest))
    printfn "flipUD delivers expected results: %A" flipUDtest

    let transposeTest= (testList |> transpose |> Set.ofList = (Set.ofList expTransposetest))
    printfn "transpose delivers expected results: %A" transposeTest

    let emptyTest= (testList |> empty |> Set.ofList = (Set.ofList expEmptytest))
    printfn "empty delivers expected results: %A" emptyTest
    let allGood = (shiftUptest=flipUDtest=transposeTest=emptyTest) 
    allGood |> printfn "All tests from %s delivered as expected:... %b" name
    printfn ""
    allGood

let testSetA = 
    ([(Red ,(1,0)); (Red ,(1,1)); (Red ,(1,2)); (Blue ,(0,1)); (Green ,(2,0))], // The starting board
    [(Green ,(1,0)); (Red ,(1,1)); (Blue ,(0,0)); (Green ,(2,0))],              // Expected result from shiftUp
    [(Red ,(1,0)); (Red ,(1,1)); (Red ,(1,2)); (Blue ,(0,1)); (Green ,(2,2))],  // Expected result from flipUD
    [(Red ,(0,1)); (Red ,(1,1)); (Red ,(2,1)); (Blue ,(1,0)); (Green ,(0,2))],  // Expected result from transpose
    [(0,0);(0,2);(2,1);(2,2)])                                                  // Expected result from empty

let testSetEmpty = 
    ([],                                                        // The starting board
    [],                                                         // Expected result from shiftUp
    [],                                                         // Expected result from flipUD
    [],                                                         // Expected result from transpose
    [(0,0);(0,1);(0,2);(1,0);(1,1);(1,2);(2,0);(2,1);(2,2)])    // Expected result from empty

let a = unitTest testSetA "testSetA"
let b = unitTest testSetEmpty "Empty set"
printfn "ALL SETS COMPLETED SUCCESSFULLY:.. %b" <| (a=b)
(*
let out = fromValue (nextColor Red)
let fi = filter 0 testList
let shu = shiftUp testList


let flipped = flipUD testList
printfn "liste er %A" testList
printfn "flippedlist er %A" flipped


*)
//addRandom Yellow testList |> printfn "%A"