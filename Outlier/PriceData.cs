using System;

namespace Outlier
{
    /// <summary>
    /// The data structure of a data point.
    /// </summary>
    public class PriceData : IEquatable<PriceData>
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }

        public bool Equals(PriceData other)
        {
            return
                this.Date.Equals(other.Date) &&
                this.Price.Equals(other.Price);
        }
    }
}
