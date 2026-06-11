using UnityEngine;
public class TextScale : TextModifier
{
    [SerializeField] private Vector2 _scale = new(0.8f, 1.2f);
    [SerializeField] private float _speed = 1f;

    private void Update()
    {
        float a = (Mathf.Sin(_speed * Time.time) + 1) * 0.5f;
        transform.localScale = Vector3.one * Mathf.Lerp(_scale.x, _scale.y,  a);
    }
}
