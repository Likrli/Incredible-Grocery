using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _mainMixer;
    [SerializeField] private AudioSource[] _allAudioSource;
    [SerializeField] private AudioClip[] _clipSounds;
    [SerializeField] private SaveData _saveData;
    [SerializeField]
    public enum Clip
    {
        SpawnBuble,
        CloseBuble,
        ClickButton,
        Money,
        SelectProduct
    }
    private void Start()
    {
        _saveData = GetComponent<SaveData>();
        _saveData.Option += OnSetOptions;
        OnSetOptions(sounds: _saveData.Sounds, music: _saveData.Music);
    }
    public void OnSetOptions(int sounds, int music)
    {
        _mainMixer.audioMixer.SetFloat("SoundsVolume", sounds * -80); //1 * -80
        _mainMixer.audioMixer.SetFloat("MusicVolume", music * -80); // 0*-80 
    }

    public void OnSetOptions(bool isSounds, int value)
    {
        switch (isSounds)
        {
            case true:
                _mainMixer.audioMixer.SetFloat("SoundsVolume", value * -80);
                break;
            case false:
                _mainMixer.audioMixer.SetFloat("MusicVolume", value * -80);
                break;
        }
    }
    public void PlayClip(int indexSource, Clip clip)
    {
        _allAudioSource[indexSource].PlayOneShot(_clipSounds[(int)clip]);
    }
    public void PlayClip()
    {
        _allAudioSource[1].PlayOneShot(_clipSounds[(int)Clip.ClickButton]);
    }


}
