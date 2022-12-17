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
        this.offset <- (((fst this.offset)), ((snd this.offset)+1))
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
                [false; true; false]]), Purple, (5, 0))
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
    inherit Tetromino (array2D ([[true; true; true; true]]), Cyan, (5, 0))
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
        let newPiece: Tetromino = listOfPiecesTypes[randomNumber()]
        Array2D.iteri (fun i j v -> if v then do if Option.isSome(this.board[(i+(fst newPiece.offset)),(j+(snd newPiece.offset))]) then do isPlaceable <- false ) newPiece.image
        if isPlaceable then
            //Array2D.iteri (fun i j v -> if v then do this.board[(i+(fst newPiece.offset)),(j+(snd newPiece.offset))] <- (Some newPiece.col)) newPiece.image
            Some newPiece
        else
            None
    member this.activePiece with get() = _activePiece
    member this.setActivePiece (p: Tetromino) = _activePiece <- p

    member this.put (t:Tetromino) : bool =
        let mutable isPlaceable = true
        try
            Array2D.iteri (fun i j v -> if v then do if Option.isSome(this.board[(i+(fst t.offset)),(j+(snd t.offset))]) then do isPlaceable <- false ) t.image
        with exn -> isPlaceable <- false
        if isPlaceable then
            Array2D.iteri (fun i j v -> if v then do try this.board[(i+(fst t.offset)),(j+(snd t.offset))] <- (Some t.col) with exn -> 1|>ignore) t.image
            true
        elif ((snd this.activePiece.offset) > -1) then
            if (fst (this.activePiece.offset) > 5) then 
                this.activePiece.offset <- ((this.width - this.activePiece.width()), (snd this.activePiece.offset))
                Array2D.iteri (fun i j v -> if v then do try this.board[(i+(fst t.offset)),(j+(snd t.offset))] <- (Some t.col) with exn -> 1|>ignore) t.image
                true
            else
                this.activePiece.offset <- ((0), (snd this.activePiece.offset))
                Array2D.iteri (fun i j v -> if v then do try this.board[(i+(fst t.offset)),(j+(snd t.offset))] <- (Some t.col) with exn -> 1|>ignore) t.image
                true
        else
            this.activePiece.offset <- ((fst this.activePiece.offset), (0))
            Array2D.iteri (fun i j v -> if v then do try this.board[(i+(fst t.offset)),(j+(snd t.offset))] <- (Some t.col) with exn -> 1|>ignore) t.image
            true

    member this.take () : Tetromino option =
        try
            Array2D.iteri (fun i j v -> if v then do this.board[(i+(fst this.activePiece.offset)),(j+(snd this.activePiece.offset))] <- None) this.activePiece.image  
            Some this.activePiece
        with exn->
            None
    
    member this.checkIfPieceFallen () : bool = 
        if ((fst this.activePiece.offset) < 0) then this.activePiece.offset <- ((0),(snd this.activePiece.offset))
        elif ((fst this.activePiece.offset) > (this.width - this.activePiece.width())) then this.activePiece.offset <- ((this.width - this.activePiece.width()),(snd this.activePiece.offset))
        let mutable isPlaceable = true
        if ((snd this.activePiece.offset) = (this.height-(this.activePiece.height()))) then isPlaceable <- false; printfn "fallen?: %A" (not(isPlaceable))
        else
            Array2D.iteri (fun i j v -> if v then do if Option.isSome(this.board[(i+(fst this.activePiece.offset)),(1+j+(snd this.activePiece.offset))]) then do isPlaceable <- false; printfn "%A %A" i j ) this.activePiece.image
            printfn "fallen?: %A" (not(isPlaceable))
        not(isPlaceable)
    //member this.pieces with get() = _pieces and set(p) = _pieces <- [p] :: _pieces

    //member this.addToBoard() =
    //    for piece in this.pieces do
    //        Array2D.iteri (fun i j v -> if v then do this.board[(i+(fst piece.offset)),(j+(snd piece.offset))] <- (Some piece.col)) piece.image



let draw (w:int) (h:int) (b:board) : Canvas.canvas =
    let C = Canvas.create w h
    let PieceWidth = w / b.width
    let PieceHight = h / b.height
    //loop through the board and draw a square on some
    b.put b.activePiece |> ignore
    Array2D.iteri (fun (i: int) (j: int) (v: Color option) -> match v with None -> None |> ignore | Some (x: Color) -> (Canvas.setFillBox C (toCanvasColor(x)) ((PieceWidth*i),(PieceHight*j)) ((PieceWidth*i+PieceWidth),(PieceHight*j+PieceHight)))) (b.board)
    C

let react (b:board) (k:Canvas.key) : (board option) =
    match Canvas.getKey k with
    | Canvas.LeftArrow ->
        b.take() |> ignore
        if b.checkIfPieceFallen() then
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            Some b
        else
            b.activePiece.offset <- (((fst b.activePiece.offset)-1), ((snd b.activePiece.offset)+1))
            Some b
    | Canvas.RightArrow ->
        b.take() |> ignore
        if b.checkIfPieceFallen() then
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            Some b
        else
            b.activePiece.offset <- (((fst b.activePiece.offset)+1), ((snd b.activePiece.offset)+1))
            Some b
    | Canvas.UpArrow ->
        b.take() |> ignore
        if b.checkIfPieceFallen() then
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            Some b
        else
            b.activePiece.rotateRight()
            Some b
    | Canvas.DownArrow ->
        b.take() |> ignore
        if b.checkIfPieceFallen() then
            b.put b.activePiece |> ignore
            b.setActivePiece (b.newPiece().Value)
            Some b
        else
            b.activePiece.offset <- (((fst b.activePiece.offset)), ((snd b.activePiece.offset)+1))
            Some b
    | _ -> None