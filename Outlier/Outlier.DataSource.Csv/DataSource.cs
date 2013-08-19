using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace Outlier.DataSource.Csv
{
    /// <summary>
    /// This is a data source using CSV files.
    /// Note that the "Export" attribute is related to the use of MEF
    /// (see http://msdn.microsoft.com/en-us/library/dd460648.aspx).
    /// </summary>
    [Export(typeof(IDataSource))]
    public class DataSource : IDataSource
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="source">The path to the CSV file containing data points.</param>
        /// <returns></returns>
        public List<PriceData> GetData(string source)
        {
            using (StreamReader r = new StreamReader(source))
            {
                var firstLine = r.ReadLine();
                var data = this.GetData(r);
                return data.ToList();
            }
        }

        /// <summary>
        /// Gets the data from a stream reader reading a CSV file.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private IEnumerable<PriceData> GetData(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var fields = line.Split(new char[] { ',' });
                var date = DateTime.Parse(fields.FirstOrDefault());
                var value = double.Parse(fields.LastOrDefault());
                yield return 
                    new PriceData
                    {
                        Date = date,
                        Price = value,
                    };
            }
        }
    }
}
