using System;
using System.Collections;
using System.Collections.Generic;

using vhasselmann.Core.Messaging.EventSystem;
using UnityEngine;

namespace vhasselmann.Core.Messaging
{
    /// <summary>
    /// MessagingManager centralize all the messaging channels
    /// </summary>
    public class MessagingManager : Singleton<MessagingManager>
    {
        // Static
        public static Dictionary<int, string> hashBackups = new Dictionary<int, string>();

        public static bool Exists()
        {
            return _Instance != null;
        }

        // Instance attributes and methods
        private Dictionary<string, Channel> m_Channels = new Dictionary<string, Channel>();

        /// <summary>
        /// Creates a hash replace strings for ints
        /// </summary>
        /// <param name="text">The text to calculate the hash</param>
        /// <returns>Returns the int of that hash calculation</returns>
        public static int Hash(string text)
        {
            int hash = UnityEngine.Animator.StringToHash(text);
            if (!hashBackups.ContainsKey(hash))
            {
                hashBackups.Add(hash, text);
            }
            return hash;
        }

        /// <summary>
        /// Is there any queued items on the MessageQueue for a specific channel?
        /// </summary>
        /// <param name="channelName">The name of the selected channel</param>
        /// <returns>Returns whether there is or not any queued items in the MessageQueue component of that channel</returns>
        public bool HasQueueItems(string channelName)
        {
            return GetChannel(channelName).HasQueuedItens();
        }

        /// <summary>
        /// Dequeues one item from the MessageQueue: removes it from the queue and returns it
        /// </summary>
        /// <param name="channelName">The name of the selected channel</param>
        /// <returns>Returns item just removed from the queue</returns>
        public SimpleBroadcast DequeueMessage(string channelName)
        {
            return GetChannel(channelName).Dequeue();
        }

        /// <summary>
        /// Queue one message to the MessageQueue
        /// </summary>
        /// <param name="channelName">The name of the selected channel</param>
        /// <param name="msg">The object you want to queue</param>
        public void QueueMessage(string channelName, SimpleBroadcast msg)
        {
            GetChannel(channelName).QueueMessage(msg);
        }

        /// <summary>
        /// Gets all the channels registered in a dictionary
        /// </summary>
        /// <returns>The dictionary of all the Channels registered</returns>
        public Dictionary<string, Channel> GetChannels()
        {
            return m_Channels;
        }

        /// <summary>
        /// Create a new channel and store it internally
        /// </summary>
        /// <param name="channelName">The name you want to give to the new channel</param>
        public Channel CreateChannel(string channelName)
        {
            if (m_Channels.ContainsKey(channelName))
            {
                return m_Channels[channelName];
            }

            Channel newChannel = new Channel(channelName);
            m_Channels.Add(channelName, newChannel);
            return newChannel;
        }

        /// <summary>
        /// Returns a specific channel registered. If there's no such channel, it will return the "general" channel and shout a warning
        /// </summary>
        /// <param name="channelName">The name of the channel</param>
        /// <returns>A Channel object</returns>
        private Channel GetChannel(string channelName)
        {
            if (!m_Channels.ContainsKey(channelName))
            {
				Debug.LogWarning("Channel " + channelName + " not found. Maybe its missing registering.");

                return m_Channels["general"];
            }

            return m_Channels[channelName];
        }

        public void RemoveChannel(string channelName)
        {
            // TODO
        }

        /// <summary>
        /// Dispatch an event out of an EventDispatcher inside a specific Channel
        /// </summary>
        /// <param name="channel">The channel name you want to dispatch</param>
        /// <param name="eventId">The event identification to dispatch</param>
        /// <param name="evt">The event data object</param>
        public void DispatchEvent(string channel, int eventId, SimpleEvent evt)
        {
            GetChannel(channel).DispatchEvent(eventId, evt);
        }

        /// <summary>
        /// Dispatch an event out of an EventDispatcher inside a specific Channel
        /// </summary>
        /// <param name="channel">The channel name you want to dispatch</param>
        /// <param name="eventId">The event identification to dispatch</param>
        /// <param name="evt">The event data object</param>
        public void DispatchEvent(string channel, string eventId, SimpleEvent evt)
        {
            GetChannel(channel).DispatchEvent(eventId, evt);
        }
		
        /// <summary>
        /// Register a new event on a specific Channel
        /// </summary>
        /// <param name="channel">The channel name to register the event</param>
        /// <param name="eventId">The event identification</param>
        /// <param name="evtAction">The action to be called when the event is dispatched</param>
		public void RegisterEvent(string channel, int eventId, Action<SimpleEvent> evtAction)
		{
			GetChannel (channel).RegisterEvent (eventId, evtAction);
		}

        /// <summary>
        /// Register a new event on a specific Channel
        /// </summary>
        /// <param name="channel">The channel name to register the event</param>
        /// <param name="eventId">The event identification</param>
        /// <param name="evtAction">The action to be called when the event is dispatched</param>
        public void RegisterEvent(string channel, string eventId, Action<SimpleEvent> evtAction)
        {
            GetChannel(channel).RegisterEvent(eventId, evtAction);
        }

        /// <summary>
        /// Register a single event on a specific Channel. If there's such event, it will be replace by the new one
        /// </summary>
        /// <param name="channel">The channel name to register the event</param>
        /// <param name="eventId">The event identification</param>
        /// <param name="evtAction">The action to be called when the event is dispatched</param>
        public void RegisterSingleEvent(string channel, int eventId, Action<SimpleEvent> evtAction)
        {
            GetChannel(channel).RegisterSingleEvent(eventId, evtAction);
        }

        /// <summary>
        /// Register a single event on a specific Channel. If there's such event, it will be replace by the new one
        /// </summary>
        /// <param name="channel">The channel name to register the event</param>
        /// <param name="eventId">The event identification</param>
        /// <param name="evtAction">The action to be called when the event is dispatched</param>
        public void RegisterSingleEvent(string channel, string eventId, Action<SimpleEvent> evtAction)
        {
            GetChannel(channel).RegisterSingleEvent(eventId, evtAction);
        }

        /// <summary>
        /// Unregister an event and action combination
        /// </summary>
        /// <param name="channel">The channel name to register the event</param>
        /// <param name="eventId">The event identification</param>
        /// <param name="evtAction">The action to be called when the event is dispatched</param>
		public void UnregisterEvent(string channel, int eventId, Action<SimpleEvent> evtAction)
		{
			GetChannel (channel).UnregisterEvent(eventId, evtAction);
		}

        /// <summary>
        /// Unregister an event and action combination
        /// </summary>
        /// <param name="channel">The channel name to register the event</param>
        /// <param name="eventId">The event identification</param>
        /// <param name="evtAction">The action to be called when the event is dispatched</param>
        public void UnregisterEvent(string channel, string eventId, Action<SimpleEvent> evtAction)
        {
            GetChannel(channel).UnregisterEvent(eventId, evtAction);
        }

        /// <summary>
        /// Will remove all the events and message queues of a specific channel. Be careful.
        /// </summary>
		public void ClearChannel(string channel)
		{
			GetChannel (channel).ClearChannel();
		}
    }
}