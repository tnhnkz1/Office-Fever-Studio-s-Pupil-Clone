using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 300;

    private Touch _touch;

    private Vector3 _touchUp;
    private Vector3 _touchDown;

    private bool _dragStarted;
    private bool _isMoving;

    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began)
            {
                _dragStarted = true;
                _isMoving = true;

                _touchUp = _touch.position;
                _touchDown = _touch.position;

                animator.SetBool("isMoving", true);
            }
        }

        if (_dragStarted)
        {
            if (_touch.phase == TouchPhase.Moved)
            {
                _touchDown = _touch.position;
                animator.SetBool("isMoving", true);
            }

            if (_touch.phase == TouchPhase.Ended)
            {
                _touchDown = _touch.position;

                _dragStarted = false;
                _isMoving = false;
                animator.SetBool("isMoving", false);
            }

            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, CalculateRotation(), rotationSpeed * Time.deltaTime);
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
        }
    }

    Quaternion CalculateRotation()
    {
        Quaternion temp = Quaternion.LookRotation(CalculateDirection(), Vector3.up);
        return temp;
    }

    Vector3 CalculateDirection()
    {
        Vector3 temp = (_touchDown - _touchUp).normalized;
        temp.z = temp.y;
        temp.y = 0;
        return temp;
    }
}
