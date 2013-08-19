using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace Outlier.OutputWriter.Csv
{
    /// <summary>
    /// This is a data output writer to CSV files.
    /// Note that the "Export" attribute is related to the use of MEF
    /// (see http://msdn.microsoft.com/en-us/library/dd460648.aspx).
    /// </summary>
    [Export(typeof(IOutputWriter))]
    public class OutputWriter : IOutputWriter
    {
        /// <summary>
        /// Writes the output.
        /// </summary>
        /// <param name="destination">The path to the destination CSV file.</param>
        /// <param name="data">The data to be written.</param>
        public void WriteOutput(string destination, IEnumerable<PriceData> data)
        {
            using (StreamWriter w = new StreamWriter(destination))
            {
                w.WriteLine("Date,Price");
                this.WriteOutput(w, data);
            }
        }

        /// <summary>
        /// Writes the output to the specified stream writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="data">The data.</param>
        private void WriteOutput(StreamWriter writer, IEnumerable<PriceData> data)
        {
            foreach (var item in data)
            {
                writer.WriteLine(
                    string.Format(
                        "{0},{1}",
                        item.Date.ToString("dd/MM/yyyy"),
                        item.Price));
            }
        }
    }
}
