module Game

type pos = int*int
type value = Red | Green | Blue | Yellow | Black
type piece = value * pos
type state = piece list

/// <summary>
/// converts the module type "value" into a canvas type "color"
/// </summary>
/// <param name="value">a value type</param>
/// <returns>an equvilant Canvas.color type</returns>
val fromValue : value -> Canvas.color

/// <summary>
/// will convert the input value to the color of 2x the value
/// for example Red -> Blue
/// </summary>
/// <param name="value">a value type</param>
/// <returns>a value type of double the value</returns>
val nextColor : value -> value

/// <summary>
/// given a state type and an int value n, filters out all but the 
/// pieces in the state which are on the n-th column.
/// </summary>
/// <param name="column">an int for the column to be filtered for (starts at 0)</param>
/// <param name="state">a state to be filtered</param>
/// <returns>a new state with only the pieces in the column given as a parameter</returns>
val filter : int -> state -> state

/// <summary>
/// Will shift all pieces up as far as possible
/// and will merge sequencial pieces of the same value in the process
/// </summary>
/// <param name="state">a state to be modified </param>
/// <returns>a state where all the pieces have been tilted up as far as possilbe</returns>
val shiftUp : state -> state

/// <summary>
/// Flips the given state upside-down
/// </summary>
/// <param name="state">a state with pieces to be flipped</param>
/// <returns>a new state which has been flipped upside-down (horizontally)</returns>
val flipUD : state -> state

/// <summary>
/// flips a state on its diagonal (diagonal between upperleft and lower right corners)
/// </summary>
/// <param name="state">a state to be flipped diagonally</param>
/// <returns>a diagonally flipped state</returns>
val transpose : state -> state

/// <summary>
/// given a state, will return a list of empty places
/// </summary>
/// <param name="state">a state to be checked for empty spaces</param>
/// <returns>Some list of empty places not taken up by element in the input state, or None if there are no empty places</returns>
val empty : state -> pos list
val addRandom : value -> state -> state option

val blindAdd : value -> state -> state

val unitTest : (state * state * state * state * pos list) -> string -> bool
