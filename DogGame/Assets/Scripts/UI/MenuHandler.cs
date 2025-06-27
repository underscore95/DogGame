using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject _mainWidget;
    [SerializeField] private GameObject _creditsWidget;
    [SerializeField] private string _gameScene = "Level_Blockout";
    [SerializeField] private CinemachineSplineDolly _cutsceneSpline;
    [SerializeField] private float _cutsceneDuration = 5.0f;
    public UnityEvent CreditsClose;
    [SerializeField] Button returnButton;
    bool creditsOpen;
    bool returnSelect;
    private bool _isReverseCutscene = false;

    private void Start()
    {

       
    }
    private void Update()
    {
        _cutsceneSpline.CameraPosition += Time.deltaTime / _cutsceneDuration * (_isReverseCutscene ? -1 : 1);
        if (_cutsceneSpline.CameraPosition >= 1) _isReverseCutscene = true;
        else if (_cutsceneSpline.CameraPosition <= 0) _isReverseCutscene = false;
        if (creditsOpen)
        {
            Cursor.visible = true;

            
        }

    }

    public void CloseCredits()
    {
        // _mainWidget.SetActive(true);
        CreditsClose.Invoke();
        _creditsWidget.SetActive(false);
        creditsOpen = false;
    }

    public void OpenCredits()
    {
        _mainWidget.SetActive(false);
        _creditsWidget.SetActive(true);
        creditsOpen = true;
        Cursor.visible = true;
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
