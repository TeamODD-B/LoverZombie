using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : SingletonGeneric<ObjectManager>
{
    //Group
    [SerializeField] private RectTransform _textGroup;
    [SerializeField] private RectTransform _imageGroup;

    //Prefab
    [SerializeField] private GameObject _textPrefab; 
    [SerializeField] private GameObject _imagePrefab;

    //List
    [SerializeField] private List<Text> _textList; 
    [SerializeField] private List<Image> _imageList; 

    protected override void Awake()
    {
        base.Awake();
        _textList = new List<Text>();
        _imageList = new List<Image>();
    }

    // Text
    private Text CreateText()
    {
        GameObject obj = Instantiate(_textPrefab, _textGroup);
        Text text = obj.GetComponent<Text>();
        _textList.Add(text);
        return text;
    }

    public Text GetText()
    {
        for (int i = 0; i < _textList.Count; i++)
        {
            Text text = _textList[i];
            if (!text.gameObject.activeSelf)
            {
                text.gameObject.SetActive(true);
                return text; 
            }
        }

        return CreateText();
    }

    // Image
    private Image CreateImage()
    {
        GameObject obj = Instantiate(_imagePrefab, _imageGroup);
        Image image = obj.GetComponent<Image>();
        _imageList.Add(image);
        return image;
    }

    public Image GetImage() 
    {
        for (int i = 0; i < _imageList.Count; i++)
        {
            Image image = _imageList[i];
            if (!image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(true);
                return image;
            }
        }

        return CreateImage();
    }
}
