using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    PlayerInput PlayerInput;
    Camera MainCamera;
    Vector2 CameraSpeed;
    public float CameraSensitivy;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        MainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDestroy()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 Orientation = MainCamera.transform.forward;
        MainCamera.transform.position += new Vector3(CameraSpeed.x, 0, CameraSpeed.y);
    }

    public void OnNavigate(InputAction.CallbackContext callbackContext)
    {        
        CameraSpeed = callbackContext.ReadValue<Vector2>() * CameraSensitivy;        
        if (callbackContext.canceled)
        {
            CameraSpeed = Vector2.zero;
        }        
    }
}
