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
        skill.Add("�γ���");
        foreach (string s in skill)
        {
            if (s == "�γ���")
            {
                Debug.Log("�γ��� �̶�� ��ų�� ������ �ֽ��ϴ�.");
            }
        }
    }
    

    void Update()
    {
        
    }
}
