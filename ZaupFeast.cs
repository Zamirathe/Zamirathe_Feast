using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Plugins;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Zamirathe_Feast
{
    internal class Feast : RocketPlugin<FeastConfiguration>
    {
        private DateTime lastFeast;
        private DateTime nextFeast;
        private List<Locs> maplocations;
        private List<Locs> locations;
        private List<FeastItem> items;
        private Locs nextLocation;
        private byte msgNum;
        private DateTime lastMsg;
        private bool running = false;
        private static Feast instance;

        private Feast()
        {
        }
        public static Feast Instance
        {
            get
            {
                if (instance == null) instance = new Feast();
                return instance;
            }
        }
        protected override void Load()
        {
            Feast.instance = this;
            this.locations = new List<Locs>();
            this.maplocations = new List<Locs>();
            if (LevelNodes.Nodes == null)
            {
                LevelNodes.load();
            }
            foreach (Node n in LevelNodes.Nodes)
            {
                Locs loc = new Locs(n.Position, ((NodeLocation)n).Name);
                maplocations.Add(loc);
            }
            List<string> usedlocs = new List<string>();
            if (this.Configuration.Items == null || !this.Configuration.Items.Any())
            {
                Logger.Log("Failed to load the configuration file.  Turned off feast.  Restart to try again.");
                return;
            };
            List<FeastItem> items = new List<FeastItem>();
            foreach (FeastItem f in this.Configuration.Items)
            {
                usedlocs = usedlocs.Concat(f.Location).ToList();
                ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.Item, f.Id);
                if (itemAsset != null && !itemAsset.Cosmetic)
                    items.Add(f);
            }
            this.items = items;
            List<string> locations = usedlocs.Distinct().ToList();
            if (locations.Contains("all") || locations.Contains("All"))
            {
                this.locations = maplocations;
            }
            else
            {
                foreach (Locs a in maplocations)
                {
                    if (locations.IndexOf(a.Name()) != -1)
                    {
                        this.locations.Add(a);
                    }
                }
            }
            this.running = true;
            this.resetFeast();
        }
        public void runFeast()
        {
            Feast feast = Feast.instance;
            int num = UnityEngine.Random.Range((int)feast.Configuration.minItemsDrop, (int)feast.Configuration.maxItemsDrop+1);
            List<int> list = new List<int>();
            foreach (FeastItem current in feast.items)
            {
                if (current.Location.Contains(feast.nextLocation.Name()) || !current.Location.Contains("all") || !current.Location.Contains("All")) 
                {
                    for (byte b = 0; b < current.Chance; b += 1)
                    {
                        list.Add(current.Id);
                    }
                }
            }
            for (int i = 0; i < num; i++)
            {
                int item = UnityEngine.Random.Range(0, list.Count);
                int num2 = list[item];
                Vector3 c = feast.nextLocation.Pos();
                c.x += (float)UnityEngine.Random.Range((int)feast.Configuration.dropRadius * -1, (int)feast.Configuration.dropRadius+1);
                c.z += (float)UnityEngine.Random.Range((int)feast.Configuration.dropRadius * -1, (int)feast.Configuration.dropRadius+1);
                if (feast.Configuration.skyDrop)
                {
                    c.y += (float)UnityEngine.Random.Range(1, (int)feast.Configuration.skyRadius);
                }
                Item g = new Item((ushort)num2, true);
                ItemManager.dropItem(g, c, false, true, true);
                list.RemoveAt(item);
            }
            feast.resetFeast();
        }
        private void checkFeast()
        {
            Feast feast = Feast.instance;
            if (feast.Configuration.Enabled)
            {
                if ((feast.nextFeast - DateTime.Now).TotalSeconds - 300.0 <= 0.0 && (DateTime.Now - feast.lastMsg).TotalSeconds >= 60.0)
                {
                    byte b = feast.msgNum;
                    if (b != 0)
                    {
                        RocketChat.Say(string.Format(feast.Configuration.msgComingFeast, feast.nextLocation.Name(), feast.msgNum));
                        feast.lastMsg = DateTime.Now;
                        feast.msgNum -= 1;
                    }
                    else
                    {
                        RocketChat.Say(string.Format(feast.Configuration.msgNowFeast, feast.nextLocation.Name()));
                        feast.lastMsg = DateTime.Now;
                        feast.runFeast();
                    }
                }
            }
        }
        public void resetFeast()
        {
            Feast feast = Feast.instance;
            feast.lastFeast = DateTime.Now;
            feast.nextFeast = feast.lastFeast.AddSeconds((double)UnityEngine.Random.Range(feast.Configuration.minDropTime, feast.Configuration.maxDropTime+1));
            feast.msgNum = 5;
            feast.lastMsg = DateTime.Now;
            feast.nextLocation = feast.locations[UnityEngine.Random.Range(0, feast.locations.Count)];
            Logger.Log("The next feast will be at " + feast.nextLocation.Name() + " at " + feast.nextFeast);
        }
        public void FixedUpdate()
        {
            Feast feast = Feast.instance;
            if (feast.Configuration.Enabled && feast.running) feast.checkFeast();
        }
        public List<Locs> getMapLocations()
        {
            return maplocations;
        }
        public List<Locs> getLocations()
        {
            return locations;
        }
        public void setNextLocation(Locs loc)
        {
           nextLocation = loc;
        }
    }
}
