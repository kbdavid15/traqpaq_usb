﻿using System;
using System.Runtime.Serialization;

namespace traqpaqWPF
{
    public class TraqPaqNotConnectedException : Exception, ISerializable
    {
        public TraqPaqNotConnectedException() { }

        public TraqPaqNotConnectedException(string message)
            : base(message)
        {

        }

        public TraqPaqNotConnectedException(string message, Exception inner)
        {

        }

        protected TraqPaqNotConnectedException(SerializationInfo info, StreamingContext context)
        {

        }
    }

    public class USBCommandFailedException : Exception, ISerializable
    {
        public USBCommandFailedException() { }
        public USBCommandFailedException(string message) : base(message) { }
        public USBCommandFailedException(string message, Exception inner) { }
        protected USBCommandFailedException(SerializationInfo info, StreamingContext context) { }
    }
}
