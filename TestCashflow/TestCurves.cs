using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

using Cashflow;

namespace TestCashflow
{
    [TestClass]
    public class TestCurves
    {

        [TestMethod]
        public void CurvesConstructorCalledWithStringCalculatesForwardCurveCorrectly()
        {
            Curve curve = new Curve("../../../TestData/DiscountRate.txt");
            Assert.AreEqual(0.00582044, curve.ForwardCurve[0], 1E-8);
            Assert.AreEqual(0.00747358, curve.ForwardCurve[1], 1E-8);
            Assert.AreEqual(0.00971311, curve.ForwardCurve[2], 1E-8);
            Assert.AreEqual(0.01204393, curve.ForwardCurve[3], 1E-8);
            Assert.AreEqual(0.01382910, curve.ForwardCurve[4], 1E-8);
        }

        [TestMethod]
        public void CurvesConstructorCalledWithListProducesSpotCurveAsListOfDoubles()
        {
            List<double> input = new List<double> { 0.00582044, 0.00664667, 0.00766778, 0.00876004, 0.00977182 };
            Curve curve = new Curve(input);
            Assert.IsInstanceOfType(curve.SpotCurve, typeof(List<double>));
        }

        [TestMethod]
        public void CurvesConstructorCalledWithListProducesForwardCurveAsListOfDoubles()
        {
            List<double> input = new List<double> { 0.00582044, 0.00664667, 0.00766778, 0.00876004, 0.00977182 };
            Curve curve = new Curve(input);
            Assert.IsInstanceOfType(curve.ForwardCurve, typeof(List<double>));
        }

        [TestMethod]
        public void CurvesConstructorCalledWithListCalculatesForwardCurveCorrectly()
        {
            List<double> input = new List<double> { 0.00582044, 0.00664667, 0.00766778, 0.00876004, 0.00977182 };
            Curve curve = new Curve(input);
            Assert.AreEqual(0.00582044, curve.ForwardCurve[0], 1E-8);
            Assert.AreEqual(0.00747358, curve.ForwardCurve[1], 1E-8);
            Assert.AreEqual(0.00971311, curve.ForwardCurve[2], 1E-8);
            Assert.AreEqual(0.01204393, curve.ForwardCurve[3], 1E-8);
            Assert.AreEqual(0.01382910, curve.ForwardCurve[4], 1E-8);
        }
    }
}
