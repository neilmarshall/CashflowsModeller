using System;
using System.Collections.Generic;

namespace Cashflow
{
    public class CashflowsException : Exception
    {
        public CashflowsException() {}

        public CashflowsException(string message) : base(message) {}

        public CashflowsException(string message, Exception inner) : base(message, inner) {}
    }

    public class Cashflows
    {
        public List<double> BaseCashflows { get; private set; }

        public Cashflows(string filepath)
        {
            BaseCashflows = Utilities.ReadListOfDoublesFromFile(filepath);
        }

        public Cashflows(List<double> cashflows)
        {
            BaseCashflows = cashflows;
        }

        // PV :: returns present value of cashflows, discounted using specified curve
        public double PV (Curve discountCurve)
        {
            if (BaseCashflows.Count != discountCurve.ForwardCurve.Count)
            {
                throw new CashflowsException(String.Format(
                    "discountCurve contains {0} rates but expected {1}",
                    discountCurve.ForwardCurve.Count, BaseCashflows.Count));
            }

            // TODO : could make this parameter variable in a future development
            const double timing_adj = 0.5;

            // working backwards from final cashflow, at each step add the current
            // cashflow and discount the running total by another year, using the
            // average forward rate, assuming cashflows occur halfway through each period
            double pv = 0;
            for (int i = BaseCashflows.Count - 1; i > 0; i--)
            {
                pv += BaseCashflows[i];
                pv /= Math.Pow((1 + discountCurve.ForwardCurve[i - 1]) * (1 + discountCurve.ForwardCurve[i]), timing_adj);
            }
            pv += BaseCashflows[0];
            pv /= Math.Pow(1 + discountCurve.ForwardCurve[0], 0.5);

            return pv;
        }


        // Inflate :: returns new Cashflows object with cashflows inflated by specified curve
        public Cashflows Inflate(Curve inflationCurve, int increase_month)
        {
            if (BaseCashflows.Count != inflationCurve.SpotCurve.Count)
            {
                throw new CashflowsException(String.Format(
                    "inflationCurve contains {0} rates but expected {1}",
                    inflationCurve.SpotCurve.Count, BaseCashflows.Count));
            }

            // create array of accumulation factors as compounded terms in 
            // inflation forward curve (adjusted for timing of cashflows); then
            // return a new Cashflows object with inflated cashflows
            List<double> accumulationFactors = new List<double> { Math.Pow(1 + inflationCurve.ForwardCurve[0], 1 - increase_month / 12.0) };
            for (int i = 1; i < BaseCashflows.Count; i++)
            {
                double accumulationFactor = accumulationFactors[i - 1];
                accumulationFactor *= Math.Pow(1 + inflationCurve.ForwardCurve[i - 1], increase_month / 12.0);
                accumulationFactor *= Math.Pow(1 + inflationCurve.ForwardCurve[i], 1 - increase_month / 12.0);
                accumulationFactors.Add(accumulationFactor);
            }

            List<double> inflatedCashflows = new List<double>();
            for (int i = 0; i < BaseCashflows.Count; i++)
            {
                inflatedCashflows.Add(BaseCashflows[i] * accumulationFactors[i]);
            }

            return new Cashflows(inflatedCashflows);
        }
    }
}
