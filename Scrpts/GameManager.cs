using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TextType
{
    设定危险区域,
    设置目标采样点, 
    选择机器人,

}
public class GameManager : MonoBehaviour
{
    #region 单例
    // 单例模式实现
    private static GameManager instance;

    public static GameManager Instance
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
    public GameObject losePanel;
    public GameObject winPanel;
    public GameObject taskPanel;
    public GameObject HintPanel;
    public GameObject instructPanel;
    public GameObject MapPanel;
    public Text hintText;

    public int Stage;

    public void NextStage()
    {

        Stage++;

        if (Stage == 1)
        {
            SetText(TextType.设定危险区域);
        }
        if (Stage == 2)
        {
            SetText(TextType.设置目标采样点);
        }
        if (Stage == 3)
        {
            SetText(TextType.选择机器人);
        }
        if (Stage == 4)
        {
            instructPanel.SetActive(false);
            //RobotManager.Instance.navCtrls[0].SetTempTargetPoints(new List<Vector3>() { new Vector3(-4, 0, -1), new Vector3(-4, 0, 3) });
        }
    }

    public void SetText(TextType textType)
    {
        switch (textType)
        {
            case TextType.设定危险区域:
                hintText.text = "Please set up the danger zone.";

                //PolygonAreaCreator.Instance.AddPolygonFace(new List<Vector3>() { Vector3.zero, new Vector3 (1,0,1),new Vector3(3,0,1)});
                //PolygonAreaCreator.Instance.AddPolygonFace(new List<Vector3>() { Vector3.zero, new Vector3 (-1,0,1),new Vector3(-3,0,1)});
                break;

            case TextType.设置目标采样点:

                hintText.text = "Set the target sampling points.";
                /*
                
                CreateTargetPoint(Vector3.forward *3 ,"1");
                CreateTargetPoint(new Vector3(1,0,2) ,"2");
                CreateTargetPoint(new Vector3(2,0,2) ,"3");
                CreateTargetPoint(new Vector3(-2,0,-2) ,"4");
                CreateTargetPoint(new Vector3(-2,0,-5) ,"5");

                ToggleOrderTracker.Instance.InitData();
                MapPanel.SetActive(true);
                */

                break;

            case TextType.选择机器人:
                hintText.text = "Select the robot and the target.";
                instructPanel.SetActive(true);
                break;
            default:
                break;
        }
        textType++;
    }
 [HideInInspector]public   Dictionary<string, TargetPointCtrl> name_TPC = new Dictionary<string, TargetPointCtrl>();
    public Transform TargetIDToggles;
    public void CreateTargetPoint(Vector3 point,string _name)
    {
     GameObject go =   Instantiate(Resources.Load<GameObject>("TargetPoint"), point, Quaternion.identity);

        go.name = _name;
        go.GetComponent<TargetPointCtrl>().SetText(_name);
        name_TPC.Add(_name,go.GetComponent<TargetPointCtrl>());

        GameObject go1 = Instantiate(Resources.Load<GameObject>("TargetIDToggle"), TargetIDToggles);
        go1.GetComponentInChildren<Text>().text = _name;
        go1.name = _name;
        ToggleOrderTracker.Instance.toggles.Add(go1.GetComponentInChildren<Toggle>());
    }
    private void Update()
    {

        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetText(TextType.设定危险区域);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetText(TextType.设置目标采样点);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetText(TextType.选择机器人);
        }
    
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RobotManager.Instance.navCtrls[0].SetTempTargetPoints(new List<Vector3>() { new Vector3(-4, 0, -1), new Vector3(-4, 0, 3) }); 
        }
        */
    }

    private void Start()
    {
        NextStage();
    }

    public void GameLose()
    {
        CountdownTimer.Instance. PauseTimer();
       losePanel.SetActive(true);
        RobotManager.Instance.StopNav();
    }

    public void GameWin()
    {
        CountdownTimer.Instance. PauseTimer();
       winPanel.SetActive(true);
        RobotManager.Instance.StopNav();
    }
}
