using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    public void gotoScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);

    }
}
