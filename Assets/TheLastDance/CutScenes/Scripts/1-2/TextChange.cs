using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextChange : MonoBehaviour
{
    private CutSceneGenerator _generator;

    [SerializeField]private TMP_Text text;

    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        _generator = FindObjectOfType<CutSceneGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = _generator.speech;
        
        if (text.text != "")
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }
}
