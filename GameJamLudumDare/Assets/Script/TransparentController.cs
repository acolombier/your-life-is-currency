using UnityEngine;

public class TransparentController : MonoBehaviour
{

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<BuildingController>())
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<BuildingController>())
        {
            meshRenderer.enabled = true;
        }
    }
}
