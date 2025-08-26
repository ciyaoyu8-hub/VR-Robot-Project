using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    #region 单例
    // 单例模式实现
    private static CountdownTimer instance;

    public static CountdownTimer Instance
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
public float countdownTime = 300f; // 5分钟 = 300秒
    public Text timerText; // UI文本组件
    private float currentTime;
    private bool isRunning = false;

    void Start()
    {
        ResetTimer();
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (currentTime <= 0)
            {
                currentTime = 0;
                isRunning = false;
                TimerFinished();
            }
        }
    }

    // 更新UI显示
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // 开始倒计时
    public void StartTimer()
    {
        isRunning = true;
    }

    // 暂停倒计时
    public void PauseTimer()
    {
        isRunning = false;
    }

    // 重置倒计时
    public void ResetTimer()
    {
        currentTime = countdownTime;
        UpdateTimerDisplay();
        isRunning = false;
    }

    // 倒计时结束回调
    void TimerFinished()
    {
        GameManager.Instance.GameLose();
    }
}