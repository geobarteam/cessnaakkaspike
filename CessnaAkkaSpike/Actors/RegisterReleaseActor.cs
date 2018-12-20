using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
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
        }
    }
}
