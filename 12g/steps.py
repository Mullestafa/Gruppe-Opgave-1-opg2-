=======
# Opgave A

class AddConst:
    def __init__(self, c):
        self.c = c
    def apply(self, inp):
        return self.c+inp
    def description(self) -> str:
        return f"add {self.c} to input"

class Repeater:
    def __init__(self, num:int):
        self.num = num
    def apply(self, inp):
        return [inp for _ in range(self.num)]
    def description(self) -> str:
        return f"give list containing input {self.num} times"

class GeneralSum:
    def __init__(self, n_elm, op:str):
        self.n_elm = n_elm
        self.op = op
        if self.op == "+":
            self.fun = (lambda x,y : x + y)
        elif self.op == "-":
            self.fun = (lambda x,y : x - y)
        elif self.op == "*":
            self.fun = (lambda x,y : x * y)
        elif self.op == "/":
            self.fun = (lambda x,y : x / y)
        elif self.op == "%":
            self.fun = (lambda x,y : x % y)
        elif self.op == "//":
            self.fun = (lambda x,y : x // y)
        else:
            raise ValueError("invalid operator")
    def apply(self, inp:list):
        acc = self.n_elm
        for i in inp:
            acc = self.fun(acc,i)
        return acc
    def description(self) -> str:
        return f"give value after accumulating (acc {self.op} elm) to each element in input list (starting with acc = {self.n_elm})"


class SumNum(GeneralSum):
    def __init__(self):
        super().__init__(0,"+")

class ProductNum(GeneralSum):
    def __init__(self):
        super().__init__(1,"*")

    #def apply(self, inp):
    #    return 
    def description(self) -> str:
        return "give value after accumulating (acc * elm) to each element in input list (starting with acc = {self.n_elm})"
    
=======

# Opgave B
class Map:
    def __init__(self,step):
        self.step = step
    def apply(self, inp:list):
        [step.apply(i) for i in inp]
        
=======
        
# Opgave C

class Pipeline:
    # Takes a list of steps as an argument
    def _init_(self, step)
        self.step = step
    
    # Adds additional steps to the list of steps
    def add_step(self, step):
        self.step.append(step)
    
    # Iterates over the list of steps and calls the apply function on every step
    def apply(self, input_):
        for self in self.step 
        input_ = step.apply(input_)
        return input()
    
    # Returns a string that lists the description of each step
    def description(self):
        return print(','.join([step.description() for step in self.step]))