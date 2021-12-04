using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReacherGoal : MonoBehaviour
{
    // 먼저 effector(hand)가 trigger하기 위해선 box collider 컴포넌트 추가
    // hand가 goal에 닿았는지 체크
    public GameObject agent;
    public GameObject hand;
    public GameObject goalOn;

    // 먼저 goal에 sphere collider에 Is Trigger 에 체크해야함.
    // 다른 collider(Trigger)가 해당 collider와 충돌했는지를 검사하는 것.
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 hand라면 GoalOn 오브젝트(파랑)을 크게 만들기
        if (other.gameObject == hand)
        {
            // 0.95f -> 1.05f
            goalOn.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        }
    }

    // Trigger가 해당 오브젝트로 부터 떠나면
    void OnTriggerExit(Collider other)
    {
        // 떠난 trigger가 hand라면 GoalOn의 크기 줄이기.
        if(other.gameObject == hand)
        {
            // 1.05f -> 0.95f
            goalOn.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
        }
    }

    // Trigger가 해당 오브젝트에 계속 있다면 보상을 준다.
    void OnTriggerStay(Collider other)
    {
        // 떠난 trigger가 hand라면 GoalOn의 크기 줄이기.
        if (other.gameObject == hand)
        {
            // Trigger(hand)가 Goal에 매번 들어갈때 마다 보상을 준다.
            agent.GetComponent<ReacherRobot>().AddReward(0.01f);
        }
    }
}
