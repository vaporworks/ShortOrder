using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Events;

namespace shortorder.wpf.cookui.Events
{
    public class OrderMadeEvent : CompositePresentationEvent<OrderMadeDefinition>
    {
        
    }

    public class OrderRankedEvent : CompositePresentationEvent<OrderRankedDefinition>
    {
        
    }

    public class OrderRankedDefinition
    {
        //public Guid 
    }
}