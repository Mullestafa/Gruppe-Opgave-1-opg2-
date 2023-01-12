<<<<<<< HEAD:12g/12g.py
#Opgave A

=======
# Opgave A
>>>>>>> 9682a0f9a0f116f445a0b81fe6081c49040c36c6:12g/steps.py
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
<<<<<<< HEAD:12g/12g.py
    #def apply(self, inp):
    #    return 
    def description(self) -> str:
        return "give value after accumulating (acc * elm) to each element in input list (starting with acc = {self.n_elm})"
    

# Opgave C
=======
>>>>>>> 9682a0f9a0f116f445a0b81fe6081c49040c36c6:12g/steps.py

# Opgave B
class Map:
    def __init__(self,step):
        self.step = step
    def apply(self, inp:list):
        [step.apply(i) for i in inp]
