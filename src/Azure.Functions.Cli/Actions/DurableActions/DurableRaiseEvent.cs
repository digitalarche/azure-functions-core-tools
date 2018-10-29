﻿using System;
using System.Threading.Tasks;
using Azure.Functions.Cli.Interfaces;
using Fclp;


namespace Azure.Functions.Cli.Actions.DurableActions
{
    [Action(Name = "raise-event", Context = Context.Durable, HelpText = "Raises an event to a specified orchestration instance")]
    class DurableRaiseEvent : BaseDurableAction
    {
        public string EventName { get; set; }

        public object EventData { get; set; }

        private readonly IDurableManager _durableManager;

        public DurableRaiseEvent(IDurableManager durableManager)
        {
            _durableManager = durableManager;
        }

        public override ICommandLineParserResult ParseArgs(string[] args)
        {
            Parser
                 .Setup<string>("eventName")
                 .WithDescription("Name of the event to raise")
                 .SetDefault($"Event_{(Guid.NewGuid().ToString("N"))}")
                 .Callback(n => EventName = n);
            Parser
               .Setup<string>("eventData")
               .WithDescription("Data to be passed to the event")
               .SetDefault(null)
               .Callback(d => EventData = d);

            return base.ParseArgs(args);
        }

        public override async Task RunAsync()
        {
            await _durableManager.RaiseEvent(Id, EventName, EventData);
        }
    }
}