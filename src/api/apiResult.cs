namespace CCXT.NET
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiResult()
        {
            this.status = -1;
            this.message = "";
        }

        /// <summary>
        /// 결과 상태 코드 (정상 : 0000, 정상이외 코드는 에러 코드 참조)
        /// </summary>
        public virtual int status
        {
            get;
            set;
        }

        /// <summary>
        /// 결과 메시지
        /// </summary>
        public virtual string message
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// data
        /// </summary>
        public virtual T data
        {
            get;
            set;
        }
    }
}