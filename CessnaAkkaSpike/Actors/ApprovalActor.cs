using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akka.Actor;
using CessnaAkkaSpike.Messages;

namespace CessnaAkkaSpike.Actors
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
            _outports.ToList().ForEach(actor => actor.Tell(message));
        }

        private void HandlePipelineMessage(PipelineMessage message)
        {
            _messagesToBeApproved.Add(message.InstallerName, message);
        }
    }
}
