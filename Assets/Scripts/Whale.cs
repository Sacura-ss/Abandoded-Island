using UnityEngine;

public class Whale : MonoBehaviour
{
    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnMouseDown()
    {
        _audioManager.PlayWhaleSFX();
    }
}
