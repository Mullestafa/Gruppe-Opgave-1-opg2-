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