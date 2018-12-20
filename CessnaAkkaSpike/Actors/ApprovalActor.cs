using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace CessnaAkkaSpike.Actors
{
    public class ApprovalActor:ReceiveActor
    {
        private readonly IActorRef[] _outports;

        public ApprovalActor(IActorRef[] outports)
        {
            _outports = outports;
        }
    }
}
