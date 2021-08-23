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
    [SerializeField] private float zoomSpeed = 1;

    // Start is called before the first frame update
    private void Start()
    {
        cameraDistance = Vector3.Distance(transform.position, rotateAroundPoint);
    }

    // Update is called once per frame
    private void Update()
    {
        // rotate around
        if (Input.GetButton("Fire2"))
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
        //zooming
        if (Input.mouseScrollDelta.y > 0 && Vector3.Distance(Vector3.zero, transform.position) > minCameraDistance)
        {
            transform.position += transform.forward * Input.mouseScrollDelta.y * zoomSpeed;
        }
        if (Input.mouseScrollDelta.y < 0 && Vector3.Distance(Vector3.zero, transform.position) < maxCameraDistance)
        {
            transform.position += transform.forward * Input.mouseScrollDelta.y * zoomSpeed;
        }
    }
}