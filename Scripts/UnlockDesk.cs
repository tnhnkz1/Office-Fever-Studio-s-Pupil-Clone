using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UnlockDesk : MonoBehaviour
{
    [SerializeField] private GameObject unlockProgress;
    [SerializeField] private GameObject newDesk;

    [SerializeField] private Image progressBar;

    [SerializeField] private TextMeshProUGUI moneyAmount;

    [SerializeField] private int deskPrice, deskRemainPrice;

    [SerializeField] private float ProgressValue;

    void Start()
    {
        deskRemainPrice = deskPrice;
    }

    private void Update()
    {
        if (deskRemainPrice == 0 || progressBar.fillAmount >= 1)
        {
            GameObject desk = Instantiate(newDesk, new Vector3(transform.position.x, -0.155f, transform.position.z)
                , Quaternion.Euler(0f, 180f, 0f));

            desk.transform.DOScale(1f, 1f).SetEase(Ease.OutElastic);

            unlockProgress.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && PlayerPrefs.GetInt("Money") > 0)
        {
            ProgressValue = Mathf.Abs(1f - CalculateMoney() / deskPrice);

            if (PlayerPrefs.GetInt("Money") >= deskPrice)
            {
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - deskPrice);

                deskRemainPrice = 0;
            }
            else
            {
                deskRemainPrice -= PlayerPrefs.GetInt("Money");
                PlayerPrefs.SetInt("Money", 0);
            }

            progressBar.DOFillAmount(ProgressValue, 1f);

            PlayerManager.Instance.moneyCounter.text = PlayerPrefs.GetInt("Money").ToString("0");

            moneyAmount.text = deskRemainPrice.ToString("0");
        }
    }

    private float CalculateMoney()
    {
        return deskRemainPrice - PlayerPrefs.GetInt("Money");
    }
}



