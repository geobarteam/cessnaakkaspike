using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Routing;
using CessnaAkkaSpike.Application.Messages;

namespace CessnaAkkaSpike.Application.Actors
{
    public class PipelineActor : ReceiveActor
    {
        public string PipelineName { get; }
        public IList<IActorRef> Pipeline { get; private set; }
        public PipelineActor(string pipelineName)
        {
            PipelineName = pipelineName;
            this.Pipeline = BuildPipeline(pipelineName);
            Receive<PipelineMessage>(message => this.Pipeline.First().Tell(message));
            Receive<ApproveMessage>(message => this.Pipeline.First(n=>n.Path.Name == "ApprovalForPRDActor").Tell(message));

        }

        public static IList<IActorRef> BuildPipeline(string pipelineName)
        {

            var registerReleaseActorRef =
                Context.ActorOf(Props.Create(() => new RegisterReleaseActor(null, new Repository.Repository())), "RegisterReleaseActor");
            var deployToPrdActorRef =
                Context.ActorOf(Props.Create(() => new DeployActor(new [] { registerReleaseActorRef }, "PRD")),  "DeployToPRDActor");
            var approvalForPrdActorRef =
                Context.ActorOf(Props.Create(() => new ApprovalActor(new []{ deployToPrdActorRef })), "ApprovalForPRDActor");
            var deployToDvlRepositoryRef = 
                Context.ActorOf(Props.Create(() => new DeployActor(new []{ approvalForPrdActorRef }, "DVL")), "DeployToDvlActor");
            var registerInstallerActorRef =
                Context.ActorOf(Props.Create(() => new RegisterInstallerActor(
                        new []{ deployToDvlRepositoryRef }, new Repository.Repository())), "RegisterInstallerActor");

            var pipeline = new List<IActorRef>();
            pipeline.Add(registerInstallerActorRef);
            pipeline.Add(deployToDvlRepositoryRef);
            pipeline.Add(approvalForPrdActorRef);
            pipeline.Add(deployToPrdActorRef);
            pipeline.Add(registerReleaseActorRef);

            return pipeline;
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeRange: TimeSpan.FromMinutes(1),
                localOnlyDecider: ex => Directive.Resume);
        }


        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineGreen("Pipeline PreStart");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineGreen("Pipeline PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineGreen("Pipeline PreRestart because: " + reason);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineGreen("Pipeline PostRestart because: " + reason);

            base.PostRestart(reason);
        }
        #endregion
    }
}
