open Tetris

let b:Tetris.board = new board(10, 20)
let C = draw 300 600 b
Canvas.show C "testing"