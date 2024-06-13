using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class TrackCheckpoints : MonoBehaviour
{
    private Transform carTransform;

    private List<CheckpointSingle> listOfCheckpoints;
    private int nextCheckpointSingleIndex;
    public event Action OnCheckpointPassed;
    public event Action OnLastCheckpointPassed;

    private void Awake()
    {
        listOfCheckpoints = new List<CheckpointSingle>();

        carTransform = FindObjectOfType<Car>().transform;

        Transform checkpointsTransform = transform.Find("Checkpoints");
        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoint(this);
            listOfCheckpoints.Add(checkpointSingle);
        }
        nextCheckpointSingleIndex = 0;

    }
    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform)
    {

        if (listOfCheckpoints.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % listOfCheckpoints.Count;
            OnCheckpointPassed?.Invoke();
        }
        else
        {
            if (nextCheckpointSingleIndex == 0)
            {
                carTransform.position = listOfCheckpoints[0].transform.position;
                carTransform.rotation = listOfCheckpoints[0].transform.rotation;
            }
            else
            {
                carTransform.position = listOfCheckpoints[nextCheckpointSingleIndex - 1].transform.position;
                carTransform.rotation = listOfCheckpoints[nextCheckpointSingleIndex - 1].transform.rotation;
            }
        }

        CheckpointSingle lastCheckpoint = listOfCheckpoints.Last();
        if (nextCheckpointSingleIndex == listOfCheckpoints.IndexOf(lastCheckpoint))
        {
            OnLastCheckpointPassed?.Invoke();
        }

    }

    public List<CheckpointSingle> GetCheckpoints()
    {
        return listOfCheckpoints;
    }
    public static float CalculateAngle(CheckpointSingle currentCheckpoint, CheckpointSingle nextCheckpoint)
    {
        Transform currentCheckpointTransform = currentCheckpoint.gameObject.transform;
        Transform nextCheckpointTransform = nextCheckpoint.gameObject.transform;
        Vector3 nextCheckpointDirection = currentCheckpointTransform.position - nextCheckpointTransform.position;

        float dotProduct = Vector3.Dot(currentCheckpointTransform.position, nextCheckpointDirection);
        float angleBetweenCheckpoints = Mathf.Acos(dotProduct / (currentCheckpointTransform.forward.magnitude * nextCheckpointDirection.magnitude));
        return angleBetweenCheckpoints * Mathf.Rad2Deg;
    }
}
