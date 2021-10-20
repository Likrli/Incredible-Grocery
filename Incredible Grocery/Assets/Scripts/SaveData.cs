using UnityEngine;
using System.IO;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;

    public delegate void SetSoundsHandler(bool value);
    public event SetSoundsHandler SetSounds;

    public delegate void SetMusicHandler(bool value);
    public event SetMusicHandler SetMusic;

    public delegate void RecalculatedCashHandler(int allCash);
    public event RecalculatedCashHandler RecalculatedCash;

    public delegate void ReceivedNotificationHandler(int valueNotification);
    public event ReceivedNotificationHandler ReceivedNotification;

    public Data AllData => _allData;
    private Data _allData;

    private const string DataJson = "Data.json";
    private string _path;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }  
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        _allData = new Data();
        _path = Path.Combine(Application.dataPath, DataJson);
    }

    public void LoadData()
    {
        if (File.Exists(_path))
        {
            _allData = JsonUtility.FromJson<Data>(File.ReadAllText(_path));
            _allData.wasLoaded = true;
        }
        else
        {
            SetDefaultValues();
            _allData.wasLoaded = false;
        }
    }
    public void SetDefaultValues()
    {
        _allData.productsAmount = new int[20];
        for (int i = 0; i < _allData.productsAmount.Length; i++)
        {
            _allData.productsAmount[i] = 3;
        }
        _allData.cash = 0;
        _allData.isMusic = true;
        _allData.isSounds = true;
        File.WriteAllText(_path, JsonUtility.ToJson(_allData));
    }

    public void SaveCash(int newCash)
    {
        _allData.cash += newCash;
        RecalculatedCash?.Invoke(_allData.cash);
        ReceivedNotification?.Invoke(newCash);
        File.WriteAllText(_path, JsonUtility.ToJson(_allData));
    }
    public void SaveOptions(bool isSounds, bool value)
    {
        switch (isSounds)
        {
            case true:
                _allData.isSounds = value;
                SetSounds?.Invoke(_allData.isSounds);
                break;
            case false:
                _allData.isMusic = value;
                SetMusic?.Invoke(_allData.isMusic);
                break;
        }
        File.WriteAllText(_path, JsonUtility.ToJson(_allData));
    }

    [System.Serializable]
    public class Data
    {
        public int[] productsAmount;
        public int cash;
        public bool isSounds;
        public bool isMusic;
        public bool wasLoaded;
    }
}
