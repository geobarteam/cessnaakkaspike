using System;
using System.Linq;
using Akka.Actor;
using CessnaAkkaSpike.Application.Messages;
using CessnaAkkaSpike.Application.Repository;

namespace CessnaAkkaSpike.Application.Actors
{
    public class RegisterInstallerActor : ReceiveActor
    {
        private readonly IActorRef[] _outports;
        private readonly IRepository _repository;

        public RegisterInstallerActor(IActorRef[] Outports, IRepository repository)
        {
            _outports = Outports;
            _repository = repository;

            Receive<PipelineMessage>(message => HandleRegisterInstallerActor(message));
        }

        private void HandleRegisterInstallerActor(PipelineMessage message)
        {

            ColorConsole.WriteMagenta($"{DateTime.Now} - Registring installer '{message.InstallerName}'");
            _repository.CreateInstaller(message.InstallerName);
            _outports.ToList().ForEach(actor => actor.Tell(message));
        }
    }
}
