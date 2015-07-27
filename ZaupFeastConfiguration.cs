using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Rocket.API;

namespace ZaupFeast
{
	public class FeastConfiguration : IRocketPluginConfiguration
	{
        public bool Enabled = true;
		public int MinDropTime = 600;
		public int MaxDropTime = 1200;
		public byte DropRadius = 20;
		public byte MinItemsDrop = 10;
		public byte MaxItemsDrop = 25;
        public bool SkyDrop = false;
        public ushort PlaneEffectId = 1001;
        public ushort SkydropEffectId = 1006;
        public string MessageColor = "red";
        public List<FeastItem> Items = new List<FeastItem>
		{
			new FeastItem(66, "Cloth", 10, new List<string>
			{
				"all"
			}),
			new FeastItem(43, "Military Ammunition Box", 10, new List<string>
			{
				"all"
			}),
			new FeastItem(44, "Civilian Ammunition Box", 10, new List<string>
			{
				"all"
			}),
			new FeastItem(13, "Canned Beans", 10, new List<string>
			{
				"all"
			}),
			new FeastItem(14, "Bottled Water", 10, new List<string>
			{
				"all"
			}),
			new FeastItem(10, "Police Vest", 10, new List<string>
			{
				"all"
			}),
			new FeastItem(251, "White Travelpack", 10, new List<string>
			{
				"all"
			}),
			new FeastItem(223, "Police Top", 10, new List<string>
			{
				"all"
			}),
			new FeastItem(224, "Police Bottom", 10, new List<string>
			{
				"all"
			}),
			new FeastItem(366, "Maple Crate", 10, new List<string>
			{
				"all"
			})
		};
	}
}
