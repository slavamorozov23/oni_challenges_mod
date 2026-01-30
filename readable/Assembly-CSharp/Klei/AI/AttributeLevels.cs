using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001025 RID: 4133
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/AttributeLevels")]
	public class AttributeLevels : KMonoBehaviour, ISaveLoadable
	{
		// Token: 0x06008043 RID: 32835 RVA: 0x00337EF1 File Offset: 0x003360F1
		public IEnumerator<AttributeLevel> GetEnumerator()
		{
			return this.levels.GetEnumerator();
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06008044 RID: 32836 RVA: 0x00337F03 File Offset: 0x00336103
		// (set) Token: 0x06008045 RID: 32837 RVA: 0x00337F0B File Offset: 0x0033610B
		public AttributeLevels.LevelSaveLoad[] SaveLoadLevels
		{
			get
			{
				return this.saveLoadLevels;
			}
			set
			{
				this.saveLoadLevels = value;
			}
		}

		// Token: 0x06008046 RID: 32838 RVA: 0x00337F14 File Offset: 0x00336114
		protected override void OnPrefabInit()
		{
			foreach (AttributeInstance attributeInstance in this.GetAttributes())
			{
				if (attributeInstance.Attribute.IsTrainable)
				{
					AttributeLevel attributeLevel = new AttributeLevel(attributeInstance);
					this.levels.Add(attributeLevel);
					attributeLevel.maxGainedLevel = this.maxAttributeLevel;
					attributeLevel.Apply(this);
				}
			}
		}

		// Token: 0x06008047 RID: 32839 RVA: 0x00337F90 File Offset: 0x00336190
		[OnSerializing]
		public void OnSerializing()
		{
			this.saveLoadLevels = new AttributeLevels.LevelSaveLoad[this.levels.Count];
			for (int i = 0; i < this.levels.Count; i++)
			{
				this.saveLoadLevels[i].attributeId = this.levels[i].attribute.Attribute.Id;
				this.saveLoadLevels[i].experience = this.levels[i].experience;
				this.saveLoadLevels[i].level = this.levels[i].level;
			}
		}

		// Token: 0x06008048 RID: 32840 RVA: 0x0033803C File Offset: 0x0033623C
		[OnDeserialized]
		public void OnDeserialized()
		{
			foreach (AttributeLevels.LevelSaveLoad levelSaveLoad in this.saveLoadLevels)
			{
				this.SetExperience(levelSaveLoad.attributeId, levelSaveLoad.experience);
				this.SetLevel(levelSaveLoad.attributeId, levelSaveLoad.level);
			}
		}

		// Token: 0x06008049 RID: 32841 RVA: 0x0033808C File Offset: 0x0033628C
		public int GetLevel(Attribute attribute)
		{
			foreach (AttributeLevel attributeLevel in this.levels)
			{
				if (attribute == attributeLevel.attribute.Attribute)
				{
					return attributeLevel.GetLevel();
				}
			}
			return 1;
		}

		// Token: 0x0600804A RID: 32842 RVA: 0x003380F4 File Offset: 0x003362F4
		public AttributeLevel GetAttributeLevel(string attribute_id)
		{
			foreach (AttributeLevel attributeLevel in this.levels)
			{
				if (attributeLevel.attribute.Attribute.Id == attribute_id)
				{
					return attributeLevel;
				}
			}
			return null;
		}

		// Token: 0x0600804B RID: 32843 RVA: 0x00338160 File Offset: 0x00336360
		public bool AddExperience(string attribute_id, float time_spent, float multiplier)
		{
			if (this.maxAttributeLevel == 0)
			{
				return false;
			}
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel == null)
			{
				global::Debug.LogWarning(attribute_id + " has no level.");
				return false;
			}
			time_spent *= multiplier;
			AttributeConverterInstance attributeConverterInstance = Db.Get().AttributeConverters.TrainingSpeed.Lookup(this);
			if (attributeConverterInstance != null)
			{
				float num = attributeConverterInstance.Evaluate();
				time_spent += time_spent * num;
			}
			bool result = attributeLevel.AddExperience(this, time_spent);
			attributeLevel.Apply(this);
			return result;
		}

		// Token: 0x0600804C RID: 32844 RVA: 0x003381D4 File Offset: 0x003363D4
		public void SetLevel(string attribute_id, int level)
		{
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel != null)
			{
				attributeLevel.SetLevel(level);
				attributeLevel.Apply(this);
			}
		}

		// Token: 0x0600804D RID: 32845 RVA: 0x003381FC File Offset: 0x003363FC
		public void SetExperience(string attribute_id, float experience)
		{
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel != null)
			{
				attributeLevel.SetExperience(experience);
				attributeLevel.Apply(this);
			}
		}

		// Token: 0x0600804E RID: 32846 RVA: 0x00338222 File Offset: 0x00336422
		public float GetPercentComplete(string attribute_id)
		{
			return this.GetAttributeLevel(attribute_id).GetPercentComplete();
		}

		// Token: 0x0600804F RID: 32847 RVA: 0x00338230 File Offset: 0x00336430
		public int GetMaxLevel()
		{
			int num = 0;
			foreach (AttributeLevel attributeLevel in this)
			{
				if (attributeLevel.GetLevel() > num)
				{
					num = attributeLevel.GetLevel();
				}
			}
			return num;
		}

		// Token: 0x0400613B RID: 24891
		private List<AttributeLevel> levels = new List<AttributeLevel>();

		// Token: 0x0400613C RID: 24892
		public int maxAttributeLevel = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL;

		// Token: 0x0400613D RID: 24893
		[Serialize]
		private AttributeLevels.LevelSaveLoad[] saveLoadLevels = new AttributeLevels.LevelSaveLoad[0];

		// Token: 0x02002725 RID: 10021
		[Serializable]
		public struct LevelSaveLoad
		{
			// Token: 0x0400AE64 RID: 44644
			public string attributeId;

			// Token: 0x0400AE65 RID: 44645
			public float experience;

			// Token: 0x0400AE66 RID: 44646
			public int level;
		}
	}
}
