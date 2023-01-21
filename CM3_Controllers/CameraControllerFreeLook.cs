using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraControllerFreeLook : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputProvider;
    [SerializeField] private CinemachineOrbitalFollow CameraZoom;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float zoomAcceleration = 2.5f;
    [SerializeField] private float zoomInnerRange = 3;
    [SerializeField] private float zoomOuterRange = 60;
    [SerializeField] private float zoomYAxis = 0f;

    private float currentOrbitRigRadius = 10f;
    private float newOrbitRigRadius = 10f;

    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    //rotate only on mouse button down
    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
        }
        return UnityEngine.Input.GetAxis(axisName);
    }

    //zoom "Cinemachine Orbital Follow" orbit radius with mousewheel
    public float ZoomYAxis
    {
        get { return zoomYAxis; }
        set
        {
            if (zoomYAxis == value) return;
            zoomYAxis = value;
            AdjustCameraZoomIndex(ZoomYAxis);
        }
    }
    private void Awake()
    {
        inputProvider.FindActionMap("FreeLookCamera").FindAction("MouseZoom").performed += cntxt => ZoomYAxis = cntxt.ReadValue<float>();
        inputProvider.FindActionMap("FreeLookCamera").FindAction("MouseZoom").canceled += cntxt => ZoomYAxis = 0f;
    }
    private void OnEnable()
    {
        inputProvider.FindAction("MouseZoom").Enable();
    }
    private void OnDisable()
    {
        inputProvider.FindAction("MouseZoom").Disable();
    }
    private void LateUpdate()
    {
        UpdateZoomLevel();
    }
    private void UpdateZoomLevel()
    {
        if (currentOrbitRigRadius == newOrbitRigRadius) { return; }
        currentOrbitRigRadius = Mathf.Lerp(currentOrbitRigRadius, newOrbitRigRadius, zoomAcceleration * Time.deltaTime);
        currentOrbitRigRadius = Mathf.Clamp(currentOrbitRigRadius, zoomInnerRange, zoomOuterRange);

        CameraZoom.Radius = currentOrbitRigRadius;
    }
    private void AdjustCameraZoomIndex(float zoomYAxis)
    {
        if (zoomYAxis == 0) { return; }
        if (zoomYAxis < 0)
        {
            newOrbitRigRadius = currentOrbitRigRadius + zoomSpeed;
        }
        if (zoomYAxis > 0)
        {
            newOrbitRigRadius = currentOrbitRigRadius - zoomSpeed;
        }
    }
}