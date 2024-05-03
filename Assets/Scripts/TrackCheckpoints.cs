using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    [SerializeField] List<Transform> carTransformList;

    List<CheckpointSingle> listOfCheckpoints;
    List<int> nextCheckpointSingleIndexList;

    private void Awake()
    {
        listOfCheckpoints = new List<CheckpointSingle>();

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
            Debug.Log("Correct");
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
    }



}
