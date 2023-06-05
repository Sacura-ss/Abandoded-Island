using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _buttonSFX;
    [SerializeField] private AudioSource _playerSFX;
    [SerializeField] private AudioSource _whaleSFX;

    private const string MIXER_MUSIC = "MusicVolume";
    private const string MIXER_SFX = "SFXVolume";

    private const float _beeSFXTime = 3.0f;

    private void Awake()
    {
        _music = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        if (_playerSFX.time > 3.0)
        {
            _playerSFX.Stop();
        }
    }

    public void SetMusicVolume(float value)
    {
        _audioMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        _audioMixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
        _playerSFX.Play();
    }

    public void PlayMusic()
    {
        _music.Play();
    }

    public void PlayButtonSFX()
    {
        _buttonSFX.Play();
    }

    public void PlayPlayerSFX()
    {
        _playerSFX.Play();
    }

    public void PlayWhaleSFX()
    {
        _whaleSFX.Play();
    }
}