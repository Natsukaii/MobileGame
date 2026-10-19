using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _debugText;
    [SerializeField] private UnityEvent<Vector3> _onTouchPosition;
    private GameObject _objectTouched;
    
    private PlayerInput _playerSystem;
    private Camera mainCamera;
    
    private InputAction _touchPositionAction;
    private InputAction _touchPressAction;
    
    private ITouch _touchedObject;
    

    private void Awake()
    {
        _playerSystem = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
        
        _touchPositionAction = _playerSystem.actions["TouchPosition"];
        _touchPressAction = _playerSystem.actions["TouchPress"];
    }

    private void OnEnable()
    {
        _touchPressAction.performed += TouchPressed;
        _touchPressAction.canceled += TouchReleased;
    }
    
    private void OnDisable()
    {
        _touchPressAction.performed -= TouchPressed;
        _touchPressAction.canceled -= TouchReleased;
    }
    
    private void Update()
    {
        if (_touchPositionAction.triggered)
        {
            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(_touchPositionAction.ReadValue<Vector2>());
            touchPosition.z = 0;
            
            _onTouchPosition.Invoke(touchPosition);
            if (_objectTouched != null)
            {
                _objectTouched.GetComponent<ITouch>().OnTouchContinue(touchPosition);
                _debugText.text = touchPosition + " " + _objectTouched.name;
                
            }
        }
       
    }

    [SerializeField] private LayerMask _layerMask;
    private void TouchPressed(InputAction.CallbackContext context)
    {
        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(_touchPositionAction.ReadValue<Vector2>());

        Vector2 point = new Vector2(
            mousePosition.x,
            mousePosition.y
        );

        Collider2D hit = Physics2D.OverlapPoint(point, _layerMask);
        if (hit == null)
        {
            _objectTouched = null;
            return;
        }

        _objectTouched = hit.gameObject;

        if (_objectTouched != null)
        {
            _touchedObject = _objectTouched.GetComponent<ITouch>();
            _touchedObject.OnTouchStart(mousePosition);
        }
        else
        {
            _objectTouched = null;
        }
    }
    
    private void TouchReleased(InputAction.CallbackContext context)
    {
        if (_objectTouched == null)
            return;

        Vector3 touchPosition = mainCamera.ScreenToWorldPoint(_touchPositionAction.ReadValue<Vector2>());
        touchPosition.z = 0;

        ITouch touch = _objectTouched.GetComponent<ITouch>();

        if (touch != null)
            touch.OnTouchEnd(touchPosition);

        _objectTouched = null;
    }
}
