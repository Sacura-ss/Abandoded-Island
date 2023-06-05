using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    [SerializeField] private int _atackValue = 1;
    private Animator _animator;
    private bool _attackEnded;
    private Collider _collider;

    public void GetDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider>();
        _attackEnded = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out CharacterController characterController))
        {
            if (_attackEnded) StartCoroutine(AttackCoroutine(characterController));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _animator.SetTrigger("wait");
    }

    private IEnumerator AttackCoroutine(CharacterController characterController)
    {
        _attackEnded = false;
        _animator.SetTrigger("atack");
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        characterController.TryGetDamage(_atackValue);
        characterController.Attack(this);
        _attackEnded = true;
    }

    private void Die()
    {
        StopAllCoroutines();
        _animator.SetTrigger("die");
        _collider.enabled = false;
    }
}