using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Printer : MonoBehaviour
{
    [SerializeField] private Transform[] PapersPlace = new Transform[18];

    [SerializeField] private GameObject paper;

    private List<GameObject> paperList = new List<GameObject>();

    public float PaperDeliveryTime, yAxis;

    public int countPapers;

    private IEnumerator printPaper;

    private bool isWorking;

    public GameObject papersParent;

    int stackCount = 3;

    void Start()
    {
        for (int i = 0; i < PapersPlace.Length; i++)
        {
            PapersPlace[i] = transform.GetChild(0).GetChild(i);
        }

        printPaper = PrintPaper(PaperDeliveryTime);

        StartCoroutine(printPaper);
    }

    public IEnumerator PrintPaper(float Time)
    {
        var PP_index = 0;

        while (true)
        {
            int paperCount = paperList.Count;
            int rowCount = (int)paperCount / stackCount;

            if (papersParent.transform.childCount < 36)
            {
                isWorking = true;
            }
            else if (papersParent.transform.childCount == 36)
            {
                isWorking = false;
            }

            if (isWorking == true)
            {
                GameObject NewPaper = Instantiate(paper, new Vector3(transform.position.x, 0.5f, transform.position.z),
                Quaternion.identity, transform.GetChild(1));

                NewPaper.transform.DOMove(new Vector3(PapersPlace[PP_index].position.x + ((float)rowCount / 6), PapersPlace[PP_index].position.y + (paperCount % stackCount) + yAxis,
                    PapersPlace[PP_index].position.z), 0.6f);

                if (PP_index < 17)
                    PP_index++;
                else
                {
                    PP_index = 0;
                    yAxis += 0.08f;
                }
            }

            yield return new WaitForSecondsRealtime(Time);
        }
    }
}
