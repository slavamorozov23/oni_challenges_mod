using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001038 RID: 4152
	public class AttributeModifierSickness : Sickness.SicknessComponent
	{
		// Token: 0x060080BD RID: 32957 RVA: 0x0033BCE8 File Offset: 0x00339EE8
		public AttributeModifierSickness(Tag minionModel, AttributeModifier[] attribute_modifiers)
		{
			this.GetAttributeModifierForMinionModel[minionModel] = attribute_modifiers;
			this.attributeModifiers = new AttributeModifier[0];
		}

		// Token: 0x060080BE RID: 32958 RVA: 0x0033BD14 File Offset: 0x00339F14
		public AttributeModifierSickness(AttributeModifier[] attribute_modifiers)
		{
			this.attributeModifiers = attribute_modifiers;
		}

		// Token: 0x060080BF RID: 32959 RVA: 0x0033BD30 File Offset: 0x00339F30
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			Attributes attributes = go.GetAttributes();
			Tag key = go.PrefabID();
			if (this.GetAttributeModifierForMinionModel.ContainsKey(key))
			{
				for (int i = 0; i < this.GetAttributeModifierForMinionModel[key].Length; i++)
				{
					AttributeModifier modifier = this.GetAttributeModifierForMinionModel[key][i];
					attributes.Add(modifier);
				}
			}
			for (int j = 0; j < this.attributeModifiers.Length; j++)
			{
				AttributeModifier modifier2 = this.attributeModifiers[j];
				attributes.Add(modifier2);
			}
			return null;
		}

		// Token: 0x060080C0 RID: 32960 RVA: 0x0033BDB4 File Offset: 0x00339FB4
		public override void OnCure(GameObject go, object instance_data)
		{
			Attributes attributes = go.GetAttributes();
			Tag key = go.PrefabID();
			if (this.GetAttributeModifierForMinionModel.ContainsKey(key))
			{
				for (int i = 0; i < this.GetAttributeModifierForMinionModel[key].Length; i++)
				{
					AttributeModifier modifier = this.GetAttributeModifierForMinionModel[key][i];
					attributes.Remove(modifier);
				}
			}
			for (int j = 0; j < this.attributeModifiers.Length; j++)
			{
				AttributeModifier modifier2 = this.attributeModifiers[j];
				attributes.Remove(modifier2);
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x060080C1 RID: 32961 RVA: 0x0033BE36 File Offset: 0x0033A036
		public AttributeModifier[] Modifers
		{
			get
			{
				return this.attributeModifiers;
			}
		}

		// Token: 0x060080C2 RID: 32962 RVA: 0x0033BE40 File Offset: 0x0033A040
		public override List<Descriptor> GetSymptoms(GameObject victim)
		{
			if (victim == null)
			{
				return this.GetSymptoms();
			}
			List<Descriptor> list = new List<Descriptor>();
			Tag key = victim.PrefabID();
			if (this.GetAttributeModifierForMinionModel.ContainsKey(key))
			{
				foreach (AttributeModifier attributeModifier in this.GetAttributeModifierForMinionModel[key])
				{
					Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
					list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, attribute.Name, attributeModifier.GetFormattedString()), string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, attribute.Name, attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Symptom, false));
				}
			}
			foreach (AttributeModifier attributeModifier2 in this.attributeModifiers)
			{
				Attribute attribute2 = Db.Get().Attributes.Get(attributeModifier2.AttributeId);
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, attribute2.Name, attributeModifier2.GetFormattedString()), string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, attribute2.Name, attributeModifier2.GetFormattedString()), Descriptor.DescriptorType.Symptom, false));
			}
			return list;
		}

		// Token: 0x060080C3 RID: 32963 RVA: 0x0033BF74 File Offset: 0x0033A174
		public override List<Descriptor> GetSymptoms()
		{
			List<Descriptor> list = new List<Descriptor>();
			foreach (Tag tag in this.GetAttributeModifierForMinionModel.Keys)
			{
				string properName = Assets.GetPrefab(tag).GetProperName();
				foreach (AttributeModifier attributeModifier in this.GetAttributeModifierForMinionModel[tag])
				{
					Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
					list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_BY_MODEL_MODIFIER_SYMPTOMS, properName, attribute.Name, attributeModifier.GetFormattedString()), string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, attribute.Name, attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Symptom, false));
				}
			}
			foreach (AttributeModifier attributeModifier2 in this.attributeModifiers)
			{
				Attribute attribute2 = Db.Get().Attributes.Get(attributeModifier2.AttributeId);
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, attribute2.Name, attributeModifier2.GetFormattedString()), string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, attribute2.Name, attributeModifier2.GetFormattedString()), Descriptor.DescriptorType.Symptom, false));
			}
			return list;
		}

		// Token: 0x04006188 RID: 24968
		private Dictionary<Tag, AttributeModifier[]> GetAttributeModifierForMinionModel = new Dictionary<Tag, AttributeModifier[]>();

		// Token: 0x04006189 RID: 24969
		private AttributeModifier[] attributeModifiers;
	}
}
