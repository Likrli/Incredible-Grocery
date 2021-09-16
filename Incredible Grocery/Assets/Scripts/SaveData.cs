using UnityEngine;

public class SaveData : MonoBehaviour
{
    public delegate void SetOptions(int S, int M);
    public event SetOptions onSetOptions;

    public delegate void SetCash(int C);
    public event SetCash onSetCash;

    private int p_cash;
    public int CASH { get { return p_cash; } }
    private int p_sounds;
    public int Sounds { get { return p_sounds; } }
    private int p_music;
    public int Music { get { return p_music; } }

    private string KEY_fLaunch = "fLaunch";
    private string KEY_Cash = "Cash";
    private string KEY_Sounds = "Sounds";
    private string KEY_Music = "Music";



    private void Awake()
    {
        if (PlayerPrefs.HasKey(KEY_fLaunch))
        {
            if (PlayerPrefs.GetInt(KEY_fLaunch) == 1)
            {
                TakeData();
            }
        }
        else
        {
            p_cash = 0; 
            p_music = 0; // 0 - enable, 1 - disable
            p_sounds = 0; 
            PlayerPrefs.SetInt(KEY_Cash, p_cash);
            PlayerPrefs.SetInt(KEY_Sounds, p_sounds);
            PlayerPrefs.SetInt(KEY_Music, p_music);
            PlayerPrefs.SetInt(KEY_fLaunch, 1);
            PlayerPrefs.Save();
        }
    }

    public void SaveCash(int newCash)
    {
        p_cash += newCash;
        PlayerPrefs.SetInt(KEY_Cash, p_cash);
        PlayerPrefs.Save();
        onSetCash(p_cash);
    }
    public void SaveOptions(int P_SOUNDS, int P_MUSIC)
    {
        PlayerPrefs.SetInt(KEY_Sounds, P_SOUNDS);
        PlayerPrefs.SetInt(KEY_Music, P_MUSIC);
        PlayerPrefs.Save();
        onSetOptions(P_SOUNDS, P_MUSIC);
    }

    public void TakeData()
    {
        p_cash = PlayerPrefs.GetInt(KEY_Cash);
        p_sounds = PlayerPrefs.GetInt(KEY_Sounds);
        p_music = PlayerPrefs.GetInt(KEY_Music);
    }
}
