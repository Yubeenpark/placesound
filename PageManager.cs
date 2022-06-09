using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public GameObject backPage, backPage1, backPage2;
    public GameObject nextpage, nextpage1, nextpage2;

    public Button nextbtn, backbtn, nextbtn1, backbtn1, nextbtn2, backbtn2;
    void Start()
    {
        nextpage.SetActive(false);
        nextpage1.SetActive(false);
        nextpage2.SetActive(false);

    }
    void Update()
    {

        nextbtn.onClick.AddListener(() => {
            nextpage.SetActive(true);
            backPage.SetActive(false);
        });

        backbtn.onClick.AddListener(() => {
            backPage.SetActive(true);
            nextpage.SetActive(false);
        });

        nextbtn1.onClick.AddListener(() => {
            nextpage1.SetActive(true);
            backPage1.SetActive(false);
        });

        backbtn1.onClick.AddListener(() => {
            backPage1.SetActive(true);
            nextpage1.SetActive(false);
        });

        nextbtn2.onClick.AddListener(() => {
            nextpage2.SetActive(true);
            backPage2.SetActive(false);
        });

        backbtn2.onClick.AddListener(() => {
            backPage2.SetActive(true);
            nextpage2.SetActive(false);
        });


    }
}
