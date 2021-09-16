using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    public Sprite[] SprBtn;
    public Image[] ImgBtn;
    public Text TxtSounds;
    public Text TxtMusic;

    private SaveData p_saveData;
    private int p_vSounds;
    private int p_vMusic;

    private void Start()
    {
        p_saveData = FindObjectOfType<SaveData>();
        p_vSounds = p_saveData.Sounds;
        p_vMusic = p_saveData.Music;
    }

    public void RefreshSetPanel()
    {
        if(p_vSounds == 0)
        {
            Btn(0, 0, "ON");
        }
        else
        {
            Btn(0, 1, "OFF");
        }

        if (p_vMusic == 0)
        {
            Btn(1, 0, "ON");
        }
        else
        {
            Btn(1, 1, "OFF");
        }
    }

    public void ToggleSounds(bool onSounds)
    {
        if (onSounds)
        {
            p_vSounds = 0;
            Btn(0, 0, "ON");
        }
        else
        {
            p_vSounds = 1;
            Btn(0, 1, "OFF");
        }
        SaveSettings();
    }

    public void ToggleMusic(bool onMusic)
    {
        if (onMusic)
        {
            p_vMusic = 0;
            Btn(1, 0, "ON");
        }
        else
        {
            p_vMusic = 1;
            Btn(1, 1, "OFF");
        }
        SaveSettings();
    }
    public void SaveSettings()
    {
        p_saveData.SaveOptions(p_vSounds, p_vMusic);
    }

    private void Btn(int indexBtn, int indexSpr, string txtBtn)
    {
        switch (indexBtn)
        {
            case 0:
                TxtSounds.text = txtBtn;
                break;
            case 1:
                TxtMusic.text = txtBtn;
                break;
        }
        ImgBtn[indexBtn].sprite = SprBtn[indexSpr];
    }
    
}
