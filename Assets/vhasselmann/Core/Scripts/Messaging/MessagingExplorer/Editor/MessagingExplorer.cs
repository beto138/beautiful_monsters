using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

using vhasselmann.Core.Messaging.EventSystem;

namespace vhasselmann.Core.Messaging
{
    public class MessagingExplorer : EditorWindow 
	{
	    private List<Channel> m_NullChannels = new List<Channel>();
        private List<KeyValuePair<int, List<Action<SimpleEvent>>>> m_NullEventLists = new List<KeyValuePair<int, List<Action<SimpleEvent>>>>();

        private string m_CurrentChannel = "";
        private int m_CurrentEventList = 0;
        private Vector2 m_ScrollPosition;
        private bool m_PauseOnNull = false;
        private bool m_ShowWarning = false;
        private float m_LastUpdateTime = (float)EditorApplication.timeSinceStartup;
     
        void OnEnable()
        {
            MessagingExplorerHelper.m_ActionCaller = new MessagingExplorerHelper.ActionCaller(ValidateAction);
            EditorApplication.update -= UpdateTimer;
            EditorApplication.update += UpdateTimer;
        }

        void OnDisable()
        {
            EditorApplication.update -= UpdateTimer;
        }

        public void ValidateAction(Action<SimpleEvent> action)
        {
            m_NullChannels.Clear();
            m_NullEventLists.Clear();

            foreach (KeyValuePair<string, Channel> channel in MessagingManager.Instance.GetChannels())
            {           
                foreach (KeyValuePair<int, List<Action<SimpleEvent>>> e in channel.Value.EventList())
                {
                    foreach (Action<SimpleEvent> a in e.Value)
                    {
                        if (a.Target.ToString() == "null")
                        {
                            if (!m_NullChannels.Contains(channel.Value)) m_NullChannels.Add(channel.Value);
                            if (!m_NullEventLists.Contains(e)) m_NullEventLists.Add(e);

                            if (action == a)
                            {
                                Repaint();
                                if (m_ShowWarning) Debug.LogError("TARGET IS NULL | " + a.Method);
                                if (m_PauseOnNull) Debug.Break();
                            }
                        }
                    }
                }
            }

        }

        void UpdateTimer()
        {
            if (EditorApplication.isPlaying && EditorApplication.timeSinceStartup - m_LastUpdateTime > 1f)
            {
                m_LastUpdateTime = (float)EditorApplication.timeSinceStartup;
                ValidateAction(null);
                Repaint();
            }
        
        }

	    // Update is called once per frame
	    void OnGUI ()
        {
            GUI.color = Color.white;

            m_ShowWarning = EditorGUILayout.Toggle("Show Warning", m_ShowWarning);
            m_PauseOnNull = EditorGUILayout.Toggle("Pause on Null",m_PauseOnNull);

            if (GUILayout.Button("Continue"))
            {
                EditorApplication.isPaused = false;
            }
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);

            GUILayout.Space(10f);

            Dictionary<string, Channel> channels = MessagingManager.Instance.GetChannels();
            foreach (KeyValuePair<string,Channel> channel in channels)
            {
                EditorGUILayout.BeginVertical();

                if (m_NullChannels.Contains(channel.Value))
                {
                    GUI.color = Color.yellow;
                }
             
                bool foldChannel = EditorGUILayout.Foldout(m_CurrentChannel.Equals(channel.Key),channel.Key + "(" + channel.Value.EventList().Count + ")");
            
                GUI.color = Color.white;

                if (foldChannel)
                {
                    m_CurrentChannel = channel.Key;
                }
                else if (m_CurrentChannel == channel.Key)
                {
                    m_CurrentChannel = "";
                }

                if (foldChannel)
                {
                    EditorGUI.indentLevel++;
                    foreach (KeyValuePair<int, List<Action<SimpleEvent>>> e in channel.Value.EventList())
                    {

                        if (m_NullEventLists.Contains(e))
                        {
                            GUI.color = Color.yellow;
                        }
                        bool foldEvent = EditorGUILayout.Foldout(m_CurrentEventList.Equals(e.Key), MessagingManager.hashBackups[e.Key] + " [" + e.Key + "] " + " (" + e.Value.Count.ToString() + ")");
                        GUI.color = Color.white;

                        if (foldEvent)
                        {
                            m_CurrentEventList = e.Key;
                        }
                        else if (m_CurrentEventList == e.Key)
                        {
                            m_CurrentEventList = 0;
                        }

                        if (foldEvent)
                        {
                            EditorGUI.indentLevel++;
                            foreach (Action<SimpleEvent> a in e.Value)
                            {
                                if (a.Target.ToString().Equals("null"))
                                {
                                    GUI.color = Color.yellow;
                                }

                                Component test = a.Target as Component;

                                GUILayout.BeginHorizontal();

                                GUI.enabled = (test != null && test.gameObject != null);

                                GUILayout.Space(40f);

                                if (GUILayout.Button("Select",GUILayout.Width(100),GUILayout.Height(20)))
                                {
                                    Selection.activeGameObject = test.gameObject;
                                }

                                GUI.enabled = true;

                                GUILayout.TextField(a.Target.ToString() + " | " + a.Method, GUI.skin.textField, GUILayout.Height(20));

                                GUILayout.EndHorizontal();

                                GUI.color = Color.white;

                            }

                            EditorGUI.indentLevel--;
                        }

                    }
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();
            }

            GUILayout.EndScrollView();
	    }
    }
}
