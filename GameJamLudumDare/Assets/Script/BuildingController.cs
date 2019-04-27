using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {

	public float buildTimer;
	public bool canBuild = true;
	public bool isBuilt = true;
    
	public Material canBuildMat;

	public Material nobuildMat;

	public void Start()
	{

	}

	public void PlaceBuilding(){
            
        
	    //	GetComponent<BoxCollider>().enabled = true;
	    GetComponent<BoxCollider>().isTrigger = true;

	}

	private void Fade(Material material, float value)
    {
        Color color = material.color;
        color.a = value;
        material.color = color;
    }

	private void OnTriggerEnter(Collider other)
	{
		if( other.GetComponent<BuildingController>() == null ){
			GetComponent<MeshRenderer>().material = canBuildMat;
			canBuild = true;
			return;
		}

		canBuild = false;

		GetComponent<MeshRenderer>().material = nobuildMat;

    }

	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<BuildingController>() == null)
        {
			GetComponent<MeshRenderer>().material = canBuildMat;
            canBuild = true;
            return;
        }
        
		canBuild = true;
		GetComponent<MeshRenderer>().material = canBuildMat;

	}

}
