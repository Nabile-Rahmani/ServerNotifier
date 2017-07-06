using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Steam.Query;

namespace ServerNotifier
{
    public partial class Notifier
    {
        #region Fields
        private readonly Dictionary<Server, byte> playerCounts = new Dictionary<Server, byte>();
        #endregion

        #region Properties
        /// <summary>
        /// The server list to listen to.
        /// </summary>
        public IList<Server> Servers { get; } = new List<Server>();

        /// <summary>
        /// Notify when this amount of players is reached.
        /// </summary>
        public byte PlayerCountTrigger { get; set; } = 1;

        /// <summary>
        /// Frequency of queries.
        /// </summary>
        public TimeSpan UpdateDelay { get; set; } = TimeSpan.FromSeconds(30.0);

        public bool IsRunning { get; private set; }
        #endregion

        #region Events
        public event Action<Notifier, EventData> PlayerCountReached;
        #endregion

        #region Methods
        private void Update()
        {
            foreach (var server in playerCounts.Keys.Except(Servers))
            {
                playerCounts.Remove(server);
            }

            Parallel.ForEach(Servers, server =>
            {
                ServerInfoResult info = server.GetServerInfo().Result;

                if (playerCounts.ContainsKey(server) && info.Players == playerCounts[server])
                    return;

                playerCounts[server] = info.Players;

                if (info.Players >= PlayerCountTrigger)
                    PlayerCountReached?.Invoke(this, new EventData
                    {
                        Server = server,
                        Info = info
                    });
            });
        }

        public void Run()
        {
            IsRunning = true;

            while (IsRunning)
            {
                Update();
                Thread.Sleep(UpdateDelay);
            }
        }

        public void Stop() => IsRunning = false;
        #endregion
    }
}
