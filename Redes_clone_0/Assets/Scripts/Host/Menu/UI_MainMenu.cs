using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] Button _playBTN;
    [SerializeField] Button _howToBTN;
    [SerializeField] Button _backBTN;
    [SerializeField] Button _quitBTN;
    [Space(10)]
    [SerializeField] GameObject _howToMenu;

    private void Awake()
    {
        _playBTN.onClick.AddListener(() => SceneManager.LoadScene(SceneIndex.LobbyMenu));
        _howToBTN.onClick.AddListener(OpenHowToMenu);
        _backBTN.onClick.AddListener(CloseHowToMenu);
        _quitBTN.onClick.AddListener(() => Application.Quit());
    }

    void OpenHowToMenu()
    {
        _playBTN.interactable = false;
        _quitBTN.interactable = false;
        _howToBTN.interactable = false;
        _howToMenu.SetActive(true);
    }
    void CloseHowToMenu()
    {
        _playBTN.interactable = true;
        _quitBTN.interactable = true;
        _howToBTN.interactable = true;
        _howToMenu.SetActive(false);
    }
}
