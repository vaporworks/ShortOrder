using System;
using Relax;
using shortoder.domain;
using shortorder.messages;
using Symbiote.Core.Memento;
using Symbiote.Core.UnitOfWork;
using Symbiote.Messaging;

namespace shortorder.domain.service.handlers.v1
{
    public class HandeMenuItemAdd : IHandle<AddMenuItem>
    {
        public IDocumentRepository Couch { get; set; }
        public IMemoizer MementoProvider { get; set; }

        public Action<IEnvelope> Handle( AddMenuItem message )
        {
            try 
            {
                var newItem = new MenuItem();
                using( var context = Context.CreateFor( newItem ) )
                {
                    newItem.Id = message.Id;
                    newItem.Name = message.Name;
                    newItem.Description = message.Description;

                    Couch.Save( newItem.Id, MementoProvider.GetMemento( newItem ) );

                    context.PublishOnCommit<MenuItemAdded>( x => 
                        {
                            x.ActorId = message.Id.ToString();
                            x.Name = message.Name;
                            x.Description = message.Description;
                        } );

                    return x => x.Acknowledge();
                }
            }
            catch (Exception ex)
            {
                return x => x.Reject( ex.ToString() );
            }
        }

        public HandeMenuItemAdd( IDocumentRepository couch, IMemoizer mementoProvider )
        {
            Couch = couch;
            MementoProvider = mementoProvider;
        }
    }
}
