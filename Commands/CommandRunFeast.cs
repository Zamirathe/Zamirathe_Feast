using System;
using System.Collections;
using System.Collections.Generic;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned.Plugins;
using Rocket.Core.Logging;

namespace Zamirathe_Feast.Commands{
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
            get { return "";}
        }
        public List<string> Aliases
        {
            get { return new List<string> { "frun" }; }
        }
        // Run the command.
        public void Execute(RocketPlayer caller, string[] command)
        {
            Utils.Respond(caller, "Starting the feast");
            Feast feast = Feast.Instance;
            feast.runFeast();
        }
    }
}