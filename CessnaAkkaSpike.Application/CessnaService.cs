using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using CessnaAkkaSpike.Application.Actors;
using CessnaAkkaSpike.Application.Messages;

namespace CessnaAkkaSpike.Application
{
    public static class CessnaService
    {
        private static ActorSystem CessnaActorSystem;

        public static void Start()
        {
            CessnaActorSystem = CreateCessnaActorSystem();
            ListenToConsole();
        }

        private static ActorSystem CreateCessnaActorSystem()
        {
            var hoconString = File.ReadAllText(".\\config.hocon");
            var cfg = ConfigurationFactory.ParseString(hoconString);

            ColorConsole.WriteLineBlue("Starting CessnaActorSystem");
            CessnaActorSystem = ActorSystem.Create("CesssnaActorSystem", cfg);

            ColorConsole.WriteLineBlue("Creating ProcessManager");
            CessnaActorSystem.ActorOf(Props.Create<ProcessManagerActor>().WithRouter(FromConfig.Instance), "ProcessManager");

            return CessnaActorSystem;
        }

        private static void ListenToConsole()
        {
            do
            {
                try
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

                    if (command.StartsWith("approve"))
                    {
                        string tenantName = command.Split(',')[1];
                        string installerName = command.Split(',')[2];

                        var message = new ApproveMessage(tenantName, installerName);
                        CessnaActorSystem.ActorSelection("/user/ProcessManager").Tell(message);
                    }

                    if (command == "exit")
                    {
                        ColorConsole.WriteLineGray("Actor system shutdown");
                        CessnaActorSystem.Terminate().Wait();
                        Console.ReadKey();
                        Environment.Exit(1);
                    }
                }
                catch (Exception e)
                {
                    ColorConsole.WriteMagenta(e.Message);
                }

            } while (true);
        }
    }
}
