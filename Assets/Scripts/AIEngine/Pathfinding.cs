using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    public int nRows;
    public GameObject[,] nodes;
    private List<GameObject> nodosAbiertos;
    private List<GameObject> nodosCerrados;
    private List<GameObject> path;
    private GameObject nodeStart, nodeGoal, nodeAux, currentNode;

    private void Awake()
    {
        nodosAbiertos = new List<GameObject>();
        nodosCerrados = new List<GameObject>();
        path = new List<GameObject>();

        nodeAux = new GameObject();
        currentNode = new GameObject();
    }

    // Initialize the nodes. Without this, the pathfinding doesn't work
    public void setNodes(GameObject[,] n)
    {
        nodes = n;
        nRows = nodes.Length;
    }

    // Pathfinding A* between two positions of the matrix
    // Returns the first node of the path
    // This pathfinding is used without diagonal movement, but it can be implemented easily
    public GameObject calculateAStar(int row1, int col1, int row2, int col2)
    {
        nodosAbiertos = new List<GameObject>();
        nodosCerrados = new List<GameObject>();
        path = new List<GameObject>();

        nodeAux = new GameObject();
        currentNode = new GameObject();

        nodeStart = nodes[col1, row1];
        nodeGoal = nodes[col2, row2];

        nodeStart.GetComponent<Node>().setG(0);
        nodeStart.GetComponent<Node>().setH(calculateH(nodeStart, nodeGoal));
        nodosAbiertos.Add(nodeStart);
        nodosCerrados.Add(nodeStart);

        if (nodeGoal != nodeStart)
        {
            while (nodosAbiertos.Count != 0 && !(nodosCerrados[nodosCerrados.Count - 1].transform.position.x == nodeGoal.transform.position.x &&
                nodosCerrados[nodosCerrados.Count - 1].transform.position.y == nodeGoal.transform.position.y))
            {

                // Get lowest f node of open nodes and switch it to closed nodes
                currentNode = getLessF(nodosAbiertos);
                nodosCerrados.Add(currentNode);
                nodosAbiertos.Remove(currentNode);
                currentNode.GetComponent<Node>().setActual(true);

                // Check the nodes adyadcent to the current node
                checkCurrentNode();

                currentNode.GetComponent<Node>().setActual(false);
            }

            if (nodosCerrados[nodosCerrados.Count - 1].transform.position.x == nodeGoal.transform.position.x &&
                nodosCerrados[nodosCerrados.Count - 1].transform.position.y == nodeGoal.transform.position.y)
            {
                path.Insert(0, nodeGoal);
                //nodeAux = nodosCerrados[nodosCerrados.Count - 1];
                nodeAux = nodeGoal;
                nodeAux.GetComponent<Node>().getFather().GetComponent<Node>().setPath(true);
                path.Insert(0, nodeAux.GetComponent<Node>().getFather());
                while (nodeAux.GetComponent<Node>().getFather() != null)
                {
                    nodeAux = path[0];
                    if (nodeAux.GetComponent<Node>().getFather() != null)
                    {
                        nodeAux.GetComponent<Node>().getFather().GetComponent<Node>().setPath(true);
                        path.Insert(0, nodeAux.GetComponent<Node>().getFather());
                    }

                }
            }
            else
            {
                path.Insert(0, nodeGoal);
                path.Insert(1, nodeGoal);
            }

        }

        if (path.Count == 1)
            path.Insert(1, path[0]);

        if (path.Count == 0)
        {
            path.Insert(0, nodeGoal);
            path.Insert(1, nodeGoal);
        }

        return path[1];
    }

    // Get the node with less f value (so it is the next node)
    private GameObject getLessF(List<GameObject> l)
    {
        GameObject res = l[0];

        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].GetComponent<Node>().getF() < res.GetComponent<Node>().getF())
                res = l[i];
        }

        return res;
    }

    // Calculate the h value between two nodes
     private float calculateH(GameObject currentNode, GameObject goal)
    {
        return Mathf.Abs(goal.transform.position.x - currentNode.transform.position.x) +
            Mathf.Abs(goal.transform.position.y - currentNode.transform.position.y);
    }

    // Check the adjacent nodes of the current node
    // This implementation only check the four direct adjacent nodes, not the diagonal, but can be added in the same way
    private void checkCurrentNode()
    {
        // Check the 4 adjacent nodes
        if (!((int)currentNode.transform.position.x - 1 < 0)) // x-1
        {
            checkNode((int)(currentNode.transform.position.x - 1), (int)currentNode.transform.position.y);
        }

        if (!((int)currentNode.transform.position.y + 1 >= nRows)) // y+1
        {
            checkNode((int)currentNode.transform.position.x, (int)(currentNode.transform.position.y + 1));
        }

        if (!((int)currentNode.transform.position.x + 1 >= nRows)) // x+1
        {
            checkNode((int)(currentNode.transform.position.x + 1), (int)currentNode.transform.position.y);
        }

        if (!((int)currentNode.transform.position.y - 1 < 0)) // y-1
        {
            checkNode((int)currentNode.transform.position.x, (int)(currentNode.transform.position.y - 1));
        }
    }

    // Check the node and add it to the open nodes if it is necessary
    private void checkNode(int x, int y)
    {
        if (!(nodes[x, y].GetComponent<Node>().isWall()) && !nodosCerrados.Contains(nodes[x, y])) // If it is not wall and if it is not in the
        {                                                                                                                           // closed list
            nodeAux = nodes[x, y];
            if (!nodosAbiertos.Contains(nodeAux)) // If it isn't in opened list
            {
                nodeAux.GetComponent<Node>().setFather(currentNode);
                nodeAux.GetComponent<Node>().setG(currentNode.GetComponent<Node>().getG() + 1);
                nodeAux.GetComponent<Node>().setH(calculateH(nodeAux, nodeGoal));
                nodeAux.GetComponent<Node>().setOpen(true);
                nodosAbiertos.Add(nodeAux);
            }
            else
            {
                if (currentNode.GetComponent<Node>().getG() + 1 < nodeAux.GetComponent<Node>().getG())
                {
                    nodosAbiertos.Remove(nodeAux);
                    nodeAux.GetComponent<Node>().setFather(currentNode);
                    nodeAux.GetComponent<Node>().setG(currentNode.GetComponent<Node>().getG() + 1);
                    nodeAux.GetComponent<Node>().setH(calculateH(nodeAux, nodeGoal));
                    nodeAux.GetComponent<Node>().setOpen(true);
                    nodosAbiertos.Add(nodeAux);
                }
            }
        }
    }
}
