open Tetris

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
