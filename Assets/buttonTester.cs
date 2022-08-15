using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonTester : MonoBehaviour
{
     List<string> skill = new List<string>();

    void Start()
    {
        
    }

    public void OnClickButton()
    {
        skill.Add("인내심");
        foreach (string s in skill)
        {
            if (s == "인내심")
            {
                Debug.Log("인내심 이라는 스킬을 가지고 있습니다.");
            }
        }
    }
    

    void Update()
    {
        
    }
}
