using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Classe simples que controla mensagens na tela
public class ScreenMsg : MonoBehaviour
{
    [SerializeField] Text txt = default;
    [SerializeField] Image panel = default;

    public void DisplayMsg(string msg, Color color)
    {
        panel.gameObject.SetActive(true);
        txt.gameObject.SetActive(true);
        txt.text = msg;
        txt.color = color;
    }
    public void DisplayMsg(string msg)
    {
        DisplayMsg(msg, Color.white);
    }

    public void RemoveMsg()
    {
        panel.gameObject.SetActive(false);
        txt.gameObject.SetActive(false);
    }
}
