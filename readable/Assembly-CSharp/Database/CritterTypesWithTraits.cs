using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000F83 RID: 3971
	public class CritterTypesWithTraits : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D93 RID: 32147 RVA: 0x0031E624 File Offset: 0x0031C824
		public CritterTypesWithTraits(List<Tag> critterTypes) : this(critterTypes, true)
		{
		}

		// Token: 0x06007D94 RID: 32148 RVA: 0x0031E630 File Offset: 0x0031C830
		public CritterTypesWithTraits(List<Tag> critterTypes, bool allRequired)
		{
			foreach (Tag key in critterTypes)
			{
				if (!this.critterTypesToCheck.ContainsKey(key))
				{
					this.critterTypesToCheck.Add(key, false);
				}
			}
			this.hasTrait = false;
			this.allRequired = allRequired;
			this.trait = GameTags.Creatures.Wild;
		}

		// Token: 0x06007D95 RID: 32149 RVA: 0x0031E6D0 File Offset: 0x0031C8D0
		public override bool Success()
		{
			HashSet<Tag> tamedCritterTypes = SaveGame.Instance.ColonyAchievementTracker.tamedCritterTypes;
			bool flag = this.allRequired;
			foreach (KeyValuePair<Tag, bool> keyValuePair in this.critterTypesToCheck)
			{
				if (this.allRequired)
				{
					flag = (flag && tamedCritterTypes.Contains(keyValuePair.Key));
				}
				else
				{
					flag = (flag || tamedCritterTypes.Contains(keyValuePair.Key));
				}
			}
			this.UpdateSavedState();
			return flag;
		}

		// Token: 0x06007D96 RID: 32150 RVA: 0x0031E76C File Offset: 0x0031C96C
		public void UpdateSavedState()
		{
			this.revisedCritterTypesToCheckState.Clear();
			HashSet<Tag> tamedCritterTypes = SaveGame.Instance.ColonyAchievementTracker.tamedCritterTypes;
			foreach (KeyValuePair<Tag, bool> keyValuePair in this.critterTypesToCheck)
			{
				this.revisedCritterTypesToCheckState.Add(keyValuePair.Key, tamedCritterTypes.Contains(keyValuePair.Key));
			}
			foreach (KeyValuePair<Tag, bool> keyValuePair2 in this.revisedCritterTypesToCheckState)
			{
				this.critterTypesToCheck[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}

		// Token: 0x06007D97 RID: 32151 RVA: 0x0031E848 File Offset: 0x0031CA48
		public void Deserialize(IReader reader)
		{
			this.critterTypesToCheck = new Dictionary<Tag, bool>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				bool value = reader.ReadByte() > 0;
				this.critterTypesToCheck.Add(new Tag(name), value);
			}
			this.hasTrait = (reader.ReadByte() > 0);
			this.trait = GameTags.Creatures.Wild;
		}

		// Token: 0x04005C38 RID: 23608
		public Dictionary<Tag, bool> critterTypesToCheck = new Dictionary<Tag, bool>();

		// Token: 0x04005C39 RID: 23609
		private Tag trait;

		// Token: 0x04005C3A RID: 23610
		private bool hasTrait;

		// Token: 0x04005C3B RID: 23611
		private bool allRequired = true;

		// Token: 0x04005C3C RID: 23612
		private Dictionary<Tag, bool> revisedCritterTypesToCheckState = new Dictionary<Tag, bool>();
	}
}
