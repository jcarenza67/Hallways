using UnityEngine;
using UnityEngine.AI;

public class MonsterChase : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;

    private bool isChasing = false;

    public void StartChase()
    {
        isChasing = true;
    }

    void Update()
    {
        if (isChasing)
        {
            agent.SetDestination(player.transform.position);
        }
    }
}


