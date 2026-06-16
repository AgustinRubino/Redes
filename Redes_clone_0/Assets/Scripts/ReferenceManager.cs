using UnityEngine;

namespace Redes
{
    public class ReferenceManager
    {
        public static GameManager GameManager { get; set; }
        public static Player Player { get; set; }
        public static PlayerConfig Config { get; set; }
        public static GhostCamera GhostCam { get; set; }
        public static WaitingCam PivotCam { get; set; }
    }
}