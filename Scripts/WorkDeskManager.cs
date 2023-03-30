using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json.Converters;

public class WorkDeskManager : MonoBehaviour
{
    [SerializeField] private Transform moneyPoint;

    [SerializeField] private GameObject money;

    List<GameObject> moneyList = new List<GameObject>();

    public List<GameObject> paperList = new List<GameObject>();

    private float yAxis = -0.11f;

    int stackCount = 10;

    bool isGiving;

    bool hasGivenMoney;

    [SerializeField] GameObject paperPoint;

    private bool moneyPointIsFull;

    private void Update()
    {
        if (hasGivenMoney && !moneyPointIsFull)
        {
            var moneyPointIndex = 0;
            
            GameObject NewMoney = Instantiate(money, new Vector3(moneyPoint.GetChild(moneyPointIndex).position.x,
                    yAxis, moneyPoint.GetChild(moneyPointIndex).position.z),
                moneyPoint.GetChild(moneyPointIndex).rotation);

            NewMoney.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.OutElastic);
            NewMoney.transform.DOLocalRotate(new Vector3(90, 0, 0), 0.1f);

            NewMoney.transform.SetParent(moneyPoint.GetChild(0).transform);

            if (moneyPointIndex < moneyPoint.childCount - 1)
            {
                moneyPointIndex++;
            }
            else
            {
                moneyPointIndex = 0;
                yAxis += 0.05f;
            }

            hasGivenMoney = false;
        }

        if (moneyPoint.GetChild(0).childCount <= 2)
        {
            yAxis = -0.1f;
        }
    }

    public void Work()
    {
        InvokeRepeating("GivePapers", 1f, 1f);
    }

    void GivePapers()
    {
        if (transform.childCount > 0 && moneyPoint.GetChild(0).childCount <= 10)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject, 0.2f);
            hasGivenMoney = true;
        }
        else
        {
            yAxis = -0.1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Work();
        }
    }
}
