using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Xml.Serialization;

using Rocket.API;
using Rocket.Core.Tasks;
using Rocket.Unturned;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using Rocket.Unturned.Plugins;
using SDG.Unturned;
using UnityEngine;

namespace ZaupFeast
{
    internal class Feast : RocketPlugin<FeastConfiguration>
    {
        public static Feast Instance;
        private DateTime nextFeast;
        private List<Locs> locations;
        internal Locs nextLocation;
        private byte msgNum;
        private DateTime lastMsg;
        private bool running = false;
        private System.Timers.Timer Timer;
        private System.Timers.Timer Timer2;
        private byte effectNum;
        private List<Vector3> drops;
        private List<Item> items;
        private int numItems;

        public override Dictionary<string, string> DefaultTranslations
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {
                        "coming_feast_msg",
                        "The feast is beginning at {0} in {1} minutes!"
                    },
                    {
                        "now_feast_msg",
                        "The feast is now at {0}!"
                    }
                };
            }
        }
        
        protected override void Load()
        {
            // Make this able to be used a singleton
            Feast.Instance = this;

            // Set some variables to empty to start.
            this.drops = new List<Vector3>();
            this.items = new List<Item>();
            this.numItems = 0;
            this.Timer = new System.Timers.Timer(11500);
            this.Timer.Elapsed += this.Timer_Elapsed;
            this.Timer.AutoReset = false;
            this.Timer2 = new System.Timers.Timer(12800);
            this.Timer2.Elapsed += this.Timer_Elapsed;
            this.Timer2.AutoReset = false;
            this.effectNum = 0;

            // Did we load the configuration file?  If not, just end.
            if (this.Configuration.Items == null || !this.Configuration.Items.Any())
            {
                Logger.Log("Failed to load the configuration file.  Turned off feast.  Restart to try again.");
                return;
            }

            // Get all the map locations.
            this.locations = new List<Locs>();
            if (LevelNodes.Nodes == null)
                LevelNodes.load();
            foreach (Node n in LevelNodes.Nodes)
            {
                Locs loc = new Locs(n.Position, ((NodeLocation)n).Name);
                this.locations.Add(loc);
            }

            // Get all the locations used by the items and remove any invalid items.
            List<string> usedlocs = new List<string>();
            foreach (FeastItem f in this.Configuration.Items)
            {
                ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, f.Id);
                if (itemAsset == null && itemAsset.Cosmetic)
                {
                    this.Configuration.Items.Remove(f);
                }
                else
                {
                    usedlocs = usedlocs.Concat(f.Location).ToList();
                }
            }

            // Remove any unused map locations from the location list.
            List<string> locations = usedlocs.Distinct().ToList();
            if (!locations.Contains("all") && !locations.Contains("All"))
            {
                foreach (Locs a in this.locations)
                {
                    if (locations.IndexOf(a.Name) == -1)
                    {
                        this.locations.Remove(a);
                    }
                }
            }

            // Now that it is all set up, we'll all the plugin to start running.  This was just to keep the FixedUpdate from running before the top finished running.
            this.running = true;
            this.resetFeast();
        }

        protected override void Unload()
        {
            Feast.Instance.Timer.Close();
        }

        internal void runFeast()
        {
            // Get how many items are going to drop and setup the item id list.
            int itemsNum = UnityEngine.Random.Range((int)Feast.Instance.Configuration.MinItemsDrop, (int)Feast.Instance.Configuration.MaxItemsDrop+1);
            List<int> list = new List<int>();
            foreach (FeastItem current in Feast.Instance.Configuration.Items)
            {
                if (current.Location.Contains(Feast.Instance.nextLocation.Name) || current.Location.Contains("all") || current.Location.Contains("All")) 
                {
                    for (byte b = 0; b < current.Chance; b++)
                    {
                        list.Add(current.Id);
                    }
                }
            }

            // Figure out the base point of the feast drop.
            Vector3 p = Feast.Instance.nextLocation.Pos;
            p.y = 500;
            RaycastHit hit;
            Physics.Raycast(p, Vector3.down, out hit, 500);
            p = hit.point;

            // Get the ending item positions and items
            List<Vector3> drops = new List<Vector3>();
            List<Item> items = new List<Item>();
            for (int i = 0; i < itemsNum; i++)
            {
                int item = UnityEngine.Random.Range(0, list.Count);
                int num2 = list[item];
                Vector3 pos = new Vector3();
                pos.x += p.x + (float)UnityEngine.Random.Range((int)Feast.Instance.Configuration.DropRadius * -1, (int)Feast.Instance.Configuration.DropRadius + 1);
                pos.z += p.z + (float)UnityEngine.Random.Range((int)Feast.Instance.Configuration.DropRadius * -1, (int)Feast.Instance.Configuration.DropRadius + 1);
                pos.y += p.y + 1f;
                Item g = new Item((ushort)num2, true);
                //drops[i] = pos;
                drops.Add(pos);
                //items[i] = g;
                items.Add(g);
                list.RemoveAt(item);
            }
            Feast.Instance.numItems = itemsNum;
            Feast.Instance.drops = drops;
            Feast.Instance.items = items;
            // See if skydrop is turned on.  If it is, let's start the effects.
            
            if (Feast.Instance.Configuration.SkyDrop)
            {
                // Trigger timer for effects
                RocketEffect plane = RocketEffectManager.GetEffectsById(Feast.Instance.Configuration.PlaneEffectId);
                Vector3 loc = new Vector3(Feast.Instance.nextLocation.Pos.x, Feast.Instance.nextLocation.Pos.y + 50, Feast.Instance.nextLocation.Pos.z + 500);
                plane.Trigger(loc);
                Feast.Instance.effectNum = 1;
                Feast.Instance.Timer.Start();
            }
            else
            {
                // No effects, just spawn in the items on the ground.
                this.spawnItems(this.items, drops);
            }
            
            // Reset the feast for the next round.
            Feast.Instance.resetFeast();
        }

        private void skyDrop(List<Vector3> pos)
        {
            RocketEffect skyDrop = RocketEffectManager.GetEffectsById(Feast.Instance.Configuration.SkydropEffectId);
            foreach (Vector3 p in pos)
            {
                skyDrop.Trigger(new Vector3(p.x, p.y + 45, p.z));
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Feast.Instance.effectNum == 1)
            {
                Feast.Instance.Timer.Stop();
                skyDrop(Feast.Instance.drops);
                Feast.Instance.effectNum = 2;
                Feast.Instance.Timer2.Start();
            }
            else
            {
                Feast.Instance.Timer2.Stop();
                Feast.Instance.spawnItems(Feast.Instance.items, Feast.Instance.drops);
                Feast.Instance.effectNum = 1;
            }
        }

        private void spawnItems(List<Item> items, List<Vector3> drops)
        {
            if (items.Count != drops.Count)
            {
                Logger.Log("Something went wrong.  Feast didn't drop!");
                return;
            }
            for (int d = 0; d < items.Count; d++ ) {
                ItemManager.dropItem(items[d], drops[d], false, true, true);
            }
        }

        private void checkFeast()
        {
            if (Feast.Instance.Configuration.Enabled)
            {
                if ((Feast.Instance.nextFeast - DateTime.Now).TotalSeconds - 300.0 <= 0.0 && (DateTime.Now - Feast.Instance.lastMsg).TotalSeconds >= 60.0)
                {
                    byte b = Feast.Instance.msgNum;
                    if (b != 0)
                    {
                        RocketChat.Say(Feast.Instance.Translate("coming_feast_msg", new object[] {
                            Feast.Instance.nextLocation.Name,
                            Feast.Instance.msgNum
                        }), RocketChat.GetColorFromName(Feast.Instance.Configuration.MessageColor, Color.red));
                        Feast.Instance.lastMsg = DateTime.Now;
                        Feast.Instance.msgNum -= 1;
                    }
                    else
                    {
                        RocketChat.Say(Feast.Instance.Translate("now_feast_msg", new object[] {
                            Feast.Instance.nextLocation.Name
                        }), RocketChat.GetColorFromName(Feast.Instance.Configuration.MessageColor, Color.red));
                        Feast.Instance.lastMsg = DateTime.Now;
                        Feast.Instance.runFeast();
                    }
                }
            }
        }
        public void resetFeast()
        {
            Feast.Instance.nextFeast = DateTime.Now.AddSeconds((double)UnityEngine.Random.Range(Feast.Instance.Configuration.MinDropTime, Feast.Instance.Configuration.MaxDropTime + 1));
            Feast.Instance.msgNum = 5;
            Feast.Instance.lastMsg = DateTime.Now;
            Feast.Instance.nextLocation = Feast.Instance.locations[UnityEngine.Random.Range(0, Feast.Instance.locations.Count)];
            Logger.Log("The next feast will be at " + Feast.Instance.nextLocation.Name + " at " + Feast.Instance.nextFeast);
        }
        public void FixedUpdate()
        {
            if (Feast.Instance.Configuration.Enabled && Feast.Instance.running) 
                Feast.Instance.checkFeast();
        }
    }

    internal class Locs
    {
        private Vector3 v;
        private string n;
        public Vector3 Pos
        {
            get
            {
                return this.v;
            }
        }
        public string Name
        {
            get
            {
                return this.n;
            }
        }
        public Locs(Vector3 Pos, string Name)
        {
            this.v = Pos;
            this.n = Name;
        }
    }

    public class FeastItem
    {
        public ushort Id;
        public string Name;
        public byte Chance;
        [XmlArray(ElementName = "Locations")]
        public List<string> Location;
        public FeastItem(ushort id, string name, byte chance, List<string> locs)
        {
            this.Id = id;
            this.Name = name;
            this.Chance = chance;
            this.Location = locs;
        }
        public FeastItem()
        {
        }
    }
}
