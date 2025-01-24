using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float currentZoom;

    private Vector2 lastMousePosition;
    private bool isRightMouseDown;
    [SerializeField] private float mouseSensitivity;


    private void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, 1f, 20f);

        Camera.main.orthographicSize = currentZoom;

        MoveCamera();

    }


    private void MoveCamera()
    {

        // 检查鼠标右键是否被按下
        if (Input.GetMouseButton(1))
        {
            isRightMouseDown = true;
        }
        else
        {
            isRightMouseDown = false;
            lastMousePosition = Input.mousePosition;
        }

        //if (Input.GetMouseButtonUp(1))
        //{
        //    lastMousePosition = Input.mousePosition;
        //}

        // 如果鼠标右键被按住，并且鼠标正在移动
        if (isRightMouseDown && (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - lastMousePosition).sqrMagnitude > 0)
        {
            Vector2 delta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - lastMousePosition; // 计算鼠标移动的差值
            transform.Translate(-delta * mouseSensitivity, Space.Self); // 根据鼠标移动差值移动摄像头
            lastMousePosition = Input.mousePosition; // 更新鼠标位置
        }
    }
}



