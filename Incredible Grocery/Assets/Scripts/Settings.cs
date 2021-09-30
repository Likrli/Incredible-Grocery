using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Button soundsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Sprite[] spritesButtons;
    [SerializeField] private Image[] imagesButtons;
    [SerializeField] private Text soundsLabel;
    [SerializeField] private Text musicLabel;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private SaveData saveData;
    private const string On = "ON";
    private const string Off = "OFF";
    private bool _isSoundsOn;
    private bool _isMusicOn;
    public void Start()
    {
        _isSoundsOn = saveData.Sounds == 0;
        _isMusicOn = saveData.Music == 0;
        soundsButton.onClick.AddListener(SwitchSounds);
        musicButton.onClick.AddListener(SwitchMusic);
        saveButton.onClick.AddListener(CloseSettingsMenu);
        settingButton.onClick.AddListener(RefreshSettingsPanel);
        settingButton.onClick.AddListener(OpenSettingsMenu);
    }
    public void RefreshSettingsPanel()
    {
        RefreshSoundsButton(spriteNumber: saveData.Sounds, labelText: saveData.Sounds == 0 ? On : Off);
        RefreshMusicButton(spriteNumber: saveData.Music, labelText: saveData.Music == 0 ? On : Off);
    }

    public void SwitchSounds()
    {
        _isSoundsOn = !_isSoundsOn;
        saveData.SaveOptions(isSound: true, value: _isSoundsOn? 0 : 1);
        RefreshSoundsButton(spriteNumber: saveData.Sounds, labelText: saveData.Sounds == 0 ? On : Off);
    }

    public void SwitchMusic()
    {
        _isMusicOn = !_isMusicOn;
        saveData.SaveOptions(isSound: false, value: _isMusicOn? 0 : 1);
        RefreshMusicButton(spriteNumber: saveData.Music, labelText: saveData.Music == 0 ? On : Off);
    }
    private void RefreshSoundsButton(int spriteNumber, string labelText)
    {
        imagesButtons[0].sprite = spritesButtons[spriteNumber];
        soundsLabel.text = labelText;
    }
    private void RefreshMusicButton(int spriteNumber, string labelText)
    {
        imagesButtons[1].sprite = spritesButtons[spriteNumber];
        musicLabel.text = labelText;
    }
    private void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }
    private void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }
}
