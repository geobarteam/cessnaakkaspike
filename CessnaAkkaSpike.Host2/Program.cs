using System;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using CessnaAkkaSpike.Application;
using CessnaAkkaSpike.Application.Actors;
using CessnaAkkaSpike.Application.Messages;

namespace CessnaAkkaSpike.Host2
{
    class Program
    {
       
        static void Main(string[] args)
        {
            CessnaService.Start();
        }
    }
}
