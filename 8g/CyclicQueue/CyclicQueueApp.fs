open CyclicQueue

[<EntryPoint>]
let main _ =
    // Write your tests here
    // (or organize your tests into functions and call them from here)
    // Exit status; consider making it the number of failed tests
    create 10
    let mutable errorCount = 0
    errorCount <- errorCount + (if List.map (fun i -> enqueue i) [1..11] = [true; true; true; true; true; true; true; true; true; true; false] then 0 else 1) // testing enqueu
    
    errorCount <- errorCount + (if List.map (fun i -> dequeue()) [1..5] = [Some 1; Some 2; Some 3; Some 4; Some 5] then 0 else 1)  // testing dequeue

    errorCount <- errorCount + (if List.map (fun i -> enqueue i) [1..5] =  [true; true; true; true; true] then 0 else 1)// testing circular iteration while enqueing

    errorCount <- errorCount + (if List.map (fun i -> dequeue()) [1..11] = [Some 6; Some 7; Some 8; Some 9; Some 10; Some 1; Some 2; Some 3; Some 4; Some 5; None] then 0 else 1) // testing circular iteration

    errorCount <- errorCount + (if List.map (fun i -> enqueue i) [1..10] = [true; true; true; true; true; true; true; true; true; true] then 0 else 1) // testing repopulating a reinitialized test
    
    errorCount <- errorCount + (if toString() = "1 2 3 4 5 6 7 8 9 10" then 0 else 1)

    printfn "error count: %A" errorCount
    errorCount
