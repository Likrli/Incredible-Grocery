using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mainMixer;
    [SerializeField] private AudioSource[] allAudioSources;
    [SerializeField] private AudioClip[] allClips;
    public static AudioManager Instance;
    public AudioClip[] AllClips => allClips;

    public enum Clip
    {
        SpawnBubble,
        CloseBubble,
        ClickButton,
        Money,
        SelectProduct,
        BadService,
        MoneyEarned,
        MoneySpent
    }
    private SaveData _saveData;

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
    }
    private void Start()
    {
        _saveData = SaveData.Instance;
        _saveData.SetSounds += OnSetSounds;
        _saveData.SetMusic += OnSetMusic;
        OnSetSounds(value: _saveData.AllData.isSounds);
        OnSetMusic(value: _saveData.AllData.isMusic);
    }
    private void OnSetSounds(bool value)
    {
        mainMixer.audioMixer.SetFloat("SoundsVolume", -80 * (value ? 0 : 1));
    }
    private void OnSetMusic(bool value)
    {
        mainMixer.audioMixer.SetFloat("MusicVolume", -80 * (value ? 0 : 1));
    }
    public void PlayClip(Clip clip)
    {
        allAudioSources[(clip == Clip.ClickButton ? 1 : 0)].PlayOneShot(allClips[(int)clip]);
    }
    public void PlaySoundsButton()
    {
        PlayClip(clip: AudioManager.Clip.ClickButton);
    }
}
