using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [Header("Movement setting")]
    public float MaximumSpeed = 0.5f;
    public float ScreenThreshold = 0.1f;

    [Header("Rotation setting")]
    public float RotationSpeed = 0.1f;

    [Header("Zoom setting")]
    public float ZoomSpeed = 0.1f;
    public float MaximumZoom = 2f;
    public float MinimumZoom = .3f;

    private float mMouseRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.mousePosition.x / Screen.width;
        float y = Input.mousePosition.y / Screen.height;
        
        float zoom = Mathf.Min(MaximumZoom, Mathf.Max(MinimumZoom, transform.localScale.y + (-Input.mouseScrollDelta.y * ZoomSpeed * Time.deltaTime)));

        transform.localScale = new Vector3(transform.localScale.x, zoom, transform.localScale.z);

        if (mMouseRotation != 0f)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (mMouseRotation - x) * RotationSpeed, transform.eulerAngles.z);
            mMouseRotation = x;
        } else
        {
            Vector3 pos = new Vector3();

            float speed = 1.0f;
            float xInMovingArea = Mathf.Abs(x - 0.5f) - 0.5f + ScreenThreshold;
            float yInMovingArea = Mathf.Abs(y - 0.5f) - 0.5f + ScreenThreshold;

            if (xInMovingArea > 0)
            {
                pos += transform.right * (x < 0.5f ? -1 : 1);
                speed = xInMovingArea;
            }

            if (yInMovingArea > 0)
            {
                pos += transform.forward * (y < 0.5f ? -1 : 1);
                speed = Mathf.Min(speed, yInMovingArea);
            }

            transform.position += pos * speed * (transform.localScale.z / MaximumZoom) * MaximumSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1))
        {
            mMouseRotation = x;
        }

        if (Input.GetMouseButtonUp(1))
        {
            mMouseRotation = 0f;
        }
    }
}
