using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificObject : SaveableObject
{
    public string sceneName;
    public string anchorID;
    public string modelName;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
       /* GameObject gm = null;
        gm.GetComponent<SaveModelManager>().Save();*/


    }
    //string anchorid
    public override void Save(int anchorid)
    {
        base.Save(anchorid);
    }
    public override void Load(string[] values)
    {
        base.Load(values);
    }
}
