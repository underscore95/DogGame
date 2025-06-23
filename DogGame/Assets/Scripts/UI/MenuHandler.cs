using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject _mainWidget;
    [SerializeField] private GameObject _creditsWidget;
    [SerializeField] private string _gameScene = "Level_Blockout";
    [SerializeField] private CinemachineSplineDolly _cutsceneSpline;
    [SerializeField] private float _cutsceneDuration = 5.0f;

    private bool _isReverseCutscene = false;
    private void Update()
    {
        _cutsceneSpline.CameraPosition += Time.deltaTime / _cutsceneDuration * (_isReverseCutscene ? -1 : 1);
        if (_cutsceneSpline.CameraPosition >= 1) _isReverseCutscene = true;
        else if (_cutsceneSpline.CameraPosition <= 0) _isReverseCutscene = false;
    }

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
