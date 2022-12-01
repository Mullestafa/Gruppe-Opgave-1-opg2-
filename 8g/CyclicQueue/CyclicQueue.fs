module CyclicQueue

type Value = int

let mutable queue : Value option[] = [||] 
let mutable first : int option = None
let mutable last : int option = None
  
let create (n:int) : unit =
    queue <- Array.zeroCreate n
    first <- Some 0
    last <- Some 0
    
let enqueue (e:Value) : bool =
    failwith "Not implemented yet: enqueue"    
   // queue <- 


let dequeue () : Value option =
    failwith "Not implemented yet: dequeue"    

let isEmpty () : bool =
    failwith "Not implemented yet: isEmpty"    


let length () : int =
    failwith "Not implemented yet: length"    

let toString () : string =
    failwith "Not implemented yet: toString"    
