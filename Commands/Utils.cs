using System;
using System.Collections;
using System.Collections.Generic;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned.Plugins;
using Rocket.Core.Logging;

namespace Zamirathe_Feast.Commands
{
    public static class Utils
    {
        public static void Respond(RocketPlayer caller, String message)
        {
            if (caller == null){
                Logger.Log(message);
            }else{
                RocketChat.Say(caller, message);
            }
        }
    }
}