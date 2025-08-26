using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotManager : MonoBehaviour
{
    #region 单例
    // 单例模式实现
    private static RobotManager instance;

    public static RobotManager Instance
    {
        get { return instance; }
        private set
        { instance = value; }
    }

    private void Awake()
    {
        // 单例唯一性检查
        if (instance != null && instance != this)
        {
            Debug.LogWarning(gameObject.name + "单例唯一性检查异常");
            Destroy(gameObject);
            return;
        }
        instance = this;
        AwakeInit();
    }
    #endregion
    void AwakeInit()
    {
    }
    public List<RobotCtrl> navCtrls;
    public void SetPoint(int RobotIndex, Vector3 targetPos)
    {
        navCtrls[RobotIndex].SetNavDestination(targetPos);
    }
    //public void SetLine(int RobotIndex, List<Vector3> path, Vector3 targetPos)
    //{
    //    StartCoroutine(SetPath(navCtrls[RobotIndex], path, targetPos));
    //}

    //IEnumerator SetPath(RobotCtrl robotCtrl, List<Vector3> path, Vector3 targetPos)
    //{
    //    path.Add(targetPos);
    //    int index = 0;
    //    robotCtrl.SetNavDestination(path[index]);
    //    while (index< path.Count-1)
    //    {
    //        if (robotCtrl.nav.remainingDistance < 0.5f && !robotCtrl.nav.pathPending)
    //        {
    //            robotCtrl.SetNavDestination(path[++index]);
    //        }
    //        yield return null;
    //    }
    //}
    private void Update()
    {
        foreach (var robotCtrl in navCtrls)
        {
            if (InstructPanelCtrl.Instance!=null&&InstructPanelCtrl.Instance.isNext && robotCtrl.nav.remainingDistance < 0.1f && !robotCtrl.nav.pathPending && robotCtrl. IsNavMove)
            {
                StartCoroutine(robotCtrl.ArriveTargetPoint());
            }
        }
    }

    public void StopNav()
    {
        foreach (var item in navCtrls)
        {
            item.nav.isStopped = true;
            item.animator.SetTrigger("IsIdle");
        }
    }
    public void TryGameWin()
    {
        foreach (var item in navCtrls)
        {
            if(item.TargetPoints.Count != 0)
            {
                return;
            }
        }
        GameManager.Instance.GameWin();
    }
}
