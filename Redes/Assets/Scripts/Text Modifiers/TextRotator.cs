using UnityEngine;

public class TextRotator : TextModifier
{
    [SerializeField] private Vector2 _maxAngle = new(-30, 30);
    [SerializeField] private float _speed = 1f;

    private void Update()
    {
        var a = (Mathf.Sin(_speed * Time.time) + 1) * 0.5f;
        transform.localRotation = Quaternion.Euler(Vector3.forward * Mathf.Lerp(_maxAngle.x, _maxAngle.y, a));
    }
}
