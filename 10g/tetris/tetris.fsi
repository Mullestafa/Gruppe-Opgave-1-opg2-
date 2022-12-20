/// <summary>
/// The Tetris module contains types and functions for creating and manipulating Tetris games.
/// </summary>
module Tetris

/// <summary>
/// The Color type represents the colors that Tetrominoes can be.
/// </summary>
type Color =
    | Yellow
    | Blue
    | Cyan
    | Orange
    | Red
    | Green
    | Purple

/// <summary>
/// Converts a Tetris Color to a Canvas color.
/// </summary>
/// <param name="c">The Tetris Color to convert.</param>
/// <returns>The Canvas color equivalent of the Tetris Color.</returns>
val toCanvasColor : c:Color -> Canvas.color

/// <summary>
/// The Position type represents the position of a Tetromino on the board.
/// </summary>
type Position = int*int

/// <summary>
/// The Tetromino type represents a Tetromino piece in a Tetris game.
/// </summary>
[<AbstractClass>]
type Tetromino =
    /// <summary>
    /// The constructor of a Tetromino with its initial shape, its final color, and its initial offset.
    /// </summary>
    /// <param name="a">The initial shape of the Tetromino.</param>
    /// <param name="c">The color of the Tetromino.</param>
    /// <param name="o">The initial offset of the Tetromino.</param>
    new: a: bool[,] * c: Color * o: Position -> Tetromino
    
    // <summary>
    /// Makes a string representation of this Tetromino.
    /// </summary>
    /// <returns>A string representation of this Tetromino.</returns>
    override ToString: unit -> string

    /// <summary>
    /// Makes a deep copy of this Tetromino.
    /// </summary>
    /// <returns>A deep copy of this Tetromino.</returns>
    abstract member clone: unit -> Tetromino

    /// <summary>
    /// Rotates the Tetromino 90 degrees clockwise such that its left-top offset is maintained.
    /// </summary>
    member rotateRight: unit -> unit

    /// <summary>
    /// The color of the Tetromino.
    /// </summary>
    member col: Color
    
    /// <summary>
    /// The present height of the Tetromino's shape.
    /// </summary>
    /// <returns>The present height of the Tetromino's shape.</returns>
    member height: unit -> int

    /// <summary>
    /// The present shape of the Tetromino.
    /// </summary>
    member image: bool[,]

    /// <summary>
    /// The present offset of the Tetromino.
    /// </summary>
    member offset: Position

    /// <summary>
    /// The present width of the Tetromino's shape.
    /// </summary>
    /// <returns>The present width of the Tetromino's shape.</returns>
    member width: unit -> int

/// <summary>
/// The board type represents the board in a Tetris game.
/// </summary>
type board =
    /// <summary>
    /// The constructor of a board with w x h fields and which creates the first active piece at the top.
    /// </summary>
    /// <param name="w">The number of fields on the board horizontally.</param>
    /// <param name="h">The number of fields on the board vertically.</param>
    /// <returns>A new Tetris board with w x h fields and the first active piece at the top.</returns>
    new: w: int * h: int -> board

    /// <summary>
    /// Makes a string representation of this board.
    /// </summary>
    /// <returns>A string representation of this board.</returns>
    override ToString: unit -> string

    /// <summary>
    /// Makes a new piece and puts it on the board if possible. Returns the piece or None.
    /// </summary>
    /// <returns>The new piece if it was successfully placed on the board, or None otherwise.</returns>
    member newPiece: unit -> Tetromino option

    /// <summary>
    /// Puts a piece on the board if possible. Returns true if successful.
    /// </summary>
    /// <param name="t">The Tetromino to place on the board.</param>
    /// <returns>True if the Tetromino was successfully placed on the board, or False otherwise.</returns>
    member put: t: Tetromino -> bool
    
    /// Take the active piece from the board. Returns a piece or None if no piece is active
    member removeFromBoard: unit -> Tetromino option
    /// Return the board
    member board: Color option[,]
    /// The number of fields on the board vertically
    member height: int
    /// The number of fields on the board horizontally
    member width: int
    // get an instance oF the active peice
    member activePiece: Tetromino
    // sets the active peice
    member setActivePiece : Tetromino -> unit

val draw : w:int -> h:int -> b:board -> Canvas.canvas

val react: b: board -> k: Canvas.key -> board option