using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.Rendering;

public class zeroAgent : Agent
{
    #region Constant
    public float livingPenalty = 0.00015f;
    public float goalReward = 1f;
    public float arrivalReward = 0.00095f;
    public float arrivalPenalty = 0.00025f;
    public float groundPenalty = 0.0005f;
    public float collideAgentPenalty = 0.01f;
    #endregion

    #region Values
    private Rigidbody rb;

    [Header("AgentValue")]
    [SerializeField] private int agentNum;
    [SerializeField] private float reward;
    private bool isInitalized = false;
    private bool isOnGround = false;
    private bool isTraining = true;

    [Header("RigidValue")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Vector3 goalPosition;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float goalDistance;
    [SerializeField] private float currentAngle;
    private int stillCounter = 0;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitializeAgent(int agentNum, bool isTraining, float moveSpeed = 0.3f, float maxSpeed = 1.0f, float turnSpeed = 50f)
    {
        if (!isInitalized)
        {
            this.agentNum = agentNum;
            this.moveSpeed = moveSpeed;
            this.turnSpeed = turnSpeed;
            this.maxSpeed = maxSpeed;
            this.isTraining = isTraining;

            reward = 0;
            isInitalized = true;
        }
    }

    public void SetPosition(Vector3 starPosition, Vector3 goalPosition)
    {
        transform.position = starPosition;
        this.startPosition = transform.position;
        this.goalPosition = goalPosition;

        goalDistance = Vector3.Distance(goalPosition, transform.position);
    }

    public override void OnEpisodeBegin()
    {
        if(!isInitalized)
        {
            reward = 0f;
            rb.velocity = Vector3.zero;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        sensor.AddObservation(localVelocity.x);
        sensor.AddObservation(localVelocity.z);

        sensor.AddObservation(goalDistance);
        sensor.AddObservation(currentAngle);

        sensor.AddObservation(isOnGround);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions.DiscreteActions[0]);
        SetAgentReward();
        if(reward < -50f)
        {
            if (isTraining)
            {
                GameObject.Find("AgentGenerator").GetComponent<AgentGenerator>().EndSignal(this, agentNum);
                isInitalized = false;
                EndEpisode();
            }
        }
    }

    private void SetAgentReward()
    {
        if (goalDistance < 2.0f)
        {
            AddReward(goalReward);
            if (isTraining)
            {
                GameObject.Find("AgentGenerator").GetComponent<AgentGenerator>().EndSignal(this, agentNum);
                isInitalized = false;
                EndEpisode();
            }
            else
            {
                GameObject.Find("AgentScheduler").GetComponent<AgentScheduler>().NextRoad(this);
            }
        }

        AddReward(-livingPenalty);
        reward -= livingPenalty;

        float currentGoalDistance = Vector3.Distance(transform.position, goalPosition);
        currentAngle = Vector3.Angle(transform.forward, goalPosition - transform.position);
        float threshholdAngle = 15 + (35 * (currentGoalDistance / (Vector3.Distance(goalPosition, startPosition))));
        if (currentGoalDistance < goalDistance)
        {
            if(currentAngle <= threshholdAngle)
            {
                AddReward(arrivalReward);
                reward += arrivalReward;
            }
            goalDistance = currentGoalDistance;
        }
        else
        {
            AddReward(-arrivalPenalty);
            reward -= arrivalPenalty;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            if(hit.transform.tag == "Ground")
            {
                isOnGround = true;
                AddReward(-groundPenalty);
                reward -= groundPenalty;
            }
            else
            {
                isOnGround = false;
            }
        }
    }

    private Vector3 unstuckAction()
    {
        int randomAction = UnityEngine.Random.Range(0, 4);
        switch (randomAction)
        {
            case 0:
                return transform.forward * 0.05f;
            case 1:
                return transform.forward * -0.025f;
            case 2:
                return transform.right * -0.025f;
            case 3:
                return transform.right * 0.025f;
        }
        return Vector3.zero;
    }

    private void MoveAgent(int action)
    {
        Vector3 moveDir = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;

        if (stillCounter >= 10)
        {
            moveDir = unstuckAction();
            stillCounter = 0;
        }

        switch (action)
        {
            case 0:
                stillCounter++;
                break;
            case 1:
                stillCounter = 0;
                moveDir = transform.forward * 0.75f;
                break;
            case 2:
                stillCounter = 0;
                moveDir = transform.forward * -0.1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
            case 5:
                stillCounter = 0;
                moveDir = transform.right * -0.1f;
                break;
            case 6:
                stillCounter = 0;
                moveDir = transform.right * 0.1f;
                break;
        }
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);
        rb.AddForce(moveDir * moveSpeed, ForceMode.VelocityChange);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Agent")
        {
            AddReward(-collideAgentPenalty);
            reward -= collideAgentPenalty;
        }
    }
}