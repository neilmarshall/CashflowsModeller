using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.IO;

using Cashflow;

namespace TestCashflow
{
    [TestClass]
    public class TestCashflows
    {
        private List<double> discountRateList;
        private List<double> inflationRateList;
        private List<double> inputCashflowsList;

        [TestInitialize]
        public void Initialize()
        {
            discountRateList = new List<double> { 0.00582044, 0.00664667, 0.00766778, 0.00876004, 0.00977182 };
            inflationRateList = new List<double> { 0.02927650, 0.02937370, 0.02948620, 0.02962850, 0.02981620 };
            inputCashflowsList = new List<double> { 5483309, 8258105, 9996353, 11190735, 12165875 };
        }

        [TestMethod]
        public void CashflowsConstructorCalledWithStringReturnsCashflowsOnCall()
        {
            Curve discountCurve = new Curve(discountRateList);
            Cashflows cashflows = new Cashflows("../../../TestData/Cashflows.txt");
            List<double> baseCashflows = cashflows.BaseCashflows;
            Assert.AreEqual(5483309, baseCashflows[0]);
            Assert.AreEqual(8258105, baseCashflows[1]);
            Assert.AreEqual(9996353, baseCashflows[2]);
            Assert.AreEqual(11190735, baseCashflows[3]);
            Assert.AreEqual(12165875, baseCashflows[4]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CashflowConstructorCalledWithEmptyListThrowsError()
        {
            // TODO : assert form of error message when file is empty
            Cashflows cashflows = new Cashflows(new List<double>());
        }

        [TestMethod]
        public void CashflowsConstructorCalledWithListReturnsCashflowsOnCall()
        {
            Curve discountCurve = new Curve(discountRateList);
            Cashflows cashflows = new Cashflows(inputCashflowsList);
            List<double> baseCashflows = cashflows.BaseCashflows;
            Assert.AreEqual(5483309, baseCashflows[0]);
            Assert.AreEqual(8258105, baseCashflows[1]);
            Assert.AreEqual(9996353, baseCashflows[2]);
            Assert.AreEqual(11190735, baseCashflows[3]);
            Assert.AreEqual(12165875, baseCashflows[4]);
        }

        [TestMethod]
        public void CashflowsCalculatesPVCorrectly()
        {
            Curve discountCurve = new Curve(discountRateList);
            Cashflows cashflows = new Cashflows(inputCashflowsList);
            Assert.AreEqual(46004733, cashflows.PV(discountCurve), 1);
        }

        [TestMethod]
        public void CashflowsInflatesCashflowsCorrectly()
        {
            Curve inflationCurve = new Curve(inflationRateList);
            Cashflows cashflows = new Cashflows(inputCashflowsList);
            Cashflows inflatedCashflows = cashflows.Inflate(inflationCurve, 12);
            double inflatedCashflowSum = 0;
            foreach (double inflatedCashflow in inflatedCashflows.BaseCashflows) {
                inflatedCashflowSum += inflatedCashflow;
            }
            Assert.AreEqual(50458603, inflatedCashflowSum, 1);
        }

        [TestMethod]
        public void CashflowsCalculatesPVOfInflatedCashflowsCorrectly()
        {
            Curve discountCurve = new Curve("../../../TestData/DiscountRate.txt");
            Curve inflationCurve = new Curve("../../../TestData/InflationRate.txt");
            Cashflows cashflows = new Cashflows("../../../TestData/Cashflows.txt");
            Cashflows inflatedCashflows = cashflows.Inflate(inflationCurve, 12);
            Assert.AreEqual(920913970, inflatedCashflows.PV(discountCurve), 1);
        }

        [TestMethod]
        [ExpectedException(typeof(CashflowsException))]
        public void CashflowsPVMethodRaisesErrorOnCurveDimensionMismatch() {
            discountRateList.RemoveAt(discountRateList.Count - 1);
            Curve discountCurve = new Curve(discountRateList);
            Cashflows cashflows = new Cashflows(inputCashflowsList);
            cashflows.PV(discountCurve);
        }

        [TestMethod]
        [ExpectedException(typeof(CashflowsException))]
        public void CashflowsInflateMethodRaisesErrorOnCurveDimensionMismatch()
        {
            inflationRateList.RemoveAt(inflationRateList.Count - 1);
            Curve inflationCurve = new Curve(inflationRateList);
            Cashflows cashflows = new Cashflows(inputCashflowsList);
            cashflows.Inflate(inflationCurve, 12);
        }
    }
}
