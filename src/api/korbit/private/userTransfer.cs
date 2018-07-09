namespace CCXT.NET.Korbit.Private
{
    public class UserTransferDetail
    {
        /// <summary>
        /// 코인의 거래 ID
        /// </summary>
        /// <returns></returns>
        public string transaction_id;

        /// <summary>
        /// 코인의 예금주
        /// </summary>
        /// <returns></returns>
        public string address;

        /// <summary>
        /// 코인이 XRP인 경우에만 표시되는 destination tag
        /// </summary>
        /// <returns></returns>
        public string destination_tag;
    
        /// <summary>
        /// 거래에 사용된 은행의 이름
        /// </summary>
        /// <returns></returns>
        public string bank;

        /// <summary>
        /// 거래에 사용된 계좌번호
        /// </summary>
        /// <returns></returns>
        public string account;

        /// <summary>
        /// 예금주
        /// </summary>
        /// <returns></returns>
        public string owner;
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserTransfer
    {
        /// <summary>
        /// 입출금의 ID 식별번호
        /// </summary>
        public ulong id;

        /// <summary>
        /// 입출금의 종류로 입금(deposit) 또는 출금(withdrawal)이 나오게 된다.
        /// </summary>
        public string type;

        /// <summary>
        /// 주문이 이루어진 화폐 단위.
        /// </summary>
        public string currency;

        /// <summary>
        /// 입출금된 화폐의 수량.
        /// </summary>
        public decimal amount;

        /// <summary>
        /// 입출금이 완료된 시각. 입출금이 완료되지 않았을 때는 이 필드는 표시되지 않는다. 
        /// Unix timestamp(milliseconds)로 제공된다.
        /// </summary>
        public long completed_at;

        /// <summary>
        /// 입출금 주문이 새로이 갱신된 시각. 이 필드값을 기준으로 입출금 항목이 최신순으로 정렬되어 리턴된다. 
        /// Unix timestamp(milliseconds)로 제공된다.
        /// </summary>
        public long updated_at;

        /// <summary>
        /// 입출금이 주문된 시각. Unix timestamp(milliseconds)로 제공된다.
        /// </summary>
        public long created_at;

        /// <summary>
        /// 현재 입출금 주문의 상태.
        /// </summary>
        public string status;

        /// <summary>
        /// 출금액에서 차감된 출금수수료. 수수료의 화폐 단위는 출금된 화폐와 같다. 수수료가 발생한 경우에만 이 필드가 표시된다.
        /// </summary>
        public decimal fee;

        /// <summary>
        /// 서브필드 (코인 또는 원화 입출금시)
        /// </summary>
        public UserTransferDetail details;
    }
}