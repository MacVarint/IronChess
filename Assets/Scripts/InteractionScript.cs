using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    Vector3 pos;

    public float aspectHeight = 20f;
    public float aspectWidth = 9f;
    //Joinked Variables
    Vector2 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    //end Joinked Variables

    // Start is called before the first frame update
    void Start()
    {
        AspectRatio();
    }

    // Update is called once per frame
    void Update()
    {
  
        //Checks if there is an Input
        if (Input.touchCount > 0)
        {
            TouchCount();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos, pos + Vector3.forward * 10);
    }
    public void AspectRatio()
    {
        aspectHeight = aspectHeight * 2;
        aspectWidth = aspectWidth * 2;
    }
    private void TouchCount()
    {
        if (Input.touchCount == 1)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            pos.z = -5;
            RaycastHit hit;
            if (Physics.Raycast(pos, transform.forward, out hit, 10))
            {
                Debug.Log(hit.transform.name);
            }
        }
        //began phase
        if (Input.GetTouch(1).phase == TouchPhase.Began)
        {
            touchStart = (Input.GetTouch(1).position + Input.GetTouch(0).position)/2;
        }
        if (Input.touchCount == 2)
        {
            JoinkedZoomCode();
            Vector2 directionV2 = touchStart - ((Input.GetTouch(1).position + Input.GetTouch(0).position) / 2);
            Vector3 direction = new Vector3(directionV2.x/Screen.width * aspectWidth, directionV2.y/Screen.height * aspectHeight, 0);
            Camera.main.transform.position += direction;
            Camera.main.transform.position = Clamp(Camera.main.transform.position, -10, 10, -10, 10);
            touchStart = ((Input.GetTouch(1).position + Input.GetTouch(0).position) / 2);
        }
    }
    public void JoinkedZoomCode()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;
        JoinkedZoom(difference * 0.01f);
    }
    void JoinkedZoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
    public Vector3 Clamp(Vector3 target, float minX, float maxX, float minY, float maxY)
    {
        target.x = Mathf.Clamp(target.x, minX, maxX);
        target.y = Mathf.Clamp(target.y, minY, maxY);
        return target;
    }
}
