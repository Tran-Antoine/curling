using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTREFLECT : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<ReflectionProbe>().RenderProbe();
        
    }
}
