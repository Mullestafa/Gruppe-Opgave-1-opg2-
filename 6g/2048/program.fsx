open Game

let draw (height:int) (width:int) (s:state) =
    let C = Canvas.create (width:int) (height:int)
    let pieceSize = height / 3
    for i in s do
        let fstCoordinate = snd i |> fun (x,y)->(x*pieceSize,y*pieceSize)
        let sndCoordinate = fstCoordinate |> fun (x,y)-> (x+pieceSize,y+pieceSize)
        Canvas.setFillBox C (fromValue (fst i)) fstCoordinate sndCoordinate
    C


let react (s:state) (k:Canvas.key) : (state option) =
    let state = addRandom Game.Red s // |> fun (Some x) -> x
    state

let windowSize = 600

do Canvas.runApp "2048" windowSize windowSize draw react (addRandom Red []|> fun (Some x) -> x)