using System;
using Akka.Actor;
using CessnaAkkaSpike.Application.Messages;
using CessnaAkkaSpike.Application.Repository;

namespace CessnaAkkaSpike.Application.Actors
{
    public class RegisterReleaseActor: ReceiveActor
    {
        private readonly IActorRef[] _outports;
        private readonly IRepository _repository;

        public RegisterReleaseActor(IActorRef[] Outports, IRepository repository)
        {
            _outports = Outports;
            _repository = repository;

            Receive<ReliableDeliveryEnvelope<PipelineMessage>>(message => HandleRegisterReleaseActor(message));
        }

        private void HandleRegisterReleaseActor(ReliableDeliveryEnvelope<PipelineMessage> message)
        {

            ColorConsole.WriteMagenta($"{DateTime.Now} - Register installer '{message.Message.InstallerName}' in release '{message.Message.PipelineName}'");
            _repository.RegisterInstallerinRelease(message.Message.InstallerName);
            Sender.Tell(new ReliableDeliveryAck(message.MessageId));
        }
    }
}
