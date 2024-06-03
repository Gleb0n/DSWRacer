using UnityEngine;
using TMPro;

public class LapTimer : MonoBehaviour
{
    // TODO : add laptime + full race time 
    public TMP_Text lapTimeText; 
    public bool startFromCheckpoint; 

    private float lapTime;
    private bool isRaceStarted;
    private bool isRaceFinished;

    private void Start()
    {
        lapTime = 0f;
        isRaceStarted = false;
        isRaceFinished = false;
        lapTimeText.text = "00:00:000";
    }

    private void Update()
    {
        if (isRaceStarted && !isRaceFinished)
        {
            lapTime += Time.deltaTime;
            UpdateLapTimeUI();
        }
    }

    public void StartRace()
    {
        if (!startFromCheckpoint)
        {
            isRaceStarted = true;
            lapTime = 0f;
        }
    }

    public void StartRaceFromCheckpoint()
    {
        if (startFromCheckpoint)
        {
            isRaceStarted = true;
            lapTime = 0f;
        }
    }

    public void StopRace()
    {
        isRaceFinished = true;
    }

    private void UpdateLapTimeUI()
    {
        int minutes = Mathf.FloorToInt(lapTime / 60F);
        int seconds = Mathf.FloorToInt(lapTime % 60F);
        int milliseconds = Mathf.FloorToInt((lapTime * 1000F) % 1000F);

        lapTimeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
