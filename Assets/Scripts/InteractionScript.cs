using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

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
    public GameObject yellowTile;
    private GameObject newRedTile;
    private Vector2Int newRedTileGridPos = new Vector2Int(0,0);
    public Transform parent;

    Dictionary<Vector2Int, GameObject> newGreenTilesPositions = new Dictionary<Vector2Int, GameObject>();

    GridManager gridManager;

    public Players players;

    GameObject escapeMenuPlayer;
    GameObject escapeMenuOpponent;

    void Start()
    {
        players = Camera.main.GetComponent<Players>();
        gridManager = parent.GetComponent<GridManager>();

        escapeMenuPlayer = GameObject.Find("PlayField/Escape Menu Player");
        escapeMenuOpponent = GameObject.Find("PlayField/Escape Menu Opponent");

        if (escapeMenuPlayer != null)
        {
            escapeMenuPlayer.SetActive(false);
        }
        if (escapeMenuOpponent != null)
        {
            escapeMenuOpponent.SetActive(false);
        }
    }

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

    //Makes sure the camera stays within the borders op the playing field no matter the zoom. 
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
                if (hit.transform.GetComponent<Tile>() == null)
                {
                    OffBoardButtons(hit);
                }
                else
                {
                    InteractionOrSelection(hit);
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

    public void CreateRedTile(RaycastHit hit)
    {
        if (newRedTile != null)
        {
            Destroy(newRedTile);
        }
        newRedTile = Instantiate(redTile, new Vector3(redTile.transform.position.x, redTile.transform.position.y, redTile.transform.position.z), Quaternion.identity, parent);
        newRedTile.transform.position = hit.transform.position;
        newRedTileGridPos = hit.transform.GetComponent<Tile>().gridpos;

        CreateGreenTiles(hit);
    }

    public void CreateGreenTiles(RaycastHit hit)
    {
        Vector2Int currentPos = hit.transform.GetComponent<Tile>().gridpos;
        Tile.chessPiece hitPiece = hit.transform.GetComponent<Tile>().current;
        Vector2[] movesCurrent = Moves.getMoveSet(hitPiece);
        if (movesCurrent != null)
        {
            newGreenTilesPositions.Clear();

            for (int i = 0; i < movesCurrent.Length; i++)
            {
                bool repeat = Moves.getRepeat(hitPiece);
                if (!repeat)
                {
                    Vector2Int tempNext = new Vector2Int(currentPos.x + (int)movesCurrent[i].x, currentPos.y + (int)movesCurrent[i].y);
                    if (tempNext.x < 8 && tempNext.x >= 0 && tempNext.y < 8 && tempNext.y >= 0)
                    {
                        //checks if the current piece is a pawn
                        if (hitPiece == Tile.chessPiece.PawnOpponent || hitPiece == Tile.chessPiece.PawnPlayer)
                        {
                            //Checks if tempNext is within the bounds of the Board array
                            //BUGGGGGGG
                            if (tempNext.x + 1 < 8)
                            {
                                if (players.CanIHitThis(GridManager.Board[tempNext.x + 1, tempNext.y].current))
                                {
                                    InstantiateYellowTiles(new Vector2(movesCurrent[i].x + 1, movesCurrent[i].y), 1, hit, new Vector2Int(tempNext.x + 1, tempNext.y));
                                }
                            }
                            if (tempNext.x - 1 >= 0)
                            {
                                Debug.Log("awdaw");
                                if (players.CanIHitThis(GridManager.Board[tempNext.x - 1, tempNext.y].current))
                                {
                                    InstantiateYellowTiles(new Vector2(movesCurrent[i].x - 1, movesCurrent[i].y), 1, hit, new Vector2Int(tempNext.x - 1, tempNext.y));
                                }
                            }
                            if (GridManager.Board[tempNext.x, tempNext.y].current == Tile.chessPiece.None)
                            {
                                InstantiateGreenTiles(movesCurrent[i], 1, hit, tempNext);
                                //checks if the pawn has not moved yet
                                if (hit.transform.GetComponent<Tile>().hasNotMovedYet)
                                {
                                    //checks from what team the current is
                                    if (hitPiece == Tile.chessPiece.PawnOpponent)
                                    {
                                        if (GridManager.Board[tempNext.x, tempNext.y - 1].current == Tile.chessPiece.None)
                                        {
                                            InstantiateGreenTiles(new Vector2(movesCurrent[i].x, movesCurrent[i].y - 1), 1, hit, new Vector2Int(tempNext.x, tempNext.y - 1));
                                        }
                                    }
                                    else
                                    {
                                        if (GridManager.Board[tempNext.x, tempNext.y + 1].current == Tile.chessPiece.None)
                                        {
                                            InstantiateGreenTiles(new Vector2(movesCurrent[i].x, movesCurrent[i].y + 1), 1, hit, new Vector2Int(tempNext.x, tempNext.y + 1));
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (GridManager.Board[tempNext.x, tempNext.y].current == Tile.chessPiece.None)
                            {
                                InstantiateGreenTiles(movesCurrent[i], 1, hit, tempNext);
                            }
                            else if (players.CanIHitThis(GridManager.Board[tempNext.x, tempNext.y].current))
                            {
                                InstantiateYellowTiles(movesCurrent[i], 1, hit, tempNext);
                            }
                        }
                    }
                }
                else
                {
                    Vector2Int tempNext = new Vector2Int(currentPos.x, currentPos.y);
                    for (int j = 1; j < 8; j++)
                    {
                        tempNext += new Vector2Int((int)movesCurrent[i].x, (int)movesCurrent[i].y);
                        if (tempNext.x < 8 && tempNext.x >= 0 && tempNext.y < 8 && tempNext.y >= 0) {
                            if (GridManager.Board[tempNext.x, tempNext.y].current == Tile.chessPiece.None)
                            {
                                InstantiateGreenTiles(movesCurrent[i], j, hit, tempNext);
                            }
                            else if (players.CanIHitThis(GridManager.Board[tempNext.x, tempNext.y].current))
                            {
                                InstantiateYellowTiles(movesCurrent[i], j, hit, tempNext);
                                break;
                            }
                            else
                            {
                                break;
                            } 
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

    private void InteractionOrSelection(RaycastHit hit)
    {
        Vector2Int hitGridPos = hit.transform.GetComponent<Tile>().gridpos;
        Tile.chessPiece hitPiece = hit.transform.GetComponent<Tile>().current;
        // if there is no red tile that means nothing has been selected so that is main prio.
        if (newRedTile == null)
        {
            if (players.CanISelectThis(hitPiece))
            {
                CreateRedTile(hit);
            }
        }
        // if the tile we pressed is within green tiles and we have a red tile that means we want to move.
        else if (newGreenTilesPositions.ContainsKey(hitGridPos))
        {
            //Checks if the target is empty or an enemy
            if (players.AttackController(hitPiece))
            {
                PointDistributor(hitGridPos);
                GridManager.Board[hitGridPos.x, hitGridPos.y].current = GridManager.Board[newRedTileGridPos.x, newRedTileGridPos.y].current;
                GridManager.Board[hitGridPos.x, hitGridPos.y].hasNotMovedYet = false;
                GridManager.Board[newRedTileGridPos.x, newRedTileGridPos.y].current = Tile.chessPiece.None;


                players.turn = !players.turn;

                if (newRedTile != null)
                {
                    Destroy(newRedTile);
                }

                gridManager.AssignSprites();
            }
            else
            {
                GridManager.Board[hitGridPos.x, hitGridPos.y].current = GridManager.Board[newRedTileGridPos.x, newRedTileGridPos.y].current;
                GridManager.Board[hitGridPos.x, hitGridPos.y].hasNotMovedYet = false;
                GridManager.Board[newRedTileGridPos.x, newRedTileGridPos.y].current = Tile.chessPiece.None;

                players.turn = !players.turn;

                if (newRedTile != null)
                {
                    Destroy(newRedTile);
                }

                gridManager.AssignSprites();

            }
        } else if (hitPiece != Tile.chessPiece.None)
        {
            if (players.CanISelectThis(hitPiece))
            {
                CreateRedTile(hit);
            }
        }
    }
    private void PointDistributor(Vector2Int hitGridPos)
    {
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.PawnOpponent)
        {
            players.ironPlayer += 5;
            players.ironPlayerTMP.text = "" + players.ironPlayer;
        }
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.PawnPlayer)
        {
            players.ironOpponent += 5;
            players.ironOpponentTMP.text = "" + players.ironOpponent;
        }
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.KnightOpponent)
        {
            players.ironPlayer += 8;
            players.ironPlayerTMP.text = "" + players.ironPlayer;
        }
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.KnightPlayer)
        {
            players.ironOpponent += 8;
            players.ironOpponentTMP.text = "" + players.ironOpponent;
        }
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.BishopOpponent)
        {
            players.ironPlayer += 10;
            players.ironPlayerTMP.text = "" + players.ironPlayer;
        }
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.BishopPlayer)
        {
            players.ironOpponent += 10;
            players.ironOpponentTMP.text = "" + players.ironOpponent;
        }
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.TowerOpponent)
        {
            players.ironPlayer += 12;
            players.ironPlayerTMP.text = "" + players.ironPlayer;
        }
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.TowerPlayer)
        {
            players.ironOpponent += 12;
            players.ironOpponentTMP.text = "" + players.ironOpponent;
        }
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.QueenOpponent)
        {
            players.ironPlayer += 20;
            players.ironPlayerTMP.text = "" + players.ironPlayer;
        }
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.QueenPlayer)
        {
            players.ironOpponent += 20;
            players.ironOpponentTMP.text = "" + players.ironOpponent;
        }
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.KingOpponent)
        {
            SceneManager.LoadScene("WhiteWonScene");
        }
        if (GridManager.Board[hitGridPos.x, hitGridPos.y].current == Tile.chessPiece.KingPlayer)
        {
            SceneManager.LoadScene("BlackWonScene");
        }
    }
    private void OffBoardButtons(RaycastHit hit)
    {
        if (hit.transform.name == "Buy Menu Player")
        {
            Debug.Log("Buy Menu Player");
        }
        if (hit.transform.name == "Buy Menu Opponent")
        {
            Debug.Log("Buy Menu Opponent");
        }
        if (hit.transform.name == "Play Local Default")
        {
            SceneManager.LoadScene("Local Default");
        }
        if (hit.transform.name == "Main Menu")
        {
            SceneManager.LoadScene("Main Menu");
        }
        if (hit.transform.name == "Quit")
        {
            Application.Quit();
        }
        if (hit.transform.name == "Shop")
        {
            SceneManager.LoadScene("Shop");
        }
        if (hit.transform.name == "Settings")
        {
            SceneManager.LoadScene("Settings");
        }
        if (hit.transform.name == "Help Menu")
        {
            SceneManager.LoadScene("Help Menu");
        }
        if (hit.transform.name == "Resume")
        {
            escapeMenuPlayer.SetActive(false);
            escapeMenuOpponent.SetActive(false);
            Time.timeScale = 1;
        }
        if (hit.transform.name == "Settings Player")
        {
            escapeMenuPlayer.SetActive(true);
            Time.timeScale = 0;
        }
        if (hit.transform.name == "Settings Opponent")
        {
            escapeMenuOpponent.SetActive(true);
            Time.timeScale = 0;
        }
        if (hit.transform.name == "Redo")
        {
            SceneManager.LoadScene("Local Default");
            Time.timeScale = 1;
        }
    }
    /*

     * Drag needs a delay to stop placing tiles on accident

     */
}