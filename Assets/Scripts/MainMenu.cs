using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;

    public void PlayGame()
    {
        _audioManager.PlayButtonSFX();
        DontDestroyOnLoad(_audioManager.gameObject);
        SceneManager.LoadScene("SampleScene");
    }

    public void SetMusicSlider(float volume)
    {
        _audioManager.PlayButtonSFX();
        _audioManager.SetMusicVolume(volume);
    }

    public void SetSFXSlider(float volume)
    {
        _audioManager.PlayButtonSFX();
        _audioManager.SetSFXVolume(volume);
    }
}