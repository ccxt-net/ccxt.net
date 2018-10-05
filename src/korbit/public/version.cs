namespace CCXT.NET.Korbit
{
    /// <summary>
    /// 
    /// </summary>
    public class KVersion
    {
        /// <summary>
        /// 
        /// </summary>
        public int major
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int minor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int revision
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return major + "." + minor + "." + revision;
        }
    }
}