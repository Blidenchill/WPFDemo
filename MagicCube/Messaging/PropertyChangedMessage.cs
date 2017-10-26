using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Messaging
{
    public class PropertyChangedMessage<T> : PropertyChangedMessageBase
    {

        public PropertyChangedMessage(object sender, T oldValue, T newValue, string propertyName)
            : base(sender, propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }


        public PropertyChangedMessage(T oldValue, T newValue, string propertyName)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public PropertyChangedMessage(object sender, object target, T oldValue, T newValue, string propertyName)
            : base(sender, target, propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T NewValue
        {
            get;
            private set;
        }


        public T OldValue
        {
            get;
            private set;
        }
    }

    public abstract class PropertyChangedMessageBase : MessageBase
    {

        protected PropertyChangedMessageBase(object sender, string propertyName)
            : base(sender)
        {
            PropertyName = propertyName;
        }

        protected PropertyChangedMessageBase(object sender, object target, string propertyName)
            : base(sender, target)
        {
            PropertyName = propertyName;
        }


        protected PropertyChangedMessageBase(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName
        {
            get;
            protected set;
        }
    }

    public class MessageBase
    {

        public MessageBase()
        {
        }

        public MessageBase(object sender)
        {
            Sender = sender;
        }


        public MessageBase(object sender, object target)
            : this(sender)
        {
            Target = target;
        }


        public object Sender
        {
            get;
            protected set;
        }


        public object Target
        {
            get;
            protected set;
        }
    }
}
