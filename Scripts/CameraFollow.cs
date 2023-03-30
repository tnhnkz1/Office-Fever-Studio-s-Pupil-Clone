using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float _chaseSpeed = 10;

    void Start()
    {
        if (!target)
        {
            target = GameObject.FindObjectOfType<PlayerMovement>().transform;
        }
    }

    
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, _chaseSpeed * Time.deltaTime);
    }
}
