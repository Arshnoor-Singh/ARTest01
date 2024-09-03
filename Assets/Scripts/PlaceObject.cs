using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlaceObject : MonoBehaviour
{
    [SerializeField] GameObject objectToPlacePrefab;

    private ARRaycastManager arRayCastManager;
    private ARPlaneManager arPlaneManager;
    private List<ARRaycastHit> arRayHits = new List<ARRaycastHit>();

    private void Awake()
    {
        arRayCastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger fingerTouch)
    {
        if(fingerTouch.index!= 0)
        {
            return;
        }

        if(arRayCastManager.Raycast(fingerTouch.currentTouch.screenPosition, arRayHits, TrackableType.PlaneWithinPolygon))
        {
            foreach(ARRaycastHit hit in arRayHits)
            {
                Pose orientation = hit.pose;
                GameObject spawnedObject = Instantiate(objectToPlacePrefab, orientation.position, orientation.rotation);
            }
        }
    }
}
