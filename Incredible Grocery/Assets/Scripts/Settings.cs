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
    private void Start()
    {
        _saveData = SaveData.Instance;
        _isSoundsOn = _saveData.AllData.isSounds;
        _isMusicOn = _saveData.AllData.isMusic;
        soundsButton.onClick.AddListener(OnClickedSoundsButton);
        musicButton.onClick.AddListener(OnClickedMusicButton);
        saveButton.onClick.AddListener(OnClickedSaveButton);
        settingButton.onClick.AddListener(OnClickedSettingButton);
    }
    private void OnClickedSettingButton()
    {
        settingsMenu.SetActive(true);
        RefreshSoundsButton(soundsValue: _saveData.AllData.isSounds);
        RefreshMusicButton(musicValue: _saveData.AllData.isMusic);
    }
    private void OnClickedSoundsButton()
    {
        _isSoundsOn = !_isSoundsOn;
        _saveData.SaveOptions(isSounds: true, value: _isSoundsOn);
        RefreshSoundsButton(soundsValue: _saveData.AllData.isSounds);
    }
    private void OnClickedMusicButton()
    {
        _isMusicOn = !_isMusicOn;
        _saveData.SaveOptions(isSounds: false, value: _isMusicOn);
        RefreshMusicButton(musicValue: _saveData.AllData.isMusic);
    }
    private void RefreshSoundsButton(bool soundsValue)
    {
        sounds.GetComponent<Image>().sprite = soundsValue? greenButton : redButton;
        soundsLabel.text = soundsValue ? On : Off;
    }
    private void RefreshMusicButton(bool musicValue)
    {
        music.GetComponent<Image>().sprite = musicValue? greenButton : redButton;
        musicLabel.text = musicValue ? On : Off;
    }
    private void OnClickedSaveButton()
    {
        settingsMenu.SetActive(false);
    }
}
