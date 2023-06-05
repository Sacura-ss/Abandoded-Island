using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform _followTransform;

    [SerializeField] private Transform _cameraTransform;

    [SerializeField] private float _movementSpeed = 1;
    [SerializeField] private float _lerpTime = 5;
    [SerializeField] private float _rotationAmount;
    [SerializeField] private Vector3 _zoomAmount;


    private Vector3 _newPosition;
    private Quaternion _newRotation;
    private Vector3 _newZoom;
    
    // camera constraints
    private Vector3 _zoomConstraintUp = new Vector3(0, 1, -1) * 850;
    private Vector3 _zoomConstraintDown = new Vector3(0, 1, -1) * 300;
    private float _movementConstraintMinX = -200f;
    private float _movementConstraintMaxX = 1000f;
    private float _movementConstraintMinZ = -200f;
    private float _movementConstraintMaxZ = 1000f;
    
    // only for mouse
    private Vector3 _dragStartPosition;
    private Vector3 _dragCurrentPosition;
    private Vector3 _rotateStartPosition;
    private Vector3 _rotateCurrentPosition;

    void Start()
    {
        instance = this;

        _newPosition = transform.position;
        _newRotation = transform.rotation;
        _newZoom = _cameraTransform.localPosition;
    }

    void Update()
    {
        if (_followTransform != null)
        {
            transform.position = _followTransform.position;
        }
        else
        {
            HandleMovementInputByKeys();
            HandleMovementInputByMouse();
            HandleRotationInputByKeys();
            HandleRotationInputByMouse();
            HandleZoom();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Vector3 currentCameraPosition = transform.position;
            _followTransform = null;
            _newPosition = currentCameraPosition;
        }
    }

    private void HandleMovementInputByKeys()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _newPosition += transform.forward * _movementSpeed;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _newPosition += transform.forward * -_movementSpeed;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _newPosition += transform.right * _movementSpeed;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _newPosition += transform.right * -_movementSpeed;
        }

        transform.position = Vector3.Lerp(transform.position, ClampCameraMovement(_newPosition), Time.deltaTime * _lerpTime);
    }

    private void HandleRotationInputByKeys()
    {
        if (Input.GetKey(KeyCode.E))
        {
            _newRotation *= Quaternion.Euler(Vector3.up * _rotationAmount);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            _newRotation *= Quaternion.Euler(Vector3.up * -_rotationAmount);
        }

        transform.rotation =
            Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * _lerpTime);
    }

    private void HandleZoom()
    {
        if (Input.GetKey(KeyCode.R) || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            _newZoom = new Vector3(0, 
                Math.Clamp((_newZoom + _zoomAmount).y, _zoomConstraintDown.y, _zoomConstraintUp.y),
                Math.Clamp((_newZoom + _zoomAmount).z, _zoomConstraintUp.z, _zoomConstraintDown.z));
        }

        if (Input.GetKey(KeyCode.F) || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            _newZoom = new Vector3(0, 
                Math.Clamp((_newZoom - _zoomAmount).y, _zoomConstraintDown.y, _zoomConstraintUp.y),
                Math.Clamp((_newZoom - _zoomAmount).z, _zoomConstraintUp.z, _zoomConstraintDown.z));
        }

        _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _newZoom,
            Time.deltaTime * _lerpTime);
    }

    private void HandleMovementInputByMouse()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (plane.Raycast(ray, out var entry))
            {
                _dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (plane.Raycast(ray, out var entry))
            {
                _dragCurrentPosition = ray.GetPoint(entry);

                //_newPosition = transform.position + _dragStartPosition - _dragCurrentPosition;
                _newPosition = ClampCameraMovement(transform.position + _dragStartPosition - _dragCurrentPosition);
            }
        }
    }
    private void HandleRotationInputByMouse()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _rotateStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            _rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = _rotateStartPosition - _rotateCurrentPosition;

            _rotateStartPosition = _rotateCurrentPosition;

            _newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }

        transform.rotation =
            Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * _lerpTime);
    }

    private Vector3 ClampCameraMovement(Vector3 targetPosition)
    {
        Camera cam = _cameraTransform.gameObject.GetComponent<Camera>(); 
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = _movementConstraintMinX + camWidth;
        float maxX = _movementConstraintMaxX - camWidth;
        float minZ = _movementConstraintMinZ + camHeight;
        float maxZ = _movementConstraintMaxZ - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newZ = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        return new Vector3(newX, targetPosition.y, newZ);
    }
}