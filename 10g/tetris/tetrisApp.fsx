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

//let m : Tetris.Tetromino = new Tetris.S()
//let min = {new S with Tetromino()}
//let a = listOfPiecesTypes[0]
 
//do s.rotateRight ()
//printfn "Testing rotation..."
//printfn "Test %A" <| if s.image = array2D [[true; false]; [true; true]; [false; true]] then "passed" else "failed"













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
