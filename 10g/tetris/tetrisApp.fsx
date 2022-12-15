open Tetris

let b:Tetris.board = new board(10, 20)
b.setActivePiece(b.newPiece().Value)
b.put b.activePiece
//b.pieces <- (new Z())


Canvas.runApp "Tetris" 300 600 draw react (board (10, 20))

