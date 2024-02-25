using System.Collections;

using UnityEngine;


public class ChangeFloodPosition : MonoBehaviour
{
    public float delayTime;

    public Vector3 posA;
    public Vector3 posB;

    private bool move;

    public void StartMoving()
    {
        StartCoroutine(WaitAndMove(delayTime));
    }

    IEnumerator WaitAndMove(float delayTime)
    {
        float totalMovementTime = 5f; //the amount of time you want the movement to take
        
        float currentMovementTime = 0f;//The amount of time that has passed
        
        while (Vector3.Distance(transform.localPosition, posB) > 0)
        {
            currentMovementTime += Time.deltaTime;
            
            transform.localPosition = Vector3.Lerp(posA, posB, currentMovementTime / totalMovementTime);
            
            yield return null;
        }
    }

    //public void WaitAndMove(float delayTime)
    //{
    //    float totalMovementTime = 5f; //the amount of time you want the movement to take
    //    float currentMovementTime = 0f;//The amount of time that has passed
    //    while (Vector3.Distance(transform.localPosition, posB) > 0)
    //    {
    //        currentMovementTime += Time.deltaTime;
    //        transform.localPosition = Vector3.Lerp(posA, posB, currentMovementTime / totalMovementTime);
    //        //yield return null;
    //    }
    //}

    // Update is called once per frame

}
