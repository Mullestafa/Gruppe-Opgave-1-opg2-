module Game

type pos = int*int
type value = Red | Green | Blue | Yellow | Black
type piece = value * pos
type state = piece list

val fromValue : value -> Canvas.color
val nextColor : value -> value
val filter : int -> state -> state
val shiftUp : state -> state
val flipUD : state -> state
val transpose : state -> state
val empty : state -> pos list
val addRandom : value -> state -> state option
