/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public delegate void OnCollectionArea();
    public static event OnCollectionArea OnPaperCollection;

    public static TriggerManager triggerManager;
    public static CollectManager collectManager;

    bool isCollecting; 

    void Start()
    {
        StartCoroutine(CollectionNumerator());
    }

    IEnumerator CollectionNumerator()
    {
        while (true)
        {
            if (isCollecting == true && TriggerManager.triggerManager != null && TriggerManager.collectManager != null && CollectManager.triggerManager != null)
            {
                OnPaperCollection();
            }
        }
        yield return new WaitForSeconds(1);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CollectionArea"))
        {
            isCollecting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CollectionArea"))
        {
            isCollecting = false;
        }
    }
}*/
