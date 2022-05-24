using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTiles : MonoBehaviour
{
    public GameObject whiteTile;
    public GameObject blackTile;
    private Transform parent;
    private float scaling = 2f;
    private float startPosition = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        parent = this.transform;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject temp;
                if ((i+j)% 2 == 0)
                {
                    temp = Instantiate(blackTile, new Vector3((i - startPosition) * scaling, (j - startPosition) * scaling, 0), Quaternion.identity, parent);
                    
                }
                else
                {
                    temp = Instantiate(whiteTile, new Vector3((i - startPosition) * scaling, (j - startPosition) * scaling, 0), Quaternion.identity, parent);
                }

                Tile tempTile = temp.AddComponent<Tile>();

                GridManager.Board[i, j] = tempTile;
                temp.name = "Tile:" + i + "," + j;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
