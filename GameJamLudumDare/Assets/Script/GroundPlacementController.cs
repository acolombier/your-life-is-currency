using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlacementController : MonoBehaviour
{

    public GameObject Controller;

    [SerializeField]
	private LayerMask ground;
    
    private GameObject currentPlaceableObject;

    private float mouseWheelRotation;

    private BuildingController buildingController;
    private Controller mController;

    public GameObject CurrentPlaceableObject { get => currentPlaceableObject; set
        {
            if (currentPlaceableObject != null)
                currentPlaceableObject.GetComponent<BuildingController>().Listener = null;
            currentPlaceableObject = value;
            if (value != null)
                value.GetComponent<BuildingController>().Listener = this;
        }
    }

    private void Start()
    {
        EventManager.StartListening("building_build_request", HandleNewObjectHotkey);
        mController = Controller.GetComponent<Controller>();
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
            CurrentPlaceableObject = Instantiate(placeableObjectPrefab);
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            mouseWheelRotation += Input.mouseScrollDelta.y;
            currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
        }
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            Destroy(currentPlaceableObject);
            CurrentPlaceableObject = null;

            foreach (BuildingController bc in FindObjectsOfType<BuildingController>())
                bc.ResetMaterial();
        }
    }

    public void Accept()
    {
        if (CanPlaceObject() && mController.CanBuy(currentPlaceableObject.GetComponent<Building>()))
        {
            mController.Buy(currentPlaceableObject.GetComponent<Building>());
            currentPlaceableObject.GetComponent<BuildingController>().PlaceBuilding();
            CurrentPlaceableObject = null;
        }
    }
}
