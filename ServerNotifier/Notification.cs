using System;

namespace ServerNotifier
{
    internal abstract partial class Notification
    {
        #region Fields
        private static readonly PlatformID platform = Environment.OSVersion.Platform;
        #endregion

        #region Properties
        public string Summary { get; set; }

        public string Body { get; set; }
        #endregion

        #region Methods
        public static Notification Create()
        {
            switch (platform)
            {
                case PlatformID.Unix:
                    return new Unix();
                default:
                    throw new PlatformNotSupportedException();
            }
        }

        public abstract void Show();
        #endregion
    }
}
