using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    private bool resetHighlight = false;
    public GameObject greenTile;
    public GameObject yellowTile;
    private GameObject newRedTile;
    private Vector2Int newRedTileGridPos = new Vector2Int(0,0);
    public Transform parent;

    Dictionary<Vector2Int, GameObject> newGreenTilesPositions = new Dictionary<Vector2Int, GameObject>();

    GridManager gridManager;

    public Players players;

    void Start()
    {
        players = Camera.main.GetComponent<Players>();
        gridManager = parent.GetComponent<GridManager>();
    }

    void Update()
    {
        //Checks if there is an Input
        if (Input.touchCount > 0)
        {
            TouchCount();
        }
        if (Input.touchCount == 0 && resetHighlight)
        {
            Destroy(newRedTile);
            resetHighlight = false;
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
            pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            pos.z = -5;
            RaycastHit hit;
            if (Physics.Raycast(pos, transform.forward, out hit, 10))
            {
                //checks if the chesspiece is currently moveble compared to playerTurns
                for (int i = 0; i < players.piecesPlayer.Length; i++)
                {
                    if (!players.turn) 
                    {
                        //Checks if the Selected is White
                        if (GridManager.Board[newRedTileGridPos.x, newRedTileGridPos.y].current == players.piecesPlayer[i])
                        {
                            MoveSelected(hit);
                            break;
                        }
                    }
                    if (players.turn)
                    {
                        //Checks if the Selected is Black
                        if (GridManager.Board[newRedTileGridPos.x, newRedTileGridPos.y].current == players.piecesOpponent[i])
                        {
                            MoveSelected(hit);
                            break;
                        }
                    }
                }
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
    public void RedTileController(RaycastHit hit)
    {
        Destroy(newRedTile);
        newRedTile = Instantiate(redTile, new Vector3(redTile.transform.position.x, redTile.transform.position.y, redTile.transform.position.z), Quaternion.identity, parent);
        newRedTile.transform.position = hit.transform.position;
        newRedTileGridPos = hit.transform.GetComponent<Tile>().gridpos;
    }
    public void GreenTileController(RaycastHit hit)
    {
        Vector2Int currentPos = hit.transform.GetComponent<Tile>().gridpos;
        Vector2[] movesCurrent = Moves.getMoveSet(hit.transform.GetComponent<Tile>().current);
        if (movesCurrent != null)
        {
            newGreenTilesPositions.Clear();
            for (int i = 0; i < movesCurrent.Length; i++)
            {
                bool repeat = Moves.getRepeat(hit.transform.GetComponent<Tile>().current);
                if (!repeat)
                {
                    Vector2Int tempNext = new Vector2Int(currentPos.x + (int)movesCurrent[i].x, currentPos.y + (int)movesCurrent[i].y);
                    if (tempNext.x < 8 && tempNext.x >= 0 && tempNext.y < 8 && tempNext.y >= 0 && GridManager.Board[tempNext.x, tempNext.y].current == Tile.chessPiece.None)
                    {
                        InstantiateGreenTiles(movesCurrent[i], 1, hit, tempNext);
                    }
                    else if (tempNext.x < 8 && tempNext.x >= 0 && tempNext.y < 8 && tempNext.y >= 0 && GridManager.Board[tempNext.x, tempNext.y].current != Tile.chessPiece.None)
                    {
                        InstantiateYellowTiles(movesCurrent[i], 1, hit, tempNext);
                    }
                }
                else
                {
                    Vector2Int tempNext = new Vector2Int(currentPos.x, currentPos.y);
                    for (int j = 1; j < 8; j++)
                    {
                        tempNext += new Vector2Int((int)movesCurrent[i].x, (int)movesCurrent[i].y);
                        if (tempNext.x < 8 && tempNext.x >= 0 && tempNext.y < 8 && tempNext.y >= 0 && GridManager.Board[tempNext.x, tempNext.y].current == Tile.chessPiece.None)
                        {
                            InstantiateGreenTiles(movesCurrent[i], j, hit, tempNext);
                        }
                        else if (tempNext.x < 8 && tempNext.x >= 0 && tempNext.y < 8 && tempNext.y >= 0 && GridManager.Board[tempNext.x, tempNext.y].current != Tile.chessPiece.None)
                        {
                            InstantiateYellowTiles(movesCurrent[i], j, hit, tempNext);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void InstantiateGreenTiles(Vector2 movesCurrent, int j, RaycastHit hit, Vector2Int tempNext)
    {
        GameObject newGreenTile = Instantiate(greenTile, new Vector3(redTile.transform.position.x, redTile.transform.position.y, redTile.transform.position.z), Quaternion.identity, newRedTile.transform);
        newGreenTile.transform.position = new Vector3(newRedTile.transform.position.x + movesCurrent.x * j * 2, newRedTile.transform.position.y + movesCurrent.y * j * 2, hit.transform.position.z);
        newGreenTilesPositions.Add(tempNext, newGreenTile);
    }
    private void InstantiateYellowTiles(Vector2 movesCurrent, int j, RaycastHit hit, Vector2Int tempNext)
    {
        GameObject newYellowTile = Instantiate(yellowTile, new Vector3(redTile.transform.position.x, redTile.transform.position.y, redTile.transform.position.z), Quaternion.identity, newRedTile.transform);
        newYellowTile.transform.position = new Vector3(newRedTile.transform.position.x + movesCurrent.x * j * 2, newRedTile.transform.position.y + movesCurrent.y * j * 2, hit.transform.position.z);
        newGreenTilesPositions.Add(tempNext, newYellowTile);
        
    }
    private void MoveSelected(RaycastHit hit)
    {
        //Checks if the clicked tile is highlighted(green)
        if (newGreenTilesPositions.ContainsKey(hit.transform.GetComponent<Tile>().gridpos))
        {
            //Checks if the target is empty or an enemy
            if (players.AttackandMoveController(hit.transform.GetComponent<Tile>().current))
            {
                hit.transform.GetComponent<Tile>().current = GridManager.Board[newRedTileGridPos.x, newRedTileGridPos.y].current;
                GridManager.Board[newRedTileGridPos.x, newRedTileGridPos.y].current = Tile.chessPiece.None;
                players.turn = !players.turn;
                resetHighlight = true;
                gridManager.AssignSprites();
            }
        }
        //calls up the tiling systems
        else
        {
            RedTileController(hit);
            GreenTileController(hit);
        }
    }
}
