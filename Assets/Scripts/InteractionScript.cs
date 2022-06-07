using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    Vector3 pos;
    Vector3 cameraLockTopRight;
    Vector3 cameraLockBottomLeft;
    public float aspectHeight = 20f;
    public float aspectWidth = 9f;
    //Joinked Variables
    Vector2 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 20;
    //end Joinked Variables
    public GameObject redTile;
    public GameObject greenTile;
    private GameObject newRedTile;
    public Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        redTile.SetActive(enabled);
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if there is an Input
        if (Input.touchCount > 0)
        {
            TouchCount();
        }
        CameraLock();

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos, pos + Vector3.forward * 10);
        Gizmos.DrawLine(cameraLockTopRight, cameraLockTopRight + Vector3.forward * 10);
        Gizmos.DrawLine(cameraLockBottomLeft, cameraLockBottomLeft + Vector3.forward * 10);
    }
    private void CameraLock()
    {
        cameraLockTopRight = new Vector3(Camera.main.transform.position.x + aspectWidth / 20 * Camera.main.orthographicSize, Camera.main.transform.position.y + aspectHeight / 20 * Camera.main.orthographicSize, -10);
        cameraLockBottomLeft = new Vector3(Camera.main.transform.position.x + -aspectWidth / 20 * Camera.main.orthographicSize, Camera.main.transform.position.y + -aspectHeight / 20 * Camera.main.orthographicSize, -10);

        if (cameraLockTopRight.x > aspectWidth)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - (cameraLockTopRight.x - aspectWidth), Camera.main.transform.position.y, -10);
        }
        if (cameraLockTopRight.y > aspectHeight)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - (cameraLockTopRight.y - aspectHeight), -10);
        }
        if (cameraLockBottomLeft.x < -aspectWidth)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - (cameraLockBottomLeft.x + aspectWidth), Camera.main.transform.position.y, -10);
        }
        if (cameraLockBottomLeft.y < -aspectHeight)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - (cameraLockBottomLeft.y + aspectHeight), -10);
        }
    }
    private void TouchCount()
    {
        if (Input.touchCount == 1)
        {
            TileSelection();
        }
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                touchStart = (Input.GetTouch(1).position + Input.GetTouch(0).position) / 2;
            }
            JoinkedZoomCode();
            Vector2 directionV2 = touchStart - ((Input.GetTouch(1).position + Input.GetTouch(0).position) / 2);
            Vector3 direction = new Vector3(directionV2.x/Screen.width * aspectWidth * 2 / 20 * Camera.main.orthographicSize, directionV2.y/Screen.height * aspectHeight * 2 / 20 * Camera.main.orthographicSize, 0);
            Camera.main.transform.position += direction;
            //Camera locks


            Camera.main.transform.position = Clamp(Camera.main.transform.position, -8.55f, 8.55f, -19, 19, -10, -10);



            touchStart = ((Input.GetTouch(1).position + Input.GetTouch(0).position) / 2);
        }
    }
    private void TileSelection()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Destroy(newRedTile);
            pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            pos.z = -5;
            RaycastHit hit;
            if (Physics.Raycast(pos, transform.forward, out hit, 10))
            {
                Debug.Log(hit.transform.name);
                newRedTile = Instantiate(redTile, new Vector3(redTile.transform.position.x, redTile.transform.position.y, redTile.transform.position.z), Quaternion.identity, parent);
                newRedTile.transform.position = hit.transform.position;
                GameObject newGreenTile = Instantiate(greenTile, new Vector3(redTile.transform.position.x, redTile.transform.position.y, redTile.transform.position.z), Quaternion.identity, newRedTile.transform);
                newGreenTile.transform.position = new Vector3(hit.transform.position.x + 2, hit.transform.position.y, hit.transform.position.z);
                //er is hier ergens een error maar ik weet niet waar
                Vector2[] movesCurrent = Moves.getMoveSet(hit.transform.GetComponent<Tile>().current);
                if (movesCurrent != null)
                {
                    for (int i = 0; i < movesCurrent.Length; i++)
                    {
                        Debug.Log(movesCurrent[i]);
                    }
                }
                Debug.Log(hit.transform.GetComponent<Tile>().gridpos);
            }
        }
    }
    void JoinkedZoomCode()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;
        JoinkedZoom(difference * 0.01f / 20 * Camera.main.orthographicSize);
    }
    void JoinkedZoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
    public Vector3 Clamp(Vector3 target, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        target.x = Mathf.Clamp(target.x, minX, maxX);
        target.y = Mathf.Clamp(target.y, minY, maxY);
        target.z = Mathf.Clamp(target.z, minZ, maxZ);
        return target;
    }    
}
