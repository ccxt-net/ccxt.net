namespace OdinSdk.BaseLib.Coin
{
    /// <summary>
    /// api call result class
    /// </summary>
    public interface IApiResult
    {
        /// <summary>
        /// is success calling
        /// </summary>
        bool success
        {
            get;
            set;
        }

        /// <summary>
        /// error or success message
        /// </summary>
        string message
        {
            get;
            set;
        }

        /// <summary>
        /// status, error code
        /// </summary>
        int statusCode
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        ErrorCode errorCode
        {
            get;
            set;
        }

        /// <summary>
        /// check implemented
        /// </summary>
        bool supported
        {
            get;
            set;
        }
    }

    /// <summary>
    /// api call result class
    /// </summary>
    public class ApiResult : IApiResult
    {
        /// <summary>
        /// api call result class
        /// </summary>
        public ApiResult(bool success = false)
        {
            if (success == true)
                this.SetSuccess();
            else
                this.SetFailure();

            this.supported = true;
        }

        /// <summary>
        /// is success calling
        /// </summary>
        public virtual bool success
        {
            get;
            set;
        }

        /// <summary>
        /// error or success message
        /// </summary>
        public virtual string message
        {
            get;
            set;
        }

        /// <summary>
        /// status, error code
        /// </summary>
        public virtual int statusCode
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual ErrorCode errorCode
        {
            get;
            set;
        }

        /// <summary>
        /// check implemented
        /// </summary>
        public virtual bool supported
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public void SetResult(IApiResult result)
        {
            this.message = result.message;
            this.errorCode = result.errorCode;
            this.statusCode = result.statusCode;
            this.success = result.success;
        }

        /// <summary>
        ///
        /// </summary>
        public void SetSuccess(
                string message = "success", ErrorCode errorCode = ErrorCode.Success,
                int statusCode = 0, bool success = true
            )
        {
            this.message = message;
            this.statusCode = statusCode;
            this.errorCode = errorCode;
            this.success = success;
        }

        /// <summary>
        ///
        /// </summary>
        public void SetFailure(
                string message = "failure", ErrorCode errorCode = ErrorCode.Failure,
                int statusCode = -1, bool success = false
            )
        {
            this.message = message;
            this.statusCode = statusCode;
            this.errorCode = errorCode;
            this.success = success;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <returns></returns>
        public string MakeMarketId(string base_name, string quote_name)
        {
            return $"{base_name.ToUpper()}/{quote_name.ToUpper()}";
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApiResult<T> : IApiResult
    {
        /// <summary>
        ///
        /// </summary>
        T result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// api call result class with 'T'
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="success"></param>
        public ApiResult(bool success = false)
            : base(success)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public virtual T result
        {
            get;
            set;
        }
    }
}