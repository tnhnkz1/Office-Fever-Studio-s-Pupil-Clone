using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PrinterManager : MonoBehaviour
{
    public List<GameObject> paperList = new List<GameObject>();
    public GameObject paperPrefab;
    public Transform exitPoint;

    bool isWorking;

    int stackCount = 10;

    void Start()
    {
        StartCoroutine(PrintPaper());
    }

    public void RemoveLast()
    {
        if (paperList.Count > 0)
        {
            Destroy(paperList[paperList.Count - 1]);
            paperList.RemoveAt(paperList.Count - 1);
        }
    }

    IEnumerator PrintPaper()
    {
        while (true)
        {
            float paperCount = paperList.Count;
            int rowCount = (int)paperCount / stackCount;

            if (isWorking == true)
            {
                GameObject temp = Instantiate(paperPrefab);
                temp.transform.position = new Vector3(exitPoint.position.x + ((float)rowCount / 4), (paperCount % stackCount) / 55 + -0.145f, exitPoint.position.z);
                paperList.Add(temp);

                if (paperList.Count >= 100)
                {
                    isWorking = false;
                }
            }
            else if (paperList.Count < 100)
            {
                isWorking = true;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}