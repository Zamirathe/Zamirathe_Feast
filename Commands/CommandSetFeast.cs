using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned.Plugins;
using Rocket.Core.Logging;

namespace Zamirathe_Feast.Commands
{
    public class CommandSetFeast : IRocketCommand
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
                return "setfeast";
            }
        }
        public string Help
        {
            get
            {
                return "Set the next feast to location. Use parameter f to force a location not in config.";
            }
        }
        public string Syntax
        {
            get { return ""; }
        }
        public List<string> Aliases
        {
            get { return new List<string> { "fset" }; }
        }
        // Run the command.
        public class loc
        {
           internal bool isLocation;
           internal bool isMapLocation;
           internal Locs listLoc;

            public loc(bool a, bool b, Locs c)
            {
                isLocation = a;
                isMapLocation = b;
                listLoc = c;
            }
        }

        public loc CheckLoc(string location_name)
        {
            Feast feast = Feast.Instance;

            Locs locResultObj = feast.getLocations().FirstOrDefault(a => a.Name() == location_name);
            Locs mapResultObj = feast.getMapLocations().FirstOrDefault(b => b.Name() == location_name);

            bool locResult = (locResultObj != null);
            bool mapResult = (mapResultObj != null);

            return new loc(locResult, mapResult, mapResultObj);
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            Feast feast = Feast.Instance;


            if (command.Length > 2 || command.Length == 0)
            {
                // Not enough, or too many params.
                Utils.Respond(caller, "Wrong usage of command setfeast.");
            }
            else if (command.Length == 2)
            {
                // Two params. Is one of them f? 
                if (command[1] != "f" && command[0] != "f")
                {
                    // Nope, that's no good.
                   Utils.Respond(caller, "Wrong usage of command setfeast.");
                }
                else
                {
                    string location = (command[0]=="f") ? command[1] : command[0];
                    loc locReturn = CheckLoc(location);
                    if (locReturn.isMapLocation)
                    {
                        Utils.Respond(caller, "Forcing feast location of " + location+".");
                        feast.setNextLocation(locReturn.listLoc);
                    }
                    else
                    {
                        Utils.Respond(caller, "Location not on map.");
                    }

                }
            }
            else
            {
                // One parameter.
                string location = command[0];
                loc locReturn = CheckLoc(location);
                if (locReturn.isLocation)
                {
                    Utils.Respond(caller, "Setting the feast to "+locReturn.listLoc.Name());
                    feast.setNextLocation(locReturn.listLoc);
                }
                else
                if (locReturn.isMapLocation)
                {
                    Utils.Respond(caller, "Location not a part of Feast locations. Use f parameter to force.");
                }
                else
                {
                    Utils.Respond(caller, "Location not on map.");
                }
            }
        }
    }

}