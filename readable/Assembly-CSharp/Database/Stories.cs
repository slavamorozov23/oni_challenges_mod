using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F61 RID: 3937
	public class Stories : ResourceSet<Story>
	{
		// Token: 0x06007CEB RID: 31979 RVA: 0x003195E8 File Offset: 0x003177E8
		public Stories(ResourceSet parent) : base("Stories", parent)
		{
			this.MegaBrainTank = base.Add(new Story("MegaBrainTank", "storytraits/MegaBrainTank", 0, 1, 43, "storytraits/mega_brain_tank").SetKeepsake("keepsake_megabrain"));
			this.CreatureManipulator = base.Add(new Story("CreatureManipulator", "storytraits/CritterManipulator", 1, 2, 43, "storytraits/creature_manipulator_retrofit").SetKeepsake("keepsake_crittermanipulator"));
			this.LonelyMinion = base.Add(new Story("LonelyMinion", "storytraits/LonelyMinion", 2, 3, 44, "storytraits/lonelyminion_retrofit").SetKeepsake("keepsake_lonelyminion"));
			this.FossilHunt = base.Add(new Story("FossilHunt", "storytraits/FossilHunt", 3, 4, 44, "storytraits/fossil_hunt_retrofit").SetKeepsake("keepsake_fossilhunt"));
			this.MorbRoverMaker = base.Add(new Story("MorbRoverMaker", "storytraits/MorbRoverMaker", 4, 5, 50, "storytraits/morb_rover_maker_retrofit").SetKeepsake("keepsake_morbrovermaker"));
			this.HijackedHeadquarters = base.Add(new Story("HijackHeadquarters", "storytraits/HijackHeadquarters", 5, 6, 57, "storytraits/hijack_headquarters_retrofit").SetKeepsake("keepsake_hijackheadquarters"));
			this.resources.Sort();
		}

		// Token: 0x06007CEC RID: 31980 RVA: 0x00319720 File Offset: 0x00317920
		public void AddStoryMod(Story mod)
		{
			mod.kleiUseOnlyCoordinateOrder = -1;
			base.Add(mod);
			this.resources.Sort();
		}

		// Token: 0x06007CED RID: 31981 RVA: 0x0031973C File Offset: 0x0031793C
		public int GetHighestCoordinate()
		{
			int num = 0;
			foreach (Story story in this.resources)
			{
				num = Mathf.Max(num, story.kleiUseOnlyCoordinateOrder);
			}
			return num;
		}

		// Token: 0x06007CEE RID: 31982 RVA: 0x00319798 File Offset: 0x00317998
		public WorldTrait GetStoryTrait(string id, bool assertMissingTrait = false)
		{
			Story story = this.resources.Find((Story x) => x.Id == id);
			if (story != null)
			{
				return SettingsCache.GetCachedStoryTrait(story.worldgenStoryTraitKey, assertMissingTrait);
			}
			return null;
		}

		// Token: 0x06007CEF RID: 31983 RVA: 0x003197DC File Offset: 0x003179DC
		public Story GetStoryFromStoryTrait(string storyTraitTemplate)
		{
			return this.resources.Find((Story x) => x.worldgenStoryTraitKey == storyTraitTemplate);
		}

		// Token: 0x06007CF0 RID: 31984 RVA: 0x0031980D File Offset: 0x00317A0D
		public List<Story> GetStoriesSortedByCoordinateOrder()
		{
			List<Story> list = new List<Story>(this.resources);
			list.Sort((Story s1, Story s2) => s1.kleiUseOnlyCoordinateOrder.CompareTo(s2.kleiUseOnlyCoordinateOrder));
			return list;
		}

		// Token: 0x04005BBE RID: 23486
		public Story MegaBrainTank;

		// Token: 0x04005BBF RID: 23487
		public Story CreatureManipulator;

		// Token: 0x04005BC0 RID: 23488
		public Story LonelyMinion;

		// Token: 0x04005BC1 RID: 23489
		public Story FossilHunt;

		// Token: 0x04005BC2 RID: 23490
		public Story MorbRoverMaker;

		// Token: 0x04005BC3 RID: 23491
		public Story HijackedHeadquarters;
	}
}
