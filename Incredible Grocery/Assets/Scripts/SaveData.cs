using UnityEngine;

public class SaveData : MonoBehaviour
{
    public int Cash => _cash;
    public int Sounds => _sounds;
    public int Music => _music; 

    private int _cash;
    private int _sounds;
    private int _music;

    private const string KeyLaunch = "fLaunch";
    private const string KeyCash = "Cash";
    private const string KeySounds = "Sounds";
    private const string KeyMusic = "Music";

    public delegate void OnSetSounds(int value);
    public event OnSetSounds SetSounds;

    public delegate void OnSetMusic(int value);
    public event OnSetMusic SetMusic;

    public delegate void OnRecalculatedCash(int allCash);
    public event OnRecalculatedCash RecalculatedCash;



    private void Awake()
    {
        if (PlayerPrefs.HasKey(KeyLaunch))
        {
            if (PlayerPrefs.GetInt(KeyLaunch) == 1)
            {
                TakeData();
            }
        }
        else
        {
            _cash = 0; 
            _music = 0;
            _sounds = 0; 
            PlayerPrefs.SetInt(KeyCash, _cash);
            PlayerPrefs.SetInt(KeySounds, _sounds);
            PlayerPrefs.SetInt(KeyMusic, _music);
            PlayerPrefs.SetInt(KeyLaunch, 1);
            PlayerPrefs.Save();
        }
    }

    public void SaveCash(int newCash)
    {
        _cash += newCash;
        PlayerPrefs.SetInt(KeyCash, _cash);
        PlayerPrefs.Save();
        RecalculatedCash?.Invoke(_cash);
    }
    public void SaveOptions(bool isSound, int value)
    {
        switch (isSound)
        {
            case true:
                _sounds = value;
                PlayerPrefs.SetInt(KeySounds, _sounds);
                SetSounds?.Invoke(_sounds);
                break;
            case false:
                _music = value;
                PlayerPrefs.SetInt(KeyMusic, _music);
                SetMusic?.Invoke(_music);
                break;
        }
        PlayerPrefs.Save();
    }
    public void TakeData()
    {
        _cash = PlayerPrefs.GetInt(KeyCash);
        _sounds = PlayerPrefs.GetInt(KeySounds);
        _music = PlayerPrefs.GetInt(KeyMusic);
    }
}
