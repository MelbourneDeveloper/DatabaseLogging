﻿using System;

namespace DatabaseLogging
{
    public class LogPropertyKey
    {
        public LogPropertyKey(Guid key, string keyName)
        {
            Key = key;
            KeyName = keyName;
        }

        public Guid Key { get; }
        public string KeyName { get; }
    }
}