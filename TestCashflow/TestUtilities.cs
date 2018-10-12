using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.IO;

using Cashflow;

namespace TestCashflow
{
    [TestClass]
    public class TestUtilities
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ReadListOfDoublesFromFileThrowsErrorIfFileDoesntExist()
        {
            // TODO : assert form of error message when file does not exist
            Utilities.ReadListOfDoublesFromFile("NonExistentFile.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadListOfDoublesFromFileThrowsErrorIfFileEmpty()
        {
            // TODO : assert form of error message when file is empty
            Utilities.ReadListOfDoublesFromFile("../../../TestData/EmptyTextFile.txt");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadListOfDoublesFromFileThrowsErrorIfDataCannotBeParsed()
        {
            // TODO : assert form of error message when file contains bad data
            Utilities.ReadListOfDoublesFromFile("../../../TestData/BadInputFile.txt");
        }

        [TestMethod]
        public void ReadListOfDoublesFromFileProducesListOfDoubles()
        {
            Assert.IsInstanceOfType(Utilities.ReadListOfDoublesFromFile("../../../TestData/DiscountRate.txt"),
                                    typeof(List<double>));
        }

        [TestMethod]
        public void ReadListOfDoublesFromFileReadsDataCorrectly()
        {
            List<double> data = Utilities.ReadListOfDoublesFromFile("../../../TestData/DiscountRate.txt");
            Assert.AreEqual(0.00582044, data[0], 1E-8);
            Assert.AreEqual(0.00664667, data[1], 1E-8);
            Assert.AreEqual(0.00766778, data[2], 1E-8);
            Assert.AreEqual(0.00876004, data[3], 1E-8);
            Assert.AreEqual(0.00977182, data[4], 1E-8);

        }
    }
}
