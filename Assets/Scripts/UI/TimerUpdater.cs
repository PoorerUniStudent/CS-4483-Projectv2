using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class TimerUpdater : MonoBehaviour
{
    private TextMeshProUGUI stageTimer;
    private TextMeshProUGUI bestTime;

    const string STAGE_TIMER_NAME = "StageTimer";
    const string BEST_STAGE_TIME_NAME = "BestStageTime";
    void Start()
    {
        stageTimer = transform.Find(STAGE_TIMER_NAME).GetComponent<TextMeshProUGUI>();
        bestTime = transform.Find(BEST_STAGE_TIME_NAME).GetComponent<TextMeshProUGUI>();

        UpdateBestTime();
    }

    void Update()
    {
        UpdateStageTimer();
    }

    void UpdateStageTimer()
    {
        float time = GameManager.instance.stageTime;
        int timeInMinutes = Mathf.Clamp(Mathf.FloorToInt(time / 60f), 0, 999);
        int timeInSeconds = Mathf.Clamp(Mathf.FloorToInt(time % 60), 0, 59);

        stageTimer.text = "Stage Time: ";

        if (timeInSeconds < 10)
        {
            stageTimer.text += timeInMinutes.ToString() + ":0" + timeInSeconds.ToString();
        }
        else
        {
            stageTimer.text += timeInMinutes.ToString() + ":" + timeInSeconds.ToString();
        }
    }

    void UpdateBestTime()
    {
        float time = GameManager.instance.GetBestStageTime();
        int timeInMinutes = Mathf.Clamp(Mathf.FloorToInt(time / 60f), 0, 999);
        int timeInSeconds = Mathf.Clamp(Mathf.FloorToInt(time % 60), 0, 59);

        bestTime.text = "Best Stage Time: ";

        if (time == 0f)
        {
            bestTime.text += "Did Not Pass";
            return;
        }

        if (timeInSeconds < 10)
        {
            bestTime.text += timeInMinutes.ToString() + ":0" + timeInSeconds.ToString();
        }
        else
        {
            bestTime.text += timeInMinutes.ToString() + ":" + timeInSeconds.ToString();
        }
    }
}
