using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class makemodel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject snow = Resources.Load<GameObject>("prefabs/snow");
        GameObject fire = Resources.Load<GameObject>("prefabs/fireblule");
        AudioSource snowaudio;

        AudioSource source = gameObject.GetComponent<AudioSource>();
   

        source.Play();
        //snow.AddComponent(AudioSource source);
        //snow.AddComponent();

        //GameObject object1 = gameObject.Find("object1");
        //AudioSource audioSource = object1.AddComponent<AudioSource>();
        //audioSource.clip = Resources.Load(sound_name);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public makemodel(string models)
    {
        if(models == "snow")
        {

        }
    }
}
