﻿using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using CessnaAkkaSpike.Application.Messages;

namespace CessnaAkkaSpike.Application.Actors
{
    public class ApprovalActor:AtLeastOnceDeliveryWithSnapshotReceiveActor<PipelineMessage>
    {
        private readonly IActorRef[] _outports;
        private Dictionary<string, PipelineMessage> _messagesToBeApproved;

        public ApprovalActor(IActorRef[] outports):base()
        {
            _outports = outports;
            _messagesToBeApproved = new Dictionary<string, PipelineMessage>();
            Command<ApproveMessage>(message => HandleApproveMessage(message));
            
        }

       

        private void HandleApproveMessage(ApproveMessage message)
        {
            var pipelineMessage = _messagesToBeApproved[message.InstallerName];
            _messagesToBeApproved.Remove(message.InstallerName);
            _outports.ToList().ForEach(actor =>
                Deliver(actor.Path, messageId => 
                    new ReliableDeliveryEnvelope<PipelineMessage>(
                        new PipelineMessage(null, message.InstallerName), messageId)));
        }

        
        protected override void HandleCommand(PipelineMessage message)
        {
            if (!_messagesToBeApproved.ContainsKey(message.InstallerName))
            {
                _messagesToBeApproved.Add(message.InstallerName, message);
            }

            ColorConsole.WriteMagenta($"{DateTime.Now} - Approval waiting for '{ message.InstallerName }'");
        }
    }
}
