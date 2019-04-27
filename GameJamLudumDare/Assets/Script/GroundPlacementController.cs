using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlacementController : MonoBehaviour {


	[SerializeField]
    private GameObject placeableObjectPrefab;

	[SerializeField]
	private LayerMask ground;

    [SerializeField]
    private KeyCode newObjectHotkey = KeyCode.A;

    private GameObject currentPlaceableObject;
    
    private float mouseWheelRotation;

	private BuildingController buildingController;

	private void Update()
    {
        HandleNewObjectHotkey();

        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }
    }
    
	private bool CanPlaceObject(){

		//Debug.Log(currentPlaceableObject);

		//currentPlaceableObject.GetComponent<BuildingController>().PlaceBuilding();

		return currentPlaceableObject.GetComponent<BuildingController>().canBuild;
	}

    private void HandleNewObjectHotkey()
    {
        if (Input.GetKeyDown(newObjectHotkey))
        {
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
            }
            else
            {
                currentPlaceableObject = Instantiate(placeableObjectPrefab);
            }
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
        Debug.Log(Input.mouseScrollDelta);
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    private void ReleaseIfClicked()
    {

        if (Input.GetMouseButtonDown(0))
        {
			if( CanPlaceObject() ){
				currentPlaceableObject = null;
			}
          
        }
    }
}
