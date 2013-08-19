using System;
using System.Collections.Generic;

namespace Outlier
{
    /// <summary>
    /// This represents an abstract detector of outliers. 
    /// </summary>
    public interface IOutlierDetector
    {
        /// <summary>
        /// Gets the outliers from the specified set of data.
        /// </summary>
        /// <param name="data">The data to be examined.</param>
        /// <returns></returns>
        List<PriceData> GetOutliers(IEnumerable<PriceData> data);

        /// <summary>
        /// Gets a clean set of data by removing the specified set of outliers 
        /// from the specifiec set of data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="outliers">The outliers.</param>
        /// <returns></returns>
        List<PriceData> GetCleanData(
            IEnumerable<PriceData> data,
            IEnumerable<PriceData> outliers);
    }
}
