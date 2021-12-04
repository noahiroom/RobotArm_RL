using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAgentFollow : MonoBehaviour
{
    public Transform BallAgentTransform;

    private Vector3 _cameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        // 카메라 위치 조정
        _cameraOffset = transform.position - BallAgentTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 각 프레임 마다 카메라가 에이전트를 따라가도록.
        transform.position = BallAgentTransform.position + _cameraOffset;

    }
}
