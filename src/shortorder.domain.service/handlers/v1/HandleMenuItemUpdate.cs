using System;
using shortoder.domain;
using shortorder.messages;
using Symbiote.Core.UnitOfWork;
using Symbiote.Messaging;

namespace shortorder.domain.service.handlers.v1
{
    public class HandleMenuItemUpdate : IHandle<MenuItem, UpdateMenuItem>
    {
        public Action<IEnvelope> Handle( MenuItem actor, UpdateMenuItem message )
        {
            try
            {
                using( var context = Context.CreateFor( actor ) ) 
                {
                    actor.Name = message.Name;
                    actor.Description = message.Description;

                    context
                        .PublishOnCommit<MenuItemChanged>( x =>
                        {
                            x.Id = message.Id;
                            x.Name = message.Name;
                            x.Description = message.Description;
                        });

                    return x => x.Acknowledge();
                }
            }
            catch (Exception e)
            {
                return x => x.Reject( e.ToString() );
            }
        }
    }
}