using System;
using System.Linq;
using System.Threading;
using Akka.Actor;
using CessnaAkkaSpike.Application.Messages;

namespace CessnaAkkaSpike.Application.Actors
{
    public class DeployActor: ReceiveActor
    {
        private readonly IActorRef[] _outports;
        private readonly string _environment;

        public DeployActor(IActorRef[] Outports, string environment)
        {
            _outports = Outports;
            _environment = environment;

            Receive<ReliableDeliveryEnvelope<PipelineMessage>>(message => HandleDeploy(message));
        }

        private void HandleDeploy(ReliableDeliveryEnvelope<PipelineMessage> message)
        {
            
            ColorConsole.WriteMagenta($"{DateTime.Now} - Starting deployment for installer '{message.Message.InstallerName}'");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            _outports.ToList().ForEach(actor => actor.Tell(message.Message));
            Sender.Tell(new ReliableDeliveryAck(message.MessageId));
        }

       
    }
}
