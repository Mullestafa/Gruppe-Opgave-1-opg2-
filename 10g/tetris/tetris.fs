﻿module Tetris

type Color =
    | Yellow
    | Blue
    | Cyan
    | Orange
    | Red
    | Green
    | Purple

type Position = int*int

let toCanvasColor(c:Color) : Canvas.color =
    match c with
    | Yellow -> Canvas.yellow
    | Blue -> Canvas.blue
    | Cyan -> Canvas.fromRgb(0,255,255)
    | Orange -> Canvas.fromRgb(255,165,0)
    | Red -> Canvas.red
    | Green -> Canvas.green
    | Purple -> Canvas.fromRgb(128,0,128)

type board (w:int, h: int) =
    let _board = Array2D.create w h None
    let mutable _score = 0
    do _board.[0 ,1] <- Some Green
    member this.score 
        with get() = _score
        and set(a) = _score <- _score + a
    member this.width = w
    member this.height = h
    member this.board with get() = _board

[<Abstract>]
type Tetromino (a:bool[,], c:Color, o: Position) =
    let mutable _position = o
    let _shape = a
    let _color = c
    let mutable _rotation = 0


    member this.position with get() = _position and set(p) = _position <- p
    member this.shape with get() = _shape
    member this.col with get() = _color
    member this.rotation with get() = _rotation and set(a) = _rotation <- o

    override ToString() =
        sprintf "piece at: %d \nwith shape: %A \nwith color: %A\nwith rotation: %A\n-----------------" this.position this.

    
    abstract member move : uint -> uint
    default this.move() = this.position <- ((fst position), ((snd position)+1))

    type S() =
        let shape = [[false; true; true]
                    [true; true; false]]
        let col = Green
        let position = (0, 0)
        inherit Tetromino (shape, position)

    type Z() =
        let shape = [[true; true; false]
                    [false; true; true]]
        let col = Red
        let position = (0, 0)
        inherit Tetromino (shape, position)
    
    type T() =
        let shape = [[true; true; true]
                    [false; true; false]]
        let col = Purple
        let position = (0, 0)
        inherit Tetromino (shape, position)
    
    type L() =
        let shape = [[true; false]
                    [true; false]
                    [true; true]]
        let col = Orange
        let position = (0, 0)
        inherit Tetromino (shape, position)

    type J() =
        let shape = [[false; true]
                    [false; true]
                    [true; true]]
        let col = Blue
        let position = (0, 0)
        inherit Tetromino (shape, position)

    type O() =
        let shape = [[true; true; false]
                    [true; true; false]]
        let col = Yellow
        let position = (0, 0)
        inherit Tetromino (shape, position)    

    type I() =
        let shape = [true; true; true; true]
        let col = Cyan
        let position = (0, 0)
        inherit Tetromino (shape, position)

let draw (w:int) (h:int) (b:board) : Canvas.canvas =
    let C = Canvas.create w h
    let PieceWidth = w / b.width
    let PieceHight = h / b.height
    //loop through the board and draw a square on some
    Array2D.iteri (fun (i: int) (j: int) (v: Color option) -> match v with None -> None |> ignore | Some (x: Color) -> (Canvas.setFillBox C (toCanvasColor(x)) ((PieceWidth*i),(PieceHight*j)) ((PieceWidth*i+PieceWidth),(PieceHight*j+PieceHight)))) (b.board)

    C

