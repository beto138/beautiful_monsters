using System;
using System.Collections;
using System.Collections.Generic;

namespace vhasselmann.Core.Messaging.QueuedMessage
{
    /// <summary>
    /// Component that queues messages and deal with them
    /// </summary>
    public class MessageQueue
    {
        private Queue<SimpleBroadcast> m_Messages;

        public MessageQueue()
        {
            m_Messages = new Queue<SimpleBroadcast>();
        }

        /// <summary>
        /// Queue one message
        /// </summary>
        /// <param name="message">The object you want to queue</param>
        public void QueueMessage(SimpleBroadcast message)
        {
            m_Messages.Enqueue(message);
        }

        /// <summary>
        /// Dequeues one item: removes it from the queue and returns it
        /// </summary>
        /// <returns>Returns item just removed from the queue</returns>
        public SimpleBroadcast DequeueMessage()
        {
            return m_Messages.Dequeue();
        }
        /// <summary>
        /// Is there any queued items?
        /// </summary>
        /// <returns>Returns whether there is or not any queued items</returns>
        public bool HasItems()
        {
            return m_Messages.Count > 0;
        }

        /// <summary>
        /// Will clear all the messages registered
        /// </summary>
        public void ClearQueue()
        {
            m_Messages.Clear();
        }
    }
}