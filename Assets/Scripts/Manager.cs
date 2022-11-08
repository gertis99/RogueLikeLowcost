using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    // Variables
    public int nRows;
    public GameObject nodePrefab;
    public GameObject[,] nodes;
    public GameObject player;
    public GameObject enemy, enemy2, obstaclePrefab, aux;
    public GameObject canvas;
    private GameObject[] enemies, obstacles;
    private int random;
    private bool isPlayerSpawned;
    public int maxEnemy, maxEnemy2;
    private int nEnemy, nEnemy2;


    // Start is called before the first frame update
    void Start()
    {
        nodes = new GameObject[nRows, nRows];
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        nEnemy = 0;
        nEnemy2 = 0;
        createMap(nRows);
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        // If there is no more enemies, create another round
        if (enemies.Length == 0)
            resetRound();

        // If the player push the space key, create another run
        if (Input.GetKeyDown(KeyCode.Space))
            resetRound();

        // If the player push the r key, reset the entire game
        //if (Input.GetKeyDown(KeyCode.R))
        //    reSetGame();
    }

    // Reset the original position for the player and the enemies
    public void reSetPositions()
    {
        player.GetComponent<Player>().reSetPosition();
        for(int i=0; i<enemies.Length; i++)
        {
            try
            {
                enemies[i].GetComponent<Enemy>().reSetPosition();
            }
            catch { }

            try
            {
                enemies[i].GetComponent<Enemy2>().reSetPosition();
            }
            catch { }
        }
        
    }


    // Create the map with the nodes, enemies and player
    void createMap(int n)
    {

        for (int i = 0; i < nRows; i++)
        {
            for (int j = 0; j < nRows; j++)
            {
                random = Random.Range(1, 10);
                nodes[i, j] = Instantiate(nodePrefab, new Vector3(i, j, 0), Quaternion.identity);
                if(random == 1)         // 10% chance there is a obstacle
                {
                    nodes[i, j].GetComponent<Node>().setWall(true);
                    Instantiate(obstaclePrefab, new Vector3(i, j, 0), Quaternion.identity);
                }
                else
                {
                    random = Random.Range(1, 100);
                    if(random == 3 && nEnemy < maxEnemy)     // 1% chance there is a enemy type 1 
                    {
                        aux = Instantiate(enemy, new Vector3(i, j, 0), Quaternion.identity);
                        aux.GetComponent<Enemy>().setOriginalPosX(i);
                        aux.GetComponent<Enemy>().setOriginalPosY(j);
                        nEnemy++;
                    }
                    else if(random == 2 && nEnemy2 < maxEnemy2)    // 1% chance there is a enemy type 2 
                    {
                        aux = Instantiate(enemy2, new Vector3(i, j, 0), Quaternion.identity);
                        aux.GetComponent<Enemy2>().setOriginalPosX(i);
                        aux.GetComponent<Enemy2>().setOriginalPosY(j);
                        nEnemy2++;
                    }
                    else
                    {
                        if (!isPlayerSpawned)   // If the node is clear and there is no player, spawn the player 
                        {
                            player.GetComponent<Player>().setOriginalPosX(i);
                            player.GetComponent<Player>().setOriginalPosY(j);
                            isPlayerSpawned = true;
                        }
                    }
                }
                nodes[i, j].SetActive(false);
            }
        }

        this.gameObject.GetComponent<Pathfinding>().setNodes(nodes);

        reSetPositions();
    }

    // Make the map white again
    public void clearMap()
    {
        for (int i = 0; i < nRows; i++)
        {
            for (int j = 0; j < nRows; j++)
            {
                nodes[i, j].GetComponent<Node>().clear();
            }
        }

        this.gameObject.GetComponent<Pathfinding>().setNodes(nodes);
    }

    // Points = 0 and create a new map with new enemies
    public void reSetGame()
    {
        canvas.GetComponent<Canvas>().setPoints(0);
        for(int i=0; i<enemies.Length; i++)
        {
            try
            {
                Destroy(enemies[i]);
            }
            catch { }
        }

        for (int i = 0; i < obstacles.Length; i++)
        {
            try
            {
                Destroy(obstacles[i]);
            }
            catch { }
        }

        for(int i=0; i<nRows; i++)
        {
            for(int j=0; j<nRows; j++)
            {
                Destroy(nodes[i, j]);
            }
        }

        nEnemy = 0;
        nEnemy2 = 0;
        //clearMap();
        createMap(nRows);
    }

    // Clean the map from enemies and obstacles and create a new round, with same lives and points
    public void resetRound()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            try
            {
                Destroy(enemies[i]);
            }
            catch { }
        }

        for (int i = 0; i < obstacles.Length; i++)
        {
            try
            {
                Destroy(obstacles[i]);
            }
            catch { }
        }

        for (int i = 0; i < nRows; i++)
        {
            for (int j = 0; j < nRows; j++)
            {
                Destroy(nodes[i, j]);
            }
        }

        nEnemy = 0;
        nEnemy2 = 0;
        //clearMap();
        createMap(nRows);
    }


    public GameObject[,] getMap()
    {
        return nodes;
    }

    public bool isAccesible(int row, int column)
    {
        if (row > nRows-1 || row < 0 || column > nRows-1 || column < 0 || nodes[column, row].GetComponent<Node>().isWall())
            return false;
        else
            return true;
    }


    public void addPoints(int n)
    {
        canvas.GetComponent<Canvas>().addPoints(n);
    }

    public void setPoints(int p)
    {
        canvas.GetComponent<Canvas>().setPoints(p);
    }
}
