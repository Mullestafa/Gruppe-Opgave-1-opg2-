namespace FSharp


module Tetris

type Color =
    | Yellow
    | Blue
    | Cyan
    | Orange
    | Red
    | Green
    | Purple

type Position = int * int

val toCanvasColor: c: Color -> Canvas.color

val randomNumber: unit -> int

[<AbstractClass>]
type Tetromino =
    
    new: a: bool array2d * c: Color * o: Position -> Tetromino
    
    override ToString: unit -> string
    
    abstract clone: unit -> Tetromino
    
    member height: unit -> int
    
    member rotateLeft: unit -> unit
    
    member rotateRight: unit -> unit
    
    member width: unit -> int
    
    member col: Color
    
    member image: bool array2d
    
    member offset: Position

type S =
    inherit Tetromino
    
    new: unit -> S
    
    override clone: unit -> Tetromino

type Z =
    inherit Tetromino
    
    new: unit -> Z
    
    override clone: unit -> Tetromino

type T =
    inherit Tetromino
    
    new: unit -> T
    
    override clone: unit -> Tetromino

type L =
    inherit Tetromino
    
    new: unit -> L
    
    override clone: unit -> Tetromino

type J =
    inherit Tetromino
    
    new: unit -> J
    
    override clone: unit -> Tetromino

type O =
    inherit Tetromino
    
    new: unit -> O
    
    override clone: unit -> Tetromino

type I =
    inherit Tetromino
    
    new: unit -> I
    
    override clone: unit -> Tetromino

val listOfPiecesTypes: Tetromino list

type board =
    
    new: w: int * h: int -> board
    
    override ToString: unit -> string
    
    member checkForNoCollision: pos: Position -> bool
    
    member checkIfLineIsFull: line: int -> bool
    
    member checkIfPieceIsFallen: t: Tetromino -> bool
    
    member makeLineBlank: line: int -> unit
    
    member moveLineDown: line: int -> unit
    
    member newPiece: unit -> Tetromino option
    
    member put: t: Tetromino -> bool
    
    member removeFullLines: unit -> unit
    
    member setActivePiece: Tetromino -> unit
    
    member snapToEgde: t: Tetromino -> unit
    
    member take: unit -> Tetromino option
    
    member activePiece: Tetromino
    
    member board: Color option array2d
    
    member height: int
    
    member width: int

val draw: w: int -> h: int -> b: board -> Canvas.canvas

val react: b: board -> k: Canvas.key -> board option

module ``18e6c5fa02bc8d7846e24bac7555e69561b6390de37d7b9e7a0c877f9d176889``


module TetrisApp

val blackTest: 'a list

val testBoard: Tetris.board

val i: Tetris.Tetromino option

val s: Tetris.S

val b: Tetris.board

