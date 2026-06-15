using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Countdown : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] float initScale = 3;
    [SerializeField] float targetScale = 1;
    [SerializeField] float duration = 0.2f;

    Coroutine _routine;

    private void OnEnable()
    {
        GameManager.Instance.startCounter.OnCounterChange += SetNumber;
    }

    private void OnDisable()
    {
        if ( _routine != null )
            StopCoroutine( _routine );
    }

    private void SetNumber(int obj)
    {
        bool end = obj <= 0;
        _text.text = end ? "GO!" : obj.ToString();
        if (_routine != null)
            StopCoroutine(_routine);
        _routine = StartCoroutine(ChangeSize(end));
    }

    IEnumerator ChangeSize(bool end)
    {
        float t = 0;
        float mult = 1 / duration;
        while (t < duration)
        {
            transform.localScale = Vector3.one * Mathf.Lerp(initScale, targetScale, t);
            t += Time.deltaTime * mult;
            yield return null;
        }
        transform.localScale = Vector3.one * targetScale;

        if (end)
        {
            _routine = null;
            yield break;
        }
        GameManager.Instance.startCounter.OnCounterChange -= SetNumber;

        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
