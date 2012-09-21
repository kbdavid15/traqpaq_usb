using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}
