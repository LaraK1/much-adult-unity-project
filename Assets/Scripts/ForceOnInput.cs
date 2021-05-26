using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceOnInput : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float force = 45;

    [SerializeField] private bool _shouldMove = false;

    private void Awake() => _rb = gameObject.GetComponent<Rigidbody2D>();

    private void Update()
    {
        if (_shouldMove == false)
        {
            // should move when mouse is clicked / screen is tapped
            _shouldMove = Input.GetMouseButton(0);
        }

    }

    private void FixedUpdate()
    {
        if (_shouldMove)
        {        
            // adds physic force to foot
            _rb.AddForce(Vector2.up * force);
            _shouldMove = false;
        }
    }
}
