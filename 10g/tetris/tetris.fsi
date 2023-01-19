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

val randomNumber: unit -> int

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

    member rotateLeft: unit -> unit

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

    type S =
        inherit Tetromino
        new: unit-> S
        
    type Z =
        inherit Tetromino
        new: unit-> Z

    type T =
        inherit Tetromino
        new: unit-> T

    type J =
        inherit Tetromino
        new: unit-> J

    type O =
        inherit Tetromino
        new: unit-> O

    type I =
        inherit Tetromino
        new: unit-> I


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

    /// <summary>
    /// Takes the active piece from the board. Returns a piece or None if no piece is active.
    /// </summary>
    /// <returns>The active piece if one exists, or None if no piece is active.</returns>
    member take: unit -> Tetromino option

    /// <summary>
    /// Returns the board.
    /// </summary>
    member board: Color option[,]

    /// <summary>
    /// The number of fields on the board vertically.
    /// </summary>
    member height: int

    /// <summary>
    /// The number of fields on the board horizontally.
    /// </summary>
    member width: int

    /// <summary>
    /// Gets an instance of the active piece.
    /// </summary>
    member activePiece: Tetromino

    /// <summary>
    /// Sets the active piece.
    /// </summary>
    /// <param name="t">The Tetromino to set as the active piece.</param>
    member setActivePiece : Tetromino -> unit

/// <summary>
/// Draws the Tetris board on a canvas.
/// </summary>
/// <param name="w">The width of the canvas.</param>
/// <param name="h">The height of the canvas.</param>
/// <param name="b">The Tetris board to draw.</param>
/// <returns>A canvas with the Tetris board drawn on it.</returns>
val draw : w:int -> h:int -> b:board -> Canvas.canvas

/// <summary>
/// Reacts to a key press in the Tetris game.
/// </summary>
/// <param name="b">The current state of the Tetris board.</param>
/// <param name="k">The key that was pressed.</param>
/// <returns>The new state of the Tetris board after reacting to the key press, or None if the game is over.</returns>
val react: b: board -> k: Canvas.key -> board option
