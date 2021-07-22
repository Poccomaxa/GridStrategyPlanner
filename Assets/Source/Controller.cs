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
    public float CameraRotationSensitivity;
    public float MinZoom;
    public float MaxZoom;
    public int Steps;

    public GameObject BrushPrefab;
    public GameObject Brush;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        MainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateZoom(0);

        Brush = GameObject.Instantiate(BrushPrefab);
    }

    private void OnDestroy()
    {

    }

    bool CameraPlaneIntersection(out Vector3 Intersection)
    {
        Plane Floor = new Plane(Vector3.up, Vector3.zero);
        float Enter = 0;
        Ray LookingForFloor = new Ray(MainCamera.transform.position, MainCamera.transform.forward);
        if (Floor.Raycast(LookingForFloor, out Enter))
        {
            Intersection = LookingForFloor.origin + LookingForFloor.direction * Enter;
            return true;
        }
        
        Intersection = Vector3.zero;
        return false;        
    }

    void UpdateZoom(int Scroll)
    {
        if (CameraPlaneIntersection(out Vector3 PlaneIntersection))
        {
            Vector3 CameraArm = MainCamera.transform.position - PlaneIntersection;
            float CurrentDistance = Mathf.Max(0, CameraArm.magnitude - MinZoom);
            float StepDistance = (MaxZoom - MinZoom) / Steps;
            int CurrentSteps = Mathf.RoundToInt(CurrentDistance / StepDistance);
            CurrentSteps = Mathf.Clamp(CurrentSteps + (int)Scroll, 0, Steps);

            float NewDistance = CurrentSteps * StepDistance + MinZoom;
            MainCamera.transform.position = PlaneIntersection + CameraArm.normalized * NewDistance;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CameraPlaneIntersection(out Vector3 RotateAround))
        {
            Vector2 Orientation = new Vector2(MainCamera.transform.forward.x, MainCamera.transform.forward.z);
            Orientation.Normalize();
            float Angle = Vector2.SignedAngle(Vector2.up, Orientation);
            Vector2 CameraSpeedRotated = Quaternion.Euler(0, 0, Angle) * CameraSpeed;
            MainCamera.transform.position += new Vector3(CameraSpeedRotated.x, 0, CameraSpeedRotated.y);

            MainCamera.transform.RotateAround(RotateAround, Vector3.up, RotationDirectionSpeed);
        }        
    }

    public void OnNavigate(InputAction.CallbackContext callbackContext)
    {        
        CameraSpeed = callbackContext.ReadValue<Vector2>() * CameraSensitivy;     
    }

    public void OnRotate(InputAction.CallbackContext callbackContext)
    {
        RotationDirectionSpeed = callbackContext.ReadValue<System.Single>() * CameraRotationSensitivity;        
    }

    public void OnZoom(InputAction.CallbackContext callbackContext)
    {
        float ScrollValue = callbackContext.ReadValue<Vector2>().y;
        if (Mathf.Abs(ScrollValue) > float.Epsilon)
        {
            int Scroll = (int)Mathf.Sign(ScrollValue) * -1;
            UpdateZoom(Scroll);            
        }
    }

    public void OnPoint(InputAction.CallbackContext callbackContext)
    {
        if (MainCamera)
        {
            Vector2 MousePosition = callbackContext.ReadValue<Vector2>();
            Ray CameraRay = MainCamera.ScreenPointToRay(MousePosition);

            if (new Plane(Vector3.up, Vector3.zero).Raycast(CameraRay, out float distance))
            {
                Vector3 PointAt = CameraRay.origin + CameraRay.direction * distance;
                Brush.transform.position = PointAt;
            }
        }
    }
}
