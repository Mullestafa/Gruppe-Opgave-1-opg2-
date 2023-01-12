import unittest
import steps
import io
import sys

class TestSteps(unittest.TestCase):
    def test_AddConstant(self):
        testConstant = steps.AddConst(5)
        self.assertEqual(testConstant.apply(5), 10)
        self.assertEqual(testConstant.description(), "add 5 to input")

    def test_Repeater(self):
        testRepeater = steps.Repeater(5)
        testValue = testRepeater.apply(12)
        self.assertEqual(len(testValue), 5)
        self.assertEqual(testValue[0], 12)
        self.assertEqual(testRepeater.description(), 'give list containing input 5 times')

    def test_GeneralSum(self):
        testGeneralSumMull = steps.GeneralSum(1,'*')
        self.assertEqual(testGeneralSumMull.apply([5,2,3]), 30)
        testGeneralSumAdd = steps.GeneralSum(0,'+')
        self.assertEqual(testGeneralSumAdd.apply([5,2,3]), 10)
        self.assertEqual(testGeneralSumAdd.description(), "give value after accumulating (acc + elm) to each element in input list (starting with acc = 0)")

    def test_SumNum(self):
        testSumNum = steps.SumNum()
        self.assertEqual(testSumNum.apply([5,2,3]), 10)
        self.assertEqual(testSumNum.description(), "give value after accumulating (acc + elm) to each element in input list (starting with acc = 0)")

    def test_ProductNum(self):
        testProductNum = steps.ProductNum()
        self.assertEqual(testProductNum.apply([5,2,3]), 30)
        self.assertEqual(testProductNum.description(), "give value after accumulating (acc * elm) to each element in input list (starting with acc = 1)")

    def test_Map(self):
        testMap = steps.Map(steps.AddConst(5))
        self.assertEqual(testMap.apply([1,2,3,4,5]), [6,7,8,9,10])
        self.assertEqual(testMap.description(),"for every element in input list: add 5 to input")

    def test_Pipeline(self):
        add_const = steps.AddConst(5)
        repeater = steps.Repeater(3)
        pipeline = steps.Pipeline([add_const, repeater])

        # test apply method
        self.assertEqual(pipeline.apply(3), [8, 8, 8])
        self.assertEqual(pipeline.apply(-2), [3, 3, 3])

        # test add_step method
        product_num = steps.ProductNum()
        pipeline.add_step(product_num)
        self.assertEqual(pipeline.apply(-2), 27)

        # test description method
        self.assertEqual(pipeline.description(), "add 5 to input\ngive list containing input 3 times\ngive value after accumulating (acc * elm) to each element in input list (starting with acc = 1)")

    def test_CsvReader(self):
        testCsvReader = steps.CsvReader()
        testDataset = testCsvReader.apply('critters.csv')
        self.assertEqual(str(testDataset[0]),"OrderedDict([('Name', 'Poppy'), ('Colour', 'peru'), ('Hit Points', '2')])")
        self.assertEqual(testCsvReader.description(),"Make csv file into list of dicts")
    
    def test_CritterStats(self):
        testCsvReader = steps.CsvReader()
        testDataset = testCsvReader.apply('critters.csv')
        testCritterStats = steps.CritterStats()
        self.assertEqual(testCritterStats.apply(testDataset)['peru'], 5)

    def test_ShowAsciiBarchart(self):
        testShowAsciiBarchart = steps.ShowAsciiBarchart()
        input_ = {"Red":2, "Green":1}
        capturedOutput = io.StringIO()
        sys.stdout = capturedOutput
        testShowAsciiBarchart.apply(input_)
        sys.stdout = sys.__stdout__ 
        self.assertEqual(capturedOutput.getvalue(), 'Red  : **\nGreen: *\n')  


if __name__ == '__main__':
    unittest.main()