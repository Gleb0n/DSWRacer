using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TrackProgressUI : MonoBehaviour
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private FinishPoint finishPoint;
    private List<CheckpointSingle> checkpointSingles;
    private TMP_Text trackProgress;

    private int checkpointsTotal;
    private float checkpointPercent;
    private float currentCheckpointPercent;

    private int numberOfCircles;
    private int passedCircles = 1;
    private void Start()
    {
        numberOfCircles = finishPoint.currentNumberOfCircles;
        trackProgress = GetComponent<TMP_Text>();
        checkpointSingles = trackCheckpoints.GetCheckpoints();

        trackCheckpoints.OnCheckpointPassed += TrackCheckpoints_OnCheckpointPassed;
        trackCheckpoints.OnLastCheckpointPassed += TrackCheckpoints_OnLastCheckpointPassed;

        checkpointsTotal = checkpointSingles.Count * numberOfCircles;
        checkpointPercent = 1f / checkpointsTotal;

        trackProgress.text = "0 %" + "\n 1/" + numberOfCircles.ToString();

    }


    public void TrackCheckpoints_OnCheckpointPassed()
    {
        currentCheckpointPercent += checkpointPercent * 100f;
        trackProgress.text = ((int) currentCheckpointPercent).ToString() + " %" +
                            "\n" + passedCircles + "/" + numberOfCircles;
    }

    private void TrackCheckpoints_OnLastCheckpointPassed()
    {
        passedCircles++;
    }

    private void OnDisable()
    {
        trackCheckpoints.OnCheckpointPassed -= TrackCheckpoints_OnCheckpointPassed;
    }
}
