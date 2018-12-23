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

            Receive<PipelineMessage>(message => HandleRegisterReleaseActor(message));
        }

        private void HandleRegisterReleaseActor(PipelineMessage message)
        {

            ColorConsole.WriteMagenta($"{DateTime.Now} - Register installer '{message.InstallerName}' in release '{message.PipelineName}'");
            _repository.RegisterInstallerinRelease(message.InstallerName);
        }
    }
}
