using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    // Variables
    public float speed, fireForce;
    private GameObject[] obstacles, enemies;
    private KeyCode lastKey;
    private int lives;
    public GameObject tearPrefab;
    public GameObject manager;
    public int posX, posY;
    private GameObject obsCollision, enemyCollision;

    // Scripts
    private Collisions collisionScript;

    // Start is called before the first frame update
    void Start()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        lives = 3;
        collisionScript = manager.GetComponent<Collisions>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // If there is no collision, the player can move
        // Only compare player with obstacles
        obsCollision = calculateAABBDetection(obstacles);
        if(obsCollision == null)
        {
            if (Input.GetKey("a"))
            {
                transform.position -= transform.right * speed * Time.deltaTime;
                lastKey = KeyCode.A;
            }
            else if (Input.GetKey("d"))
            {
                transform.position += transform.right * speed * Time.deltaTime;
                lastKey = KeyCode.D;
            }
            else if (Input.GetKey("w"))
            {
                transform.position += transform.up * speed * Time.deltaTime;
                lastKey = KeyCode.W;
            }
            else if (Input.GetKey("s"))
            {
                transform.position -= transform.up * speed * Time.deltaTime;
                lastKey = KeyCode.S;
            }
        }
        else
        {
            if (calculateGJKDetection(obstacles))
            {
                if (lastKey == KeyCode.A)
                {
                    transform.position += transform.right * 0.1f;
                }

                if (lastKey == KeyCode.D)
                {
                    transform.position -= transform.right * 0.1f;
                }

                if (lastKey == KeyCode.W)
                {
                    transform.position -= transform.up * 0.1f;
                }

                if (lastKey == KeyCode.S)
                {
                    transform.position += transform.up * 0.1f;
                }
            }
            else
            {
                if (Input.GetKey("a"))
                {
                    transform.position -= transform.right * speed * Time.deltaTime;
                    lastKey = KeyCode.A;
                }
                else if (Input.GetKey("d"))
                {
                    transform.position += transform.right * speed * Time.deltaTime;
                    lastKey = KeyCode.D;
                }
                else if (Input.GetKey("w"))
                {
                    transform.position += transform.up * speed * Time.deltaTime;
                    lastKey = KeyCode.W;
                }
                else if (Input.GetKey("s"))
                {
                    transform.position -= transform.up * speed * Time.deltaTime;
                    lastKey = KeyCode.S;
                }
            }
        }
        

        // Fix wall collisions
        if (transform.position.x <= 0)
            transform.position = new Vector3(0.05f, transform.position.y, 0);

        if (transform.position.y <= 0)
            transform.position = new Vector3(transform.position.x, 0.05f, 0);

        if(transform.position.x >= 14)
            transform.position = new Vector3(13.95f, transform.position.y, 0);

        if (transform.position.y >= 14)
            transform.position = new Vector3(transform.position.x, 13.95f, 0);


        // If the player makes a collision with an enemy...
        try
        {
            enemyCollision = calculateAABBDetection(enemies);
            if (enemyCollision != null &&  calculateGJKDetection(enemyCollision))
            {
                this.lessLive(1);
                manager.GetComponent<Manager>().reSetPositions();
            }
        }catch { }
        
    }

    private void Update()
    {
        // Fire a tear depending on the arrow pushed
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            fireTear(transform.position, 1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            fireTear(transform.position, 3);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            fireTear(transform.position, 2);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            fireTear(transform.position, 4);
        }
    }

    // Getters
    public int getLives()
    {
        return lives;
    }

    public int getOriginalPosX()
    {
        return posX;
    }

    public int getOriginalPosY()
    {
        return posY;
    }

    // Setters
    public void setOriginalPosX(int p)
    {
        posX = p;
    }

    public void setOriginalPosY(int p)
    {
        posY = p;
    }

    // Reduce the lives n times
    public void lessLive(int n)
    {
        lives -= n;
        if (lives <= 0)
        {
            manager.GetComponent<Manager>().reSetGame();
            lives = 3;
        }
    }

    // Move the player to the original position
    public void reSetPosition()
    {
        gameObject.transform.position = new Vector3(posX, posY, 0);
    }

    // Fire a tear in the position pos and the direction dir
    private void fireTear(Vector3 pos, int dir)
    {
        Vector3[] positions = new Vector3[3];
        GameObject tear = Instantiate(tearPrefab, pos, this.transform.rotation) as GameObject;
        if(dir == 1)
        {
            positions[0] = new Vector3(-0.25f, 0, 0);
            positions[1] = new Vector3(0, 0.5f, 0);
            positions[2] = new Vector3(0.25f, 0, 0);
            tear.GetComponent<LineRenderer>().SetPositions(positions);
            tear.GetComponent<Tear>().setVelocity((new Vector3(0, 1, 0)) * fireForce);
        }else if(dir == 2)
        {
            tear.GetComponent<Tear>().setVelocity((new Vector3(1, 0, 0)) * fireForce);
        }else if(dir == 3)
        {
            positions[0] = new Vector3(-0.25f, 0, 0);
            positions[1] = new Vector3(0, -0.5f, 0);
            positions[2] = new Vector3(0.25f, 0, 0);
            tear.GetComponent<LineRenderer>().SetPositions(positions);
            tear.GetComponent<Tear>().setVelocity((new Vector3(0, -1, 0)) * fireForce);
        }else if(dir == 4)
        {
            positions[0] = new Vector3(0, 0.25f, 0);
            positions[1] = new Vector3(-0.5f, 0, 0);
            positions[2] = new Vector3(0, -0.25f, 0);
            tear.GetComponent<LineRenderer>().SetPositions(positions);
            tear.GetComponent<Tear>().setVelocity((new Vector3(-1, 0, 0)) * fireForce);
        }
    }

    // Calculate the GJK collision with every object in obs
    bool calculateGJKDetection(GameObject[] obs)
    {
        bool res = false;

        for(int i=0; i<obs.Length && !res; i++)
        {
            if (obs[i] != null && collisionScript.checkGJKDetection(this.gameObject, obs[i]))
                res = true;
        }

        return res;
    }

    // Calculate the GJK collision with game object obs
    bool calculateGJKDetection(GameObject obs)
    {
        bool res = false;

        if (collisionScript.checkGJKDetection(this.gameObject, obs))
            res = true;

        return res;
    }

    // Calculate the AABB collision with every object in obs
    GameObject calculateAABBDetection(GameObject[] obs)
    {
        for (int i = 0; i < obs.Length; i++)
        {
            if (obs[i] != null && collisionScript.checkAABBDetection(this.gameObject, obs[i]))
                return obs[i];
        }

        return null;
    }
}
