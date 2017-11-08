using System;
using System.Collections;
using System.Collections.Generic;

namespace vhasselmann.Core.Messaging.EventSystem
{
    public class SimpleEvent : SimpleBroadcast
    {
        public SimpleEvent()
        {

        }

        public SimpleEvent(object data)
        {
            this.m_Data = data;
        }
    }
}