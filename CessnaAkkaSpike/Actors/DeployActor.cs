using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace CessnaAkkaSpike.Actors
{
    public class DeployActor: ReceiveActor
    {
        private readonly IActorRef[] _outports;
        private readonly string _environment;

        public DeployActor(IActorRef[] Outports, string environment)
        {
            _outports = Outports;
            _environment = environment;
        }
    }
}
