using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Akka.Actor;
using CessnaAkkaSpike.Messages;
using CessnaAkkaSpike.Repository;

namespace CessnaAkkaSpike.Actors
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
