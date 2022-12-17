module Tetris

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

let randomNumber() =
  let random = System.Random()
  random.Next(0, 7)


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

    abstract member clone : unit -> Tetromino

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
    
    member this.height ():int =
        Array2D.length2 this.image
    
    member this.width ():int =
        Array2D.length1 this.image


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
    //do _board.[0 ,1] <- Some Green
    member this.width = w
    member this.height = h
    member this.board with get() = _board
    
    override this.ToString() =
        sprintf "board: %A" this.board
    
    member this.newPiece () : Tetromino option =
        let mutable isPlaceable = true
        let newPiece: Tetromino = listOfPiecesTypes[randomNumber()].clone()
        Array2D.iteri (fun i j v -> if v then do if Option.isSome(this.board[(i+(fst newPiece.offset)),(j+(snd newPiece.offset))]) then do isPlaceable <- false ) newPiece.image
        if isPlaceable then
            printfn "new piece successfully created"
            Some newPiece
        else
            printfn "failed to create new piece: %A" newPiece
            None

    member this.activePiece with get() = _activePiece
    member this.setActivePiece (p: Tetromino) = _activePiece <- p

    member this.put (t:Tetromino) : bool =
        Array2D.iteri (fun i j v -> if v then do this.board[(i+(fst t.offset)),(j+(snd t.offset))] <- (Some t.col)) t.image
        true

    member this.removeFromBoard () : Tetromino option =
        Array2D.iteri (fun i j v -> if v then do this.board[(i+(fst this.activePiece.offset)),(j+(snd this.activePiece.offset))] <- None) this.activePiece.image  
        Some this.activePiece


    member this.checkForNoCollision (pos: Position) =
        let mutable isPlaceable = true 

        Array2D.iteri (fun i j v -> if v then do if Option.isSome(this.board[(i+(fst pos)),(j+(snd pos))]) then do isPlaceable <- false) this.activePiece.image
        isPlaceable
    member this.checkIfPieceIsFallen (t:Tetromino) : bool =
        let mutable pieceIsFallen = false

        if (snd t.offset)=(this.height-t.height()) then pieceIsFallen <- true
        elif not(this.checkForNoCollision((((fst t.offset)),((snd t.offset)+1)))) then pieceIsFallen <- true
        pieceIsFallen
    member this.snapToEgde (t:Tetromino) =
        if (fst t.offset) < 5 then
            t.offset <- ((0),(snd t.offset))
        else
            t.offset <- ((this.width-t.width()),(snd t.offset))


let draw (w:int) (h:int) (b:board) : Canvas.canvas =
    let C = Canvas.create w h
    let PieceWidth = w / b.width
    let PieceHight = h / b.height
    //loop through the board and draw a square on some
    Array2D.iteri (fun (i: int) (j: int) (v: Color option) -> match v with None -> None |> ignore | Some (x: Color) -> (Canvas.setFillBox C (toCanvasColor(x)) ((PieceWidth*i),(PieceHight*j)) ((PieceWidth*i+PieceWidth),(PieceHight*j+PieceHight)))) (b.board)
    C

let react (b:board) (k:Canvas.key) : (board option) =
    match Canvas.getKey k with
    | Canvas.LeftArrow ->
        b.removeFromBoard() |> ignore
        if not((fst b.activePiece.offset) =0) then
            if b.checkForNoCollision((((fst b.activePiece.offset)-1),(snd b.activePiece.offset))) then
                b.activePiece.offset <- (((fst b.activePiece.offset)-1), ((snd b.activePiece.offset)))

        if not(b.checkIfPieceIsFallen(b.activePiece)) then
            b.activePiece.offset <- (((fst b.activePiece.offset)), ((snd b.activePiece.offset)+1))
            b.put b.activePiece |> ignore
            Some b
        else
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            b.put b.activePiece |> ignore
            Some b
    | Canvas.RightArrow ->
        b.removeFromBoard() |> ignore
        if not((fst b.activePiece.offset) = (b.width - (b.activePiece.width()))) then
            if b.checkForNoCollision((((fst b.activePiece.offset)+1),(snd b.activePiece.offset))) then
                b.activePiece.offset <- (((fst b.activePiece.offset)+1), ((snd b.activePiece.offset)))

        if not(b.checkIfPieceIsFallen(b.activePiece)) then
            b.activePiece.offset <- (((fst b.activePiece.offset)), ((snd b.activePiece.offset)+1))
            b.put b.activePiece |> ignore
            Some b
        else
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            b.put b.activePiece |> ignore
            Some b

    | Canvas.UpArrow ->
        b.removeFromBoard() |> ignore
        if not(b.checkIfPieceIsFallen(b.activePiece)) then
            b.activePiece.offset <- (((fst b.activePiece.offset)), ((snd b.activePiece.offset)+1))
            b.activePiece.rotateRight()
            if (fst b.activePiece.offset) > (b.width-b.activePiece.width()) then b.snapToEgde(b.activePiece)
            elif (fst b.activePiece.offset) < (0) then b.snapToEgde(b.activePiece)
            b.put b.activePiece |> ignore
            Some b
        else
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            b.put b.activePiece |> ignore
            Some b
    | Canvas.DownArrow ->
        b.removeFromBoard() |> ignore
        if not(b.checkIfPieceIsFallen(b.activePiece)) then
            b.activePiece.offset <- (((fst b.activePiece.offset)), ((snd b.activePiece.offset)+1))
            b.put b.activePiece |> ignore
            Some b
        else
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            b.put b.activePiece |> ignore            
            Some b
    | _ -> None