using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // Variables
    public bool goal, start, wall, path, open, actual;
    public float g, f, h;
    private GameObject nodeFather;

    // Scripts
    private SpriteRenderer rendererScriot;

    // Start is called before the first frame update
    void Start()
    {
        goal = false;
        start = false;
        path = false;
        open = false;
        actual = false;
        this.GetComponent<SpriteRenderer>().color = Color.white;
        nodeFather = null;
        g = 0; f = 0; h = 0;
        rendererScriot = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goal)
            rendererScriot.color = Color.red;
        else if (start)
            rendererScriot.color = Color.green;
        else if(wall)
            rendererScriot.color = Color.grey;
        else if (path)
            rendererScriot.color = Color.blue;
        else if (open && !actual)
            rendererScriot.color = Color.yellow;
        else if (actual)
            rendererScriot.color = Color.black;
        else
            rendererScriot.color = Color.white;
    }

    // Getters
    public bool isGoal()
    {
        return goal;
    }

    public bool isStart()
    {
        return start;
    }

    public bool isWall()
    {
        return wall;
    }

    public bool isPath()
    {
        return path;
    }

    public bool isOpen()
    {
        return open;
    }

    public float getF()
    {
        return f;
    }

    public float getH()
    {
        return h;
    }

    public float getG()
    {
        return g;
    }

    public GameObject getFather()
    {
        return nodeFather;
    }

    // Setters
    public void setF(float _f)
    {
        f = _f;
    }

    public void setH(float _h)
    {
        h = _h;
        f = h + g;
    }

    public void setG(float _g)
    {
        g = _g;
        f = h + g;
    }

    public void setFather(GameObject _f)
    {
        nodeFather = _f;
    }

    public void setGoal(bool g)
    {
        goal = g;

        if (goal)
            rendererScriot.color = Color.red;
    }

    public void setStart(bool s)
    {
        start = s;

        if (start)
            rendererScriot.color = Color.green;
    }

    public void setPath(bool p)
    {
        path = p;
    }

    public void setOpen(bool o)
    {
        open = o;
    }

    public void setActual(bool a)
    {
        actual = a;
    }

    public void setWall(bool w)
    {
        wall = w;
    }

    // Reset the nodes values
    public void clear()
    {
        goal = false;
        start = false;
        path = false;
        open = false;
        actual = false;
        this.GetComponent<SpriteRenderer>().color = Color.white;
        nodeFather = null;
        g = 0; f = 0; h = 0;
    }
}
