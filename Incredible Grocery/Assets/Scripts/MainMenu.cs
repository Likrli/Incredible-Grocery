using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private List<Button> allMenuButtons;

    private const string MainScene = "MainScene";
    private SaveData _saveData;

    private void Start()
    {
        _saveData = SaveData.Instance;
        _saveData.LoadData();
        continueButton.interactable = _saveData.AllData.wasLoaded;
        continueButton.onClick.AddListener(OnClickedContinueButton);
        newGameButton.onClick.AddListener(OnClickedNewGameButton);
        foreach (var button in allMenuButtons)
        {
            button.onClick.AddListener(AudioManager.Instance.PlaySoundsButton);
        }
    }
    private void OnClickedContinueButton()
    {
        SceneManager.LoadScene(MainScene);
    }
    private void OnClickedNewGameButton()
    {
        _saveData.SetDefaultValues();
        SceneManager.LoadScene(MainScene);
    }
}
