using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class UI_ProgressBar : NetworkBehaviour
{
    bool _isInitialized = false;

    [Header("Refereneces")]
    [SerializeField] Slider _slider;
    [SerializeField] Image _mainHandle;

    private void OnEnable()
    {
        if (_isInitialized) return;

        
    }
}
