using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; // 用于DateTime

public class ToggleOrderTracker : MonoBehaviour
{
    #region 单例
    // 单例模式实现
    private static ToggleOrderTracker instance;

    public static ToggleOrderTracker Instance
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
[HideInInspector]  public List<Toggle> toggles;
    [HideInInspector] public List<Toggle> selectedOrder = new List<Toggle>();
    private Dictionary<Toggle, DateTime> selectionTime = new Dictionary<Toggle, DateTime>();

  public  void InitData()
    {
        // 初始化事件监听
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(toggle, isOn));

            // 处理初始已勾选状态
            if (toggle.isOn)
            {
                RecordToggleSelection(toggle);
            }
        }
    }

    private void OnToggleChanged(Toggle changedToggle, bool isOn)
    {
        if (isOn)
        {
            RecordToggleSelection(changedToggle);
        }
        else
        {
            RemoveToggleSelection(changedToggle);
        }
    }

    private void RecordToggleSelection(Toggle toggle)
    {
        // 如果之前已记录，先移除旧记录
        if (selectionTime.ContainsKey(toggle))
        {
            selectedOrder.Remove(toggle);
        }

        DateTime timestamp = DateTime.Now;
        selectionTime[toggle] = timestamp;
        selectedOrder.Add(toggle);
    }

    private void RemoveToggleSelection(Toggle toggle)
    {
        if (selectionTime.ContainsKey(toggle))
        {
            selectedOrder.Remove(toggle);
            selectionTime.Remove(toggle);
        }
    }

    // 获取当前勾选顺序（按选择时间排序）
    public List<Toggle> GetSelectionOrder()
    {
        return new List<Toggle>(selectedOrder); // 返回副本
    }

}