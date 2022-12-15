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

    //member this.clone() : Tetromino =
    //    new Tetromino(_shape, _color, _position)

    member this.rotateRight () =
        let transpose(a:bool[,]) =
            let r = Array2D.init (Array2D.length2(a)) (Array2D.length1(a)) (fun _ _ -> true)
            Array2D.iteri (fun i j v -> Array2D.set r j i v) a
            r
        let flip(a:bool[,]) = 
            let r = Array2D.init (Array2D.length1(a)) (Array2D.length2(a)) (fun _ _ -> true)
            Array2D.iteri (fun i j v -> Array2D.set r i (Array2D.length2(a)-j) v) a
            r
        
        this.image <- flip(transpose(this.image))
    
    member this.height () =
        Array2D.length1 this.image
    
    member this.width () =
        Array2D.length2 this.image

type board (w:int, h: int) =
    let _board = Array2D.create w h None
    let mutable _pieces = []
    do _board.[0 ,1] <- Some Green
    member this.width = w
    member this.height = h
    member this.board with get() = _board
    member this.pieces with get() = _pieces and set(p) = _pieces <- [p] :: _pieces

    member this.addToBoard() =
        for piece in this.pieces do
            Array2D.iteri (fun i j v -> if v then do this.board[(i+(fst piece.offset)),(j+(snd piece.offset))] <- (Some piece.col)) piece.image

type S() =
    inherit Tetromino ([[false; true; true]
                [true; true; false]], Green, (0, 0))


type Z() =
    let image = [[true; true; false]
                [false; true; true]]
    let col = Red
    let offset = (0, 0)
    inherit Tetromino (image, col, offset)

type T() =
    let image = [[true; true; true]
                [false; true; false]]
    let col = Purple
    let offset = (0, 0)
    inherit Tetromino (image, col, offset)

type L() =
    let image = [[true; false]
                [true; false]
                [true; true]]
    let col = Orange
    let offset = (0, 0)
    inherit Tetromino (image, col, offset)

type J() =
    let image = [[false; true]
                [false; true]
                [true; true]]
    let col = Blue
    let offset = (0, 0)
    inherit Tetromino (image, col, offset)

type O() =
    let image = [[true; true; false]
                [true; true; false]]
    let col = Yellow
    let offset = (0, 0)
    inherit Tetromino (image, col, offset)    

type I() =
    let image = [[true; true; true; true]]
    let col = Cyan
    let offset = (0, 0)
    inherit Tetromino (image, col, offset)

let draw (w:int) (h:int) (b:board) : Canvas.canvas =
    let C = Canvas.create w h
    let PieceWidth = w / b.width
    let PieceHight = h / b.height
    //loop through the board and draw a square on some
    Array2D.iteri (fun (i: int) (j: int) (v: Color option) -> match v with None -> None |> ignore | Some (x: Color) -> (Canvas.setFillBox C (toCanvasColor(x)) ((PieceWidth*i),(PieceHight*j)) ((PieceWidth*i+PieceWidth),(PieceHight*j+PieceHight)))) (b.board)

    C

