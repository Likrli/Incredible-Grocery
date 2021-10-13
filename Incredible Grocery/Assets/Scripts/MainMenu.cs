using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;

    private const string MainScene = "MainScene";
    private SaveData _saveData;

    private void Start()
    {
        _saveData = SaveData.instance;
        _saveData.LoadData();
        continueButton.interactable = _saveData.WasLoaded;
        continueButton.onClick.AddListener(ContinueGame);
        newGameButton.onClick.AddListener(StartNewGame);
    }
    private void ContinueGame()
    {
        SceneManager.LoadScene(MainScene);
    }
    private void StartNewGame()
    {
        _saveData.SetInitialValues();
        SceneManager.LoadScene(MainScene);
    }


}
