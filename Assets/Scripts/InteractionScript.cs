using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    Vector3 pos;

    //Joinked Variables
    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    //end Joinked Variables

    // Start is called before the first frame update
    void Start()
    {

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
    private void TouchCount()
    {
        /*if (Input.touchCount == 1)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            pos.z = -5;
            RaycastHit hit;
            if (Physics.Raycast(pos, transform.forward, out hit, 10))
            {
                Debug.Log(hit.transform.name);
            }
        }*/
        if (Input.GetMouseButtonDown(1))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(1))
        {
            JoinkedZoomAndMoveCode();
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
        else
        {
            //Nothing
        }
    }
    public void JoinkedZoomAndMoveCode()
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
}
