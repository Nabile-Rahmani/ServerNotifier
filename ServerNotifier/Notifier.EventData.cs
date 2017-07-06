using Steam.Query;

namespace ServerNotifier
{
    public partial class Notifier
    {
        public struct EventData
        {
            #region Properties
            public Server Server { get; set; }

            public ServerInfoResult Info { get; set; }
            #endregion
        }
    }
}
