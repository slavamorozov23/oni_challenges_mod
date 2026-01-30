using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001024 RID: 4132
	public class AttributeLevel
	{
		// Token: 0x06008039 RID: 32825 RVA: 0x00337C20 File Offset: 0x00335E20
		public AttributeLevel(AttributeInstance attribute)
		{
			this.notification = new Notification(MISC.NOTIFICATIONS.LEVELUP.NAME, NotificationType.Good, new Func<List<Notification>, object, string>(AttributeLevel.OnLevelUpTooltip), null, true, 0f, null, null, null, true, false, false);
			this.attribute = attribute;
		}

		// Token: 0x0600803A RID: 32826 RVA: 0x00337C74 File Offset: 0x00335E74
		public int GetLevel()
		{
			return this.level;
		}

		// Token: 0x0600803B RID: 32827 RVA: 0x00337C7C File Offset: 0x00335E7C
		public void Apply(AttributeLevels levels)
		{
			Attributes attributes = levels.GetAttributes();
			if (this.modifier != null)
			{
				attributes.Remove(this.modifier);
				this.modifier = null;
			}
			this.modifier = new AttributeModifier(this.attribute.Id, (float)this.GetLevel(), DUPLICANTS.MODIFIERS.SKILLLEVEL.NAME, false, false, true);
			attributes.Add(this.modifier);
		}

		// Token: 0x0600803C RID: 32828 RVA: 0x00337CE1 File Offset: 0x00335EE1
		public void SetExperience(float experience)
		{
			this.experience = experience;
		}

		// Token: 0x0600803D RID: 32829 RVA: 0x00337CEA File Offset: 0x00335EEA
		public void SetLevel(int level)
		{
			this.level = level;
		}

		// Token: 0x0600803E RID: 32830 RVA: 0x00337CF4 File Offset: 0x00335EF4
		public float GetExperienceForNextLevel()
		{
			float num = Mathf.Pow((float)this.level / (float)this.maxGainedLevel, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600f;
			return Mathf.Pow(((float)this.level + 1f) / (float)this.maxGainedLevel, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600f - num;
		}

		// Token: 0x0600803F RID: 32831 RVA: 0x00337D56 File Offset: 0x00335F56
		public float GetPercentComplete()
		{
			return this.experience / this.GetExperienceForNextLevel();
		}

		// Token: 0x06008040 RID: 32832 RVA: 0x00337D68 File Offset: 0x00335F68
		public void LevelUp(AttributeLevels levels)
		{
			this.level++;
			this.experience = 0f;
			this.Apply(levels);
			this.experience = 0f;
			if (PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, this.attribute.modifier.Name, levels.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			}
			levels.GetComponent<Notifier>().Add(this.notification, string.Format(MISC.NOTIFICATIONS.LEVELUP.SUFFIX, this.attribute.modifier.Name, this.level));
			StateMachine.Instance instance = new UpgradeFX.Instance(levels.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, -0.1f));
			ReportManager.Instance.ReportValue(ReportManager.ReportType.LevelUp, 1f, levels.GetProperName(), null);
			instance.StartSM();
			levels.Trigger(-110704193, this.attribute.Id);
		}

		// Token: 0x06008041 RID: 32833 RVA: 0x00337E80 File Offset: 0x00336080
		public bool AddExperience(AttributeLevels levels, float experience)
		{
			if (this.level >= this.maxGainedLevel)
			{
				return false;
			}
			this.experience += experience;
			this.experience = Mathf.Max(0f, this.experience);
			if (this.experience >= this.GetExperienceForNextLevel())
			{
				this.LevelUp(levels);
				return true;
			}
			return false;
		}

		// Token: 0x06008042 RID: 32834 RVA: 0x00337ED9 File Offset: 0x003360D9
		private static string OnLevelUpTooltip(List<Notification> notifications, object data)
		{
			return MISC.NOTIFICATIONS.LEVELUP.TOOLTIP + notifications.ReduceMessages(false);
		}

		// Token: 0x04006135 RID: 24885
		public float experience;

		// Token: 0x04006136 RID: 24886
		public int level;

		// Token: 0x04006137 RID: 24887
		public AttributeInstance attribute;

		// Token: 0x04006138 RID: 24888
		public AttributeModifier modifier;

		// Token: 0x04006139 RID: 24889
		public Notification notification;

		// Token: 0x0400613A RID: 24890
		public int maxGainedLevel = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL;
	}
}
