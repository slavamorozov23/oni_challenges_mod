using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;

namespace Database
{
	// Token: 0x02000F89 RID: 3977
	public class EatXKCalProducedByY : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DAE RID: 32174 RVA: 0x0031EDAF File Offset: 0x0031CFAF
		public EatXKCalProducedByY(int numCalories, List<Tag> foodProducers)
		{
			this.numCalories = numCalories;
			this.foodProducers = foodProducers;
		}

		// Token: 0x06007DAF RID: 32175 RVA: 0x0031EDC8 File Offset: 0x0031CFC8
		public override bool Success()
		{
			List<string> list = new List<string>();
			foreach (ComplexRecipe complexRecipe in ComplexRecipeManager.Get().recipes)
			{
				foreach (Tag b in this.foodProducers)
				{
					using (List<Tag>.Enumerator enumerator3 = complexRecipe.fabricators.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current == b)
							{
								list.Add(complexRecipe.FirstResult.ToString());
							}
						}
					}
				}
			}
			return WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(list.Distinct<string>().ToList<string>()) / 1000f > (float)this.numCalories;
		}

		// Token: 0x06007DB0 RID: 32176 RVA: 0x0031EEE4 File Offset: 0x0031D0E4
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.foodProducers = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.foodProducers.Add(new Tag(name));
			}
			this.numCalories = reader.ReadInt32();
		}

		// Token: 0x06007DB1 RID: 32177 RVA: 0x0031EF34 File Offset: 0x0031D134
		public override string GetProgress(bool complete)
		{
			string text = "";
			for (int i = 0; i < this.foodProducers.Count; i++)
			{
				if (i != 0)
				{
					text += COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.PREPARED_SEPARATOR;
				}
				BuildingDef buildingDef = Assets.GetBuildingDef(this.foodProducers[i].Name);
				if (buildingDef != null)
				{
					text += buildingDef.Name;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_ITEM, text);
		}

		// Token: 0x04005C45 RID: 23621
		private int numCalories;

		// Token: 0x04005C46 RID: 23622
		private List<Tag> foodProducers;
	}
}
