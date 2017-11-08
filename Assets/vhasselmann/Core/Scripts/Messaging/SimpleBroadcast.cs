using System;
using System.Collections;
using System.Collections.Generic;

namespace vhasselmann.Core.Messaging
{
    public class SimpleBroadcast
    {
        public int m_Type;
        public object m_Data;

        public SimpleBroadcast()
        {
            this.m_Type = 0;
            this.m_Data = null;
        }

        public SimpleBroadcast(int type)
        {
            this.m_Type = type;
            this.m_Data = null;
        }

        public SimpleBroadcast(int type, object data)
        {
            this.m_Type = type;
            this.m_Data = data;
        }
    }
}