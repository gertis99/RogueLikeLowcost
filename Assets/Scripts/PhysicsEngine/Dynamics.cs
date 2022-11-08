using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamics : MonoBehaviour
{
    private Vector3 relativeVeolicity;
    private Vector3 normalCol, penetrationCol, impulse;
    private float normalVeloicty, impulseScalar, restitution;

    // Solve collision for the gameobjects in movement
    public void solveCollision(GameObject tear, GameObject enemyTear)
    {
        // Calculate the relaitve velocity of the collision
        relativeVeolicity = enemyTear.GetComponent<EnemyTear>().getVelocity() - tear.GetComponent<Tear>().getVelocity();

        // Calculate the normal collision
        normalCol = (enemyTear.transform.position - tear.transform.position).normalized;

        // Calculate the velocity on normal
        normalVeloicty = Vector3.Dot(relativeVeolicity, normalCol);

        // Only resolve if are not separating
        if (normalVeloicty <= 0)
        {
            // Calculate restitution
            restitution = min(tear.GetComponent<Tear>().getRestitution(), enemyTear.GetComponent<EnemyTear>().getRestitution());

            // Calculate impulse scalar (mass = 0.5)
            impulseScalar = -(1 + restitution) * normalVeloicty;
            impulseScalar /= (float)(1 / tear.GetComponent<Tear>().getMass() + 1 / enemyTear.GetComponent<EnemyTear>().getMass());

            // Apply impulse
            impulse = impulseScalar * normalCol;
            tear.GetComponent<Tear>().setVelocity(tear.GetComponent<Tear>().getVelocity() - ((float)(1 / tear.GetComponent<Tear>().getMass()) * impulse));
            enemyTear.GetComponent<EnemyTear>().setVelocity(enemyTear.GetComponent<EnemyTear>().getVelocity() + ((float)(1 / enemyTear.GetComponent<EnemyTear>().getMass()) * impulse));
        }
    }

    float min(float a, float b)
    {
        if (a <= b)
            return a;
        else
            return b;
    }
}
