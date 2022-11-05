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
        | s when s.Length = 1 -> s 
        | elem::rest -> if (fst elem) = (fst rest.Head) then 
                            [(nextColor (fst elem),snd elem)] 
                                @ rest.Tail else [elem] @ (mergeTiles rest)
        | [] -> []

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



let empty (s:state) : (pos list) = 
    let posList = List.map (fun i -> snd i) s
    let refList = List.concat (List.map (fun i -> List.map (fun j -> (i,j)) [0..2]) [0..2])
    List.except posList refList

let addRandom (color:value) (s:state) : state option =
    let emptySlots = empty s
    if emptySlots.Length > 0 then
        let rnd = Random().Next(0, emptySlots.Length)
        Some ([(color, emptySlots[rnd])] @ s)
    else None

let recklessAdd (color:value) (s:state) = addRandom color s |> Option.get

let newTileIfMoved (col:value) (newState:state) (oldState:state) : state option =
    if (Set.ofList newState) = (Set.ofList oldState) then
        None
    else
        addRandom col newState

// UNITTEST WOOOO
let unitTest ((testList:state), (expShiftUptest:state), (expFlipUDtest:state), (expTransposetest:state), (expEmptytest:pos list), (expAddRandom:bool), (expNextColorValue:Canvas.color option)) (name:string) : bool = 
//    let testList = testList
    let testSet = Set.ofList testList

    let shiftUptest= (testList |> shiftUp |> Set.ofList = (Set.ofList expShiftUptest))
    printfn "shiftUp delivers expected results: %A" shiftUptest

    let flipUDtest= (testList |> flipUD |> Set.ofList = (Set.ofList expFlipUDtest))
    printfn "flipUD delivers expected results: %A" flipUDtest

    let transposeTest= (testList |> transpose |> Set.ofList = (Set.ofList expTransposetest))
    printfn "transpose delivers expected results: %A" transposeTest

    let emptyTest= (testList |> empty |> Set.ofList = (Set.ofList expEmptytest))
    printfn "empty delivers expected results: %A" emptyTest

    let addRandomTest : bool =
        let oneAddedOpt = testList |> addRandom Red
        match oneAddedOpt with 
            | None -> if expAddRandom = false then true else false
            | Some oneAdded ->
                let oneAddedSet = oneAdded |> Set.ofList
                let subs : bool = Set.isSubset testSet oneAddedSet // should return true when all elements in testset are in newset
                let union : bool = (Set.union testSet oneAddedSet) = oneAddedSet
                let added : bool = (Set.count oneAddedSet - Set.count testSet) = 1 //returns true when there is only one tile added
                (subs=true && union=true && added=true && expAddRandom=true) // function returns true when all subtests succeeded
    printfn "addRandom delivers expected results: %A" addRandomTest

    let nextColorValueTest : bool =
        let sortedlist = testList |> List.sortBy (fun (_,(x,_)) -> x) 
                                    |> List.sortBy (fun (_,(_,y)) -> y)
        match sortedlist with
            | (thiscol,position)::tail -> 
                let colorvalue:Canvas.color = thiscol |> nextColor |> fromValue
                (Some colorvalue = expNextColorValue)
            | _ -> (expNextColorValue = None)



    let allGood = (shiftUptest=true &&
                    flipUDtest=true &&
                    transposeTest=true &&
                    emptyTest=true &&
                    addRandomTest=true &&
                    nextColorValueTest=true) 
    printfn "All tests from %s delivered as expected:... %b" <|| (name, allGood)
    printfn ""
    allGood


