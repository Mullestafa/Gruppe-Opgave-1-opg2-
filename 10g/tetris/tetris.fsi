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

(* [<Class>]
type board =
    new : int*int -> board
    member width: int
    member height: int
    member board: Color option [,] *)




type Position = int*int

[<AbstractClass>]
type Tetromino =
    /// The constructor with its initial shape , its final color ,and its inital offset
    new: a: bool[,] * c: Color * o: Position -> Tetromino
    /// Make a string representation of this piece
    override ToString: unit -> string
    /// Make a deep copy of this piece
    abstract member clone: unit -> Tetromino
    /// Rotates the piece 90 degrees clock -wise such that its left -top offset is maintained 
    member rotateRight: unit -> unit
    /// The piece 'color
    member col: Color
    /// The present height of the shape
    member height: unit -> int
    /// The piece 'present shape
    member image: bool[,]
    /// The piece 'present offset
    member offset: Position
    /// The present width of the shape
    member width: unit -> int

type board =
    /// The constructor of a boad of w x h fields and which creates the first active piece at the top
    new: w: int * h: int -> board
    /// Make a string representation of this board
    override ToString: unit -> string
    /// Make a new piece and put it on the board if possible. Returns the piece or None
    member newPiece: unit -> Tetromino option
    /// Put a piece on the board if possible. Returns true if successful
    member put: t: Tetromino -> bool
    /// Take the active piece from the board. Returns a piece or None if no piece is active
    member take: unit -> Tetromino option
    /// Return the board
    member board: Color option[,]
    /// The number of fields on the board vertically
    member height: int
    /// The number of fields on the board horizontally
    member width: int
    
    member activePiece: Tetromino
    member setActivePiece : Tetromino -> unit

val draw : w:int -> h:int -> b:board -> Canvas.canvas

val react: b: board -> k: Canvas.key -> board option