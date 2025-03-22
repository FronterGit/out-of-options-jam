using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    [SerializeField] private new Camera camera;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float zoomAmount;
    [SerializeField] private float defaultZoom;

    [SerializeField] private Vector3 targetRotation;

    private Vector3 defaultRotation;


    // Start is called before the first frame update
    void Start()
    {
        defaultRotation = camera.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
        ZoomToPoint();
        
    }

    public void ZoomToPoint()
    {
        float targetZoom = Input.GetKey(KeyCode.Space) ? zoomAmount : defaultZoom;

        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetZoom, Time.deltaTime * zoomSpeed);


        Vector3 newRotation = Input.GetKey(KeyCode.Space) ? targetRotation : defaultRotation;
        // smooth lerp the camera rotation to the target rotation
        camera.transform.eulerAngles = Vector3.Lerp(camera.transform.eulerAngles, newRotation, Time.deltaTime * zoomSpeed);



    }
}
