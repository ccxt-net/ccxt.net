using Xunit;
using Xunit.Abstractions;

namespace XUnit
{
    [Collection("public test collection")]
    public partial class PublicApi : XUnit.RootApi
    {
        /// <summary>
        /// 
        /// </summary>
        public PublicApi(ITestOutputHelper outputHelper)
                : base(outputHelper)
        {
            base.tRootFolder = @"..\..\..\public\json\";
        }
    }
}