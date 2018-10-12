using Xunit;
using Xunit.Abstractions;

namespace XUnit
{
    [Collection("trade test collection")]
    public partial class TradeApi : XUnit.RootApi
    {
        /// <summary>
        /// 
        /// </summary>
        public TradeApi(ITestOutputHelper outputHelper)
                : base(outputHelper)
        {
            base.tRootFolder = @"..\..\..\trade\json\";
        }
    }
}