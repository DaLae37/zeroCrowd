using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentGenerator : MonoBehaviour
{
    public zeroAgent agent;
    public Transform agents;

    public int roadNum = 1;
    public bool isTraining;

    public int maxAgents = 3;
    public float agentGenerateTime = 1;
    public float []agentGenerateTimer;
    public int []currentAgents;
    public Transform[] zones;
    // Start is called before the first frame update
    void Start()
    {
        agentGenerateTimer = new float[roadNum];
        currentAgents = new int[roadNum];
        zones = new Transform[roadNum];
        for (int i = 0; i < roadNum; i++)
        {
            zones[i] = GameObject.Find("Road" + (i+1).ToString()).transform;
            currentAgents[i] = 0;
            agentGenerateTimer[i] = 0;
        }

        InitAgents();
    }

    void InitAgents()
    {
        for (int i = 0; i < roadNum; i++)
        {
            int current = currentAgents[i];
            for (int j = 0; j < maxAgents - current; j++)
            {
                zeroAgent tmp = Instantiate(agent);
                tmp.transform.SetParent(agents);
                tmp.InitializeAgent(i, isTraining, Random.Range(0.25f, 0.4f), Random.Range(0.9f, 1.3f));

                int r = Random.Range(1, 3);
                tmp.SetPosition(zones[i].Find("Goal" + r.ToString()).position,
                    zones[i].Find("Goal" + ((r == 1) ? 2 : 1).ToString()).position);

                currentAgents[i]++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndSignal(zeroAgent g, int i)
    {
        int r = Random.Range(1, 3);
        g.SetPosition(zones[i].Find("Goal" + r.ToString()).position,
            zones[i].Find("Goal" + ((r == 1) ? 2 : 1).ToString()).position);
    }
}
