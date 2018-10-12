using System;
using System.Collections.Generic;
using System.IO;

namespace Cashflow
{
    public class Curve
    {
        protected List<double> _spotCurve;
        public List<double> SpotCurve { get => _spotCurve; }

        protected List<double> _forwardCurve;
        public List<double> ForwardCurve { get => _forwardCurve; }

        // class constructor - takes a filepath as an argument from which to read data
        public Curve(string filepath)
        {
            _spotCurve = Utilities.ReadListOfDoublesFromFile(filepath);
            _forwardCurve = GetSpotCurveFromForwardCurve(_spotCurve);
        }

        // class constructor - takes a List<double> as an argument
        public Curve(List<double> spotCurve)
        {
            if (spotCurve.Count == 0)
            {
                throw new InvalidDataException("Cannot call constructor with empty list");
            }

            _spotCurve = spotCurve;
            _forwardCurve = GetSpotCurveFromForwardCurve(_spotCurve);
        }

        // gets a forward curve from a spot curve
        private List<double> GetSpotCurveFromForwardCurve(List<double> spotCurve)
        {
            // a spot curve with one term generates an identical forward curve
            if (spotCurve.Count == 1) { return spotCurve; }

            // otherwise generate the curve - noting that the first terms are identical
            List<double> forwardCurve = new List<double> { spotCurve[0] };
            for (int term = 1; term < spotCurve.Count; term++) {
                double forward_rate = Math.Pow(1 + spotCurve[term], term + 1) / Math.Pow(1 + spotCurve[term - 1], term) - 1;
                forwardCurve.Add(forward_rate);
            }

            return forwardCurve;
        }
    }
}
