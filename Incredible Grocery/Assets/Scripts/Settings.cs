using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Button soundsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Sprite greenButton;
    [SerializeField] private Sprite redButton;
    [SerializeField] private GameObject sounds;
    [SerializeField] private GameObject music;
    [SerializeField] private Text soundsLabel;
    [SerializeField] private Text musicLabel;
    [SerializeField] private GameObject settingsMenu;
    private SaveData _saveData;
    private const string On = "ON";
    private const string Off = "OFF";
    private bool _isSoundsOn;
    private bool _isMusicOn;
    public void Start()
    {
        _saveData = SaveData.instance;
        _isSoundsOn = _saveData.Sounds == 0;
        _isMusicOn = _saveData.Music == 0;
        soundsButton.onClick.AddListener(SwitchSounds);
        musicButton.onClick.AddListener(SwitchMusic);
        saveButton.onClick.AddListener(CloseSettingsMenu);
        settingButton.onClick.AddListener(RefreshSettingsPanel);
        settingButton.onClick.AddListener(OpenSettingsMenu);
    }
    public void RefreshSettingsPanel()
    {
        RefreshSoundsButton(soundsValue: _saveData.Sounds, labelText: _saveData.Sounds == 0 ? On : Off);
        RefreshMusicButton(musicValue: _saveData.Music, labelText: _saveData.Music == 0 ? On : Off);
    }

    public void SwitchSounds()
    {
        _isSoundsOn = !_isSoundsOn;
        _saveData.SaveOptions(isSound: true, value: _isSoundsOn? 0 : 1);
        RefreshSoundsButton(soundsValue: _saveData.Sounds, labelText: _saveData.Sounds == 0 ? On : Off);
    }

    public void SwitchMusic()
    {
        _isMusicOn = !_isMusicOn;
        _saveData.SaveOptions(isSound: false, value: _isMusicOn? 0 : 1);
        RefreshMusicButton(musicValue: _saveData.Music, labelText: _saveData.Music == 0 ? On : Off);
    }
    private void RefreshSoundsButton(int soundsValue, string labelText)
    {
        sounds.GetComponent<Image>().sprite = soundsValue == 0 ? greenButton : redButton;
        soundsLabel.text = labelText;
    }
    private void RefreshMusicButton(int musicValue, string labelText)
    {
        music.GetComponent<Image>().sprite = musicValue == 0 ? greenButton : redButton;
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
