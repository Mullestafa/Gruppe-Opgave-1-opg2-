open Game

let draw (h:int) (w:int) (s:state) =
    let C = Canvas.create (w:int) (h:int)
    let pieceSize = h / 3
    for i in s do
        let fstCoordinate = snd i |> fun (x,y)->(x*pieceSize,y*pieceSize)
        let sndCoordinate = fstCoordinate |> fun (x,y)-> (x+pieceSize,y+pieceSize)
        Canvas.setFillBox C (fromValue (fst i)) fstCoordinate sndCoordinate
    C


let react (s:state) (k:Canvas.key) : (state option) =
    match Canvas.getKey k with
    | Canvas.LeftArrow ->
        let state = transpose s |> shiftUp |> transpose
        let state = addRandom Game.Red state |> fun (Some x) -> x
        Some state
    | Canvas.RightArrow ->
        let state = transpose s |> flipUD |> shiftUp |> flipUD |> transpose
        let state = addRandom Game.Red state |> fun (Some x) -> x
        Some state
    | Canvas.UpArrow ->
        let state = s |> shiftUp
        let state = addRandom Game.Red state |> fun (Some x) -> x
        Some state
    | Canvas.DownArrow ->
        let state = s |> flipUD |> shiftUp |> flipUD
        let state = addRandom Game.Red state |> fun (Some x) -> x
        Some state    
    | _ -> None


do Canvas.runApp "2048" 600 600 draw react (addRandom Blue (addRandom Red []|> fun (Some x) -> x)|> fun (Some x) -> x)