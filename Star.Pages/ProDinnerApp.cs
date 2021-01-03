using Star.Core;

namespace Star.Pages
{
    public class ProDinnerApp : ProDinnerPage
    {
        #region Constructors

        public ProDinnerApp(IWebTest test) : base(ref test)
        {
        }

        #endregion

        #region Public methods

        public override bool IsActive()
        {
            return true;
        }

        #endregion
    }
}
