using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akka.Actor;
using CessnaAkkaSpike.Messages;
using CessnaAkkaSpike.Repository;

namespace CessnaAkkaSpike.Actors
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
