using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel.Composition;
using System.Linq;

namespace Outlier.SimpleDetector
{
    /// <summary>
    /// This is an outlier detector.
    /// Note that the "Export" attribute is related to the use of MEF
    /// (see http://msdn.microsoft.com/en-us/library/dd460648.aspx).
    /// </summary>
    [Export(typeof(IOutlierDetector))]
    public class OutlierDetector : IOutlierDetector
    {
        /// <summary>
        /// Gets the outliers from the specified set of data.
        /// </summary>
        /// <param name="data">The data to be examined.</param>
        /// <returns></returns>
        public List<PriceData> GetOutliers(
            IEnumerable<PriceData> data)
        {
            List<PriceData> outliers = new List<PriceData>();

            if (data == null)
                return outliers;

            var dataSize = data.Count();

            if (dataSize == 0)
                return outliers;

            int sampleSize = int.Parse(ConfigurationManager.AppSettings["Outlier.SimpleDetector.SampleSize"]);
            if (sampleSize > dataSize)
            {
                return GetOutliersFromSamples(data).ToList();
            }

            for (int i = 0; i < dataSize - sampleSize + 1; i++)
            {
                outliers.AddRange(
                    GetOutliersFromSamples(
                        data.Skip(i).Take(sampleSize)));
            }
            return outliers.Distinct().ToList();
        }

        /// <summary>
        /// Gets a clean set of data by removing the specified set of outliers
        /// from the specifiec set of data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="outliers">The outliers.</param>
        /// <returns></returns>
        public List<PriceData> GetCleanData(
            IEnumerable<PriceData> data, 
            IEnumerable<PriceData> outliers)
        {
            return GetCleanDataQuery(data, outliers).ToList();
        }

        #region helper methods
        /// <summary>
        /// Gets the outliers from the specified set of samples.
        /// </summary>
        /// <remarks>Screen level is set according the number of standard deviation defined 
        /// in application configuration. The sample data size is also configured in a similar way.</remarks>
        /// <param name="sampleData">The sample data.</param>
        /// <returns></returns>
        private static IEnumerable<PriceData> GetOutliersFromSamples(IEnumerable<PriceData> sampleData)
        {
            if (sampleData.Count() == 0)
                yield break;

            var average = sampleData.Average(i => i.Price);
            var sd = GetStandardDeviation(sampleData);

            var screenLevel = int.Parse(ConfigurationManager.AppSettings["Outlier.SimpleDetector.ScreenLevel"]);
            var lowerBound = average - (screenLevel * sd);
            var upperBound = average + (screenLevel * sd);

            foreach (var i in sampleData)
                if (i.Price < lowerBound || i.Price > upperBound)
                    yield return i;
        }

        /// <summary>
        /// Gets the standard deviation from the specified set of data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private static double GetStandardDeviation(IEnumerable<PriceData> data)
        {
            var average = data.Average(i => i.Price);
            var sum = data.Sum(i => Math.Pow(i.Price - average, 2));
            return Math.Sqrt((sum) / (data.Count() - 1)); 
        }

        /// <summary>
        /// Gets a representation of the clean data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="outliers">The outliers.</param>
        /// <returns></returns>
        private static IEnumerable<PriceData> GetCleanDataQuery(
            IEnumerable<PriceData> data,
            IEnumerable<PriceData> outliers)
        {
            return 
                from i in data
                where !outliers.Contains(i)
                select i;
        }
        #endregion
    }
}
