using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextChange2 : MonoBehaviour
{
    private CutScene2 _cutScene2;

    public TMP_Text Text;
    public TMP_Text Instructions;

    public GameObject ticket;

    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        _cutScene2 = FindObjectOfType<CutScene2>();
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = _cutScene2.speech;
        Instructions.text = _cutScene2.instructions;
        ticket.SetActive(_cutScene2.watch);
        
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
