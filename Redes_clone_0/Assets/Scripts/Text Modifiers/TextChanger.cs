using UnityEngine;

public class TextChanger : TextModifier
{
    [SerializeField] string[] _texts;
    [SerializeField] float _time = 0.5f;
    float _timer = 0;
    int _currentIndex = 0;

    private void Update()
    {
        if (_timer > _time)
        {
            _timer = 0;
            _currentIndex = (_currentIndex + 1) % _texts.Length;
            _text.text = _texts[_currentIndex];
        }
        else _timer += Time.deltaTime;
    }
}