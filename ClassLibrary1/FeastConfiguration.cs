using Rocket.API;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace Zamirathe_Feast
{
	public class FeastConfiguration : IRocketPluginConfiguration
	{
		public bool Enabled;
		public int minDropTime;
		public int maxDropTime;
		public byte dropRadius;
		public byte minItemsDrop;
		public byte maxItemsDrop;
        public bool skyDrop;
        public byte skyRadius;
        [XmlArray("Items"), XmlArrayItem("FeastItem")]
        /*public List<FeastItem> Items = new List<FeastItem>
                    {
                        new FeastItem(66, "Cloth", 10, new List<string>
                        {
                            "all"
                        })};*/
        public List<FeastItem> Items;
		public string msgComingFeast;
		public string msgNowFeast;
		public IRocketPluginConfiguration DefaultConfiguration
		{
			get
			{
				return new FeastConfiguration
				{
					Enabled = true,
					minDropTime = 600,
					maxDropTime = 1200,
					dropRadius = 20,
					minItemsDrop = 10,
					maxItemsDrop = 25,
                    skyDrop = false,
                    skyRadius = 10,
					Items = new List<FeastItem>
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
					},
					msgComingFeast = "The feast is beginning at {0} in {1} minutes!",
					msgNowFeast = "The feast is now at {0}!"
				};
			}
		}
	}
}
