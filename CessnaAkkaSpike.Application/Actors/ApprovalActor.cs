using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using CessnaAkkaSpike.Application.Messages;

namespace CessnaAkkaSpike.Application.Actors
{
    public class ApprovalActor:ReceiveActor
    {
        private readonly IActorRef[] _outports;

        private Dictionary<string, PipelineMessage> _messagesToBeApproved;

        public ApprovalActor(IActorRef[] outports)
        {
            _outports = outports;
            _messagesToBeApproved = new Dictionary<string, PipelineMessage>();
            Receive<PipelineMessage>(message => HandlePipelineMessage(message));
            Receive<ApproveMessage>(message => HandleApproveMessage(message));
        }

       

        private void HandleApproveMessage(ApproveMessage message)
        {
            var pipelineMessage = _messagesToBeApproved[message.InstallerName];
            _messagesToBeApproved.Remove(message.InstallerName);
            _outports.ToList().ForEach(actor => actor.Tell(new PipelineMessage(null, message.InstallerName)));
        }

        private void HandlePipelineMessage(PipelineMessage message)
        {
            if (!_messagesToBeApproved.ContainsKey(message.InstallerName))
            {
                _messagesToBeApproved.Add(message.InstallerName, message);
            }
            
            ColorConsole.WriteMagenta($"{DateTime.Now} - Approval waiting for '{ message.InstallerName }'");
        }
    }
}
