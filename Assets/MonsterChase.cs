using UnityEngine;
using UnityEngine.AI;

public class MonsterChase : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;

    private bool isChasing = false;

    private void OnEnable()
    {
        BathroomDoorScript.OnBathroomDoorOpened += StartChase;
    }

    private void OnDisable()
    {
        BathroomDoorScript.OnBathroomDoorOpened -= StartChase;
    }

    public void StartChase()
    {
        isChasing = true;
    }

    void Update()
    {
        if(isChasing)
        {
            agent.SetDestination(player.transform.position);

            RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit))
        {
            
            SimpleOpenClose door = hit.transform.GetComponent<SimpleOpenClose>();
            if (door && !door.objectOpen)
            {
                
                door.ObjectClicked();
            }
        }
        }
    }
}


