using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using CessnaAkkaSpike.Application.Messages;

namespace CessnaAkkaSpike.Application.Actors
{
    public class ApprovalActor:AtLeastOnceDeliveryWithSnapshotReceiveActor<ReliableDeliveryEnvelope<PipelineMessage>>
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
                        new PipelineMessage(message.PipelineName, message.InstallerName), messageId)));
        }

        protected override void HandleCommand(ReliableDeliveryEnvelope<PipelineMessage> message)
        {
            if (!_messagesToBeApproved.ContainsKey(message.Message.InstallerName))
            {
                _messagesToBeApproved.Add(message.Message.InstallerName, message.Message);
            }

            ColorConsole.WriteMagenta($"{DateTime.Now} - Approval waiting for '{ message.Message.InstallerName }'");
            Sender.Tell(new ReliableDeliveryAck(message.MessageId));
        }



        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineGreen("Approval PreStart");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineGreen("Approval PostStop");
        }

       

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineGreen("Approval PreRestart because: " + reason);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineGreen("Approval PostRestart because: " + reason);

            base.PostRestart(reason);
        }
        #endregion
    }
}
