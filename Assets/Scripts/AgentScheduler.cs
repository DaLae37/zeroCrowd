using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentScheduler : MonoBehaviour
{
    public Transform[] zones;
    int currnetRoad = 1;
    int endRoad = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextRoad(zeroAgent agent)
    {
        if(++currnetRoad < endRoad)
        {
            agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            GameObject road = GameObject.Find("Road" + currnetRoad.ToString());
            agent.SetPosition(road.transform.Find("Goal1").transform.position, 
                road.transform.Find("Goal2").transform.position);
        }
    }
}
