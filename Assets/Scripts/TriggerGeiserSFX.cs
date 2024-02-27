using UnityEngine;

public class TriggerGeiserSFX : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference geiserSFX;    

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (!geiserSFX.IsNull)
                FMODUnity.RuntimeManager.PlayOneShot(geiserSFX);
        }
    }
}
