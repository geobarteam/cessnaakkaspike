using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akka.Actor;
using Akka.Persistence;
using CessnaAkkaSpike.Application.Messages;

namespace CessnaAkkaSpike.Application.Actors
{
    public abstract class AtLeastOnceDeliveryWithSnapshotReceiveActor<T> :AtLeastOnceDeliveryReceiveActor
    {
        public override string PersistenceId => Context.Self.Path.ToString();
        private int _counter = 0;

        private ICancelable _recurringSnapshotCleanup;

        private class CleanSnapshots { }


        public AtLeastOnceDeliveryWithSnapshotReceiveActor()
        {

            Recover<SnapshotOffer>(offer => offer.Snapshot is Akka.Persistence.AtLeastOnceDeliverySnapshot, offer =>
            {
                var snapshot = offer.Snapshot as Akka.Persistence.AtLeastOnceDeliverySnapshot;
                SetDeliverySnapshot(snapshot);
            });

            Command<T>(message =>
            {
                HandleCommand(message);
                
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
            base.PostStop();
        }

        protected abstract void HandleCommand(T message);
    }
}
