using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentGenerator : MonoBehaviour
{
    public zeroAgent agent;
    public zeroAgent[] agents;

    public float agentGenerateTime = 1;
    public float agentGenerateTimer;
    public int maxAgents = 1;
    public int currentAgents;

    public Transform[] zones;
    // Start is called before the first frame update
    void Start()
    {
        agents = new zeroAgent[maxAgents];
        currentAgents = 0;
        agentGenerateTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        agentGenerateTimer += Time.deltaTime;
        if(currentAgents < maxAgents && agentGenerateTimer > agentGenerateTime)
        {
            zeroAgent tmp = Instantiate(agent);
            agents[currentAgents] = tmp;
            tmp.InitializeAgent(currentAgents);
            tmp.SetPosition(zones[0].position, zones[1].position);

            currentAgents++;
            agentGenerateTimer = 0;
        }
    }
}
