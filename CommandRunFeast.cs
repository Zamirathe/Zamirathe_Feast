using System;
using System.Collections;
using System.Collections.Generic;

using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace ZaupFeast
{
    public class CommandRunFeast : IRocketCommand
    {
        public string Name
        {
            get { return "runfeast"; }
        }
        public string Help
        {
            get { return "Immediately starts the feast"; }
        }
        public bool AllowFromConsole
        {
            get { return true; }
        }
        public string Syntax
        {
            get { return ""; }
        }
        public List<string> Aliases
        {
            get { return new List<string> { "frun" }; }
        }
        public List<string> Permissions
        {
            get { return new List<string>(); }
        }
        // Run the command.
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedChat.Say(Feast.Instance.Translate("now_feast_msg", new object[] {
                Feast.Instance.nextLocation.Name
                }), UnturnedChat.GetColorFromName(Feast.Instance.Configuration.Instance.MessageColor, Color.red));
            Feast.Instance.runFeast();
        }
    }
}
