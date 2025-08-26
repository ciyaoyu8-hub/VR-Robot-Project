using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotCtrl : MonoBehaviour
{
   [HideInInspector]public NavMeshAgent nav;
    [HideInInspector] public Animator animator;
    public RobotInfoItemCtrl RobotInfoItem;

    [HideInInspector] public List<TargetPointCtrl> TargetPoints = new List<TargetPointCtrl>();
    
    public void AddTargetPoints(TargetPointCtrl tpc)
    {
        TargetPoints.Add(tpc);

        tempTargetPoints.Add(tpc.transform.position);
    }

    [HideInInspector] public List<Vector3> tempTargetPoints = new List<Vector3>();
    [HideInInspector] public bool IsNavMove;
    public Vector3 CurrentTargetPoints
    {
      
        get {
            return tempTargetPoints[index]; }
    }
    [HideInInspector] public int index;
    [HideInInspector] public int tempLenght;
    public Material material;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        RobotInfoItem.SetState(RobotState.Idle);
        StartCoroutine(MovePoint());
    }
    public IEnumerator ArriveTargetPoint()
    {
        IsNavMove = false;
        if (tempTargetPoints.Count > 0)
        {
            tempTargetPoints.RemoveAt(0);
        }
        else
        {
            yield break;
        }

       
        if (tempLenght>0)
        {
            tempLenght--;
            if (tempTargetPoints.Count > 0)
            {

                SetNavDestination(tempTargetPoints[0]);
            }

            ObjectPool.Instance.Recycle(TempTargetPoints[0]);
            TempTargetPoints.RemoveAt(0);
            yield break;
        }
        nav.isStopped = true;
        RobotInfoItem.SetState(RobotState.DataSampling);
        animator.SetTrigger("IsCollection");

        yield return new WaitForSeconds(3);
        if (tempTargetPoints.Count > 0)
        {

            SetNavDestination(tempTargetPoints[0]);
        }


        TargetPoints[0].SetArriveColor();
        TargetPoints.RemoveAt(0);
        if (TargetPoints.Count > 0)
        {
            nav.isStopped = false;
        
        }
      else  if (TargetPoints.Count ==0)
        {

            RobotInfoItem.SetState(RobotState.Idle);
            animator.SetTrigger("IsIdle");
            RobotManager.Instance.TryGameWin();
        }
    }
    List<GameObject> TempTargetPoints = new List<GameObject>();
    public void SetTempTargetPoints(List<Vector3> vector3s)
    {
        foreach (var item in TempTargetPoints)
        {

            ObjectPool.Instance.Recycle(item);
        }
        TempTargetPoints.Clear();
        foreach (var item in vector3s)
        {

            TempTargetPoints.Add(ObjectPool.Instance.Create("TempTargetPoint", item, Quaternion.identity));
        }
        tempLenght = vector3s.Count;
           tempTargetPoints = vector3s;
        foreach (var item in TargetPoints)
        {
            tempTargetPoints.Add(item.transform.position);
        }
        SetNavDestination(tempTargetPoints[0]);
    }
    LineRenderer lineRenderer; // 用于可视化路径
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
    public void SetNavDestination(Vector3 targetPos)
    {
        nav.ResetPath();
        // 先设置目标点
        nav.SetDestination(targetPos);
        IsNavMove = true;

        // 启动协程等待路径计算完成
        StartCoroutine(WaitForPathCalculation(targetPos));
    }

    private IEnumerator WaitForPathCalculation(Vector3 targetPos)
    {
        yield return null;
        // 等待路径计算完成（最多等待1秒防止卡死）
        float timeout = 1f;
        while (!nav.hasPath && timeout > 0)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }

        // 确认路径已生成
        if (nav.hasPath)
        {
            // 获取路径拐点
            Vector3[] pathCorners = nav.path.corners;

            // 可视化路径
            lineRenderer.positionCount = pathCorners.Length;
            for (int i = 0; i < pathCorners.Length; i++)
            {
                //lineRenderer.SetPosition(i,new Vector3(pathCorners[i].x,0.02f, pathCorners[i].z));
                lineRenderer.SetPosition(i, pathCorners[i]);
            }
        }
        else
        {
            Debug.LogWarning("Path calculation failed!");
        }

        // 更新动画状态（无论路径是否生成成功都执行）
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != Animator.StringToHash("Walk"))
        {
            RobotInfoItem.SetState(RobotState.Walk);
            animator.SetTrigger("IsWalk");
        }
    }

    //public void SetNavDestination(Vector3 targetPos)
    //{
    //    Debug.Log(nav.hasPath);
    //    if (nav.hasPath) // 确保路径已计算
    //    {
    //        // 获取路径拐点
    //        Vector3[] pathCorners = nav.path.corners;

    //        // 可视化路径（用LineRenderer）
    //        lineRenderer.positionCount = pathCorners.Length;
    //        for (int i = 0; i < pathCorners.Length; i++)
    //        {
    //            lineRenderer.SetPosition(i, pathCorners[i]);
    //        }
    //    }
    //    nav.SetDestination(targetPos);
    //    if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != Animator.StringToHash("Walk"))
    //    {

    //        RobotInfoItem.SetState(RobotState.Walk);
    //        animator.SetTrigger("IsWalk");
    //    }
    //    IsNavMove = true;
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer ==LayerMask.NameToLayer("PolygonAreaCreator"))
        {
            RobotInfoItem.SetState(RobotState.HitObstacle);
            GameManager.Instance.GameLose();
        }
    }
    List<GameObject> MovePoints = new List<GameObject>();
    bool CanCreateMovePoint;
    IEnumerator MovePoint()
    {
        while (true)
        {
            CanCreateMovePoint = true;
            foreach (var item in MovePoints)
            {
                if (Vector3.Distance(item.transform.position, transform.position) < 0.01f)
                {
                    CanCreateMovePoint = false;
                    break;
                }
            }
            if (CanCreateMovePoint)
            {
                MovePoints.Add(ObjectPool.Instance.Create("TempTargetPoint", transform.position, Quaternion.identity));
                MovePoints[MovePoints.Count - 1].GetComponentInChildren<Renderer>().material = material;
            }

            yield return new WaitForSeconds(0.1f);
        }
       
    }
}
