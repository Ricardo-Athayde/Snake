using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.EventManager;

namespace Snake
{
    //Controla os Inputs do jogo e envia mensagens atraves do EventManager
    public class InputController : MonoBehaviour
    {
        [SerializeField] KeyCode leftKeyCode = default;
        [SerializeField] KeyCode rightKeyCode = default;
        [SerializeField] KeyCode upKeyCode = default;
        [SerializeField] KeyCode downKeyCode = default;
        [SerializeField] KeyCode specialActionKeyCode = default;

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(leftKeyCode))
            {
                EventManager.BroadcastEvent("MovementKeyPressed", new Hashtable() { { "Code", "Left" } });
            }
            else if (Input.GetKeyDown(rightKeyCode))
            {
                EventManager.BroadcastEvent("MovementKeyPressed", new Hashtable() { { "Code", "Right" } });
            }
            else if (Input.GetKeyDown(upKeyCode))
            {
                EventManager.BroadcastEvent("MovementKeyPressed", new Hashtable() { { "Code", "Up" } });
            }
            else if (Input.GetKeyDown(downKeyCode))
            {
                EventManager.BroadcastEvent("MovementKeyPressed", new Hashtable() { { "Code", "Down" } });
            }
            else if(Input.GetKeyDown(specialActionKeyCode))
            {
                EventManager.BroadcastEvent("SpecialKeyPressed", null);
            }
        }
    }
}
