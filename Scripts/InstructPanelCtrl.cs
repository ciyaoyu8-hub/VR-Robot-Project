using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructPanelCtrl : MonoBehaviour
{
    #region 单例
    // 单例模式实现
    private static InstructPanelCtrl instance;

    public static InstructPanelCtrl Instance
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
public Button Next;
    public Button Lock;
   public ToggleGroup RobotIToggleGroup;
    List<int> robotIDs = new List<int>();
    [HideInInspector] public bool isNext;
    private void Start()
    {
        Next.onClick.AddListener(() =>
        {
            GameManager.Instance.NextStage();
            if (robotIDs.Count==0)
            {
                return;
            }
            foreach (var item in robotIDs)
            {

                RobotManager.Instance.SetPoint(item, RobotManager.Instance.navCtrls[item].CurrentTargetPoints);
            }
            isNext = true;
            robotIDs.Clear();
        });
        Lock.onClick.AddListener(()=>
        {
            Toggle toggle = null;
            foreach (var item in RobotIToggleGroup.ActiveToggles())
            {
                toggle = item;
                break;  
            }
            if (toggle==null)
            {
                return;
            }
            int robotID = int.Parse(toggle.name) - 1;
            robotIDs.Add(robotID);
            if (ToggleOrderTracker.Instance.GetSelectionOrder().Count==0)
            {
                return;
            }
            foreach (var item in ToggleOrderTracker.Instance.GetSelectionOrder())
            {
                RobotManager.Instance.navCtrls[robotID].AddTargetPoints(GameManager.Instance.name_TPC[item.name]);

            }
            foreach (var item in ToggleOrderTracker.Instance.GetSelectionOrder())
            {
                item.isOn = false;
                item.interactable = false;
            }
            ToggleOrderTracker.Instance.selectedOrder.Clear();
            toggle.interactable = false;
            bool isAllInteractableFalse = true;
            foreach (Transform item in RobotIToggleGroup.transform)
            {
                if (item.GetComponent<Toggle>().interactable)
                {

                    item.GetComponent<Toggle>().isOn = true;
                    isAllInteractableFalse = false;
                }
            }
            if (isAllInteractableFalse)
            {
                RobotIToggleGroup.allowSwitchOff = true;
                foreach (var item in RobotIToggleGroup.ActiveToggles())
                {
                    item.isOn = false;
                    break;
                }
            }

        });
    }
}
