using UnityEngine;
using System.IO;

public class SaveData : MonoBehaviour
{
    public static SaveData instance;
    public Product product;
    public AudioManager audioManager;
    public int Cash => _cash;
    public int Sounds => _sounds;
    public int Music => _music;
    public bool WasLoaded => _wasLoaded;

    private int _cash;
    private int _sounds;
    private int _music;
    private bool _wasLoaded;

    private const string KeyLaunch = "fLaunch";
    private const string KeyCash = "Cash";
    private const string KeySounds = "Sounds";
    private const string KeyMusic = "Music";
    private const string DataJson = "Data.json";
    private string _path;

    public delegate void OnSetSounds(int value);
    public event OnSetSounds SetSounds;

    public delegate void OnSetMusic(int value);
    public event OnSetMusic SetMusic;

    public delegate void OnRecalculatedCash(int allCash);
    public event OnRecalculatedCash RecalculatedCash;

    public delegate void OnReceivedNotification(int valueNotification);
    public event OnReceivedNotification ReceivedNotification;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }  
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            audioManager = GetComponent<AudioManager>();
        }
        _path = Path.Combine(Application.dataPath, DataJson);
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(KeyLaunch))
        {
            if (PlayerPrefs.GetInt(KeyLaunch) == 1)
            {
                TakeData();
                _wasLoaded = true;
            }
        }
        else
        {
            SetInitialValues();
        }
    }
    public void SetInitialValues()
    {
        _cash = 0;
        _music = 0;
        _sounds = 0;
        _wasLoaded = false;
        PlayerPrefs.SetInt(KeyCash, _cash);
        PlayerPrefs.SetInt(KeySounds, _sounds);
        PlayerPrefs.SetInt(KeyMusic, _music);
        PlayerPrefs.SetInt(KeyLaunch, 1);
        PlayerPrefs.Save();
        product.productsAmount = new int[20];
        for (int i = 0; i < product.productsAmount.Length; i++)
        {
            product.productsAmount[i] = 3;
        }
        File.WriteAllText(_path, JsonUtility.ToJson(product));
    }

    public void SaveCash(int newCash)
    {
        _cash += newCash;
        PlayerPrefs.SetInt(KeyCash, _cash);
        PlayerPrefs.Save();
        RecalculatedCash?.Invoke(_cash);
        ReceivedNotification?.Invoke(newCash);
        File.WriteAllText(_path, JsonUtility.ToJson(product));
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
        if (File.Exists(_path))
        {
            product = JsonUtility.FromJson<Product>(File.ReadAllText(_path));
        }
        else
        {
            SetInitialValues();
        }
    }

    [System.Serializable]
    public class Product
    {
        public int[] productsAmount;
    }
}
