using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum RobotState
{
    Idle,// 待机,
    Walk,// 移动,
    HitObstacle, //碰到障碍物,
    DataSampling, //数据采样,
}
public class RobotInfoItemCtrl : MonoBehaviour
{
    public Transform Robot_TF;
    public Text PosValue;
    public Text StateValue;
    private void Update()
    {
        PosValue.text = Robot_TF.position.ToString("f1");
    }
    public void SetState(RobotState robotState)
    {
        StateValue.text = robotState.ToString();
    }
}
