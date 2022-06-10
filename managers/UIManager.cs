using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject intropg;
    public GameObject selectpg, musicpg, makepg;
    public Button plusbutton;
    public Button  backtohome1, bakctoslect, backtomusic,backtointro;
    public Button model1, model2, model3, model4, model5;
    void Start()
    {

        selectpg.SetActive(false);
        musicpg.SetActive(false);
        makepg.SetActive(false);

    }
    public void NextPage()
    {
        plusbutton.onClick.AddListener(() =>
        {
            selectpg.SetActive(true);
            intropg.SetActive(false);
            Debug.Log("+버튼 눌림");
        });

        backtohome1.onClick.AddListener(() =>
        {
            intropg.SetActive(true);
            selectpg.SetActive(false);
        });
        bakctoslect.onClick.AddListener(() =>
        {
            selectpg.SetActive(true);
            musicpg.SetActive(false);
        });
        backtomusic.onClick.AddListener(() =>
        {
            musicpg.SetActive(true);
            makepg.SetActive(false);
        });
        backtointro.onClick.AddListener(() =>
        {
            makepg.SetActive(false);
            intropg.SetActive(true);
            
        });
    }

    public void GotoMusic()
    {
        selectpg.SetActive(false);
        musicpg.SetActive(true);
}
}



