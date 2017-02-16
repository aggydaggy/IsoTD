using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {

    public GridMapDB MapsDB;
    public GameObject DecorationPrefab;
    public int MapToLoad;
    public int StartingGold;

    GridMap currentMap;
    GameObject[,] tileGrid;
    List<Vector2> StartTiles = new List<Vector2>();
    List<Vector2> GoalTiles = new List<Vector2>();
    int rows = 0;
    int cols = 0;
    int currentWave = 0;
    bool isWaveRunning = false;
    
    void Start () {
        StartTiles.Clear();
        GoalTiles.Clear();
        currentMap = MapsDB.Maps[MapToLoad];
        StartCoroutine(CreateGrid());
	}

    IEnumerator CreateGrid()
    {
        WaitForSeconds wait = new WaitForSeconds(0.005f);
        string[] gridrows = currentMap.Grid.Split('|');
        rows = gridrows.Length;
        cols = gridrows[0].Split(',').Length;
        tileGrid = new GameObject[rows,cols];
        GridTile groundTile = currentMap.GroundTile;
        for (int i = 0; i < rows; i++)
        {
            string[] gridcols = gridrows[i].Split(',');
            for (int j = 0; j < cols; j++)
            {
                string[] gridPositionMakeup = gridcols[j].Split('^');
                int numOfGround = gridPositionMakeup.Length > 1 ? int.Parse(gridPositionMakeup[1]) : 1;
                int tileNum = int.Parse(gridPositionMakeup[0]);
                GridTile tile = tileNum >= 0 && tileNum < currentMap.Tiles.Length ? currentMap.Tiles[tileNum] : null;
                if (tile != null)
                {
                    GameObject prefab = tile.Tile;
                    List<GameObject> ground = new List<GameObject>();
                    for(int k = 1; k <= numOfGround; k++)
                    {
                       ground.Add(Instantiate(groundTile.Tile, new Vector3((float)i * -groundTile.X, (groundTile.Y*k) - (groundTile.Y/ 2), (float)j * -groundTile.Z), Quaternion.Euler(0f, 0f, 0f)));
                    }


                    prefab = Instantiate(prefab, new Vector3((float)i * -tile.X, (float)(tile.Y/2.0 + ((groundTile.Y*numOfGround))), (float)j * -tile.Z), Quaternion.Euler(0f, 0f, 0f));
                    tileGrid[i, j] = prefab;

                    if (tile.Name == "Start")
                    {
                        StartTiles.Add(new Vector2((float)i, (float)j));
                    }
                    if (tile.Name == "End")
                    {
                        GoalTiles.Add(new Vector2((float)i, (float)j));
                    }
                    TileInfo prefabInfo = prefab.GetComponent<TileInfo>();
                    if (prefabInfo != null)
                    {
                        prefabInfo.GridPosition = new Vector2(i, j);
                        prefabInfo.BaseTileInfo = tile;
                        if(ground.Count > 0)
                        {
                            prefabInfo.Grounds.AddRange(ground);
                        }
                        prefabInfo.ElevationLevels = numOfGround;
                    }
                    yield return wait;
                }
                else
                {
                    tileGrid[i, j] = null;
                }
            }
        }

        //Place decorations

        string[] decorationRows = currentMap.DecorationGrid.Split('|');
        for (int i = 0; i < rows; i++)
        {
            string[] decorationCols = decorationRows[i].Split(',');
            for (int j = 0; j < cols; j++)
            {
                int decorationNnum = int.Parse(decorationCols[j]);
                if (decorationNnum >= 0 && decorationNnum < currentMap.Decorations.Length)
                {
                    GameObject tile = GetTile(i, j);
                    if (tile != null)
                    {
                        TileInfo tileInfo = tile.GetComponent<TileInfo>();
                        if (tileInfo != null)
                        {
                            GameObject decoration = Instantiate(DecorationPrefab, tile.transform.position + (Vector3.up * tileInfo.BaseTileInfo.Y), Quaternion.Euler(0f, 0f, 0f));
                            decoration.GetComponent<SpriteRenderer>().sprite = currentMap.Decorations[decorationNnum];
                        }
                    }
                }
            }
        }
        StartCoroutine(RampRoadTiles());
        WeightTiles();
    }

    IEnumerator RampRoadTiles()
    {
        WaitForSeconds wait = new WaitForSeconds(0.005f);
        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                GameObject tile = GetTile(i, j);
                if (tile != null)
                {
                    TileInfo tileInfo = tile.GetComponent<TileInfo>();
                    if (tileInfo != null && tileInfo.IsWalkable)
                    {
                        GameObject floorUnderTile = tileInfo.Grounds.LastOrDefault();
                        Mesh mesh = tile.GetComponent<MeshFilter>().mesh;
                        Vector3[] vertices = mesh.vertices;
                        //Only need to check right and down because we are going left to right, up to down ramping checks.

                        //Check Down
                        GameObject tileDown = GetTile(i + 1, j);
                        if (tileDown != null)
                        {
                            TileInfo downInfo = tileDown.GetComponent<TileInfo>();
                            if (downInfo != null && downInfo.IsWalkable && downInfo.ElevationLevels != tileInfo.ElevationLevels)
                            {
                                int change = downInfo.ElevationLevels > tileInfo.ElevationLevels ? 1 : -1;
                                int floorsUp = Mathf.Abs(downInfo.ElevationLevels - tileInfo.ElevationLevels);
                                int heightPerFloor = (int)floorUnderTile.transform.localScale.y;
                                vertices[3].y += change* floorsUp * heightPerFloor;
                                vertices[5].y += change * floorsUp * heightPerFloor;
                                vertices[9].y += change * floorsUp * heightPerFloor;
                                vertices[11].y += change * floorsUp * heightPerFloor;
                                vertices[17].y += change * floorsUp * heightPerFloor;
                                vertices[18].y += change * floorsUp * heightPerFloor;
                                vertices[1].y += change * floorsUp * heightPerFloor;
                                vertices[7].y += change * floorsUp * heightPerFloor;
                                vertices[14].y += change * floorsUp * heightPerFloor;
                                vertices[15].y += change * floorsUp * heightPerFloor;
                                vertices[16].y += change * floorsUp * heightPerFloor;
                                vertices[19].y += change * floorsUp * heightPerFloor;

                                mesh.vertices = vertices;
                                mesh.RecalculateBounds();
                                mesh.RecalculateNormals();

                                for(int k = tileInfo.Grounds.Count-1, l = 0; k >= 0 && l < floorsUp; k--, l++)
                                {
                                    GameObject floorDown = tileInfo.Grounds[k];
                                    if (floorDown != null)
                                    {
                                        Mesh floorMesh = floorDown.GetComponent<MeshFilter>().mesh;
                                        Vector3[] floorVertices = floorMesh.vertices;

                                        floorVertices[3].y += change * (floorsUp + (l * change));
                                        floorVertices[5].y += change * (floorsUp + (l * change));
                                        floorVertices[9].y += change * (floorsUp + (l * change));
                                        floorVertices[11].y += change * (floorsUp + (l * change));
                                        floorVertices[17].y += change * (floorsUp + (l * change));
                                        floorVertices[18].y += change * (floorsUp + (l * change));

                                        if (change < 0)
                                        {
                                            floorVertices[1].y += change * (floorsUp + (l * change));
                                            floorVertices[7].y += change * (floorsUp + (l * change));
                                            floorVertices[14].y += change * (floorsUp + (l * change));
                                            floorVertices[15].y += change * (floorsUp + (l * change));
                                            floorVertices[16].y += change * (floorsUp + (l * change));
                                            floorVertices[19].y += change * (floorsUp + (l * change));
                                        }

                                        floorMesh.vertices = floorVertices;
                                        floorMesh.RecalculateBounds();
                                        floorMesh.RecalculateNormals();
                                    }
                                }

                                yield return wait;
                            }
                        }

                        //Check Right

                        GameObject tileRight = GetTile(i, j+1);
                        if (tileRight != null)
                        {
                            TileInfo rightInfo = tileRight.GetComponent<TileInfo>();
                            if (rightInfo != null && rightInfo.IsWalkable && rightInfo.ElevationLevels != tileInfo.ElevationLevels)
                            {
                                int change = rightInfo.ElevationLevels > tileInfo.ElevationLevels ? 1 : -1;
                                int floorsUp = Mathf.Abs(rightInfo.ElevationLevels - tileInfo.ElevationLevels);
                                int heightPerFloor = (int)floorUnderTile.transform.localScale.y;
                                vertices[4].y += change * floorsUp * heightPerFloor;
                                vertices[5].y += change * floorsUp * heightPerFloor;
                                vertices[10].y += change * floorsUp * heightPerFloor;
                                vertices[11].y += change * floorsUp * heightPerFloor;
                                vertices[18].y += change * floorsUp * heightPerFloor;
                                vertices[21].y += change * floorsUp * heightPerFloor;
                                vertices[6].y += change * floorsUp * heightPerFloor;
                                vertices[7].y += change * floorsUp * heightPerFloor;
                                vertices[12].y += change * floorsUp * heightPerFloor;
                                vertices[15].y += change * floorsUp * heightPerFloor;
                                vertices[19].y += change * floorsUp * heightPerFloor;
                                vertices[20].y += change * floorsUp * heightPerFloor;

                                mesh.vertices = vertices;
                                mesh.RecalculateBounds();
                                mesh.RecalculateNormals();


                                for (int k = tileInfo.Grounds.Count-1, l = 0; k >= 0 && l < floorsUp; k--, l++)
                                {
                                    GameObject floorRight = tileInfo.Grounds[k];
                                    if (floorRight != null)
                                    {

                                        Mesh floorMesh = floorRight.GetComponent<MeshFilter>().mesh;
                                        Vector3[] floorVertices = floorMesh.vertices;

                                        floorVertices[4].y += change * (floorsUp + (l * change));
                                        floorVertices[5].y += change * (floorsUp + (l * change));
                                        floorVertices[10].y += change * (floorsUp + (l * change));
                                        floorVertices[11].y += change * (floorsUp + (l * change));
                                        floorVertices[18].y += change * (floorsUp + (l * change));
                                        floorVertices[21].y += change * (floorsUp + (l * change));

                                        if (change < 0)
                                        {
                                            floorVertices[6].y += change * (floorsUp + (l * change));
                                            floorVertices[7].y += change * (floorsUp + (l * change));
                                            floorVertices[12].y += change * (floorsUp + (l * change));
                                            floorVertices[15].y += change * (floorsUp + (l * change));
                                            floorVertices[19].y += change * (floorsUp + (l * change));
                                            floorVertices[20].y += change * (floorsUp + (l * change));
                                        }

                                        floorMesh.vertices = floorVertices;
                                        floorMesh.RecalculateBounds();
                                        floorMesh.RecalculateNormals();
                                    }
                                }
                                yield return wait;
                            }
                        }
                    }
                }
            }
        }
    }

	void WeightTiles()
    {
        Queue<Vector2> floodQueue = new Queue<Vector2>();
        for(int i = 0; i < GoalTiles.Count; i++)
        {
            floodQueue.Clear();
            floodQueue.Enqueue(GoalTiles[i]);
            int pass = 0;
            while (floodQueue.Count > 0)
            {
                Vector2 position = floodQueue.Dequeue();
                int x = (int)position.x;
                int y = (int)position.y;
                if (x >= 0 && y >= 0 && x < rows && y < cols)
                {
                    GameObject tileAtPos = tileGrid[x, y];
                    if (tileAtPos != null)
                    {
                        TileInfo tileInfo = tileAtPos.GetComponent<TileInfo>();
                        if (tileInfo.IsWalkable && !tileInfo.GoalsCheckedFor.Contains(GoalTiles[i]))
                        {
                            tileInfo.GoalsCheckedFor.Add(GoalTiles[i]);
                            List<Vector2> toAdd = new List<Vector2>();
                            if (!tileInfo.WeightToGoal.ContainsKey(GoalTiles[i]))
                            {
                                tileInfo.WeightToGoal[GoalTiles[i]] = pass;
                            }
                            else
                            {
                                pass = pass < tileInfo.WeightToGoal[GoalTiles[i]] ? pass : tileInfo.WeightToGoal[GoalTiles[i]];
                            }
                            
                            toAdd.Add(new Vector2(x, y + 1));
                            toAdd.Add(new Vector2(x + 1, y));
                            toAdd.Add(new Vector2(x - 1, y));
                            toAdd.Add(new Vector2(x, y - 1));

                            for(int j = 0; j < toAdd.Count; j++)
                            {
                                Vector2 vector = toAdd[j];
                                if(!floodQueue.Contains(vector))
                                {
                                    floodQueue.Enqueue(vector);
                                }
                                // Weight the tile that just got enqueued with the current pass if it exists
                                if (vector.x >= 0 && vector.x < rows && vector.y >= 0 && vector.y < cols)
                                {
                                    GameObject queuedTile = tileGrid[(int)vector.x, (int)vector.y];
                                    if (queuedTile != null)
                                    {
                                        TileInfo info = queuedTile.GetComponent<TileInfo>();
                                        if (!info.WeightToGoal.ContainsKey(GoalTiles[i]) && info.IsWalkable)
                                        {
                                            info.WeightToGoal[GoalTiles[i]] = pass + 1;
                                        }
                                    }
                                }
                            }
                            pass += 1;
                        }
                    }
                }
            }

        }
    }

	// Update is called once per frame
	void Update () {
        //Testing
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (!isWaveRunning)
            {
                SpawnWave();
            }
        }
    }

    private void SpawnWave()
    {
        if(currentWave < currentMap.EnemyWaves.Length)
        {
            EnemyWave waveInfo = currentMap.EnemyWaves[currentWave];
            StartCoroutine(RunWave(waveInfo));
        }
    }

    IEnumerator RunWave(EnemyWave wave) 
    {
        isWaveRunning = true;
        WaitForSeconds spawnWait = new WaitForSeconds(wave.TimeBetweenSpawns);
        int spawnCount = wave.Count;
        for (int i = spawnCount; i >= 0; i--)
        {
            for (int j = 0; j < wave.Enemies.Length; j++)
            {
                Vector2 spawnTile = StartTiles[Random.Range(0, StartTiles.Count)];
                GameObject spawnTileObject = tileGrid[(int)spawnTile.x, (int)spawnTile.y];
                GameObject enemySpawned = Instantiate(wave.Enemies[j].BaseEnemy, spawnTileObject.transform.position + (Vector3.up * 2), Quaternion.Euler(0f, 0f, 0f));
                WalkToGoal enemyWalk = enemySpawned.GetComponent<WalkToGoal>();
                enemyWalk.SetValues(wave, spawnTile);
            }
            yield return spawnWait;
        }
        currentWave++;
        isWaveRunning = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(new Vector3(-40f, 0f, -40f), new Vector3(80f, 1f, 80f));
    }

    public GridMap GetMapInfo()
    {
        return currentMap;
    }

    public GameObject GetTile(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < rows && y < cols)
        {
            return tileGrid[x, y];
        }
        return null;
    }
}
