using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject _mainWidget;
    [SerializeField] private GameObject _creditsWidget;
    [SerializeField] private string _gameScene = "Level_Blockout";

    public void CloseCredits()
    {
        _mainWidget.SetActive(true);
        _creditsWidget.SetActive(false);
    }

    public void OpenCredits()
    {
        _mainWidget.SetActive(false);
        _creditsWidget.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(_gameScene, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
