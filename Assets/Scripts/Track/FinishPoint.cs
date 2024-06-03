using UnityEngine;
using System.Collections;
using System.Threading;

public class FinishPoint : MonoBehaviour
{
    public Transform podium;
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private int numberOfCircles = 1;
    public int currentNumberOfCircles { get; private set; }
    [SerializeField] private LapTimer lapTimer; 

    private void Awake()
    {
        currentNumberOfCircles = numberOfCircles;
        trackCheckpoints.OnLastCheckpointPassed += TrackCheckpoints_OnLastCheckpointPassed;
        this.gameObject.SetActive(false);
    }

    private void TrackCheckpoints_OnLastCheckpointPassed()
    {
        currentNumberOfCircles--;
        if (currentNumberOfCircles == 0)
        {
            this.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarController>(out CarController controller))
        {
            Debug.Log("FINISH!!!");
            DisableControl(controller);
            TeleportToPodium(controller);
            // StartCoroutine(EnableControlAfterDelay(controller, 2.0f)); // delay może nie będzie jechał sam :)

            if (lapTimer != null)
            {
                lapTimer.StopRace(); // Zatrzymaj licznik czasu na końcu wyścigu
            }
        }
    }

    private void DisableControl(CarController controller)
    {
        var playerInput = controller.GetComponent<PlayerInput>();
        var carController = controller.GetComponent<CarController>();
        var smokeEffect = controller.GetComponent<smokeEffect>();
        if (playerInput && carController  != null)
        {
            playerInput.enabled = false;
            carController.enabled = false;
            smokeEffect.enabled = false;
        }
    }

    private void TeleportToPodium(CarController controller)
    {
        if (podium != null)
        {
            //Debug.Log(podium.position + "," + podium.rotation);
            var rb = controller.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // rb.isKinematic = true; // Off fizyki na teleport
            }

            controller.transform.position = podium.position;
            controller.transform.rotation = Quaternion.identity;

            if (rb != null)
            {
                // rb.isKinematic = false; // On fizyki po teleporcie
                rb.constraints = RigidbodyConstraints.FreezeAll;    
            }
        }
    }

    private IEnumerator EnableControlAfterDelay(CarController controller, float delay) //chyba wywalić upewnić się
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("OK");
        if (controller != null)
        {
            var rb = controller.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // On fizyki po teleporcie
                rb.constraints = RigidbodyConstraints.FreezeAll;    
            }
        }
    }
}
