using UnityEngine;
using UnityEngine.AI;

public class MonsterChase : MonoBehaviour
{
    public GameObject player;
    private NavMeshAgent agent;
    private bool chasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        BathroomDoorScript.OnBathroomDoorOpened += StartChase;
    }

    void Update()
    {
        if(chasing)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    public void StartChase()
    {
        chasing = true;
    }

    public void StopChase()
    {
        chasing = false; 
        agent.ResetPath();
    }
}
