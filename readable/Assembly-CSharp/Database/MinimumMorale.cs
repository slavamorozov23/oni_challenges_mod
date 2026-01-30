using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F70 RID: 3952
	public class MinimumMorale : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D34 RID: 32052 RVA: 0x0031D733 File Offset: 0x0031B933
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE, this.minimumMorale);
		}

		// Token: 0x06007D35 RID: 32053 RVA: 0x0031D74F File Offset: 0x0031B94F
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE_DESCRIPTION, this.minimumMorale);
		}

		// Token: 0x06007D36 RID: 32054 RVA: 0x0031D76B File Offset: 0x0031B96B
		public MinimumMorale(int minimumMorale = 16)
		{
			this.minimumMorale = minimumMorale;
		}

		// Token: 0x06007D37 RID: 32055 RVA: 0x0031D77C File Offset: 0x0031B97C
		public override bool Success()
		{
			bool flag = true;
			foreach (object obj in Components.MinionAssignablesProxy)
			{
				GameObject targetGameObject = ((MinionAssignablesProxy)obj).GetTargetGameObject();
				if (targetGameObject != null && !targetGameObject.HasTag(GameTags.Dead))
				{
					AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(targetGameObject.GetComponent<MinionModifiers>());
					flag = (attributeInstance != null && attributeInstance.GetTotalValue() >= (float)this.minimumMorale && flag);
				}
			}
			return flag;
		}

		// Token: 0x06007D38 RID: 32056 RVA: 0x0031D824 File Offset: 0x0031BA24
		public void Deserialize(IReader reader)
		{
			this.minimumMorale = reader.ReadInt32();
		}

		// Token: 0x06007D39 RID: 32057 RVA: 0x0031D832 File Offset: 0x0031BA32
		public override string GetProgress(bool complete)
		{
			return this.Description();
		}

		// Token: 0x04005C2B RID: 23595
		public int minimumMorale;
	}
}
