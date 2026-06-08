using Fusion;
using UnityEngine;

public class CarModel : NetworkBehaviour
{
    [SerializeField] float _maxHP = 100;
    [SerializeField] float _currentHP = 100;

    [SerializeField] MeshRenderer _mesh;

    [Networked, OnChangedRender(nameof(OnColorChanged))]
    public Color MeshColor { get; set; }
    public void SetPosition(Vector3 pos) => transform.position = pos;
    public void SetRotation(Quaternion rot) => transform.rotation = rot;

    public override void Spawned()
    {

        if (_mesh == null) _mesh = GetComponentInChildren<MeshRenderer>();
        _mesh.material.color = MeshColor;
    }

    private void OnColorChanged()
    {
        _mesh.material.color = MeshColor;
    }
}