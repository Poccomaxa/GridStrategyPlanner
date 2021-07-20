using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    PlayerInput PlayerInput;
    Camera MainCamera;
    Vector2 CameraSpeed;
    float RotationDirectionSpeed;
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
        Vector2 Orientation = new Vector2(MainCamera.transform.forward.x, MainCamera.transform.forward.z);
        Orientation.Normalize();
        float Angle = Vector2.SignedAngle(Vector2.up, Orientation);        
        Vector2 CameraSpeedRotated = Quaternion.Euler(0, 0, Angle) * CameraSpeed;
        MainCamera.transform.position += new Vector3(CameraSpeedRotated.x, 0, CameraSpeedRotated.y);

        Plane Floor = new Plane(Vector3.up, Vector3.zero);
        float Enter = 0;
        Ray LookingForFloor = new Ray(MainCamera.transform.position, MainCamera.transform.forward);
        if (Floor.Raycast(LookingForFloor, out Enter))
        {
            Vector3 RotateAround = LookingForFloor.origin + LookingForFloor.direction * Enter;
            MainCamera.transform.RotateAround(RotateAround, Vector3.up, RotationDirectionSpeed);
        }        
    }

    public void OnNavigate(InputAction.CallbackContext callbackContext)
    {        
        CameraSpeed = callbackContext.ReadValue<Vector2>() * CameraSensitivy;     
    }

    public void OnRotate(InputAction.CallbackContext callbackContext)
    {
        RotationDirectionSpeed = callbackContext.ReadValue<System.Single>();        
    }
}
