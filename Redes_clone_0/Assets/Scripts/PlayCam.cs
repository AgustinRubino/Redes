using Fusion;
using System.Collections;
using UnityEngine;

public class PlayCam : MonoBehaviour
{
    [SerializeField] Camera _cam;
    [SerializeField] float _dashFOV = 80;
    [SerializeField] float _dashFOVBeginTime = 1;
    [SerializeField] float _dashFOVWaitTime = 1;
    [SerializeField] float _dashFOVEndTime = 2;

    float _baseFOV;
    Coroutine _dashFovRoutine;

    private void Start()
    {
        _baseFOV = _cam.fieldOfView;
    }

    public void ActiveDash()
    {
        if (_dashFovRoutine != null)
        {
            StopCoroutine(_dashFovRoutine);
            _cam.fieldOfView = _baseFOV;
        }
        _dashFovRoutine = StartCoroutine(DashEffect(_dashFOVBeginTime, _dashFOVEndTime));
    }

    IEnumerator DashEffect(float begin, float end)
    {
        float t = 0;
        float mult = 1 / begin;
        while (t < 1)
        {
            _cam.fieldOfView = Mathf.Lerp(_baseFOV, _dashFOV, t);
            t += Time.deltaTime * mult;
            yield return null;
        }
        _cam.fieldOfView = _dashFOV;

        yield return new WaitForSeconds(_dashFOVWaitTime);

        t = 0;
        mult = 1 / end;
        while (t < 1)
        {
            _cam.fieldOfView = Mathf.Lerp(_dashFOV, _baseFOV, t);
            t += Time.deltaTime * mult;
            yield return null;
        }
        _cam.fieldOfView = _baseFOV;
        _dashFovRoutine = null;
    }
}
