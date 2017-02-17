using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridPosition))]
public class WalkToGoal : MonoBehaviour {

    GridManager manager;
    float speed;
    Vector2 targetTile;
    Vector3 targetPosition;
    Vector2 targetExit = Vector2.zero;
    bool hitExit = false;

    private void Awake()
    {
        manager = FindObjectOfType<GridManager>();
    }

    public void SetValues(EnemyWave waveInfo, Vector2 beginTile)
    {
        speed = waveInfo.Speed;
        targetTile = beginTile;
        SetNewTarget(targetTile);
    }

    private void Update()
    {
        Vector3 dir = targetPosition - transform.position;
        float distanceToTravel = speed * Time.deltaTime;
        if (dir.z > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (dir.magnitude <= distanceToTravel)
        {
            if (hitExit)
            {
                Destroy(gameObject); //TODO: Deal with losing life on a hit, dereference from any tower targets.
            }
            else
            {
                SetNewTarget(targetTile);
            }
        }
        else
        {
            transform.Translate(dir.normalized * distanceToTravel, Space.World);
        }


    }

    public void SetNewTarget(Vector2 tile)
    {
        targetTile = tile;

        int lowestNeighbor = 1000000;
        List<GameObject> possibleTargets = new List<GameObject>();
        List<Vector2> tilesToCheck = new List<Vector2>();
        tilesToCheck.Add(new Vector2(tile.x, tile.y + 1));
        tilesToCheck.Add(new Vector2(tile.x, tile.y - 1));
        tilesToCheck.Add(new Vector2(tile.x + 1, tile.y));
        tilesToCheck.Add(new Vector2(tile.x - 1, tile.y));


        for(int i = 0; i < tilesToCheck.Count; i++)
        {
            int lowestTileWeight = 1000000000;
            int x = (int)tilesToCheck[i].x;
            int y = (int)tilesToCheck[i].y;
            GameObject tileCheck = manager.GetTile(x, y);

            if (tileCheck != null)
            {
                TileInfo tileInfo = tileCheck.GetComponent<TileInfo>();
                if (tileInfo != null)
                {
                    //If there's a target exit, find the lowest number of that exit, otherwise choose the lowest of all.
                    if (targetExit != Vector2.zero && tileInfo.WeightToGoal.ContainsKey(targetTile))
                    {
                        lowestTileWeight = (int)Mathf.Min((float)lowestTileWeight, (float)tileInfo.WeightToGoal[targetTile]);
                    }
                    else
                    {
                        List<Vector2> keys = tileInfo.WeightToGoal.Keys.ToList();
                        for(int j = 0; j < keys.Count; j++)
                        {
                            lowestTileWeight = (int)Mathf.Min((float)lowestTileWeight, tileInfo.WeightToGoal[keys[j]]);
                        }
                    }
                }
                if (lowestTileWeight < lowestNeighbor)
                {
                    possibleTargets.Clear();
                    possibleTargets.Add(tileCheck);
                    lowestNeighbor = lowestTileWeight;
                }
                else if (lowestTileWeight == lowestNeighbor)
                {
                    possibleTargets.Add(tileCheck);
                }
            }
        }

        //Choose a new target from possibile tiles
        GameObject newTileTarget = possibleTargets[Random.Range(0, possibleTargets.Count)];
        targetTile = newTileTarget.GetComponent<TileInfo>().GridPosition;
        targetPosition = new Vector3(newTileTarget.transform.position.x, newTileTarget.transform.position.y, newTileTarget.transform.position.z);
        targetPosition += Vector3.up * 2; // TODO: Use enemy variable
        if (lowestNeighbor == 0)
        {
            hitExit = true;
        }
    }

}
