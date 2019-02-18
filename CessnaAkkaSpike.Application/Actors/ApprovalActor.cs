using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using CessnaAkkaSpike.Application.Messages;
using CessnaAkkaSpike.Application.Repository;

namespace CessnaAkkaSpike.Application.Actors
{
    public class ApprovalActor:AtLeastOnceDeliveryWithSnapshotReceiveActor<ReliableDeliveryEnvelope<PipelineMessage>>
    {
        private readonly IActorRef[] _outports;
        private readonly IRepository _repository;

        public ApprovalActor(IActorRef[] outports, IRepository repository):base()
        {
            _outports = outports;
            _repository = repository;
            Command<ApproveMessage>(message => HandleApproveMessage(message));
            
        }
        
        private void HandleApproveMessage(ApproveMessage message)
        {
            var approvals = _repository.ReadAllApprovals();
            var pipelineMessage = approvals[message.InstallerName];
            approvals.Remove(message.InstallerName);
            _outports.ToList().ForEach(actor =>
                Deliver(actor.Path, messageId => 
                    new ReliableDeliveryEnvelope<PipelineMessage>(
                        new PipelineMessage(message.PipelineName, message.InstallerName), messageId)));
            _repository.StoreApprovals(approvals);
        }

        protected override void HandleCommand(ReliableDeliveryEnvelope<PipelineMessage> message)
        {
            var approvals = _repository.ReadAllApprovals();
            if (!approvals.ContainsKey(message.Message.InstallerName))
            {
                approvals.Add(message.Message.InstallerName, message.Message);
                _repository.StoreApprovals(approvals);
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
