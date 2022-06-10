using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.Azure.SpatialAnchors.Unity.Examples;
public class SceneName : MonoBehaviour
{
    // Start is called before the first frame update


    public InputField Inpf;


    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => AzureSpatialAnchorsNearbyDemoScript.getSceneName(Inpf.text));

    }



}

