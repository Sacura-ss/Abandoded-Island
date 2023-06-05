using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int _point = 3;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterController characterController))
        {
           characterController.GetPoint(_point);
           //_animator.SetTrigger("open");
           Destroy(gameObject);
        }
    }
}
