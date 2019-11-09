using System;
using System.Collections.Generic;
using System.Text;

namespace HostCommunication.HostModels
{
    public class DbDescription
    {
        public string Name { get; set; }

        public string Server { get; set; }

        public bool Exists { get; set; }

        public bool IsCurrentlyConnected { get; set; }

        public int WorkingOrderWhenPaired { get; set; }

        public bool IsFromCurrentMaster { get; set; }

        public MirrorSide MirrorSide { get; set; }

        public List<DbDescription> DbMirrors { get; set; } = new List<DbDescription>();

    }
}
