using UnityEngine;
using System.Collections;
using System;
using vhasselmann.Core.Messaging.EventSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MessagingExplorerHelper
{
    public delegate void ActionCaller(Action<SimpleEvent> action);
    public static ActionCaller m_ActionCaller;

    public static void Invoke(Action<SimpleEvent> action)
    {
        if (m_ActionCaller != null)
        {
            m_ActionCaller(action);
        }
    }

}
