#r "nuget:DIKU.Canvas, 1.0"
#load "tetris.fs"
open Tetris



// først tester vi
let blackTest = []
let testBoard:Tetris.board = new board(10, 20)
let i = testBoard.newPiece()
let s = S()
printfn "%A" <| s

do s.rotateRight()
printfn "Testing rotating a piece to the right... %A" 
    <| if s.image = array2D [[true;false]; [true;true];[false;true]] then "passed" else "failed"

do s.rotateLeft()
printfn "Testing rotating a piece to the left... %A" 
    <| if s.image = array2D [[false; true; true]; [true;true; false]] then "passed" else "failed"

do testBoard.setActivePiece(s)
printfn "Testing setting active piece... %A" 
    <| if testBoard.activePiece = s then "passed" else "failed"

// do testBoard.put testBoard.activePiece
// printfn "Testing placing piece on canvas... %A" 
//     <| if Array2D.iter () then "passed" else "failed"
// -------------------------------------------

// White box tests:

let testToCanvasColor() : string=
    let yellowColor = toCanvasColor Yellow
    let blueColor = toCanvasColor Blue
    let cyanColor = toCanvasColor Cyan
    let orangeColor = toCanvasColor Orange
    let redColor = toCanvasColor Red
    let greenColor = toCanvasColor Green
    let purpleColor = toCanvasColor Purple

    // Assert that the correct color is returned for each input
    assert(yellowColor = Canvas.yellow)
    assert(blueColor = Canvas.blue)
    assert(cyanColor = Canvas.fromRgb(0,255,255))
    assert(orangeColor = Canvas.fromRgb(255,165,0))
    assert(redColor = Canvas.red)
    assert(greenColor = Canvas.green)
    assert(purpleColor = Canvas.fromRgb(128,0,128))
    "passed"


printfn "Testing converting to canvas color... %A" <| testToCanvasColor()

let testRandomNumber() =
    let random = randomNumber()
    // Assert that the returned value is between 0 and 7
    assert(random >= 0 && random <= 7)
    "passed"

printfn "Testing random number generation... %A" <| testRandomNumber()

// -------------------------------------------



let b:Tetris.board = new board(10, 20)
b.setActivePiece(b.newPiece().Value)
b.put b.activePiece
//b.pieces <- (new Z())

try
    Canvas.runApp "Tetris" 300 600 draw react (board (10, 20))
with
    | Failure(msg) when msg = "---------------Game Over---------------" -> printfn "%s" msg
    | exn ->  printfn "%A" exn
    //| e -> printfn "%A" e
