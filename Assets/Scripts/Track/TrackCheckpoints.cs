using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class TrackCheckpoints : MonoBehaviour
{
    private List<Transform> carTransformList = new List<Transform>();

    private List<CheckpointSingle> listOfCheckpoints;
    private List<int> nextCheckpointSingleIndexList;

    public event Action OnCheckpointPassed;
    public event Action OnLastCheckpointPassed;

    private void Awake()
    {
        listOfCheckpoints = new List<CheckpointSingle>();

        FindObjectsOfType<CarController>().ToList().ForEach(car =>
        {
            carTransformList.Add(car.GetComponent<Transform>());
        });

        Transform checkpointsTransform = transform.Find("Checkpoints");
        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoint(this);
            listOfCheckpoints.Add(checkpointSingle);
        }

        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);     
        }


    }
    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform)
    {

        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        int currentCheckpointSingleIndex = nextCheckpointSingleIndex - 1;
        if (listOfCheckpoints.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)] = (nextCheckpointSingleIndex + 1) % listOfCheckpoints.Count;
            OnCheckpointPassed?.Invoke();
        }
        else
        {
            if (currentCheckpointSingleIndex >= 0)
            {
                carTransform.position = listOfCheckpoints[currentCheckpointSingleIndex].transform.position;
                carTransform.rotation = listOfCheckpoints[currentCheckpointSingleIndex].transform.rotation;
            }
            else
            {
                carTransform.position = listOfCheckpoints[0].transform.position;
                carTransform.rotation = listOfCheckpoints[0].transform.rotation;
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
