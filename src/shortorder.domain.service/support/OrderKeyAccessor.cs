﻿using System;
using shortoder.domain;
using Symbiote.Core;

namespace shortorder.domain.service
{
    public class OrderKeyAccessor : IKeyAccessor<Order>
    {
        public string GetId( Order actor )
        {
            return actor.Id.ToString();
        }

        public void SetId<TKey>( Order actor, TKey key )
        {
            actor.Id = Guid.Parse( key.ToString() );
        }
    }

    public class OrderMementoKeyAccessor : IKeyAccessor<OrderMemento>
    {
        public string GetId( OrderMemento actor )
        {
            return actor.Id.ToString();
        }

        public void SetId<TKey>( OrderMemento actor, TKey key )
        {
            actor.Id = Guid.Parse( key.ToString() );
            actor.DocumentId = key.ToString();
        }
    }
}