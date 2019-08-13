using Xunit;
using Xunit.Abstractions;

namespace XUnit
{
    [Collection("private test collection")]
    public partial class PrivateApi : XUnit.RootApi
    {
        /// <summary>
        ///
        /// </summary>
        public PrivateApi(ITestOutputHelper outputHelper)
                : base(outputHelper)
        {
            base.tRootFolder = @"..\..\..\private\json\";
        }
    }
}