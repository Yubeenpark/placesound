using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.SpatialAnchors.Unity.Examples;
public class SaveModelManager : MonoBehaviour
{
    private static SaveModelManager instance;
     List<string> anchorids;
    int num = 0;
    public List<SaveableObject> SaveableObjects { get; private set; }

    public static SaveModelManager Instance
    {
        get
        {
            if(instance == null)
           {
                instance = GameObject.FindObjectOfType<SaveModelManager>();
            }
            return instance;
        }
   /*     set
        {
            this.instance = value;
        }*/
    }
    // Start is called before the first frame update
    void Awake()
    {
        SaveableObjects = new List<SaveableObject>();
        Debug.Log("Awake");
    }
    public void Save()
    {
        PlayerPrefs.SetInt("ObjectCount", SaveableObjects.Count);
       /* foreach(string ancrhoid in anchorids)
        {
            SaveableObjects[num].Save(ancrhoid);
            num++;
        }*/
       for(int i=0; i < SaveableObjects.Count; i++)
        {
            SaveableObjects[i].Save(i);
        }
    }

    public void Load(string name)
    {
        foreach(SaveableObject obj in SaveableObjects)
        {
            if(obj!= null)
            {
                Destroy(obj.gameObject);
            }
        }
        SaveableObjects.Clear();

        int objectCount = PlayerPrefs.GetInt("ObjectCount");
        Debug.Log("Load");
        for (int i = 0; i < objectCount; i++)
        {
            string[] value = PlayerPrefs.GetString(i.ToString()).Split('_');
            GameObject tmp = null;
            // switch (value[0])
            tmp = Instantiate(Resources.Load(name) as GameObject);
           // switch (value[0])
            
          
            if (tmp != null)
            {
                tmp.GetComponent<SaveableObject>().Load(value);
            }

           
        
           
            tmp.GetComponent<SaveableObject>().Load(value);
        } 
    }

    public Vector3 StringToVector(string value)
    {
        //(1,2,3,3)
        value = value.Trim(new char[] { '(', ')' });
        // 1,2,3,3,
        value = value.Replace(" ", "");

        string[] pos = value.Split(',');

        //[0]=1, [1]=2


        return new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
    }

    public Quaternion StringToQuaternion(string value)
    {
        return Quaternion.identity;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
