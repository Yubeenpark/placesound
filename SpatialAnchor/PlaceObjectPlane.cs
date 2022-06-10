using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]

public class PlaceObjectPlane : MonoBehaviour
{

    private ARPlaneManager m_ARPlanceManager;

    private ARRaycastManager arRaycastManager;

    static List<ARRaycastHit> s_hits = new List<ARRaycastHit>();

    private bool _planesFound;

    private int _objPlacedIndex;

    public GameObject musicpage, freepage;
    public GameObject sample;
    private bool modelalready=false;
    public GameObject UIpanel;
    public GameObject toggelButton;
    int modelnum = 0;
    private void Awake()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        if (arRaycastManager == null)
        {
            Debug.Log("Missing ARRaycastManager in scene");
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (!_planesFound)
        {
            ScanPlances();
        }
        
        else
        {
            if (Havemodel(modelalready))
                MakeModel(sample);

        }
        
    }
    public static void getObject(GameObject model)
    {


    }
    public void Toggle()
    {
        UIpanel = new GameObject();
        UIpanel = EventSystem.current.currentSelectedGameObject;
        UIpanel.SetActive(!UIpanel.activeSelf);
        var togglebuttonText = toggelButton.gameObject.GetComponent<TextMeshProUGUI>();
        togglebuttonText.text = UIpanel.activeSelf ? "UI OFF" : "UI ON";

    }

    private void ScanPlances()
    {
        if (m_ARPlanceManager.trackables.count > 0)
            _planesFound = true;
    }

    /*
    private void PlaceObject()
    {


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (m_Raycastmanager.Raycast(touch.position, s_hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_hits[0].pose;

                    SetVisualElement(hitPose);
                }
            }
        }

    }

    private void SetVisualElement(Pose hitPose)
    {
        if (_objPlacedIndex < visualElements.Count)
        {
            Instantiate(visualElements[_objPlacedIndex], hitPose.position, hitPose.rotation);
        }
        else
        {
            HidePlanes();
        }
        _objPlacedIndex++;
    }

   */

    private void HidePlanes()
    {
        foreach(var plane in m_ARPlanceManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
    public bool Havemodel(bool have)
    {
        if (have)
        {
            modelalready = true;
            return true;
        }
        else
        {
            modelalready = false;
            return false;
        }
    }

    public void MakeModel(GameObject model)
    {
        try
        {

               sample = model;
            Debug.Log("Makemodel로 넘어오기 성공");
            //Instantiate(model, Vector3.zero, Quaternion.identity);
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    var touchPosition = touch.position;

                    bool isOverUI = touchPosition.IsPointOverUIObject();

                    if (isOverUI)
                    {
                        Debug.Log("레이캐스트에 블락당함.");
                    }

                 
                 
                    Debug.Log("터치함");
                    if (!isOverUI && arRaycastManager.Raycast(touchPosition, s_hits, TrackableType.PlaneWithinPolygon))
                    {
                        Debug.Log("Touch 함");
                        Pose hitPose = s_hits[0].pose;
                        
                      
                        //Instantiate(model, hitPose.position, hitPose.rotation);
                        Debug.Log("생성함");
                        model.name = "model" + modelnum;
                        modelnum++;
                        //makeElements(hitPose,model);
                    }
                }
                if (touch.phase == TouchPhase.Stationary) Debug.Log("터치 Stationary!");

            }
            else
            {
            
            }
            
        }
        catch(Exception e)
        {
            Debug.Log("에러남: " + e);
        }
    
    }


    /*private void makeElements(Pose hitPose, GameObject model)
    {
        if (_objPlacedIndex < snowElements.Count)
        {
            Instantiate(snowElements[_objPlacedIndex], hitPose.position, hitPose.rotation);
        }
        else
        {
            HidePlanes();
        }
        _objPlacedIndex++;
    }
    */

}
