using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectManager : MonoBehaviour
{
    public List<GameObject> paperList = new List<GameObject>();

    public GameObject paperPrefab;
    public GameObject Player;
    public GameObject Inside;
    public GameObject closingTheDoor;
    public GameObject closingTheDoor2;
    public GameObject Inside_2;
    public GameObject closingTheDoor_3;
    public GameObject closingTheDoor_4;

    public Transform collectionPoint;
    public Transform M_Door;
    public Transform H_Door;
    
    public static PrinterManager printerManager;
    public static WorkerManager workerManager;
    public static BuyArea areatoBuy;
    

    int paperLimit = 9;

    public int moneyCount = 0;

    int Money = 0;

    bool isCollecting, isGiving, isOpen, isClose;

    public TMPro.TextMeshProUGUI Money_TMP;

    void Start()
    {
        StartCoroutine(CollectionNumerator());
    }

    void IncreaseMoney()
    {
        moneyCount += 5;
    }

    void BuyArea()
    {
        if (CollectManager.areatoBuy != null)
        {
            if (moneyCount >= 1)
            {
                CollectManager.areatoBuy.Buy(1);
                moneyCount -= 1;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CollectionArea"))
        {
            isCollecting = true;
            printerManager = other.gameObject.GetComponent<PrinterManager>();
        }

        if (other.gameObject.CompareTag("WorkArea"))
        {
            isGiving = true;
            workerManager = other.gameObject.GetComponent<WorkerManager>();
        }

        if (other.gameObject.CompareTag("BuyArea"))
        {
            areatoBuy = other.gameObject.GetComponent<BuyArea>();
            BuyArea();
            CollectManager.areatoBuy.Buy(0);
        }
        
        if (other.gameObject.CompareTag("Door"))
        {
            H_Door.DORotate(new Vector3(0, -15, 0), 0.3f);
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            isOpen = true;
            closingTheDoor2.SetActive(true);
        }

        if (other.gameObject.CompareTag("Door_2"))
        {
            M_Door.DORotate(new Vector3(0, -15, 0), 0.3f);
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            isOpen = true;
            closingTheDoor_4.SetActive(true);
        }

        if (other.gameObject.CompareTag("Close"))
        {
            H_Door.DORotate(new Vector3(0, 180, 0), 0.3f);
        }

        if (other.gameObject.CompareTag("Close_2"))
        {
            H_Door.DORotate(new Vector3(0, 90, 0), 0.3f);
            closingTheDoor2.SetActive(false);
            H_Door.GetComponent<BoxCollider>().enabled = true;
        }

        if (other.gameObject.CompareTag("Close_3"))
        {
            M_Door.DORotate(new Vector3(0, 180, 0), 0.3f);
        }

        if (other.gameObject.CompareTag("Close_4"))
        {
            M_Door.DORotate(new Vector3(0, 90, 0), 0.3f);
            closingTheDoor_4.SetActive(false);
            M_Door.GetComponent<BoxCollider>().enabled = true;
        }

        if (other.gameObject.CompareTag("Inside"))
        {
            H_Door.DORotate(new Vector3(0, 90, 0), 1.5f);
            H_Door.tag = "OpenTheDoor";
            closingTheDoor2.SetActive(false);
        }

        if (other.gameObject.CompareTag("Inside2"))
        {
            M_Door.DORotate(new Vector3(0, 90, 0), 1.5f);
            M_Door.tag = "OpenTheDoor_2";
            closingTheDoor_4.SetActive(false);
        }

        if (other.gameObject.CompareTag("OpenTheDoor"))
        {
            H_Door.DORotate(new Vector3(0, 180, 0), 1.5f);
            closingTheDoor.SetActive(true);
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        if (other.gameObject.CompareTag("OpenTheDoor_2"))
        {
            M_Door.DORotate(new Vector3(0, 180, 0), 1.5f);
            closingTheDoor_3.SetActive(true);
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CollectionArea"))
        {
            isCollecting = false;
            printerManager = null;
        }

        if (other.gameObject.CompareTag("WorkArea"))
        {
            isGiving = false;
            workerManager = null;
        }

        if (other.gameObject.CompareTag("BuyArea"))
        {
            areatoBuy = null;
        }

        if (other.gameObject.CompareTag("Inside"))
        {
            H_Door.GetComponent<BoxCollider>().enabled = true;
        }

        if (other.gameObject.CompareTag("Inside2"))
        {
            M_Door.GetComponent<BoxCollider>().enabled = true;
        }

        if (other.gameObject.CompareTag("Close"))
        {
            H_Door.DORotate(new Vector3(0, 90, 0), 1.5f);
            closingTheDoor.SetActive(false);
            H_Door.tag = "Door";
            H_Door.GetComponent<BoxCollider>().enabled = true;
        }

        if (other.gameObject.CompareTag("Close_3"))
        {
            M_Door.DORotate(new Vector3(0, 90, 0), 1.5f);
            closingTheDoor_3.SetActive(false);
            M_Door.tag = "Door_2";
            M_Door.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Money"))
        {
            IncreaseMoney();
            Destroy(other.gameObject);
            Handheld.Vibrate();

            Money += 5;
            Money_TMP.text = "" + moneyCount;
        }
    }

    public void RemoveLast()
    {
        if (paperList.Count > 0)
        {
            Destroy(paperList[paperList.Count - 1]);
            paperList.RemoveAt(paperList.Count - 1);
        }
    }  

    public void GivePaper()
    {
       if (paperList.Count > 0)
        {
            CollectManager.workerManager.GetPaper();
            RemoveLast();
        }
    }

    IEnumerator CollectionNumerator()
    {
        while (true)
        {
            if (paperList.Count <= paperLimit && isCollecting == true)
            {
                GameObject temp = Instantiate(paperPrefab, collectionPoint);
                temp.transform.position = new Vector3(collectionPoint.position.x, ((float)paperList.Count / 20) + 1, collectionPoint.position.z);
                paperList.Add(temp);
                Handheld.Vibrate();

                if (CollectManager.printerManager != null)
                {
                    CollectManager.printerManager.RemoveLast();
                }
            }

            yield return new WaitForSeconds(0.01f);

            if (isGiving == true)
            {
                GivePaper();
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    /*void GetPaper()
    {
        if (paperList.Count <= paperLimit)
        {
            GameObject temp = Instantiate(paperPrefab, collectionPoint);
            temp.transform.position = new Vector3(collectionPoint.position.x, ((float)paperList.Count / 20) + 1, collectionPoint.position.z);
            paperList.Add(temp);
        }
    }*/
}
