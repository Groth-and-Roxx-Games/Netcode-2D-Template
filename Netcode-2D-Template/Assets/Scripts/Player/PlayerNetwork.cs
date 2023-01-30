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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        aimTransform = transform.Find("AimedParent");
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
}
