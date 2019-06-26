namespace CCXT.NET.Coin
{
    /// <summary>
    ///
    /// </summary>
    public interface INameResult
    {
#if DEBUG
        /// <summary>
        ///
        /// </summary>
        string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    ///
    /// </summary>
    public class NameResult : ApiResult<string>, INameResult
    {
        /// <summary>
        ///
        /// </summary>
        public NameResult(bool success = true)
        {
            if (success == true)
                this.SetSuccess();
            else
                this.SetFailure();
        }

#if DEBUG
        /// <summary>
        ///
        /// </summary>
        public virtual string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    ///
    /// </summary>
    public interface IBoolResult
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class BoolResult : ApiResult<bool>, IBoolResult
    {
        /// <summary>
        ///
        /// </summary>
        public BoolResult(bool success = true)
        {
            if (success == true)
            {
                this.SetSuccess();
                this.result = true;
            }
            else
            {
                this.SetFailure();
                this.result = false;
            }
        }
    }
}