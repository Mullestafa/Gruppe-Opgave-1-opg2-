module Tetris

type Color =
    | Yellow
    | Blue
    | Cyan
    | Orange
    | Red
    | Green
    | Purple

val toCanvasColor : c:Color -> Canvas.color

[<Class>]
type board =
    new : int*int -> board
    member width: int
    member height: int
    member board: Color option [,]


val draw : w:int -> h:int -> b:board -> Canvas.canvas

type position = int*int

type tetromino =
    /// The constructor with its initial shape , its final color ,and its inital offset
    new: a: bool[,] * c: Color * o: position -> tetromino
    /// Make a string representation of this piece
    override ToString: unit -> string
    /// Make a deep copy of this piece
    member clone: unit -> tetromino
    /// Rotates the piece 90 degrees clock -wise such that its left -top offset is maintained 
    member rotateRight: unit -> unit
    /// The piece 'color
    member col: Color
    /// The present height of the shape
    member height: int
    /// The piece 'present shape
    member image: bool[,]

    /// The piece 'present offset
    member offset: position
    /// The present width of the shape
    member width: int