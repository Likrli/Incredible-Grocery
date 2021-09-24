using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Sprite[] _spriteButton;
    [SerializeField] private Image[] _imageButton;
    [SerializeField] private Text _laybleSounds;
    [SerializeField] private Text _laybleMusic;
    [SerializeField] private SaveData _saveData;
    public void RefreshSetPanel()
    {
        RefreshButtons(numberButton: 0, numberSprite: _saveData.Sounds, textBtn: _saveData.Sounds == 0? "ON" : "OFF");
        RefreshButtons(numberButton: 1, numberSprite: _saveData.Music, textBtn: _saveData.Music == 0 ? "ON" : "OFF");
    }

    public void SwitchSounds(bool sounds)
    {
        if (sounds)
        {
            RefreshButtons(numberButton: 0, numberSprite: 0, textBtn: "ON");
        }
        else
        {
            RefreshButtons(numberButton: 0, numberSprite: 1, textBtn: "OFF");
        }
        _saveData.SaveOptions(isSound: true, value: sounds==true ? 0 : 1);
    }

    public void SwitchMusic(bool music)
    {
        if (music)
        {
            RefreshButtons(numberButton: 1, numberSprite: 0, textBtn: "ON");
        }
        else
        {
            RefreshButtons(numberButton: 1, numberSprite: 1, textBtn: "OFF");
        }
        _saveData.SaveOptions(isSound: false, value: music == true ? 0 : 1);
    }

    private void RefreshButtons(int numberButton, int numberSprite, string textBtn)
    {
        switch (numberButton)
        {
            case 0:
                _laybleSounds.text = textBtn;
                break;
            case 1:
                _laybleMusic.text = textBtn;
                break;
        }
        _imageButton[numberButton].sprite = _spriteButton[numberSprite];
    }
    
}
