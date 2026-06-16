using System;
using UnityEngine;

namespace Redes
{
    public class WaitingCam : MonoBehaviour
    {
        [SerializeField] Camera _pivotCam;

        private void Awake()
        {
            ReferenceManager.PivotCam = this;
            GameManager.OnGameManagerSpawned += CheckManager;
        }
        private void OnEnable()
        {
            GameManager.OnGameStateChanged += CheckState;
        }
        private void OnDisable()
        {
            
            GameManager.OnGameStateChanged -= CheckState;
        }
        private void CheckManager(GameManager manager)
        {
            GameManager.OnGameManagerSpawned -= CheckManager;
            CheckState(manager.GameState);
        }

        private void CheckState(EGameState state)
        {
            if (state == EGameState.WaitingPlayers)
                _pivotCam.gameObject.SetActive(true);
            else _pivotCam.gameObject.SetActive(false);
        }

    }
}