using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private GameObject _unitCharacter;
    [SerializeField] private int _health = 30;
    [SerializeField] private int _atackValue = 2;
    [SerializeField] private TMP_Text _pointText;
    private Animator _characterAnimator;
    private AudioManager _audioManager;
    private int _pointCount;

    public event Action GameEnded;
    
    private void Awake()
    {
        _characterAnimator = _unitCharacter.GetComponent<Animator>();
        _audioManager = FindObjectOfType<AudioManager>();
        _pointCount = 0;
        _pointText.text = PointSaver.Point + "";
    }

    public void PlayMoveAnimation(float parameter)
    {
        _characterAnimator.SetFloat("Move", parameter);
    }

    public void PlaySound()
    {
        if(_audioManager) _audioManager.PlayPlayerSFX();
    }

    public void TryGetDamage(int damage)
    {
        System.Random random = new System.Random(); 
        int num = random.Next(1, 13);
        if ((num % 2) == 0)
        {
            _health -= damage;
        }
        if (_health <= 0)
        {
            EndGame();
        }
    }

    public void Attack(Enemy enemy)
    {
        _characterAnimator.SetTrigger("attack");
        enemy.GetDamage(_atackValue);
    }

    public void GetPoint(int point)
    {
        _pointCount += point;
        PointSaver.Point = _pointCount;
        _pointText.text = PointSaver.Point + "";
        if (PointSaver.Point >= PointSaver.MaxPoint) GameEnded?.Invoke();
    }

    private void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
