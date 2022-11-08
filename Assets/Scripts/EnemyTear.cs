using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTear : MonoBehaviour
{
    // Variables
    public Vector3 veolcity = new Vector3(0, 0, 0);
    public float drag, mass, restitution;
    private GameObject[] obstacles, walls;
    public GameObject player, manager;
    private GameObject obsCollision;

    // Scripts
    private Player playerScript;
    private Collisions collisionScript;

    // Start is called before the first frame update
    void Start()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        walls = GameObject.FindGameObjectsWithTag("Wall");
        player = GameObject.Find("Player");
        manager = GameObject.Find("GameManager");
        playerScript = player.GetComponent<Player>();
        collisionScript = manager.GetComponent<Collisions>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerScript == null)
            playerScript = player.GetComponent<Player>();

        // Change the velocity through the time
        if (getVelocity().x != 0 || getVelocity().y != 0)
        {
            setVelocity((1 - (drag * Time.deltaTime)) * getVelocity());
            transform.position += getVelocity() * Time.deltaTime;

            // Destroy the tear when it is very slow
            if (Mathf.Abs(getVelocity().x) < 1 && Mathf.Abs(getVelocity().y) < 1)
                Destroy(this.gameObject);
        }

        // Destroy the tear if it makes collision with an obstacle
        obsCollision = calculateAABBDetection(obstacles);
        if (obsCollision != null && calculateGJKDetection(obstacles))
        {
            Destroy(this.gameObject);
        }

        if (calculateGJKDetection(walls))
        {
            Destroy(this.gameObject);
        }

        // Destroy the tear if it makes collision with the player, and reduce its lives in 1
        if (collisionScript.checkAABBDetection(this.gameObject, player) && collisionScript.checkGJKDetection(this.gameObject, player))
        {
            Destroy(this.gameObject);
            playerScript.lessLive(1);
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
    
    // Check collision for every obstacle
    bool calculateGJKDetection(GameObject[] obs)
    {

        for (int i = 0; i < obs.Length; i++)
        {
            if (collisionScript.checkGJKDetection(this.gameObject, obs[i]))
                return true;
        }

        return false;
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
