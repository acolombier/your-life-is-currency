using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingController : MonoBehaviour
{

    public float buildTimer;

    public bool canBuild = true;

    private bool isBuilt = false;

    public bool isPlaced = false;

    public LayerMask objectsToRemove;

    public Material canBuildMat;

    public Material nobuildMat;

    public float areaToRemoveObjects;

    public GameObject buildingProgress;

    private Collider col;

    private MeshRenderer meshRenderer;
    private GameObject mBuildingInProgress;

    Building building;
    public GroundPlacementController Listener;

    public bool IsBuilt {
        get => isBuilt;
        set {
            GetComponent<MeshRenderer>().enabled = value;

            if (!value)
            {
                mBuildingInProgress = Instantiate(buildingProgress);
                mBuildingInProgress.transform.position = transform.position;
                mBuildingInProgress.transform.rotation = transform.rotation;
            } else
            {
                Destroy(mBuildingInProgress);
            }

            isBuilt = value;
        }
    }

    public GameObject Progress { get
        {
            return mBuildingInProgress;
        }
    }

    private void Start()
    {
        col = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
        building = GetComponent<Building>();
    }

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaToRemoveObjects, objectsToRemove);
        
        if (!isBuilt && isPlaced && (Time.fixedTime >= buildTimer))
        {
            IsBuilt = true;
            BuildingManager.Instance.AddBuilding(building);
        }
        

    }

    public void PlaceBuilding()
    {
        col.isTrigger = true;
        IsBuilt = false;
        RemoveObjectInArea(transform.position, areaToRemoveObjects);

        buildTimer = Time.fixedTime + building.buildTime;
        mBuildingInProgress.GetComponent<Animator>().speed = 10f / building.buildTime;
        isPlaced = true;
    }

    private void Fade(Material material, float value)
    {
        Color color = material.color;
        color.a = value;
        material.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BuildingController>())
        {
            canBuild = false;
            meshRenderer.material = nobuildMat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BuildingController>())
        {
            ResetMaterial();
        }
    }

    public void ResetMaterial()
    {
        canBuild = true;
        meshRenderer.material = canBuildMat;

    }


    private void RemoveObjectInArea(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, objectsToRemove);

        int i = 0;
        while (i < hitColliders.Length)
        {
            Destroy(hitColliders[i].transform.gameObject);
            i++;
        }
    }

    public void OnMouseDown()
    {
        if (Listener == null)
        {
            return;
        }
        Listener.Accept();
    }
}
