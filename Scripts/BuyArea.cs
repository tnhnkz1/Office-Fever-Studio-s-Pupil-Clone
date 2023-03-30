using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyArea : MonoBehaviour
{
    public GameObject Buy_Area, Worker;

    public float cost, currentMoney, progress;

    public TMPro.TextMeshProUGUI Money_TMP;

    float Money = 0;

    void Update()
    {
        //moneyCount /= cost;
        //cost -= currentMoney;
        
    }

    public void Buy(int goldAmount)
    {
        currentMoney += goldAmount;
        progress = currentMoney / cost;
        
        if (progress >= 1)
        {
            Buy_Area.SetActive(false);
            Worker.SetActive(true);
            this.enabled = false;

            //Money /= cost;
            Money_TMP.text = "" + currentMoney;
        }
    }
}
