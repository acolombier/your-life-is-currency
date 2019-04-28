using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlacementController : MonoBehaviour
{

    [SerializeField]
	private LayerMask ground;
    
    private GameObject currentPlaceableObject;

    private float mouseWheelRotation;

    private BuildingController buildingController;

    private void Start()
    {
        EventManager.StartListening("building_build_request", HandleNewObjectHotkey);
    }

    private void Update()
    {
        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }
    }

    private bool CanPlaceObject()
    {
        return currentPlaceableObject.GetComponent<BuildingController>().canBuild;
    }

    private void HandleNewObjectHotkey(object[] args)
    {
        GameObject placeableObjectPrefab = (GameObject)args[0];
        if (currentPlaceableObject != null)
        {
            Destroy(currentPlaceableObject);
        }
        else
        {
            currentPlaceableObject = Instantiate(placeableObjectPrefab);
        }
    }

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ground))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void RotateFromMouseWheel()
    {
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CanPlaceObject())
            {

                currentPlaceableObject.GetComponent<BuildingController>().PlaceBuilding();
                currentPlaceableObject = null;

            }

        }
    }
}
