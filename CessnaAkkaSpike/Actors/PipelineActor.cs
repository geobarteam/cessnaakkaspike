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
