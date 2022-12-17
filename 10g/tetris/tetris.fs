module Tetris

(* The Color type represents the colors that can be used in the game.*)
type Color =
    | Yellow
    | Blue
    | Cyan
    | Orange
    | Red
    | Green
    | Purple

(* The Position type is a tuple of two integers representing a position on the game board. *)
type Position = int*int

(* The toCanvasColor function takes a Color value as input
 and returns the corresponding Canvas.color. This function
 is used to map the Color values to colors that can be used
  to draw on the canvas. *)
let toCanvasColor(c:Color) : Canvas.color =
    match c with
    | Yellow -> Canvas.yellow
    | Blue -> Canvas.blue
    | Cyan -> Canvas.fromRgb(0,255,255)
    | Orange -> Canvas.fromRgb(255,165,0)
    | Red -> Canvas.red
    | Green -> Canvas.green
    | Purple -> Canvas.fromRgb(128,0,128)

(* The randomNumber function returns a random integer between 0 and 7.
 In this module, this is used to randomly choose one of the 7 Tetrominos *)
let randomNumber() =
  let random = System.Random()
  random.Next(0, 7)

(* The Tetromino type is an abstract class representing a Tetromino piece in the game.
   It has three members: _position, _shape, and _color.*)
[<AbstractClass>]
type Tetromino (a:bool[,], c:Color, o: Position) =
    let mutable _position = o
    let mutable _shape = a
    let mutable _color = c

    member this.offset with get() = _position and set(p) = _position <- p
    member this.image with get() = _shape and set(s) = _shape <- s
    member this.col with get() = _color and set(c) = _color <- c

    override this.ToString() =
        sprintf "piece at: %A \nwith shape: %A \nwith color: %A\n-----------------" this.offset this.image this.col

    (* The clone method is an abstract method that creates a copy of the Tetromino. *)
    abstract member clone : unit -> Tetromino

    (* The rotateRight method rotates the Tetromino 90 degrees clockwise.
    It does this by first transposing the _shape field (interchanging its rows and columns),
    then flipping it horizontally. *)
    member this.rotateRight () =
        let transpose(a:bool[,]) =
            let r = Array2D.init (Array2D.length2(a)) (Array2D.length1(a)) (fun _ _ -> true)
            Array2D.iteri (fun i j v -> Array2D.set r j i v) a
            r
        let flip(a:bool[,]) = 
            let r = Array2D.init (Array2D.length1(a)) (Array2D.length2(a)) (fun _ _ -> true)
            Array2D.iteri (fun i j v -> Array2D.set r i (Array2D.length2(a)-1-j) v) a
            r
        this.image <- flip(transpose(this.image))
    
    (* The height method returns the height of the Tetromino, which is the number of rows in the _shape field. *)
    member this.height ():int =
        Array2D.length2 this.image
    (* The width method returns the width of the Tetromino, which is the number of columns in the _shape field. *)
    member this.width ():int =
        Array2D.length1 this.image

(*The S, Z, T, L, J, O, and I types are all classes that inherit from the Tetromino abstract class. Each of these
classes represents a specific type of Tetromino piece with a unique shape and color. The clone method is overridden
in each of these classes to create a copy of the specific Tetromino.*)
type S() =
    inherit Tetromino (array2D ([[false; true; true];
                [true; true; false]]), Green, (5, 0))
    override this.clone() =
        let r = new S()
        r.offset <- this.offset
        r


type Z() =
    inherit Tetromino (array2D ([[true; true; false];
                [false; true; true]]), Red, (5, 0))
    override this.clone() =
        let r = new Z()
        r.offset <- this.offset
        r

type T() =
    inherit Tetromino (array2D ([[true; true; true];
                [false; true; false]]), Purple, (4, 0))
    override this.clone() =
        let r = new T()
        r.offset <- this.offset
        r    

type L() =
    inherit Tetromino (array2D ([[true; false];
                [true; false];
                [true; true]]), Orange, (5, 0))
    override this.clone() =
        let r = new L()
        r.offset <- this.offset
        r
type J() =
    inherit Tetromino (array2D ([[false; true];
                [false; true];
                [true; true]]), Blue, (5, 0))
    override this.clone() =
        let r = new J()
        r.offset <- this.offset
        r
type O() =
    inherit Tetromino (array2D ([[true; true];
                [true; true]]), Yellow, (5, 0))  
    override this.clone() =
        let r = new O()
        r.offset <- this.offset
        r
type I() =
    inherit Tetromino (array2D ([[true]; [true]; [true]; [true]]), Cyan, (3, 0))
    override this.clone() =
        let r = new I()
        r.offset <- this.offset
        r

let listOfPiecesTypes: Tetromino list = [S(); T(); Z(); L(); J(); O(); I()]


type board (w:int, h: int) =
    let _board = Array2D.create w h None
    let mutable _activePiece: Tetromino = T()
    member this.width = w
    member this.height = h
    member this.board with get() = _board
    
    override this.ToString() =
        sprintf "board: %A" this.board
    
    (*function that generates a new Tetromino piece and places it on the board
    will print Game Over if it cannot place the new piece*)
    member this.newPiece () : Tetromino option =
        let mutable isPlaceable = true
        let newPiece: Tetromino = listOfPiecesTypes[randomNumber()].clone()
        Array2D.iteri (fun i j v -> if v then do if Option.isSome(this.board[(i+(fst newPiece.offset)),(j+(snd newPiece.offset))]) then do isPlaceable <- false ) newPiece.image
        if isPlaceable then
            Some newPiece
        else
            printfn "---------------Game Over---------------"
            None

    member this.activePiece with get() = _activePiece
    member this.setActivePiece (p: Tetromino) = _activePiece <- p

    (*paints a Tetromino piece on the board*)
    member this.put (t:Tetromino) : bool =
        (*iterates through the shape of the tetromino and translates the shape index to actual coordinates
        by adding the piece offset*)
        Array2D.iteri (fun i j v -> if v then do this.board[(i+(fst t.offset)),(j+(snd t.offset))] <- (Some t.col)) t.image
        true

    (*removes the active piece from the board (paints over it) and returns it.*)
    member this.removeFromBoard () : Tetromino option =
        Array2D.iteri (fun i j v -> if v then do this.board[(i+(fst this.activePiece.offset)),(j+(snd this.activePiece.offset))] <- None) this.activePiece.image  
        Some this.activePiece

    (*checks if the active piece can be moved to the given position on the board without colliding with other pieces*)
    member this.checkForNoCollision (pos: Position) =
        let mutable isPlaceable = true 
        Array2D.iteri (fun i j v -> if v then do if Option.isSome(this.board[(i+(fst pos)),(j+(snd pos))]) then do isPlaceable <- false) this.activePiece.image
        isPlaceable

    (*checks if the given Tetromino has reached the bottom of the board or if it would collide with another piece if moved down.*)    
    member this.checkIfPieceIsFallen (t:Tetromino) : bool =
        let mutable pieceIsFallen = false
        if (snd t.offset)=(this.height-t.height()) then pieceIsFallen <- true
        elif not(this.checkForNoCollision((((fst t.offset)),((snd t.offset)+1)))) then pieceIsFallen <- true
        pieceIsFallen

    (*moves the given Tetromino to the left or right edge of the board (whatever is closest).*)    
    member this.snapToEgde (t:Tetromino) =
        if (fst t.offset) < 5 then
            t.offset <- ((0),(snd t.offset))
        else
            t.offset <- ((this.width-t.width()),(snd t.offset))

    (*checks if a given row on the board is full.*)
    member this.checkIfLineIsFull (line:int) : bool =
        let mutable isFull = true
        for i in [0..this.width-1] do
            if Option.isNone(this.board[i, line]) then isFull <- false
        isFull 

    (*moves a row down, overwriting the one below it.*)
    member this.moveLineDown (line:int) =
        for i in [0..this.width-1] do
            this.board[i, line+1] <- this.board[i, line]
    
    (*clears a given row on the board.*)
    member this.makeLineBlank (line:int) =
        for i in [0..this.width-1] do
            this.board[i, line] <- None

    (*removes any full rows from the board and moves the rows above them down.*)        
    member this.removeFullLines () =
        this.removeFromBoard() |> ignore
        let mutable i = this.height - 1
        while not(i = -1) do
            if this.checkIfLineIsFull(i) then
                for j in [(i-1)..(-1)..0] do
                    this.moveLineDown(j)
                this.makeLineBlank(0)
            else
                i <- i-1
        this.put(this.activePiece) |> ignore


(*gets called every new frame. removes full lines and draws the baord on the canvas*)
let draw (w:int) (h:int) (b:board) : Canvas.canvas =
    let C = Canvas.create w h
    let PieceWidth = w / b.width
    let PieceHight = h / b.height
    b.removeFullLines()
    Array2D.iteri (fun (i: int) (j: int) (v: Color option) -> match v with None -> None |> ignore | Some (x: Color) -> (Canvas.setFillBox C (toCanvasColor(x)) ((PieceWidth*i),(PieceHight*j)) ((PieceWidth*i+PieceWidth),(PieceHight*j+PieceHight)))) (b.board)
    C

(*called when user input is detected. Updates the game*)
let react (b:board) (k:Canvas.key) : (board option) =
    match Canvas.getKey k with
    | Canvas.LeftArrow ->
        b.removeFromBoard() |> ignore //first removes the active piece from the board.
        
        (*If the left arrow key is pressed, it moves the active piece one space to the left if possible.*)
        if not((fst b.activePiece.offset) =0) then
            if b.checkForNoCollision((((fst b.activePiece.offset)-1),(snd b.activePiece.offset))) then
                b.activePiece.offset <- (((fst b.activePiece.offset)-1), ((snd b.activePiece.offset)))

        (*checks if the piece has reached the bottom of the board or if it has collided with another piece.*)
        if not(b.checkIfPieceIsFallen(b.activePiece)) then //If it has not, it moves the active piece down by one space and places it on the board.
            b.activePiece.offset <- (((fst b.activePiece.offset)), ((snd b.activePiece.offset)+1))
            b.put b.activePiece |> ignore
            Some b
        else //If it has, it creates a new active piece and places it on the board.
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            b.put b.activePiece |> ignore
            Some b
    | Canvas.RightArrow ->
        b.removeFromBoard() |> ignore // first removes the active piece from the board.

        (*If the right arrow key is pressed, it moves the active piece one space to the right if possible.*)
        if not((fst b.activePiece.offset) = (b.width - (b.activePiece.width()))) then
            if b.checkForNoCollision((((fst b.activePiece.offset)+1),(snd b.activePiece.offset))) then
                b.activePiece.offset <- (((fst b.activePiece.offset)+1), ((snd b.activePiece.offset)))

        (*checks if the piece has reached the bottom of the board or if it has collided with another piece.*)
        if not(b.checkIfPieceIsFallen(b.activePiece)) then //If it has not, it moves the active piece down by one space and places it on the board.
            b.activePiece.offset <- (((fst b.activePiece.offset)), ((snd b.activePiece.offset)+1))
            b.put b.activePiece |> ignore
            Some b
        else //If it has, it creates a new active piece and places it on the board.
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            b.put b.activePiece |> ignore
            Some b

    | Canvas.UpArrow ->
        b.removeFromBoard() |> ignore // first removes the active piece from the board.

        (*checks if the piece has reached the bottom of the board or if it has collided with another piece.*)
        if not(b.checkIfPieceIsFallen(b.activePiece)) then //If it has not, it moves the active piece down by one space and places it on the board.
            b.activePiece.offset <- (((fst b.activePiece.offset)), ((snd b.activePiece.offset)+1))
            b.activePiece.rotateRight()
            if (fst b.activePiece.offset) > (b.width-b.activePiece.width()) then b.snapToEgde(b.activePiece)
            elif (fst b.activePiece.offset) < (0) then b.snapToEgde(b.activePiece)
            b.put b.activePiece |> ignore
            Some b
        else //If it has, it creates a new active piece and places it on the board.
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            b.put b.activePiece |> ignore
            Some b
    | Canvas.DownArrow ->
        b.removeFromBoard() |> ignore // first removes the active piece from the board.

        (*checks if the piece has reached the bottom of the board or if it has collided with another piece.*)
        if not(b.checkIfPieceIsFallen(b.activePiece)) then //If it has not, it moves the active piece down by one space and places it on the board.
            b.activePiece.offset <- (((fst b.activePiece.offset)), ((snd b.activePiece.offset)+1))
            b.put b.activePiece |> ignore
            Some b
        else //If it has, it creates a new active piece and places it on the board.
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            b.put b.activePiece |> ignore            
            Some b
    | _ -> None