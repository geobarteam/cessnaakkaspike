using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Akka.Actor;
using CessnaAkkaSpike.Messages;

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

            Receive<PipelineMessage>(message => HandleDeploy(message));
        }

        private void HandleDeploy(PipelineMessage message)
        {
            ColorConsole.WriteMagenta($"{DateTime.Now} - Starting deployment for installer '{message.InstallerName}'");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            _outports.ToList().ForEach(actor => actor.Tell(message));
        }

       
    }
}
