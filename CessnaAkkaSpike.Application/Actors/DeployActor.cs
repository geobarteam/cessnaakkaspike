using System;
using System.Linq;
using System.Threading;
using Akka.Actor;
using CessnaAkkaSpike.Application.Messages;

namespace CessnaAkkaSpike.Application.Actors
{
    public class DeployActor: AtLeastOnceDeliveryWithSnapshotReceiveActor<ReliableDeliveryEnvelope<PipelineMessage>>
    {
        private readonly IActorRef[] _outports;
        private readonly string _environment;

        public DeployActor(IActorRef[] Outports, string environment)
        {
            _outports = Outports;
            _environment = environment;

           
        }


        protected override void HandleCommand(ReliableDeliveryEnvelope<PipelineMessage> message)
        {
            ColorConsole.WriteMagenta($"{DateTime.Now} - Starting deployment in {_environment} for installer '{message.Message.InstallerName}'");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            _outports.ToList().ForEach(actor =>
                Deliver(actor.Path, messageId => new ReliableDeliveryEnvelope<PipelineMessage>(message.Message, messageId)));
            Sender.Tell(new ReliableDeliveryAck(message.MessageId));
        }
    }
}
