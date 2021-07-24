using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    PlayerInput playerInput;
    Camera mainCamera;
    Vector2 cameraSpeed;
    float rotationDirectionSpeed;

    public float cameraSensitivy;
    public float cameraRotationSensitivity;
    public float minZoom;
    public float maxZoom;
    public int steps;

    public GameObject brushPrefab;
    public Foundation brush;
    public Foundation MainGrid;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateZoom(0);

        brush = GameObject.Instantiate(brushPrefab).GetComponent<Foundation>();
    }

    private void OnDestroy()
    {

    }

    bool CameraPlaneIntersection(out Vector3 Intersection)
    {
        Plane Floor = new Plane(Vector3.up, Vector3.zero);
        float Enter = 0;
        Ray LookingForFloor = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
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
            Vector3 CameraArm = mainCamera.transform.position - PlaneIntersection;
            float CurrentDistance = Mathf.Max(0, CameraArm.magnitude - minZoom);
            float StepDistance = (maxZoom - minZoom) / steps;
            int CurrentSteps = Mathf.RoundToInt(CurrentDistance / StepDistance);
            CurrentSteps = Mathf.Clamp(CurrentSteps + (int)Scroll, 0, steps);

            float NewDistance = CurrentSteps * StepDistance + minZoom;
            mainCamera.transform.position = PlaneIntersection + CameraArm.normalized * NewDistance;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CameraPlaneIntersection(out Vector3 RotateAround))
        {
            Vector2 Orientation = new Vector2(mainCamera.transform.forward.x, mainCamera.transform.forward.z);
            Orientation.Normalize();
            float Angle = Vector2.SignedAngle(Vector2.up, Orientation);
            Vector2 CameraSpeedRotated = Quaternion.Euler(0, 0, Angle) * cameraSpeed;
            mainCamera.transform.position += new Vector3(CameraSpeedRotated.x, 0, CameraSpeedRotated.y);

            mainCamera.transform.RotateAround(RotateAround, Vector3.up, rotationDirectionSpeed);
        }        
    }

    public void OnNavigate(InputAction.CallbackContext callbackContext)
    {        
        cameraSpeed = callbackContext.ReadValue<Vector2>() * cameraSensitivy;     
    }

    public void OnRotate(InputAction.CallbackContext callbackContext)
    {
        rotationDirectionSpeed = callbackContext.ReadValue<System.Single>() * cameraRotationSensitivity;        
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
        if (mainCamera && brush)
        {
            Vector2 MousePosition = callbackContext.ReadValue<Vector2>();
            Ray CameraRay = mainCamera.ScreenPointToRay(MousePosition);

            if (new Plane(Vector3.up, Vector3.zero).Raycast(CameraRay, out float distance))
            {
                Vector3 PointAt = CameraRay.origin + CameraRay.direction * distance;

                GridCell closestCell = MainGrid.GetClosestCell(PointAt);
                //Debug.LogFormat("Closest grid cell: {0}", closestCell.transform.position);
                if (closestCell != null)
                {
                    GridCell closestBrushCell = brush.GetClosestCell(new Vector3(0, 0, 0));
                    //Debug.LogFormat("Closest brush cell: {0}", closestBrushCell.transform.localPosition);
                    brush.transform.position = closestCell.transform.position - closestBrushCell.transform.localPosition;
                }
                else
                {
                    brush.transform.position = PointAt;
                }
            }
        }
    }

    public void OnClick(InputAction.CallbackContext callbackContext)
    {
        if (mainCamera && brush)
        {
            GameObject PlacedObject = GameObject.Instantiate(brush.gameObject);
            PlacedObject.transform.position = brush.transform.position;
        }
    }
}
