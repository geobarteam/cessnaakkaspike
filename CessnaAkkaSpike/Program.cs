using System;
using Akka.Actor;
using CessnaAkkaSpike.Actors;
using CessnaAkkaSpike.Messages;

namespace CessnaAkkaSpike
{
    class Program
    {
        private static ActorSystem CessnaActorSystem;
        static void Main(string[] args)
        {
            ColorConsole.WriteLineBlue("Starting CessnaActorSystem");
            CessnaActorSystem = ActorSystem.Create("CesssnaActorSystem");

            ColorConsole.WriteLineBlue("Creating ProcessManager");
            CessnaActorSystem.ActorOf(Props.Create<ProcessManagerActor>(), "ProcessManager");

            do
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                ColorConsole.WriteLineGray("enter a command and hit enter");

                var command = Console.ReadLine();
                if (command.StartsWith("register"))
                {
                    string tenantName = command.Split(',')[1];
                    string installerName = command.Split(',')[2];

                    var message = new PipelineMessage(tenantName, installerName);
                    CessnaActorSystem.ActorSelection("/user/ProcessManager").Tell(message);
                }

                if (command == "exit")
                {
                    ColorConsole.WriteLineGray("Actor system shutdown");
                    CessnaActorSystem.Terminate().Wait();
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            } while (true);

        }
    }
}
