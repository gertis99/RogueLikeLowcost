using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public float speed, fireRate, fireForce;
    public GameObject manager;
    public GameObject enemyTear;
    public float posX, posY;
    public int lives;

    // Start is called before the first frame update
    void Start()
    {
        lives = 5;
        fireForce = 2;
        manager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Getters
    public float getSpeed()
    {
        return speed;
    }

    public float getFireRate()
    {
        return fireRate;
    }

    public float getFireForce()
    {
        return fireForce;
    }

    public float getPosX()
    {
        return posX;
    }

    public float getPosY()
    {
        return posY;
    }

    public int getLives()
    {
        return lives;
    }

    // Setters
    public void setSpeed(float s)
    {
        speed = s;
    }

    public void setFireRate(float s)
    {
        fireRate = s;
    }

    public void setFireForce(float s)
    {
        fireForce = s;
    }

    public void setOriginalPosX(int p)
    {
        posX = p;
    }
    public void setOriginalPosY(int p)
    {
        posY = p;
    }

    public void setLives(int n)
    {
        lives = n;
    }

    // Move the object to the original position
    public void reSetPosition()
    {
        this.gameObject.transform.position = new Vector3(posX, posY, 0);
    }

    // Reduce the lives in n times
    public void lessLive(int n)
    {
        lives -= n;

        if (lives <= 0)
        {
            Destroy(this.gameObject);
            manager.GetComponent<Manager>().addPoints(3);
        }

    }
    
    // Instantiate a tear in the position of the object
    public GameObject fireTear()
    {
        return Instantiate(enemyTear, transform.position, transform.rotation) as GameObject;
    }
}
