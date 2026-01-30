using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001027 RID: 4135
	public class Attributes
	{
		// Token: 0x06008067 RID: 32871 RVA: 0x0033859E File Offset: 0x0033679E
		public IEnumerator<AttributeInstance> GetEnumerator()
		{
			return this.AttributeTable.GetEnumerator();
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06008068 RID: 32872 RVA: 0x003385B0 File Offset: 0x003367B0
		public int Count
		{
			get
			{
				return this.AttributeTable.Count;
			}
		}

		// Token: 0x06008069 RID: 32873 RVA: 0x003385BD File Offset: 0x003367BD
		public Attributes(GameObject game_object)
		{
			this.gameObject = game_object;
		}

		// Token: 0x0600806A RID: 32874 RVA: 0x003385D8 File Offset: 0x003367D8
		public AttributeInstance Add(Attribute attribute)
		{
			AttributeInstance attributeInstance = this.Get(attribute.Id);
			if (attributeInstance == null)
			{
				attributeInstance = new AttributeInstance(this.gameObject, attribute);
				this.AttributeTable.Add(attributeInstance);
			}
			return attributeInstance;
		}

		// Token: 0x0600806B RID: 32875 RVA: 0x00338610 File Offset: 0x00336810
		public void Add(AttributeModifier modifier)
		{
			AttributeInstance attributeInstance = this.Get(modifier.AttributeId);
			if (attributeInstance != null)
			{
				attributeInstance.Add(modifier);
			}
		}

		// Token: 0x0600806C RID: 32876 RVA: 0x00338634 File Offset: 0x00336834
		public void Remove(AttributeModifier modifier)
		{
			if (modifier == null)
			{
				return;
			}
			AttributeInstance attributeInstance = this.Get(modifier.AttributeId);
			if (attributeInstance != null)
			{
				attributeInstance.Remove(modifier);
			}
		}

		// Token: 0x0600806D RID: 32877 RVA: 0x0033865C File Offset: 0x0033685C
		public float GetValuePercent(string attribute_id)
		{
			float result = 1f;
			AttributeInstance attributeInstance = this.Get(attribute_id);
			if (attributeInstance != null)
			{
				result = attributeInstance.GetTotalValue() / attributeInstance.GetBaseValue();
			}
			else
			{
				global::Debug.LogError("Could not find attribute " + attribute_id);
			}
			return result;
		}

		// Token: 0x0600806E RID: 32878 RVA: 0x0033869C File Offset: 0x0033689C
		public AttributeInstance Get(string attribute_id)
		{
			for (int i = 0; i < this.AttributeTable.Count; i++)
			{
				if (this.AttributeTable[i].Id == attribute_id)
				{
					return this.AttributeTable[i];
				}
			}
			return null;
		}

		// Token: 0x0600806F RID: 32879 RVA: 0x003386E6 File Offset: 0x003368E6
		public AttributeInstance Get(Attribute attribute)
		{
			return this.Get(attribute.Id);
		}

		// Token: 0x06008070 RID: 32880 RVA: 0x003386F4 File Offset: 0x003368F4
		public float GetValue(string id)
		{
			float result = 0f;
			AttributeInstance attributeInstance = this.Get(id);
			if (attributeInstance != null)
			{
				result = attributeInstance.GetTotalValue();
			}
			else
			{
				global::Debug.LogError("Could not find attribute " + id);
			}
			return result;
		}

		// Token: 0x06008071 RID: 32881 RVA: 0x0033872C File Offset: 0x0033692C
		public AttributeInstance GetProfession()
		{
			AttributeInstance attributeInstance = null;
			foreach (AttributeInstance attributeInstance2 in this)
			{
				if (attributeInstance2.modifier.IsProfession)
				{
					if (attributeInstance == null)
					{
						attributeInstance = attributeInstance2;
					}
					else if (attributeInstance.GetTotalValue() < attributeInstance2.GetTotalValue())
					{
						attributeInstance = attributeInstance2;
					}
				}
			}
			return attributeInstance;
		}

		// Token: 0x06008072 RID: 32882 RVA: 0x00338794 File Offset: 0x00336994
		public string GetProfessionString(bool longform = true)
		{
			AttributeInstance profession = this.GetProfession();
			if ((int)profession.GetTotalValue() == 0)
			{
				return string.Format(longform ? UI.ATTRIBUTELEVEL : UI.ATTRIBUTELEVEL_SHORT, 0, DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_NAME);
			}
			return string.Format(longform ? UI.ATTRIBUTELEVEL : UI.ATTRIBUTELEVEL_SHORT, (int)profession.GetTotalValue(), profession.modifier.ProfessionName);
		}

		// Token: 0x06008073 RID: 32883 RVA: 0x00338808 File Offset: 0x00336A08
		public string GetProfessionDescriptionString()
		{
			AttributeInstance profession = this.GetProfession();
			if ((int)profession.GetTotalValue() == 0)
			{
				return DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_DESC;
			}
			return string.Format(DUPLICANTS.ATTRIBUTES.PROFESSION_DESC, profession.modifier.Name);
		}

		// Token: 0x04006147 RID: 24903
		public List<AttributeInstance> AttributeTable = new List<AttributeInstance>();

		// Token: 0x04006148 RID: 24904
		public GameObject gameObject;
	}
}
