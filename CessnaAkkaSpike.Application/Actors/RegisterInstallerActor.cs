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
    public class RegisterInstallerActor : AtLeastOnceDeliveryWithSnapshotReceiveActorFilter<PipelineMessage>
    {
        private readonly IRepository _repository;


        public RegisterInstallerActor(IActorRef[] Outports, IRepository repository):base(Outports)
        {
            _repository = repository;
        }


        protected override void HandleCommand(PipelineMessage message)
        {
            ColorConsole.WriteMagenta($"{DateTime.Now} - Registring installer '{message.InstallerName}'");
            _repository.CreateInstaller(message.InstallerName);
        }
    }
}
