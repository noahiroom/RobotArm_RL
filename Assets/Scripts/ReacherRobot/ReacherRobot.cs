using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ReacherRobot : Agent
{

    // 유니티 각 게임 오브젝트와 그 부분들을 할당하기 위해. 먼저 선언
    public GameObject pendulumA;
    public GameObject pendulumB;
    public GameObject pendulumC;
    public GameObject pendulumD;
    public GameObject pendulumE;
    public GameObject pendulumF;

    Rigidbody m_RbA;
    Rigidbody m_RbB;
    Rigidbody m_RbC;
    Rigidbody m_RbD;
    Rigidbody m_RbE;
    Rigidbody m_RbF;

    public GameObject hand;
    public GameObject goal;

    // 공 움직이기 세팅
    public float m_GoalHeight = 1.2f;
    float m_GoalRadius;     // 공이 존재할 수 있는 범위의 반경
    float m_GoalDegree;     // 한번 로테이트 할때 얼마나 많이 돌것인가
    float m_GoalSpeed;      // 공이 돌아가는 속도
    float m_GoalDeviation;  // 공이 올라가고 내려가는 정도
    float m_GoalDeviationFreq;  // 얼마나 자주 올라가고 내려올 것인가.

    public override void Initialize()
    {
        // 각 오브젝트에 미리 추가시킨 Rigidbody 컴포넌트를 불러와.
        m_RbA = pendulumA.GetComponent<Rigidbody>();
        m_RbB = pendulumB.GetComponent<Rigidbody>();
        m_RbC = pendulumC.GetComponent<Rigidbody>();
        m_RbD = pendulumD.GetComponent<Rigidbody>();
        m_RbE = pendulumE.GetComponent<Rigidbody>();
        m_RbF = pendulumF.GetComponent<Rigidbody>();

        SetResetParameters();// Start()와 Initialize()는 같은 기능이니까. 
    }

    public override void OnEpisodeBegin()
    {
        // 각 에피소드가 처음 시작할때 각 오브젝트들의 위치 및 회전, 속도 정보 를 초기화(자신의 본 위치(유니티상) + 부모의 위치)
        // transform.position : 부모의 위치를 반영해야 나중에 많은 복사본을 만들때 오류없음.
        pendulumA.transform.position = new Vector3(0f, 0.55f, 0f) + transform.position;
        pendulumA.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        m_RbA.velocity = Vector3.zero;
        m_RbA.angularVelocity = Vector3.zero;

        pendulumB.transform.position = new Vector3(-0.15f, 0.55f, 0f) + transform.position;
        pendulumB.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        m_RbB.velocity = Vector3.zero;
        m_RbB.angularVelocity = Vector3.zero;

        pendulumC.transform.position = new Vector3(-0.15f, 1.375f, 0f) + transform.position;
        pendulumC.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        m_RbC.velocity = Vector3.zero;
        m_RbC.angularVelocity = Vector3.zero;

        pendulumD.transform.position = new Vector3(-0.15f, 1.375f, 0f) + transform.position;
        pendulumD.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        m_RbD.velocity = Vector3.zero;
        m_RbD.angularVelocity = Vector3.zero;

        pendulumE.transform.position = new Vector3(-0.15f, 2f, 0f) + transform.position;
        pendulumE.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        m_RbE.velocity = Vector3.zero;
        m_RbE.angularVelocity = Vector3.zero;

        pendulumF.transform.position = new Vector3(-0.15f, 2.11f, 0f) + transform.position;
        pendulumF.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        m_RbF.velocity = Vector3.zero;
        m_RbF.angularVelocity = Vector3.zero;

        SetResetParameters(); // 에피소드 시작할때마다 공의 위치,속도 정보 리셋

        // Start할때 공의 위치정보 업데이트하기
        m_GoalDegree += m_GoalSpeed; // 각속도 증가시킴.
        UpdateGoalPosition();
    }

    public void SetResetParameters()
    {
        m_GoalRadius = Random.Range(1f, 1.3f);
        m_GoalDegree = Random.Range(0f, 360f);
        m_GoalSpeed = Random.Range(-2f, 2f);
        m_GoalDeviation = Random.Range(-1f, 1f);
        m_GoalDeviationFreq = Random.Range(0f, 3.14f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 총 관찰해야하는 수 : 85 space size(13*6 + 7)
        // 각 joint 마다의 정보(위치,회전,속도,각속도)를 모두 sensing 하는 부분
        sensor.AddObservation(pendulumA.transform.localPosition); //3  // position 전체중 위치 vs localPosition 해당 부모에서의 위치?
        sensor.AddObservation(pendulumA.transform.rotation); //4
        sensor.AddObservation(m_RbA.velocity); //3
        sensor.AddObservation(m_RbA.angularVelocity); //3
        

        sensor.AddObservation(pendulumB.transform.localPosition);
        sensor.AddObservation(pendulumB.transform.rotation);
        sensor.AddObservation(m_RbB.velocity);
        sensor.AddObservation(m_RbB.angularVelocity);

        sensor.AddObservation(pendulumC.transform.localPosition);
        sensor.AddObservation(pendulumC.transform.rotation);
        sensor.AddObservation(m_RbC.velocity);
        sensor.AddObservation(m_RbC.angularVelocity);

        sensor.AddObservation(pendulumD.transform.localPosition);
        sensor.AddObservation(pendulumD.transform.rotation);
        sensor.AddObservation(m_RbD.velocity);
        sensor.AddObservation(m_RbD.angularVelocity);

        sensor.AddObservation(pendulumE.transform.localPosition);
        sensor.AddObservation(pendulumE.transform.rotation);
        sensor.AddObservation(m_RbE.velocity);
        sensor.AddObservation(m_RbE.angularVelocity);

        sensor.AddObservation(pendulumF.transform.localPosition);
        sensor.AddObservation(pendulumF.transform.rotation);
        sensor.AddObservation(m_RbF.velocity);
        sensor.AddObservation(m_RbF.angularVelocity);

        // 타겟 공의 위치, 잡는 손(effector)의 위치, 
        sensor.AddObservation(goal.transform.localPosition); //3
        sensor.AddObservation(hand.transform.localPosition); //3

        // 공의 속도도 sensing한다.
        sensor.AddObservation(m_GoalSpeed); // 1

    }
    // action을 받을 때마다 업데이트한다. 
    public override void OnActionReceived(float[] vectorAction)
    {
        // vectorAction 6개 - 각 조인트 6개에 대하여.
            // Continuous : Decimal , Discrete : 0 , 1 
        // 각 joint의 회전력(torque) : 해당 방향으로 밀어주는 힘 (따라서 150f 곱해 키워줌)
        var torque = Mathf.Clamp(vectorAction[0], -1f, 1f) * 150f;
        m_RbA.AddTorque(new Vector3(0f, torque, 0f));

        torque = Mathf.Clamp(vectorAction[1], -1f, 1f) * 150f;
        m_RbB.AddTorque(new Vector3(0f, 0f, torque));

        torque = Mathf.Clamp(vectorAction[2], -1f, 1f) * 150f;
        m_RbC.AddTorque(new Vector3(0f, 0f, torque));

        torque = Mathf.Clamp(vectorAction[3], -1f, 1f) * 150f;
        m_RbD.AddTorque(new Vector3(0f, torque, 0f));

        torque = Mathf.Clamp(vectorAction[4], -1f, 1f) * 150f;
        m_RbE.AddTorque(new Vector3(0f, 0f, torque));

        torque = Mathf.Clamp(vectorAction[5], -1f, 1f) * 150f;
        m_RbF.AddTorque(new Vector3(0f, torque, 0f));

        // action을 받을때마다 업데이트 하니까 Update()함수와 비슷
        m_GoalDegree += m_GoalSpeed; // 각속도 증가시킴.
        UpdateGoalPosition(); 
    }

    void UpdateGoalPosition()
    {
        var m_GoalDegree_rad = m_GoalDegree * Mathf.PI / 180f;
        var goalX = m_GoalRadius * Mathf.Cos(m_GoalDegree_rad);
        var goalZ = m_GoalRadius * Mathf.Sin(m_GoalDegree_rad);
        var goalY = m_GoalHeight + m_GoalDeviation * Mathf.Cos(m_GoalDeviationFreq * m_GoalDegree_rad);

        goal.transform.position = new Vector3(goalX, goalY, goalZ) + transform.position; 
        // 공의 포지션은 부모의 위치 정보를 반영해야하므로 더해준다.
    }
}
