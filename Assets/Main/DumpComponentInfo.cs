using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpComponentInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var allcomps = gameObject.GetComponents(typeof(MonoBehaviour));
        foreach (var comp in allcomps)
        {
            var typeName = comp == null ? "none" : comp.GetType().Name;
            Debug.Log($"{gameObject.name} has Component={typeName}");
        }
    }
}
