using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Vector2 _movementInput;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform customCursorTransform;
    [SerializeField] private Vector3 mousePosition;
    [SerializeField] private SpriteRenderer cursorSpriteRenderer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        aimTransform = transform.Find("AimedParent");
        customCursorTransform = transform.Find("PlayerCursor");
        cursorSpriteRenderer = GameObject.Find("CursorSprite").GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        if (IsLocalPlayer) 
        { 
            CustomCursor();
            MousePositionHandler();
        }
    }

    private void FixedUpdate() 
    {
        if (!IsOwner) return;
        _rigidbody.velocity = _movementInput * _movementSpeed;
    }
    private void OnMove(InputValue inputValue) 
    {
        _movementInput = inputValue.Get<Vector2>();
    }

        public void MousePositionHandler() 
    {
        // Grabs the main camera and does a "screen to world point" on the mouse position
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Sets the z axis to 0
        mouseWorldPosition.z = 0f;
        mousePosition = mouseWorldPosition;
        Vector3 aimDirection = (mouseWorldPosition - transform.position).normalized;
        // Convert to a Euler angle
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void CustomCursor() 
    {
        Cursor.visible = false;
        customCursorTransform.position = mousePosition;
    }
}
