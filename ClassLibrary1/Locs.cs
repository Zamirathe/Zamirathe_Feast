using System;
using UnityEngine;
namespace Zamirathe_Feast
{
	public class Locs
	{
		private Vector3 v;
		private string n;
		public Vector3 Pos()
		{
			return this.v;
		}
		public string Name()
		{
			return this.n;
		}
		public Locs(Vector3 Pos, string Name)
		{
			this.v = Pos;
			this.n = Name;
		}
	}
}
