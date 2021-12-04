using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmTarget : MonoBehaviour
{
    public GameObject agent;
    public GameObject hand;
    public GameObject goalOn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == hand)
        {
            goalOn.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject == hand)
        {
            goalOn.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == hand)
        {
            agent.GetComponent<RobotArmAgent>().SetReward(1.0f);
            agent.GetComponent<RobotArmAgent>().EndEpisode();
        }
    }
}
