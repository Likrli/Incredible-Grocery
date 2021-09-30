using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mainMixer;
    [SerializeField] private AudioSource[] allAudioSources;
    [SerializeField] private AudioClip[] allClips;
    public enum Clip
    {
        SpawnBuble,
        CloseBuble,
        ClickButton,
        Money,
        SelectProduct
    }
    private SaveData _saveData;

    private void Start()
    {
        _saveData = GetComponent<SaveData>();
        _saveData.SetSounds += OnSetSounds;
        _saveData.SetMusic += OnSetMusic;
        OnSetSounds(value: _saveData.Sounds);
        OnSetMusic(value: _saveData.Music);
    }
    public void OnSetSounds(int value)
    {
        mainMixer.audioMixer.SetFloat("SoundsVolume", value * -80);
    }
    public void OnSetMusic(int value)
    {
        mainMixer.audioMixer.SetFloat("MusicVolume", value * -80);
    }

    public void PlayClip(Clip clip)
    {
        allAudioSources[(clip == Clip.ClickButton ? 1 : 0)].PlayOneShot(allClips[(int)clip]);
    }
}
