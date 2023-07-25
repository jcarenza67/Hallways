using UnityEngine;
using UnityEngine.AI;

public class MonsterChase : MonoBehaviour
{
    public GameObject player;
    private NavMeshAgent agent;
    private bool chasing = false; // Add this line

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        BathroomDoorScript.OnBathroomDoorOpened += StartChase; // Listen for the bathroom door opening
    }

    void Update()
    {
        if(chasing) // Only chase the player if we're supposed to be
        {
            agent.SetDestination(player.transform.position);
        }
    }

    public void StartChase()
    {
        chasing = true; // Set chasing to true when StartChase is called
    }

    public void StopChase()
    {
        chasing = false; // Set chasing to false when StopChase is called
        agent.ResetPath(); // Clear the agent's path when we stop chasing
    }
}
