using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class RobotArmAgent : Agent
{
    // 유니티 게임 오브젝트 선언하고, 매뉴얼로 할당한다..
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
    public GameObject target;

    public override void Initialize()
    {
        // 각 오브젝트에 미리 추가시킨 Rigidbody 컴포넌트 불러와
        m_RbA = pendulumA.GetComponent<Rigidbody>();
        m_RbB = pendulumB.GetComponent<Rigidbody>();
        m_RbC = pendulumC.GetComponent<Rigidbody>();
        m_RbD = pendulumD.GetComponent<Rigidbody>();
        m_RbE = pendulumE.GetComponent<Rigidbody>();
        m_RbF = pendulumF.GetComponent<Rigidbody>();

   
    }

    public override void OnEpisodeBegin()
    {
        // agent 위치 회전 속도 각속도 세팅
        // 각 에피소드가 처음 시작할때 각 오브젝트들의 위치 및 회전, 속도 정보 를 초기화(자신의 본 위치(유니티상) + 부모의 위치)
        // transform.position : 부모의 위치를 반영해야 나중에 많은 복사본을 만들때 오류없음.
        pendulumA.transform.position = new Vector3(0f, 0.55f, 0f) + transform.position;
        pendulumA.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        m_RbA.velocity = Vector3.zero;
        m_RbA.angularVelocity = Vector3.zero;

        pendulumB.transform.position = new Vector3(-0.15f, 0.55f, 0f) + transform.position;
        pendulumB.transform.rotation = Quaternion.Euler(0f, -90f, 90f);
        m_RbB.velocity = Vector3.zero;
        m_RbB.angularVelocity = Vector3.zero;

        pendulumC.transform.position = new Vector3(-0.97f, 0.55f, 0f) + transform.position;
        pendulumC.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        m_RbC.velocity = Vector3.zero;
        m_RbC.angularVelocity = Vector3.zero;

        pendulumD.transform.position = new Vector3(-0.97f, 0.55f, 0f) + transform.position;
        pendulumD.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        m_RbD.velocity = Vector3.zero;
        m_RbD.angularVelocity = Vector3.zero;

        pendulumE.transform.position = new Vector3(-0.97f, 1.175f, 0f) + transform.position;
        pendulumE.transform.rotation = Quaternion.Euler(0f, -90f, 90f);
        m_RbE.velocity = Vector3.zero;
        m_RbE.angularVelocity = Vector3.zero;

        pendulumF.transform.position = new Vector3(-1.08f, 1.175f, 0f) + transform.position;
        pendulumF.transform.rotation = Quaternion.Euler(0f, -90f, 90f);
        m_RbF.velocity = Vector3.zero;
        m_RbF.angularVelocity = Vector3.zero;

        // 타겟 - 위치 세팅
        var posX = 2f - Random.value * 4f;
        var posZ = 2f - Random.value * 4f;
        while ((Mathf.Pow(posX, 2) + Mathf.Pow(posZ, 2) > 3.5) || (Mathf.Pow(posX, 2) + Mathf.Pow(posZ, 2) < 0.7))
        {
            posX = 2f - Random.value * 4f;
            posZ = 2f - Random.value * 4f;
        }
        //target.transform.position = new Vector3(posX, 0.1f - transform.position.y, posZ) + transform.position;
        target.transform.localPosition = new Vector3(posX, 0.1f, posZ);


    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 총 13*6 + 6 = 84
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
        sensor.AddObservation(target.transform.localPosition); //3
        sensor.AddObservation(hand.transform.localPosition); //3
    }

    public float speed = 5f;
    public override void OnActionReceived(float[] vectorAction)
    {
        /*
        // Continuous version 
        var torque = Mathf.Clamp(vectorAction[0], -1f, 1f) * 150f;
        m_RbA.AddTorque(new Vector3(0f, torque, 0f));

        torque = Mathf.Clamp(vectorAction[1], -1f, 1f) * 150f;
        m_RbB.AddTorque(new Vector3(0f, 0f, torque));

        torque = Mathf.Clamp(vectorAction[2], -1f, 1f) * 150f;
        m_RbC.AddTorque(new Vector3(0f, 0f, torque));

        //torque = Mathf.Clamp(vectorAction[3], -1f, 1f) * 150f;
        //m_RbD.AddTorque(new Vector3(0f, torque, 0f));

        //torque = Mathf.Clamp(vectorAction[4], -1f, 1f) * 150f;
        //m_RbE.AddTorque(new Vector3(0f, 0f, torque));

        //torque = Mathf.Clamp(vectorAction[5], -1f, 1f) * 150f;
        //m_RbF.AddTorque(new Vector3(0f, torque, 0f));
        */


        // Discrete version 
        float r_RbA = vectorAction[0] <= 1 ? vectorAction[0] : -1; //vectorAction[0] == 0, 1, 2
        float r_RbB = vectorAction[1] <= 1 ? vectorAction[1] : -1;
        float r_RbC = vectorAction[2] <= 1 ? vectorAction[2] : -1;
        m_RbA.AddTorque(new Vector3(0f, r_RbA * speed, 0f));
        m_RbB.AddTorque(new Vector3(0f, 0f, r_RbB * speed));
        m_RbC.AddTorque(new Vector3(0f, 0f, r_RbC * speed));
        //m_RbD.AddTorque(new Vector3(0f, vectorAction[3] * speed, 0f));
        //m_RbE.AddTorque(new Vector3(0f, 0f, vectorAction[4] * speed));
        //m_RbF.AddTorque(new Vector3(0f, vectorAction[5] * speed, 0f));
        
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
        actionsOut[2] = Input.GetAxis("Fire1");
    }
}
