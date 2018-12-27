using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Akka.Actor;
using Akka.IO;
using Akka.Persistence;
using CessnaAkkaSpike.Application.Messages;
using CessnaAkkaSpike.Application.Repository;

namespace CessnaAkkaSpike.Application.Actors
{
    public class RegisterInstallerActor : AtLeastOnceDeliveryReceiveActor
    {
        private readonly IActorRef[] _outports;
        private readonly IRepository _repository;

        public override string PersistenceId => Context.Self.Path.Name;
        private int _counter = 0;


        private ICancelable _recurringMessageSend;
        private ICancelable _recurringSnapshotCleanup;

        private class DoSend { }
        private class CleanSnapshots { }

        public RegisterInstallerActor(IActorRef[] Outports, IRepository repository)
        {
            _outports = Outports;
            _repository = repository;

            Recover<SnapshotOffer>(offer => offer.Snapshot is Akka.Persistence.AtLeastOnceDeliverySnapshot, offer =>
            {
                var snapshot = offer.Snapshot as Akka.Persistence.AtLeastOnceDeliverySnapshot;
                SetDeliverySnapshot(snapshot);
            });

           
            Command<PipelineMessage>(message =>
            {
                ColorConsole.WriteMagenta($"{DateTime.Now} - Registring installer '{message.InstallerName}'");
                _repository.CreateInstaller(message.InstallerName);

                _outports.ToList().ForEach(actor =>
                    Deliver(actor.Path, messageId => new ReliableDeliveryEnvelope<PipelineMessage>(message, messageId)));

                // save the full state of the at least once delivery actor
                // so we don't lose any messages upon crash
                SaveSnapshot(GetDeliverySnapshot());
            });

            Command<ReliableDeliveryAck>(ack =>
            {
                ConfirmDelivery(ack.MessageId);
            });

            Command<CleanSnapshots>(clean =>
            {
                // save the current state (grabs confirmations)
                SaveSnapshot(GetDeliverySnapshot());
            });

            Command<SaveSnapshotSuccess>(saved =>
            {
                var seqNo = saved.Metadata.SequenceNr;
                DeleteSnapshots(new SnapshotSelectionCriteria(seqNo, saved.Metadata.Timestamp.AddMilliseconds(-1))); // delete all but the most current snapshot
            });

            Command<SaveSnapshotFailure>(failure =>
            {
                ColorConsole.WriteLineRed(failure.ToString());
            });
            
            
        }
        
        protected override void PreStart()
        {
            _recurringSnapshotCleanup =
                Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(10), Self, new CleanSnapshots(), ActorRefs.NoSender);

            base.PreStart();
        }

        protected override void PostStop()
        {
            _recurringSnapshotCleanup?.Cancel();
            _recurringMessageSend?.Cancel();

            base.PostStop();
        }

        
    }
}
