using System.Collections;
using UnityEngine;

public class UI_Deactivate : MonoBehaviour
{
    [SerializeField] float _timeToDeactivate = 5;
    private void OnEnable()
    {
        StartCoroutine(Deactivation());

    }

    IEnumerator Deactivation()
    {
        yield return new WaitForSeconds(_timeToDeactivate);
        gameObject.SetActive(false);
    }
}
