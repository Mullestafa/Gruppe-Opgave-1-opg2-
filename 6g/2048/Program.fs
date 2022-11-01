﻿module Game



type pos = int*int
type value = Red | Green | Blue | Yellow | Black | Empty
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

(*

let flipUD (s:state) : state =
    //(i,j) -> (2 -i,j), e.g.

let transpose (s:state) : state =

*)


// val empty : state -> pos list
// val addRandom : value -> state -> state option
//let hejsa = Canvas.Item("r")
let testlist = [(Red ,(1,0)); (Red ,(1,1)); (Blue ,(0,2)); (Black ,(2,1)); (Red ,(2,0)); (Red ,(0,1)); (Red ,(1,2))]
let out = fromValue (nextColor Red)
let fi = filter 0 testlist
let shu = shiftUp testlist

printfn "%A" shu


