using System;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02000F2C RID: 3884
	public class ChoreGroups : ResourceSet<ChoreGroup>
	{
		// Token: 0x06007C3A RID: 31802 RVA: 0x00309528 File Offset: 0x00307728
		private ChoreGroup Add(string id, string name, Klei.AI.Attribute attribute, string sprite, int default_personal_priority, bool user_prioritizable = true)
		{
			ChoreGroup choreGroup = new ChoreGroup(id, name, attribute, sprite, default_personal_priority, user_prioritizable);
			base.Add(choreGroup);
			return choreGroup;
		}

		// Token: 0x06007C3B RID: 31803 RVA: 0x00309550 File Offset: 0x00307750
		public ChoreGroups(ResourceSet parent) : base("ChoreGroups", parent)
		{
			this.Combat = this.Add("Combat", DUPLICANTS.CHOREGROUPS.COMBAT.NAME, Db.Get().Attributes.Digging, "icon_errand_combat", 5, true);
			this.LifeSupport = this.Add("LifeSupport", DUPLICANTS.CHOREGROUPS.LIFESUPPORT.NAME, Db.Get().Attributes.LifeSupport, "icon_errand_life_support", 5, true);
			this.Toggle = this.Add("Toggle", DUPLICANTS.CHOREGROUPS.TOGGLE.NAME, Db.Get().Attributes.Toggle, "icon_errand_toggle", 5, true);
			this.MedicalAid = this.Add("MedicalAid", DUPLICANTS.CHOREGROUPS.MEDICALAID.NAME, Db.Get().Attributes.Caring, "icon_errand_care", 4, true);
			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				this.Rocketry = this.Add("Rocketry", DUPLICANTS.CHOREGROUPS.ROCKETRY.NAME, Db.Get().Attributes.SpaceNavigation, "icon_errand_rocketry", 4, true);
			}
			this.Basekeeping = this.Add("Basekeeping", DUPLICANTS.CHOREGROUPS.BASEKEEPING.NAME, Db.Get().Attributes.Strength, "icon_errand_tidy", 4, true);
			this.Cook = this.Add("Cook", DUPLICANTS.CHOREGROUPS.COOK.NAME, Db.Get().Attributes.Cooking, "icon_errand_cook", 3, true);
			this.Art = this.Add("Art", DUPLICANTS.CHOREGROUPS.ART.NAME, Db.Get().Attributes.Art, "icon_errand_art", 3, true);
			this.Research = this.Add("Research", DUPLICANTS.CHOREGROUPS.RESEARCH.NAME, Db.Get().Attributes.Learning, "icon_errand_research", 3, true);
			this.MachineOperating = this.Add("MachineOperating", DUPLICANTS.CHOREGROUPS.MACHINEOPERATING.NAME, Db.Get().Attributes.Machinery, "icon_errand_operate", 3, true);
			this.Farming = this.Add("Farming", DUPLICANTS.CHOREGROUPS.FARMING.NAME, Db.Get().Attributes.Botanist, "icon_errand_farm", 3, true);
			this.Ranching = this.Add("Ranching", DUPLICANTS.CHOREGROUPS.RANCHING.NAME, Db.Get().Attributes.Ranching, "icon_errand_ranch", 3, true);
			this.Build = this.Add("Build", DUPLICANTS.CHOREGROUPS.BUILD.NAME, Db.Get().Attributes.Construction, "icon_errand_toggle", 2, true);
			this.Dig = this.Add("Dig", DUPLICANTS.CHOREGROUPS.DIG.NAME, Db.Get().Attributes.Digging, "icon_errand_dig", 2, true);
			this.Hauling = this.Add("Hauling", DUPLICANTS.CHOREGROUPS.HAULING.NAME, Db.Get().Attributes.Strength, "icon_errand_supply", 1, true);
			this.Storage = this.Add("Storage", DUPLICANTS.CHOREGROUPS.STORAGE.NAME, Db.Get().Attributes.Strength, "icon_errand_storage", 1, true);
			this.Recreation = this.Add("Recreation", DUPLICANTS.CHOREGROUPS.RECREATION.NAME, Db.Get().Attributes.Strength, "icon_errand_storage", 1, false);
			Debug.Assert(true);
		}

		// Token: 0x06007C3C RID: 31804 RVA: 0x003098B8 File Offset: 0x00307AB8
		public ChoreGroup FindByHash(HashedString id)
		{
			ChoreGroup result = null;
			foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
			{
				if (choreGroup.IdHash == id)
				{
					result = choreGroup;
					break;
				}
			}
			return result;
		}

		// Token: 0x04005840 RID: 22592
		public ChoreGroup Build;

		// Token: 0x04005841 RID: 22593
		public ChoreGroup Basekeeping;

		// Token: 0x04005842 RID: 22594
		public ChoreGroup Cook;

		// Token: 0x04005843 RID: 22595
		public ChoreGroup Art;

		// Token: 0x04005844 RID: 22596
		public ChoreGroup Dig;

		// Token: 0x04005845 RID: 22597
		public ChoreGroup Research;

		// Token: 0x04005846 RID: 22598
		public ChoreGroup Farming;

		// Token: 0x04005847 RID: 22599
		public ChoreGroup Ranching;

		// Token: 0x04005848 RID: 22600
		public ChoreGroup Hauling;

		// Token: 0x04005849 RID: 22601
		public ChoreGroup Storage;

		// Token: 0x0400584A RID: 22602
		public ChoreGroup MachineOperating;

		// Token: 0x0400584B RID: 22603
		public ChoreGroup MedicalAid;

		// Token: 0x0400584C RID: 22604
		public ChoreGroup Combat;

		// Token: 0x0400584D RID: 22605
		public ChoreGroup LifeSupport;

		// Token: 0x0400584E RID: 22606
		public ChoreGroup Toggle;

		// Token: 0x0400584F RID: 22607
		public ChoreGroup Recreation;

		// Token: 0x04005850 RID: 22608
		public ChoreGroup Rocketry;
	}
}
