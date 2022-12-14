module Tetris

type Color =
    | Yellow
    | Blue
    | Cyan
    | Orange
    | Red
    | Green
    | Purple

type board (w:int, h: int) =
    let _board = Array2D.create w h None
    do _board .[0 ,1] <- Some Green
    member this.width = w
    member this.height = h
    member this.board with get() = _board
    type state = board


//work in progress
let draw (w:int) (h:int) (b:board) : Canvas.canvas =
    let C = Canvas.create w h
    let PieceWidth = w / b.width
    let PieceHight = h / b.height
    Array2D.iter () (b.board())

    for column in [0..(b.width-1)] do
        for row in [0..(b.height-1)] do
            match ((b.board())[column][row]) with
            | None -> ignore
            | Some x ->
                Canvas.setFillBox C x (PieceWidth*row) (PieceHight*column)

let b = board(10, 20)
let C = draw 300 600 b
show C "testing"