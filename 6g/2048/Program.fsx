open Game

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
        let newstate = s |> transpose |> shiftUp |> transpose
        newTileIfMoved Red newstate s

    | Canvas.RightArrow ->
        let newstate = s |> transpose |> flipUD |> shiftUp |> flipUD |> transpose
        newTileIfMoved Red newstate s

    | Canvas.UpArrow ->
        let newstate = s |> shiftUp
        newTileIfMoved Red newstate s

    | Canvas.DownArrow ->
        let newstate = s |> flipUD |> shiftUp |> flipUD
        newTileIfMoved Red newstate s
    | _ -> None



do Canvas.runApp "2048" 600 600 draw react ([] |> recklessAdd Red |> recklessAdd Blue)
