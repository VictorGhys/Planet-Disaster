using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    private Vector2 oldPos;

    [SerializeField] private Vector3 rotateAroundPoint;
    [SerializeField] private float rotateSpeed = 0.5f;
    private float cameraDistance;
    [SerializeField] private float minCameraDistance = 30;
    [SerializeField] private float maxCameraDistance = 100;

    // Start is called before the first frame update
    private void Start()
    {
        cameraDistance = Vector3.Distance(transform.position, rotateAroundPoint);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            float moveX = oldPos.x - x;
            float moveY = oldPos.y - y;
            transform.RotateAround(rotateAroundPoint, transform.up, -moveX * rotateSpeed);
            transform.RotateAround(rotateAroundPoint, transform.right, moveY * rotateSpeed);
            oldPos.x = Input.mousePosition.x;
            oldPos.y = Input.mousePosition.y;
        }
        else
        {
            oldPos.x = Input.mousePosition.x;
            oldPos.y = Input.mousePosition.y;
        }
    }
}