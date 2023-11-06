using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentScheduler : MonoBehaviour
{
    int currnetRoad = 1;
    int endRoad = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextRoad(zeroAgent agent, Vector3 goal)
    {
        if(++currnetRoad < endRoad)
        {
            agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Collider[] collides = Physics.OverlapSphere(goal, 1);
            bool isSetRoad = false;
            while (!isSetRoad)
            {
                for (int i = 0; i < collides.Length; i++)
                {
                    if (collides[i].tag == "Goal" && collides[i].transform.position != goal)
                    {
                        int dice = Random.Range(1, 7);
                        if(dice < 3)
                        {
                            Transform road = collides[i].transform.parent;
                            agent.SetPosition(agent.transform.position,
                                road.Find(((collides[i].name == "Goal1") ? "Goal2" : "Goal1")).transform.position);
                            isSetRoad = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
