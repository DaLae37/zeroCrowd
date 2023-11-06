using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road
{
    public int id;
    public int leftRoadsNum;
    public int rightRoadsNum;
    public List<Road> leftRoads;
    public List<Road> rightRoads;
}

public class EnvironmentGenerator : MonoBehaviour
{
    public GameObject []roadg;

    List<Road> roads = new List<Road>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RoadGenerate()
    {
        for (int i = 0; i < roadg.Length; i++) {
            Road road = new Road();
            road.id = (i + 1);
            
        }
    }
}
