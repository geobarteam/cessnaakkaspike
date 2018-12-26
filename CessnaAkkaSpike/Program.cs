using System;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using CessnaAkkaSpike.Application;
using CessnaAkkaSpike.Application.Actors;
using CessnaAkkaSpike.Application.Messages;

namespace CessnaAkkaSpike.Host
{
    class Program
    {
        private static ActorSystem CessnaActorSystem;static void Main(string[] args)
        {
            CessnaService.Start();
           
        }

       
    }
}
