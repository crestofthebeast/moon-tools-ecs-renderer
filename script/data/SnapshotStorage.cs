using System.Collections.Generic;
using MoonTools.ECS;

namespace GodotMoonTools.Data
{
    public static class SnapshotStorage
    {
        static Snapshot Snapshot;

        public static void SetSnapshot(Snapshot snapshot)
        {
            Snapshot = snapshot;
        }

        public static Snapshot GetSnapshot()
        {
            return Snapshot;
        }
    }
}