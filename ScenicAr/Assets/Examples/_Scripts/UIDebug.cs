using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDebug : MonoBehaviour
{
    private static UIDebug instance;
    public static UIDebug Instance { get { return instance; } }
    [SerializeField]
    private TMP_Text debugText;
    private string text;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        debugText.text = text;
    }

    public void Set(string text)
    {
        this.text = text;
    }
    public void Append(string text)
    {
        this.text += text;
    }
}
