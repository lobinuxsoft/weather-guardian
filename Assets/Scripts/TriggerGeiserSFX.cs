using UnityEngine;

public class TriggerGeiserSFX : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference geiserSFX;

    [SerializeField] private FMODUnity.EventReference rockSFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (!geiserSFX.IsNull)
                FMODUnity.RuntimeManager.PlayOneShot(geiserSFX);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (!rockSFX.IsNull)
                FMODUnity.RuntimeManager.PlayOneShot(rockSFX);
        }
    }
}
