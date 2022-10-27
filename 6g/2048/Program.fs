module Game



type pos = int*int
type value = Red | Green | Blue | Yellow | Black
type piece = value * pos
type state = piece list

let fromValue (v:value) : Canvas.color=
    match v with
    | Red -> {r=255uy g=0uy b=0uy a=0uy}
    | Green -> {r=0uy g=255uy b=0uy a=0uy}
    | Blue -> {r=0uy g=0uy b=255uy a=0uy}
    | Yellow -> {r=255uy g=255uy b=0uy a=0uy}
    | Black -> {r=0uy g=0uy b=uy a=0uy}

let nextColor (v:value) : value =
    let colorDict = dict[Red, 2; Green, 4; Blue, 8; Yellow, 16; Black, 32]
    //ideen er at finde værdien til value, at gange med 2 og så finde farven til værdien

let filter (n:int) (s:state) : state =
    let rec elemSearch(n:int) (s:state) =
        match s with
        | ele::rest ->
            match ((snd (snd ele))=n) with
            | true -> [ele] @ (elemSearch n rest)
            | _ -> elemSearch n rest
        | _ -> []
    elemSearch n s

let shiftUp (s:state) : state =
    //find empty positioner i hver kolonne og ryk hvert brik med det antal

let flipUD (s:state) : state =
    //(i,j) -> (2 -i,j), e.g.

let transpose (s:state) : state =




// val empty : state -> pos list
// val addRandom : value -> state -> state option