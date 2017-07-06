namespace ServerNotifier
{
    internal abstract partial class Notification
    {
        public class Unix : Notification
        {
            #region Methods
            public override void Show()
            {
                new Notifications.Notification
                {
                    Summary = Summary,
                    Body = Body
                }.Show();
            }
            #endregion
        }
    }
}
