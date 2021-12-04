using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class BallAgentLogic : Agent
{
    Rigidbody rBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform target;

    public override void OnEpisodeBegin()
    {
        // 1. 에이전트 위치 결정Reset agent
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(-9, 0.5f, 0);

        // 2. 타깃 위치 결정Move target to new random spot (limited spot)
        target.localPosition = new Vector3(12 + Random.value * 8, Random.value * 3, Random.value * 10 - 5);
            // (12~20, 0~3, -5~5)
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent position & Agent velocity
        // 센서로 관측할 것들 입력 -> 총 9개 숫자 -> Behavior Parameters의 Space Size 9개
        sensor.AddObservation(target.localPosition); // 타겟의 포지션
        sensor.AddObservation(this.transform.localPosition); // agent의 포지션
        sensor.AddObservation(rBody.velocity);
    }
    public float speed = 20;
    public override void OnActionReceived(float[] vectorAction)
    {
        /*
            branches size = 2
            branch0 size = 2 설정 : 0, 1    -> vectorAction[0] : 0, 1
                0 : 움직이지 않음. 1 : 1만큼 움직이도록 (x방향)
            branch1 size = 3 설정 : 0, 1, 2 -> vectorAction[1] : 0, 1, 2
                0 : 움직이지 않음. 1 : 1만큼 움직이게(z반대방향), 2 : 1만큼 움직이게 (z방향)
        */
        // control에 의해 force를 받아 움직이게됨.
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];

        if (vectorAction[1] == 2) controlSignal.z = 1;    // z방향으로 1(왼쪽)
        else controlSignal.z = -vectorAction[1]; // z방향으로 -1(오른쪽)
        

        // 정해진 구간에서! 방향에 속도 곱하기!
        if (this.transform.localPosition.x < 8.5f)
        {
            // 점프구간(8.5)전까지만 움직이는 게 가능하도록!
            rBody.AddForce(controlSignal * speed);
        }

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);
        // Reached target - 타깃과 근접하면 에피소드 종료
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        // Fell of platform - 바닥으로 떨어지면 에피소드 종료
        if(this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
    }




}
