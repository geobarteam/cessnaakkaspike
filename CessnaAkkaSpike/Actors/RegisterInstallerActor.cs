using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Akka.Actor;
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
        }

    }
}
