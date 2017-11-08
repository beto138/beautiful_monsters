using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace vhasselmann.Core.Messaging.EventSystem
{
    /// <summary>
    /// Component responsible for dispatching events and executing its associated actions
    /// </summary>
    public class EventDispatcher
    {
        public Dictionary<int, List<Action<SimpleEvent>>> m_EventList;

        public EventDispatcher()
        {
            m_EventList = new Dictionary<int, List<Action<SimpleEvent>>>();
        }

        /// <summary>
        /// Dispatch and call all the actions related to this event
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The data to be passed to the actions</param>
        public void DispatchEvent(int eventId, SimpleEvent eventData)
        {
            if (!m_EventList.ContainsKey(eventId))
            {
                return;
            }

            List<Action<SimpleEvent>> actions = m_EventList[eventId];

            if (actions == null)
            {
                return;
            }

            if (eventData != null)
            {
                eventData.m_Type = eventId;
            }

            foreach (Action<SimpleEvent> action in actions.ToArray())
            {
                if (action == null)
                {
                    continue;
                }
                action.Invoke(eventData);
                MessagingExplorerHelper.Invoke(action);
            }
        }

        /// <summary>
        /// Dispatch and call all the actions related to this event
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The data to be passed to the actions</param>
        public void DispatchEvent(string eventId, SimpleEvent eventData)
        {
            DispatchEvent(MessagingManager.Hash(eventId), eventData);
        }

        /// <summary>
        /// Register one event to the dispatcher
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The action to execute when the event is dispatched</param>
        public void RegisterEvent(int eventId, Action<SimpleEvent> action)
        {
            if (!m_EventList.ContainsKey(eventId))
            {
                List<Action<SimpleEvent>> events = new List<Action<SimpleEvent>>();
                m_EventList[eventId] = events;
            }
            m_EventList[eventId].Add(action);
            MessagingExplorerHelper.Invoke(null);
        }

        /// <summary>
        /// Register one event to the dispatcher
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The action to execute when the event is dispatched</param>
        public void RegisterEvent(string eventId, Action<SimpleEvent> action)
        {
            RegisterEvent(MessagingManager.Hash(eventId), action);
        }

        /// <summary>
        /// Method to register only one single action to a eventId. If there's any other action currently associated with this event, it
        /// will be replaced by the new one
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="action">The action to be executed when the event id dispatched</param>
        public void RegisterSingleEvent(int eventId, Action<SimpleEvent> action)
        {
            if (!m_EventList.ContainsKey(eventId))
            {
                List<Action<SimpleEvent>> events = new List<Action<SimpleEvent>>();
                m_EventList[eventId] = events;
            }

            m_EventList[eventId].Clear();
            m_EventList[eventId].Add(action);
            MessagingExplorerHelper.Invoke(null);
        }

        /// <summary>
        /// Method to register only one single action to a eventId. If there's any other action currently associated with this event, it
        /// will be replaced by the new one
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="action">The action to be executed when the event id dispatched</param>
        public void RegisterSingleEvent(string eventId, Action<SimpleEvent> action)
        {
            RegisterSingleEvent(MessagingManager.Hash(eventId), action);
        }

        /// <summary>
        /// Unregister the given event and action combination
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="action">The action to be executed when the event id dispatched</param>
        public void UnregisterEvent(int eventId, Action<SimpleEvent> action)
        {
            if (!m_EventList.ContainsKey(eventId))
            {
                return;
            }

            m_EventList[eventId].Remove(action);

            if (m_EventList[eventId].Count == 0)
            {
                m_EventList.Remove(eventId);
            }
            MessagingExplorerHelper.Invoke(null);
        }

        /// <summary>
        /// Unregister the given event and action combination
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="action">The action to be executed when the event id dispatched</param>
        public void UnregisterEvent(string eventId, Action<SimpleEvent> action)
        {
            UnregisterEvent(MessagingManager.Hash(eventId), action);
        }

        /// <summary>
        /// Will remove all the events and message queues of this channel. Be careful.
        /// </summary>
		public void ClearAllEvents()
		{
            m_EventList.Clear();
		}
    }
}