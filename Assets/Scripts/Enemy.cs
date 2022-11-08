using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Variables
    public float speed;
    public GameObject manager;
    public float posX, posY;
    public int lives;

    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
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

    // Move the object to it original position
    public void reSetPosition()
    {
        this.gameObject.transform.position = new Vector3(posX, posY, 0);
    }

    // Reduce the lives n times
    public void lessLive(int n)
    {
        lives -= n;

        if (lives <= 0)
        {
            Destroy(this.gameObject);
            manager.GetComponent<Manager>().addPoints(1);
        }
            
    }

}
