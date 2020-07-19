using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Util.EventManager
{
    //Utilizado para que possamos passar parametros nos eventos
    public class HashTableEvent : UnityEvent<Hashtable> { }

    //Sistema simples de troca de mensagens atraves de UnityEvents
    public class EventManager : MonoBehaviour
    {
        //O dicionario que ira guardar os eventos
        Dictionary<string, HashTableEvent> events;
        static EventManager mng;

        //Singleton
        public static EventManager instance
        {
            get
            {
                if (!mng)
                {
                    mng = FindObjectOfType<EventManager>();
                    if (!mng)
                    {
                        Debug.LogWarning("No Event Manager Found on Scene! Please place the event manager manualy on scene before playing again.");
                    }
                    else
                    {
                        mng.Init();
                    }
                }
                return mng;
            }
        }

        //Inicializa o dicionario de eventos caso ainda nao tenha sido criado
        void Init()
        {
            if (events == null)
            {
                events = new Dictionary<string, HashTableEvent>();
            }
        }

        //Adiciona um observador para um evento
        public static void AddListener(string eventName, UnityAction<Hashtable> listener)
        {
            HashTableEvent someEvent = null;
            if (instance.events.TryGetValue(eventName, out someEvent))
            {
                someEvent.AddListener(listener);
            }
            else
            {
                someEvent = new HashTableEvent();
                someEvent.AddListener(listener);
                instance.events.Add(eventName, someEvent);
            }
        }

        //Remove um observador de um evento
        public static void RemoveListner(string eventName, UnityAction<Hashtable> listener)
        {
            if (!mng) { return; } //Impede que tentemos remover um evento se o singleton nao foi criado ,inicializado ou foi removido.
            HashTableEvent someEvent = null;
            if (instance.events.TryGetValue(eventName, out someEvent))
            {
                someEvent.RemoveListener(listener);
            }
        }


        //Envia uma mensagem para todos os observadores do evento, passando os parametros necessarios
        public static void BroadcastEvent(string eventName, Hashtable eventParams = default)
        {
            HashTableEvent someEvent = null;
            if (instance.events.TryGetValue(eventName, out someEvent))
            {
                someEvent.Invoke(eventParams);
            }
            else
            {
                Debug.LogWarningFormat("Broadcasting event \"{0}\" but no one is listening", eventName);
            }
        }

        //Envia uma mensagem para todos os observadores do evento, sem enviar parametros
        public static void BroadcastEvent(string eventName)
        {
            BroadcastEvent(eventName, null);
        }

        //Impime a tabela de eventos registrados. Utilizado para Debug.
        public static void PrintAllEvents()
        {
            string str = "Events";
            foreach (KeyValuePair<string, HashTableEvent> pair in instance.events)
            {
                str += "\n" + pair.Key + "\t" + pair.Value;
            }
            Debug.Log(str);
        }
    }

}

