using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateByCode : MonoBehaviour
{
    struct SDataA
    {
        public int A;
        public float B;
        public string C;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("zzz 这个脚本是通过代码AddComponent直接创建的");
        TestAOTGeneric();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestAOTGeneric()
    {
        var dict = new Dictionary<string, SDataA>();
        dict["123"] = new SDataA() {A = 1, B = 2, C = "123"};
        Debug.Log($"AOT泛型补充元数据机制测试正常, A=1=={dict["123"].A}, B=2=={dict["123"].B}, C='123'=={dict["123"].C}");
    }
}
