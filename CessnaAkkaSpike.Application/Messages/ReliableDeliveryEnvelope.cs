﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CessnaAkkaSpike.Application.Messages
{
    public class ReliableDeliveryEnvelope<TMessage>
    {
        public ReliableDeliveryEnvelope(TMessage message, long messageId)
        {
            Message = message;
            MessageId = messageId;
        }

        public TMessage Message { get; private set; }

        public long MessageId { get; private set; }
    }

    public class ReliableDeliveryAck
    {
        public ReliableDeliveryAck(long messageId)
        {
            MessageId = messageId;
        }

        public long MessageId { get; private set; }
    }



}
