using Fusion;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

namespace Redes
{
    public class GhostCamera : SimulationBehaviour
    {
        List<PlayerRef> _players;
        PlayerRef _currentTarget;

        [SerializeField] Camera _ghostCam;
        [SerializeField] int _currentIndex = 0;
        [SerializeField] TMP_Text _text;

        Coroutine _targetRoutine;

        private void Awake()
        {
            ReferenceManager.GhostCam = this;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            ReferenceManager.GameManager.Runner.AddGlobal(this);
            PlayerDetector.OnPlayerJoined += CheckPlayers;
            PlayerDetector.OnPlayerLeft += CheckPlayers;
            CheckPlayers(default);
            _ghostCam.enabled = true;
            _targetRoutine = StartCoroutine(SetPlayerTarget());
        }

        private void OnDisable()
        {
            if (_targetRoutine != null)
                StopCoroutine(_targetRoutine);
            if (ReferenceManager.GameManager != null)
                ReferenceManager.GameManager.Runner.RemoveGlobal(this);
            PlayerDetector.OnPlayerJoined -= CheckPlayers;
            PlayerDetector.OnPlayerLeft -= CheckPlayers;
            _ghostCam.enabled = false;
        }

        private IEnumerator SetPlayerTarget()
        {
            Transform target = transform;
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _currentIndex = (_currentIndex - 1 + _players.Count) % _players.Count;
                    CheckPlayer(false, target);
                }
                else if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    _currentIndex = (_currentIndex + 1) % _players.Count;
                    CheckPlayer(true, target);
                }
                yield return null;
            }

            void CheckPlayer(bool ascending, Transform target)
            {
                if (_currentTarget == _players[_currentIndex]) return;

                _currentTarget = _players[_currentIndex];
                var obj = Runner.GetPlayerObject(_players[_currentIndex]).GetComponent<Player>();
                if (obj.IsGhost)
                {
                    _currentIndex = ascending ? (_currentIndex + 1) % _players.Count : (_currentIndex - 1 + _players.Count) % _players.Count;
                    CheckPlayer(ascending, target);
                    return;
                }
                target = obj.transform;
                transform.parent = target;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                _text.text = obj.name;
            }
        }

        private void CheckPlayers(PlayerRef _)
        {
            _players = Runner.ActivePlayers.ToList();
            if (_currentIndex >= _players.Count)
            {
                _currentIndex = _players.Count - 1;
            }
        }
    }
}