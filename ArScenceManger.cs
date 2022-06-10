using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArScenceManger : MonoBehaviour
{

    public void GotoMain()
    {
        SceneManager.LoadScene("intro", LoadSceneMode.Single);
    }

    public void GotoScence(string scenceName)
    {
        SceneManager.LoadScene(scenceName, LoadSceneMode.Single);
    }
}
