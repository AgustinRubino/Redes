using Fusion;
using UnityEngine;

namespace Host
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] public Transform[] spawnPositions;

        public static GameManager Local { get; private set; }

        #region MonoBehaviour
        private void Awake()
        {
            if (Local == null)
                Local = this;
        }
        #endregion
    }
}