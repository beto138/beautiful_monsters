using System;
using System.Collections;
using System.Collections.Generic;

using vhasselmann.Core.Messaging.EventSystem;
using vhasselmann.Core.Messaging.QueuedMessage;

namespace vhasselmann.Core.Messaging
{
    /// <summary>
    /// Channels agregate an EventDispatcher and a MessageQueue
    /// </summary>
    public class Channel
    {
        private string m_Name;
        private EventDispatcher m_EventDispatcher;
        private MessageQueue m_MessageQueue;

        public Channel() : this("unnamed")
        {
            
        }

        public Channel(string name)
        {
            this.m_Name = name;
            m_EventDispatcher = new EventDispatcher();
            m_MessageQueue = new MessageQueue();
        }

        /// <summary>
        /// The Name associated with this Channel
        /// </summary>
        /// <returns>Returns the name of the Channel</returns>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// Is there any queued items on the MessageQueue?
        /// </summary>
        /// <returns>Returns whether there is or not any queued items in the MessageQueue component</returns>
        public bool HasQueuedItens()
        {
            return m_MessageQueue.HasItems();
        }

        public Dictionary<int, List<Action<SimpleEvent>>> EventList()
        {
            return m_EventDispatcher.m_EventList;
        }

        /// <summary>
        /// Dequeues one item from the MessageQueue: removes it from the queue and returns it
        /// </summary>
        /// <returns>Returns item just removed from the queue</returns>
        public SimpleBroadcast Dequeue()
        {
            return m_MessageQueue.DequeueMessage();
        }

        /// <summary>
        /// Queue one message to the MessageQueue
        /// </summary>
        /// <param name="msg">The object you want to queue</param>
        public void QueueMessage(SimpleBroadcast msg)
        {
            m_MessageQueue.QueueMessage(msg);
        }

        /// <summary>
        /// Register one event to the dispatcher
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The action to execute when the event is dispatched</param>
        public void RegisterEvent(int eventId, Action<SimpleEvent> evt)
        {
            m_EventDispatcher.RegisterEvent(eventId, evt);
        }

        /// <summary>
        /// Register one event to the dispatcher
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The action to execute when the event is dispatched</param>
        public void RegisterEvent(string eventId, Action<SimpleEvent> evt)
        {
            m_EventDispatcher.RegisterEvent(eventId, evt);
        }

        /// <summary>
        /// Register one single event to the dispatcher, if there is any other, it will replace for this current one
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The action to execute when the event is dispatched</param>
        public void RegisterSingleEvent(int eventId, Action<SimpleEvent> evt)
        {
            m_EventDispatcher.RegisterSingleEvent(eventId, evt);
        }

        /// <summary>
        /// Register one single event to the dispatcher, if there is any other, it will replace for this current one
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The action to execute when the event is dispatched</param>
        public void RegisterSingleEvent(string eventId, Action<SimpleEvent> evt)
        {
            m_EventDispatcher.RegisterSingleEvent(eventId, evt);
        }

        /// <summary>
        /// Unregister the event given by params
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The action to execute when the event is dispatched</param>
        public void UnregisterEvent(int eventId, Action<SimpleEvent> evt)
        {
            m_EventDispatcher.UnregisterEvent(eventId, evt);
        }

        /// <summary>
        /// Unregister the event given by params
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The action to execute when the event is dispatched</param>
        public void UnregisterEvent(string eventId, Action<SimpleEvent> evt)
        {
            m_EventDispatcher.UnregisterEvent(eventId, evt);
        }

        /// <summary>
        /// Dispatch and call all the actions related to this event
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The data to be passed to the actions</param>
        public void DispatchEvent(int eventId, SimpleEvent evt)
        {
            m_EventDispatcher.DispatchEvent(eventId, evt);
        }

        /// <summary>
        /// Dispatch and call all the actions related to this event
        /// </summary>
        /// <param name="eventId">The event identification</param>
        /// <param name="evt">The data to be passed to the actions</param>
        public void DispatchEvent(string eventId, SimpleEvent evt)
        {
            m_EventDispatcher.DispatchEvent(eventId, evt);
        }

        /// <summary>
        /// Will remove all the events and message queues of this channel. Be careful.
        /// </summary>
        public void ClearChannel()
        {
            m_EventDispatcher.ClearAllEvents();
            m_MessageQueue.ClearQueue();
        }
    }
}