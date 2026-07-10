using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Countdown : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] int _maxBound = 5;
    [SerializeField] Vector2 _scaleSize;
    [SerializeField] AnimationCurve _alphaCurve;
    [SerializeField] AnimationCurve _sizeLerpCurve;
    Coroutine _routine;

    private void OnEnable()
    {
        Host.GameManager.OnCounter += SetNumber;
        _text.gameObject.SetActive( false );
    }

    private void OnDisable()
    {
        if ( _routine != null )
            StopCoroutine( _routine );
        Host.GameManager.OnCounter -= SetNumber;
    }

    public void SetNumber(int num)
    {
        Debug.Log("number sent to UI: " + num);
        if (num > _maxBound) return;

        if (!_text.gameObject.activeSelf) _text.gameObject.SetActive(true);
        _text.text = num <= 0 ? "GO!" : num.ToString();
        if (_routine != null)
            StopCoroutine(_routine);
        _routine = StartCoroutine(ChangeSize());
    }

    IEnumerator ChangeSize()
    {
        float t = 0;
        while (t < 1)
        {
            transform.localScale = Vector3.one * Mathf.Lerp(_scaleSize.x, _scaleSize.y, _sizeLerpCurve.Evaluate(t));
            _text.alpha = _alphaCurve.Evaluate(t);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        _text.gameObject.SetActive(false);
        
        _routine = null;
    }
}
