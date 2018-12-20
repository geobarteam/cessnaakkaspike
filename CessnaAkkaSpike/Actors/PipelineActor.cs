using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace CessnaAkkaSpike.Actors
{
    public class PipelineActor : ReceiveActor
    {
        public string PipelineName { get; }

        public PipelineActor(string pipelineName)
        {
            PipelineName = pipelineName;
            BuildPipeline(pipelineName);


        }

        public static void BuildPipeline(string pipelineName)
        {
            var registerReleaseActorRef =
                Context.ActorOf(Props.Create(() => new RegisterReleaseActor(null, new Repository.Repository())), pipelineName + "RegisterReleaseActor");
            var deployToPrdActorRef =
                Context.ActorOf(Props.Create(() => new DeployActor(null, "PRD")), pipelineName + "DeployToPRDActor");
            var approvalForPrdActorRef =
                Context.ActorOf(Props.Create(() => new ApprovalActor(new []{ deployToPrdActorRef })), pipelineName + "ApprovalForPRDActor");
            var deployToDvlRepositoryRef = 
                Context.ActorOf(Props.Create(() => new DeployActor(new []{ approvalForPrdActorRef }, "DVL")), pipelineName + "DeployToDvlRepository");
            var registerInstallerActorRef =
                Context.ActorOf(Props.Create(() => new RegisterInstallerActor(
                        new []{ deployToDvlRepositoryRef }, new Repository.Repository())), pipelineName + "RegisterInstallerActor");


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
