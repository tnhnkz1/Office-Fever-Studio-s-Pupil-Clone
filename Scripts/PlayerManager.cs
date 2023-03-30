using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    private Vector3 direction;
    private Camera cam;
    private Animator animator;

    [SerializeField] private float playerSpeed;

    [SerializeField] List<Transform> papers = new List<Transform>();

    [SerializeField] private Transform papersPlace;
    [SerializeField] private Transform paper;
    [SerializeField] private Transform player;
    [SerializeField] private Transform M_Door;
    [SerializeField] private Transform H_Door;
    [SerializeField] private Transform moneyPoint;

    public Transform paperPoint;

    private float yAxis, delay;

    public TextMeshProUGUI moneyCounter;

    [SerializeField] private GameObject Inside;
    [SerializeField] private GameObject closingTheDoor;
    [SerializeField] private GameObject closingTheDoor2;
    [SerializeField] private GameObject Inside_2;
    [SerializeField] private GameObject closingTheDoor_3;
    [SerializeField] private GameObject closingTheDoor_4;
    

    private bool isOpen;
    private bool isTaken;

    public static PlayerManager Instance;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();

        papers.Add(papersPlace);
    }

    void Update()
    {
        if (papers.Count > 1)
        {
            for (int i = 1; i < papers.Count; i++)
            {
                var firstPaper = papers[i - 1];
                var secondPaper = papers[i];

                secondPaper.DOMove(new Vector3(Mathf.Lerp(secondPaper.position.x, firstPaper.position.x, Time.deltaTime * 500f),
                Mathf.Lerp(secondPaper.position.y, firstPaper.position.y + 0.15f, Time.deltaTime * 10f), firstPaper.position.z), 0.0001f);
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1f))
        {
            if (hit.collider.CompareTag("WorkArea") && papers.Count > 1)
            {
                var WorkDesk = hit.collider.transform;

                if (WorkDesk.childCount > 0)
                {
                    yAxis = WorkDesk.GetChild(WorkDesk.childCount - 1).position.y;
                }
                else
                {
                    yAxis = paperPoint.position.y;
                }

                for (var index = papers.Count - 1; index >= 1; index--)
                {
                    papers[index].DOJump(new Vector3(WorkDesk.position.x, yAxis, WorkDesk.position.z), 0.02f, 1, 1)
                        .SetDelay(delay).SetEase(Ease.Flash);

                    papers[index].parent = paperPoint;
                    papers.RemoveAt(index);

                    yAxis += 0.1f;
                    delay += 0.01f;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Money"))
        {
            Destroy(other.gameObject);

            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 5);

            moneyCounter.text = "" + PlayerPrefs.GetInt("Money");

            isTaken = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("CollectionArea") && papers.Count < 11)
        {
            if (collision.gameObject.transform.childCount > 2)
            {
                var paper = collision.gameObject.transform.GetChild(1);
                paper.rotation = Quaternion.Euler(paper.rotation.x, 0, paper.rotation.z);
                papers.Add(paper);
                paper.parent = null;

                if (collision.gameObject.transform.parent.GetComponent<Printer>().countPapers > 1)
                    collision.gameObject.transform.parent.GetComponent<Printer>().countPapers--;

                if (collision.gameObject.transform.parent.GetComponent<Printer>().yAxis > 0f)
                    collision.gameObject.transform.parent.GetComponent<Printer>().yAxis -= 0f;
            }
        }

        if (collision.gameObject.CompareTag("Door"))
        {
            H_Door.DOLocalRotate(new Vector3(0, -65, 0), 0.1f);
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            isOpen = true;
            closingTheDoor.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("OpenTheDoor"))
        {
            H_Door.DOLocalRotate(new Vector3(0, 65, 0), 1f);
            closingTheDoor.SetActive(true);
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        if (collision.gameObject.CompareTag("Door_2"))
        {
            M_Door.DOLocalRotate(new Vector3(0, -65, 0), 0.1f);
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            isOpen = true;
            closingTheDoor_3.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("OpenTheDoor_2"))
        {
            M_Door.DOLocalRotate(new Vector3(0, 65, 0), 1f);
            closingTheDoor_3.SetActive(true);
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Inside"))
        {
            H_Door.DOLocalRotate(new Vector3(0, 0, 0), 1f);
            H_Door.tag = "OpenTheDoor";
            closingTheDoor2.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Inside2"))
        {
            M_Door.DOLocalRotate(new Vector3(0, 0, 0), 1f);
            M_Door.tag = "OpenTheDoor_2";
            closingTheDoor_4.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
            H_Door.DOLocalRotate(new Vector3(0, 0, 0), 1f);
            closingTheDoor.SetActive(false);
            H_Door.tag = "Door";
            H_Door.GetComponent<BoxCollider>().enabled = true;
        }

        if (other.gameObject.CompareTag("Close_3"))
        {
            M_Door.DOLocalRotate(new Vector3(0, 0, 0), 1f);
            closingTheDoor_3.SetActive(false);
            M_Door.tag = "Door_2";
            M_Door.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
