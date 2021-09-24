using UnityEngine;

public class SaveData : MonoBehaviour
{
    private int _cash;
    private int _sounds;
    private int _music;
    public int Cash { get { return _cash; } }
    public int Sounds { get { return _sounds; } }
    public int Music { get { return _music; } }

    private const string KeyLaunch = "fLaunch";
    private const string KeyCash = "Cash";
    private const string KeySounds = "Sounds";
    private const string KeyMusic = "Music";

    public delegate void GetOprions(bool isSounds, int value);
    public event GetOprions Option;

    public delegate void CountCash(int allCash);
    public event CountCash Recalculation;



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
            _music = 0; // 0 - enable, 1 - disable
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
        Recalculation?.Invoke(_cash);
    }
    public void SaveOptions(bool isSound, int value)
    {
        switch (isSound)
        {
            case true:
                _sounds = value;
                PlayerPrefs.SetInt(KeySounds, _sounds);
                break;
            case false:
                _music = value;
                PlayerPrefs.SetInt(KeyMusic, _music);
                break;
        }
        PlayerPrefs.Save();
        Option?.Invoke(isSound, value);
    }
    public void TakeData()
    {
        _cash = PlayerPrefs.GetInt(KeyCash);
        _sounds = PlayerPrefs.GetInt(KeySounds);
        _music = PlayerPrefs.GetInt(KeyMusic);
    }
}
