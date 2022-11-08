using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    Vector3[] vertices1;
    Vector3[] vertices2;
    private Vector3 direction;
    private Vector3 origin;
    private Vector3 A, B, C, AB, AO, AC, ABperp, ACperp;
    private List<Vector3> simplex;

    private void Awake()
    {
        origin = new Vector3(0, 0, 0);
        simplex = new List<Vector3>(3);
    }

    public bool checkAABBDetection(GameObject box1, GameObject box2)
    {
        if ((box1.transform.position.x + box1.transform.lossyScale.x / 2 + 0.2f) > (box2.transform.position.x - box2.transform.lossyScale.x / 2 - 0.2f)
                    && (box1.transform.position.x - box1.transform.lossyScale.x / 2 -0.2f) < (box2.transform.position.x + box2.transform.lossyScale.x / 2 +0.2f)
                    && (box1.transform.position.y - box1.transform.lossyScale.x / 2 -0.2f) < (box2.transform.position.y + box2.transform.lossyScale.x / 2+0.2f)
                    && (box1.transform.position.y + box1.transform.lossyScale.x / 2+0.2f) > (box2.transform.position.y - box2.transform.lossyScale.x / 2-0.2f))
        {
            return true;
        }
        else
            return false;
    }

    public bool checkGJKDetection(GameObject box1, GameObject box2)
    {
        simplex.Clear();
        // Vertices
        vertices1 = new Vector3[box1.GetComponent<LineRenderer>().positionCount];
        vertices2 = new Vector3[box2.GetComponent<LineRenderer>().positionCount];


        box1.GetComponent<LineRenderer>().GetPositions(vertices1);
        box2.GetComponent<LineRenderer>().GetPositions(vertices2);



        for (int i = 0; i < vertices1.Length; i++)
        {
            vertices1[i].x += box1.transform.position.x;
            vertices1[i].y += box1.transform.position.y;
            //print(vertices1[i]);
        }

        for (int i = 0; i < vertices2.Length; i++)
        {
            vertices2[i].x += box2.transform.position.x;
            vertices2[i].y += box2.transform.position.y;
        }

        // Direction vector
        direction = (box2.transform.position - box1.transform.position).normalized;

        // Simplex points (with supports points)
        simplex.Add(getSupport(vertices1, vertices2, direction));

        direction = origin - simplex[0];

        // Loop
        while (true)
        {
            A = getSupport(vertices1, vertices2, direction);

            if (Vector3.Dot(A, direction) < 0) // Collision impossible
                return false;
            else
            {
                simplex.Add(A);
                //simplexObject.GetComponent<LineRenderer>().SetPositions(simplex.ToArray());
                if (handleSimplex(simplex, direction))
                    return true;
                //else
                // return false;
            }

        }
    }

    Vector3 getSupport(Vector3[] ver1, Vector3[] ver2, Vector3 dir)
    {
        return (getFurthestPoint(ver1, dir) - getFurthestPoint(ver2, -dir)).normalized;
    }

    Vector3 getFurthestPoint(Vector3[] ver, Vector3 dir)
    {
        Vector3 res = ver[0];
        float max = Vector3.Dot(ver[0], dir);

        for (int i = 1; i < ver.Length; i++)
        {
            if (Vector3.Dot(ver[i], dir) > max)
            {
                res = ver[i];
                max = Vector3.Dot(ver[i], dir);
            }

        }

        return res;
    }

    bool handleSimplex(List<Vector3> simpl, Vector3 dir)
    {

        if (simpl.Count == 2)
            return lineCase(simpl, dir);
        else
            return triangleCase(simplex, direction);
    }

    bool lineCase(List<Vector3> simpl, Vector3 dir)
    {
        //simplex.Clear();
        B = simplex[0];
        A = simplex[1];
        AB = (B - A).normalized;
        AO = (origin - A).normalized;
        ABperp = tripleProduct(AB, AO, AB);
        direction = ABperp;
        return false;
    }

    bool triangleCase(List<Vector3> simpl, Vector3 dir)
    {
        //simplex.Clear();
        simplex.Add(getSupport(vertices1, vertices2, direction));
        C = simplex[0];
        B = simplex[1];
        A = simplex[2];

        AB = (B - A).normalized;
        AC = (C - A).normalized;
        AO = (origin - A).normalized;
        ABperp = tripleProduct(AC, AB, AB);
        ACperp = tripleProduct(AB, AC, AC);

        if (Vector3.Dot(ABperp, AO) > 0)
        {
            simplex.Remove(C);
            direction = ABperp;
            return false;
        }
        else if (Vector3.Dot(ACperp, AO) > 0)
        {
            simplex.Remove(B);
            direction = ACperp;
            return false;
        }
        else
        {
            return true;
        }
    }

    Vector3 tripleProduct(Vector3 a, Vector3 b, Vector3 c)
    {
        return Vector3.Cross(Vector3.Cross(a, b), c).normalized;
    }
}
