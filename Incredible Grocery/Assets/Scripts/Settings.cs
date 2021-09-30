using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Sprite[] spriteButton;
    [SerializeField] private Image[] imageButton;
    [SerializeField] private Text soundsLayble;
    [SerializeField] private Text musicLayble;
    [SerializeField] private SaveData saveData;
    public void RefreshSetPanel()
    {
        RefreshButtons(numberButton: 0, numberSprite: saveData.Sounds, textBtn: saveData.Sounds == 0? "ON" : "OFF");
        RefreshButtons(numberButton: 1, numberSprite: saveData.Music, textBtn: saveData.Music == 0 ? "ON" : "OFF");
    }

    public void SwitchSounds(bool isSoundsOn)
    {
        if (isSoundsOn)
        {
            RefreshButtons(numberButton: 0, numberSprite: 0, textBtn: "ON");
        }
        else
        {
            RefreshButtons(numberButton: 0, numberSprite: 1, textBtn: "OFF");
        }
        saveData.SaveOptions(isSound: true, value: isSoundsOn? 0 : 1);
    }

    public void SwitchMusic(bool isMusicOn)
    {
        if (isMusicOn)
        {
            RefreshButtons(numberButton: 1, numberSprite: 0, textBtn: "ON");
        }
        else
        {
            RefreshButtons(numberButton: 1, numberSprite: 1, textBtn: "OFF");
        }
        saveData.SaveOptions(isSound: false, value: isMusicOn? 0 : 1);
    }

    private void RefreshButtons(int numberButton, int numberSprite, string textBtn)
    {
        switch (numberButton)
        {
            case 0:
                soundsLayble.text = textBtn;
                break;
            case 1:
                musicLayble.text = textBtn;
                break;
        }
        imageButton[numberButton].sprite = spriteButton[numberSprite];
    }
    
}
