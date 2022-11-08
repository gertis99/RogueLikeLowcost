using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{

    // Variables
    private int vidas, points;
    public Text vidasText, pointsText;
    public GameObject player;

    // Scripts
    private Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        playerScript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerScript == null)
            playerScript = player.GetComponent<Player>();

        vidas = playerScript.getLives();
        vidasText.text = "Vidas: " + vidas;
        pointsText.text = "Points: " + points;
        
    }

    public void setPoints(int p)
    {
        points = p;
    }

    public void addPoints(int n)
    {
        points += n;
    }
}
