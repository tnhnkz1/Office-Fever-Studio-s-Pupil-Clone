using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public List<GameObject> paperList = new List<GameObject>();
    List<GameObject> moneyList = new List<GameObject>();

    public GameObject paperPrefab;
    public Transform paperPoint;
    public GameObject moneyPrefab;
    public Transform moneyPoint;

    int stackCount = 10;

    bool isGiving;

    void Start()
    {
        StartCoroutine(GenerateMoney());
    }

    IEnumerator GenerateMoney()
    {
        while (true)
        {
            float moneyCount = moneyList.Count;
            int rowCount = (int)moneyCount / stackCount;

            if (isGiving == true && paperList.Count > 0)
            {
                GameObject temp = Instantiate(moneyPrefab);
                temp.transform.position = new Vector3(moneyPoint.position.x + ((float)rowCount / -4), (moneyCount % stackCount) / 30 + -0.15f, moneyPoint.position.z);
                moneyList.Add(temp);
                RemoveLast();
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void GetPaper()
    {
        GameObject temp = Instantiate(paperPrefab);
        temp.transform.position = new Vector3(paperPoint.position.x, ((float)paperList.Count / 20) + 0.767f, paperPoint.position.z);
        paperList.Add(temp);
        isGiving = true;
    }

    public void RemoveLast()
    {
        if (paperList.Count > 0)
        {
            Destroy(paperList[paperList.Count - 1]);
            paperList.RemoveAt(paperList.Count - 1);
        }
    }
}
