using System;
using System.Collections.Generic;

namespace Outlier
{
    /// <summary>
    /// This represents an abstract data source for the price data.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="source">The source and this can be anything from a file path or a connection string.</param>
        /// <returns></returns>
        List<PriceData> GetData(string source);
    }
}
