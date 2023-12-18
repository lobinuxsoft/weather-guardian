using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodManager : MonoBehaviour
{
    [SerializeField] List<ChangeFloodPosition> changeFloods = new List<ChangeFloodPosition>();

    public void MoveFloods()
    {
        foreach (ChangeFloodPosition flood in changeFloods)
        {
            flood.StartMoving();
        }
    }
}
