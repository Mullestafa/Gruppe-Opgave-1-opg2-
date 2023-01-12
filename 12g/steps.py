import csv
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


# Opgave B
class Map:
    def __init__(self,step):
        self.step = step
    def apply(self, inp:list):
        return [self.step.apply(i) for i in inp]
    def description(self):
        return f"for every element in input list: {self.step.description()}"


# Opgave D
class CsvReader:
    def apply(self, file:str):
        output = []
        with open(file, newline='') as csvfile:
            file_reader = csv.DictReader(csvfile)
            for row in file_reader:
                output.append(row)
        return output
    def description(self):
        return "Make csv file into list of dicts"

class CritterStats:
    def apply(self, data:dict):
        return "hey"
