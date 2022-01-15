using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextChanger : MonoBehaviour
{
    private CutScene4 generator;

    public TMP_Text Text;

    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        generator = FindObjectOfType<CutScene4>();
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = generator.speech;
        if (Text.text != "")
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }
}
