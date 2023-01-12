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
        ## Allow passing of string type operators, for ease of use
        if op == "+":
            fun = (lambda x,y : x + y)
        elif op == "-":
            fun = (lambda x,y : x - y)
        elif op == "*":
            fun = (lambda x,y : x * y)
        elif op == "/":
            fun = (lambda x,y : x / y)
        elif op == "%":
            fun = (lambda x,y : x % y)
        elif op == "//":
            fun = (lambda x,y : x // y)
        else:
            #otherwise, assume it's a custom lambda function
            fun = op

        self.fun = fun
        self.op = op
        self.n_elm = n_elm
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


# Opgave C
class Pipeline:
    # Takes a list of steps as an argument
    def __init__(self, steps:list):
        self.steps = steps
    
    # Adds additional steps to the list of steps
    def add_step(self, step):
        self.steps.append(step)
    
    # Iterates over the list of steps and calls the apply function on every step
    def apply(self, input_):
        result = input_
        for step in self.steps:
            result = step.apply(result)
        return result
    
    # Returns a string that lists the description of each step
    def description(self):
        return ('\n'.join([step.description() for step in self.steps]))

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
    def apply(self, data:list):
        colourdict = {}
        for i in data:
            this_colour = i['Colour']
            if this_colour in colourdict.keys():
                colourdict[this_colour] += 1
            else:
                colourdict[this_colour] = 1
        return colourdict

class ShowAsciiBarchart:
    def apply(self, d:dict):
        keylist = []
        maxlen = 0
        for i in d.keys():
            if len(i) > maxlen:
                maxlen = len(i)
            keylist.append(i)

        for i in keylist:
            wordlen = len(i)
            out = ""
            out += i                        # add color
            out += (maxlen - wordlen) * ' ' # add whitespace 
            out += ": "                     # add colon
            out += '*' * d[i]               # add asterisks
            print(out)







