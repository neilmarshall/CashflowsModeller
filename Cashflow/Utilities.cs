using System;
using System.Collections.Generic;
using System.IO;

namespace Cashflow
{
    public static class Utilities
    {
        /*
         * reads data from a file and parses as a list of doubles, and throws
         * errors if file cannot be found, is empty, or cannot be parsed
         */

        public static List<double> ReadListOfDoublesFromFile(string filepath)
        {
            // check file exists / user has permission to access file
            if (!File.Exists(filepath))
                throw new FileNotFoundException(String.Format(
                    "{0} could not be found", filepath));

            // check file is not empty
            if (new FileInfo(filepath).Length == 0)
                throw new InvalidDataException(String.Format(
                    "{0} contains no data", filepath));

            // read data from file and parse as floating point numbers
            List<double> data = new List<double>();
            using (StreamReader sr = new StreamReader(filepath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (!double.TryParse(line, out double value))
                        throw new InvalidDataException(String.Format(
                            "{0} could not be parsed as a floating point number", line));
                    data.Add(value);
                }
            }
            return data;
        }
    }
}
