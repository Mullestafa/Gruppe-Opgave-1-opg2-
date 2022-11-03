open Game

let testSetA = 
    ([(Red ,(1,0)); (Red ,(1,1)); (Red ,(1,2)); (Blue ,(0,1)); (Green ,(2,0))], // The starting board
    [(Green ,(1,0)); (Red ,(1,1)); (Blue ,(0,0)); (Green ,(2,0))],              // Expected result from shiftUp
    [(Red ,(1,0)); (Red ,(1,1)); (Red ,(1,2)); (Blue ,(0,1)); (Green ,(2,2))],  // Expected result from flipUD
    [(Red ,(0,1)); (Red ,(1,1)); (Red ,(2,1)); (Blue ,(1,0)); (Green ,(0,2))],  // Expected result from transpose
    [(0,0);(0,2);(2,1);(2,2)],                                                  // Expected result from empty
    true,                                                                     // Expected result from addRandom. true means new tile was created
    Some Canvas.green)
let testSetEmpty = 
    ([],                                                        // The starting board
    [],                                                         // Expected result from shiftUp
    [],                                                         // Expected result from flipUD
    [],                                                         // Expected result from transpose
    [(0,0);(0,1);(0,2);(1,0);(1,1);(1,2);(2,0);(2,1);(2,2)],    // Expected result from empty
    true,
    None)

let testSetFull =
    ([(Red ,(0,0)); (Yellow, (1,0)); (Green, (2,0)); (Green, (0,1)); (Black, (1,1)); (Red, (2,1)); (Blue, (0,2)); (Black, (1,2)); (Red, (2,2))],
    [(Red ,(0,0)); (Yellow, (1,0)); (Green, (2,0)); (Green, (0,1)); (Black, (1,1)); (Green, (2,1)); (Blue, (0,2))],
    [(Red ,(0,2)); (Yellow, (1,2)); (Green, (2,2)); (Green, (0,1)); (Black, (1,1)); (Red, (2,1)); (Blue, (0,0)); (Black, (1,0)); (Red, (2,0))],
    [(Red ,(0,0)); (Yellow, (0,1)); (Green, (0,2)); (Green, (1,0)); (Black, (1,1)); (Red, (1,2)); (Blue, (2,0)); (Black, (2,1)); (Red, (2,2))],
    [],
    false,
    Some Canvas.green)

let a = unitTest testSetA "testSetA"
let b = unitTest testSetEmpty "Empty set"
let c = unitTest testSetFull "Full set"
printfn "ALL SETS COMPLETED SUCCESSFULLY:.. %b" <| (a=true&&b=true&&c=true)


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
