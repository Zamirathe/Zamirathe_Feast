using System;
using System.Collections;
using System.Collections.Generic;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned.Plugins;
using Rocket.Core.Logging;
using UnityEngine;
using SDG.Unturned;

namespace ZaupFeast
{
    public class CommandRunFeast : IRocketCommand
    {
        public bool RunFromConsole
        {
            get
            {
                return true;
            }
        }
        public string Name
        {
            get
            {
                return "runfeast";
            }
        }
        public string Help
        {
            get
            {
                return "Immediately starts the feast";
            }
        }
        public string Syntax
        {
            get 
            {
                return ""; 
            }
        }
        public List<string> Aliases
        {
            get { return new List<string> { "frun" }; }
        }
        // Run the command.
        public void Execute(RocketPlayer caller, string[] command)
        {
            RocketChat.Say(Feast.Instance.Translate("now_feast_msg", new object[] {
                Feast.Instance.nextLocation.Name
                }), RocketChat.GetColorFromName(Feast.Instance.Configuration.MessageColor, Color.red));
            Feast.Instance.runFeast();
        }
    }
}
