using Fusion;
using UnityEngine;

public class PlayerModel : NetworkBehaviour
{
    [SerializeField] float _maxHP = 100;
    [SerializeField] float _currentHP = 100;

    [SerializeField] MeshRenderer _mesh;
    [SerializeField] public ForceMovement movement;

    [Networked, OnChangedRender(nameof(OnColorChanged))]
    public Color MeshColor { get; set; }
    public void SetPosition(Vector3 pos) => transform.position = pos;
    public void SetRotation(Quaternion rot) => transform.rotation = rot;

    public override void Spawned()
    {
        movement = GetComponent<ForceMovement>();
        if (_mesh == null) _mesh = GetComponentInChildren<MeshRenderer>();
        _mesh.material.color = MeshColor;
    }

    private void OnColorChanged()
    {
        _mesh.material.color = MeshColor;
    }
}