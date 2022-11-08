using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tear : MonoBehaviour
{
    // Variables
    public Vector3 veolcity = new Vector3(0, 0, 0);
    public float drag, mass, restitution;
    private GameObject manager;
    private GameObject[] obstacles, enemies, walls, enemyTears;
    private GameObject obsCollision, enemyCollision, tearCollision;

    // Scripts
    private Collisions collisionScript;
    private Dynamics dynamicScript;

    // Start is called before the first frame update
    void Start()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        walls = GameObject.FindGameObjectsWithTag("Wall");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        manager = GameObject.Find("GameManager");
        collisionScript = manager.GetComponent<Collisions>();
        dynamicScript = manager.GetComponent<Dynamics>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyTears = GameObject.FindGameObjectsWithTag("EnemyTear");
        tearCollision = calculateGJKDetection(enemyTears);
        if(tearCollision != null)
        {
            dynamicScript.solveCollision(this.gameObject, tearCollision);
            //Destroy(tearCollision);
        }

        // Change the velocity throught the time
        if (getVelocity().x != 0 || getVelocity().y != 0)
        {
            setVelocity((1 - (drag * Time.deltaTime)) * getVelocity());
            transform.position += getVelocity() * Time.deltaTime;

            // Destroy if the tear is very slow
            if (Mathf.Abs(getVelocity().x) < 1 && Mathf.Abs(getVelocity().y) < 1)
                Destroy(this.gameObject);
                
        }

        // Destroy if it makes collision with an obstacle
        obsCollision = calculateAABBDetection(obstacles);
        if (obsCollision != null && calculateGJKDetection(obstacles))
        {
            Destroy(this.gameObject);
        }

        if (calculateGJKDetection(walls))
        {
            Destroy(this.gameObject);
        }

        // Check if it impact to an enemy
        // If it is true, reduce its live in one and destroy the tear
        enemyCollision = calculateAABBDetection(enemies);
        if (enemyCollision != null && calculateGJKDetection(enemyCollision))
        {
            Destroy(this.gameObject);
            try
            {
                enemyCollision.GetComponent<Enemy>().lessLive(1);
            }
            catch { }

            try
            {
                enemyCollision.GetComponent<Enemy2>().lessLive(1);
            }
            catch { }
        }
    }

    // Getters
    public Vector3 getVelocity()
    {
        return veolcity;
    }

    public float getMass()
    {
        return mass;
    }

    public float getRestitution()
    {
        return restitution;
    }

    // Setters
    public void setVelocity(Vector3 v)
    {
        veolcity = v;
    }

    public void setVelocityX(float v)
    {
        veolcity.x = v;
    }

    public void setVelocityY(float v)
    {
        veolcity.y = v;
    }

    // Check collision with every game object in obs
    GameObject calculateGJKDetection(GameObject[] obs)
    {

        for (int i = 0; i < obs.Length; i++)
        {
            if (collisionScript.checkGJKDetection(this.gameObject, obs[i]))
                return obs[i];
        }

        return null;
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
