using System;
using System.Collections.Generic;
using System.IO;

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
        // TODO : consider making set property protected if felt valuable to be able to inherit this class
        public List<double> BaseCashflows { get; private set; }

        public Cashflows(string filepath)
        {
            BaseCashflows = Utilities.ReadListOfDoublesFromFile(filepath);
        }

        public Cashflows(List<double> cashflows)
        {
            if (cashflows.Count == 0)
            {
                throw new InvalidDataException("Cannot call constructor with empty list");
            }

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

            // create array of discount factors as compounded terms in discount forward curve
            // TODO : could make timing_adj parameter an input variable in a future development
            const double timing_adj = 0.5;
            List<double> discountFactors = AccumulateForwardCurve(discountCurve.ForwardCurve, timing_adj);

            // return sum of each cashflow divided by appropriate discount factor
            double pv = 0;
            for (int i = 0; i < BaseCashflows.Count; i++)
            {
                pv += BaseCashflows[i] / discountFactors[i];
            }

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

            // create array of accumulation factors as compounded terms in inflation forward curve
            List<double> accumulationFactors = AccumulateForwardCurve(inflationCurve.ForwardCurve, increase_month / 12.0);

            // return a new Cashflows object with inflated cashflows
            List<double> inflatedCashflows = new List<double>();
            for (int i = 0; i < BaseCashflows.Count; i++)
            {
                inflatedCashflows.Add(BaseCashflows[i] * accumulationFactors[i]);
            }

            return new Cashflows(inflatedCashflows);
        }

        // AccumulateForwardCurve :: Returns accumulated terms in a forward curve
        //
        // Rates are interpolated using specified timing parameter (specified as the
        // proportion of the first term to compound)
        private List<double> AccumulateForwardCurve(List<double> forwardCurve, double timing_adj)
        {
            List<double> accumulatedForwardCurve = new List<double>();

            // forward rate for initial time period
            accumulatedForwardCurve.Add(Math.Pow(1 + forwardCurve[0], 1 - timing_adj));

            // forward rates for subsequent time periods
            if (forwardCurve.Count > 1)
            {
                for (int i = 1; i < forwardCurve.Count; i++)
                {
                    double accumulatedForwardRate = Math.Pow(1 + forwardCurve[i - 1], timing_adj) * Math.Pow(1 + forwardCurve[i], 1 - timing_adj);
                
                    accumulatedForwardCurve.Add(accumulatedForwardCurve[i - 1] * accumulatedForwardRate);
                }
            }

            return accumulatedForwardCurve;
        }
    }
}
