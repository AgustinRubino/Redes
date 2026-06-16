using System.Collections.Generic;
using UnityEngine;

namespace Redes
{
    [CreateAssetMenu(menuName = "SO/Car Models", fileName = "Car Models")]
    public class CarModels : ScriptableObject
    {
        [SerializeField] private List<GameObject> _models;

        public List<GameObject> Models => _models;
    }
}