namespace CCXT.NET.Korbit
{
    /// <summary>
    /// 
    /// </summary>
    public class Version
    {
        /// <summary>
        /// 
        /// </summary>
        public int major;

        /// <summary>
        /// 
        /// </summary>
        public int minor;

        /// <summary>
        /// 
        /// </summary>
        public int revision;

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