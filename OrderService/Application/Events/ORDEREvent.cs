﻿using Orders.Application.Models;
using MicroRabbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.Application.Events
{
    public class ORDEREvent : Event
    {
        public CreateOrder CreateOrder { get; set; }
        public ORDEREvent(CreateOrder createOrder)
        {
            CreateOrder = createOrder;
        }
    }
}