namespace OdinSdk.BaseLib.Coin.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class XTriggerSite
    {
        /// <summary>
        /// 
        /// </summary>
        public string site_name
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal btc_price
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal ask_price
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal bid_price
        {
            get; set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class XTriggerMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public string quote_name
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string base_name
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public XTriggerSite a_site
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public XTriggerSite b_site
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal fee
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal profit_qty
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal profit_rate
        {
            get; set;
        }
    }
}