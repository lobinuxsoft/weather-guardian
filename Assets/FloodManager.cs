using System.Collections.Generic;

using UnityEngine;

public class FloodManager : MonoBehaviour
{
    [SerializeField] List<ChangeFloodPosition> changeFloods = new List<ChangeFloodPosition>();

    [SerializeField] private FMODUnity.EventReference floodStartSFX;

    [SerializeField] private FMODUnity.EventReference floodStopSFX;

    int amountOfFloodsThatStop;

    private void Start()
    {
        amountOfFloodsThatStop = 0;

        if (changeFloods != null) 
        {
            for (short i = 0; i < changeFloods.Count; i++) 
            {
                changeFloods[i].onTargetPositionAchieved += StopFloodsSfx;
            }
        }
    }

    private void OnDestroy()
    {
        if (changeFloods != null)
        {
            for (short i = 0; i < changeFloods.Count; i++)
            {
                changeFloods[i].onTargetPositionAchieved -= StopFloodsSfx;
            }
        }
    }

    public void MoveFloods()
    {
        if (!floodStartSFX.IsNull)
            FMODUnity.RuntimeManager.PlayOneShot(floodStartSFX);

        foreach (ChangeFloodPosition flood in changeFloods)
        {
            flood.StartMoving();
        }
    }

    void StopFloodsSfx() 
    {
        amountOfFloodsThatStop++;

        if (amountOfFloodsThatStop >= changeFloods.Count) 
        {
            if (!floodStopSFX.IsNull)
                FMODUnity.RuntimeManager.PlayOneShot(floodStopSFX);

            amountOfFloodsThatStop = 0;
        }
    }
}
