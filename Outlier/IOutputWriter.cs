using System;
using System.Collections.Generic;

namespace Outlier
{
    /// <summary>
    /// This represents an abstract data output writer.
    /// </summary>
    public interface IOutputWriter
    {
        /// <summary>
        /// Writes the output.
        /// </summary>
        /// <param name="destination">The destination and this can be anything from a file path to a connection string.</param>
        /// <param name="data">The data to be written.</param>
        void WriteOutput(string destination, IEnumerable<PriceData> data);
    }
}
