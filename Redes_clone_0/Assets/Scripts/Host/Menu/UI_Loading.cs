using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Host
{

    public class UI_Loading : MonoBehaviour
    {
        [SerializeField] UI_LobbyMenu _lobby;
        [SerializeField] float _time = 5;
        Coroutine _routine;
        private void OnEnable()
        {
            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(Wait());
        }

        void OnDisable() {
            if (_routine != null) StopCoroutine(_routine); 
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(_time);
            _lobby.OpenLobby();
        }
    }

}