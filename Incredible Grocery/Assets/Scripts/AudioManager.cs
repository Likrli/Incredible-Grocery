using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup MainMixer;
    public AudioSource[] AllAudioSource;
    public AudioClip[] ClipSounds;
    private SaveData p_saveData;
    private void Start()
    {
        p_saveData = FindObjectOfType<SaveData>();
        p_saveData.onSetOptions += SetOptions;
        SetOptions(p_saveData.Sounds, p_saveData.Music);
    }

    public void SetOptions(int sonds, int music)
    {
        MainMixer.audioMixer.SetFloat("SoundsVolume", sonds * -80); // (0*-80db = 0db enabled) (1*-80db = -80db disabled)
        MainMixer.audioMixer.SetFloat("MusicVolume", music * -80);
    }

    /// <numClip>
    /// 0 - Spawn cloud
    /// 1 - Close cloud
    /// 2 - Btn click
    /// 3 - Money (cashier)
    /// 4 - Select product
    /// </numClip>
    public void PlayClip(int indexSource, int numClip)
    {
        AllAudioSource[indexSource].PlayOneShot(ClipSounds[numClip]);
    }
    public void PlayClip(int numClip)
    {
        AllAudioSource[1].PlayOneShot(ClipSounds[numClip]); //For Btn
    }


}
