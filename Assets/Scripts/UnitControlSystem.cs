using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class UnitControlSystem : MonoBehaviour
{
    private Camera _mainCamera;
    private NavMeshAgent _navMeshAgent;
    private CharacterController _characterController;
    private WinPanel _winPanel;

    private void Start()
    {
        _mainCamera = Camera.main;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        _characterController.GameEnded += EndGame;
        _winPanel = FindObjectOfType<WinPanel>(true);
    }

    private void Update()
    {
        _characterController.PlayMoveAnimation(_navMeshAgent.velocity.magnitude);

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out var raycastHit))
            {
                if (raycastHit.transform.TryGetComponent(out Enemy enemy))
                {
                    _navMeshAgent.SetDestination(enemy.transform.position);
                }
                else if (raycastHit.transform.TryGetComponent(out Chest chest))
                {
                    var h = _navMeshAgent.SetDestination(chest.transform.position);
                }
                else
                {
                    //  calculation for a new path
                    var t = _navMeshAgent.SetDestination(raycastHit.point);
                    _characterController.PlaySound();
                }
            }
        }
    }

    private void OnMouseDown()
    {
        CameraController.instance._followTransform = transform;
    }

    private void EndGame()
    {
        _winPanel.ShowPanel();
    }
}