using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Akka.Actor;
using Akka.IO;
using Akka.Persistence;
using CessnaAkkaSpike.Application.Messages;
using CessnaAkkaSpike.Application.Repository;

namespace CessnaAkkaSpike.Application.Actors
{
    public class RegisterInstallerActor : AtLeastOnceDeliveryWithSnapshotReceiveActor<PipelineMessage>
    {
        private readonly IRepository _repository;


        private readonly IActorRef[] _outports;

        public RegisterInstallerActor(IActorRef[] outports, IRepository repository)
        {
            _outports = outports;
            _repository = repository;
        }


        protected override void HandleCommand(PipelineMessage message)
        {
            ColorConsole.WriteMagenta($"{DateTime.Now} - Registring installer '{message.InstallerName}'");
            _repository.CreateInstaller(message.InstallerName);

            _outports.ToList().ForEach(actor =>
                Deliver(actor.Path, messageId => new ReliableDeliveryEnvelope<PipelineMessage>(message, messageId)));
        }
    }
}
