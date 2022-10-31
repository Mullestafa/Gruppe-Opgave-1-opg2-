module Game



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
            let (_,(_,y)) = ele
            match (y=n) with
            | true -> [ele] @ (filter n rest)
            | _ -> filter n rest
        | _ -> []







let shiftUp (s:state) : state =
    //find empty positioner i hver kolonne og ryk hvert brik med det antal
(*

let flipUD (s:state) : state =
    //(i,j) -> (2 -i,j), e.g.

let transpose (s:state) : state =

*)


// val empty : state -> pos list
// val addRandom : value -> state -> state option
//let hejsa = Canvas.Item("r")
let out = fromValue (nextColor Red)
let fi = filter 0 [(Red ,(1,0)); (Red ,(1,1))]
printfn "%A" fi


