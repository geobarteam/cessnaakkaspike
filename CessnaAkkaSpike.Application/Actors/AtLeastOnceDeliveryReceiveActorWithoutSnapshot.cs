using System;
using System.Collections.Generic;
using System.Text;
using Akka.Persistence;
using CessnaAkkaSpike.Application.Messages;

namespace CessnaAkkaSpike.Application.Actors
{
    public abstract class AtLeastOnceDeliveryReceiveActorWithoutSnapshot<T>: AtLeastOnceDeliveryReceiveActor
    {
        public override string PersistenceId => Context.Self.Path.ToString();

        public AtLeastOnceDeliveryReceiveActorWithoutSnapshot()
        {
            Recover<MsgSent<T>>(msgSent => Handler(msgSent));
            Recover<MsgConfirmed>(msgConfirmed => Confirm(msgConfirmed));

            Command<T>(msg =>
            {
                Persist(new MsgSent<T>(msg), Handler);
            });

            Command<ReliableDeliveryAck>(confirm =>
            {
                Persist(new MsgConfirmed(confirm.MessageId), Confirm);
            });
        }

        protected void Handler(MsgSent<T> msgSent)
        {
            HandleCommand(msgSent.Message);
        }

        protected abstract void HandleCommand(T message);

        private void Confirm(MsgConfirmed msgConfirmed)
        {
            ConfirmDelivery(msgConfirmed.DeliveryId);
        }


    }
}
