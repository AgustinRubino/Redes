using UnityEngine;
using TMPro;

public abstract class TextModifier : MonoBehaviour
{
    protected TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }
}
