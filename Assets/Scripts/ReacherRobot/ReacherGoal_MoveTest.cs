using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReacherGoal_MoveTest : MonoBehaviour
{
    public float m_GoalHeight = 1.2f;

    public GameObject goal;
    float m_GoalRadius;     // 공이 존재할 수 있는 범위의 반경
    float m_GoalDegree;     // 한번 로테이트 할때 얼마나 많이 돌것인가
    float m_GoalSpeed;      // 공이 돌아가는 속도
    float m_GoalDeviation;  // 공이 올라가고 내려가는 정도
    float m_GoalDeviationFreq;  // 얼마나 자주 올라가고 내려올 것인가.
    void Start()
    {
        SetResetParameters();
    }

    public void SetResetParameters()
    {
        m_GoalRadius = Random.Range(1f, 1.3f);
        m_GoalDegree = Random.Range(0f, 360f);
        m_GoalSpeed = Random.Range(-2f, 2f);
        m_GoalDeviation = Random.Range(-1f, 1f);
        m_GoalDeviationFreq = Random.Range(0f, 3.14f);
    }

    void Update()
    {
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
    }

}
