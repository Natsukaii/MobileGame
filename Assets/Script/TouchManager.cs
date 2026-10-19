using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private TMP_Text debugText;
    [SerializeField] private UnityEvent<Vector3> onTouchPosition;
    private GameObject objectTouched;
    
    private PlayerInput playerSystem;
    
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    

    private void Awake()
    {
        playerSystem = GetComponent<PlayerInput>();
        
        touchPositionAction = playerSystem.actions["TouchPosition"];
        touchPressAction = playerSystem.actions["TouchPress"];
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressed;
    }
    
    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
    }
    
    private void Update()
    {
        if (touchPositionAction.triggered)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
            Vector3 newPosition = new Vector3();
            touchPosition.z = 0;
            
            onTouchPosition.Invoke(touchPosition);
            if (objectTouched != null)
                objectTouched.GetComponent<ITouch>().OnTouchContinue(touchPosition);
        }
        else
        {
            //objectTouched = null;
        }
       
    }

    [SerializeField] private LayerMask groundLayer;
    private void TouchPressed(InputAction.CallbackContext context)
    {
        Vector3 mousePosition =
            Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());

        Vector2 point = new Vector2(
            mousePosition.x,
            mousePosition.y
        );

        Collider2D hit = Physics2D.OverlapPoint(point);

        objectTouched = hit.gameObject;
        
        if (objectTouched != null)
            objectTouched.GetComponent<ITouch>().OnTouchStart();
    }
}
