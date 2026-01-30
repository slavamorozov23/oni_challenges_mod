using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001051 RID: 4177
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Modifiers")]
	public class Modifiers : KMonoBehaviour, ISaveLoadableDetails
	{
		// Token: 0x0600815B RID: 33115 RVA: 0x0033EAE8 File Offset: 0x0033CCE8
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.amounts = new Amounts(base.gameObject);
			this.sicknesses = new Sicknesses(base.gameObject);
			this.attributes = new Attributes(base.gameObject);
			foreach (string id in this.initialAmounts)
			{
				this.amounts.Add(new AmountInstance(Db.Get().Amounts.Get(id), base.gameObject));
			}
			foreach (string text in this.initialAttributes)
			{
				Attribute attribute = Db.Get().CritterAttributes.TryGet(text);
				if (attribute == null)
				{
					attribute = Db.Get().PlantAttributes.TryGet(text);
				}
				if (attribute == null)
				{
					attribute = Db.Get().Attributes.TryGet(text);
				}
				DebugUtil.Assert(attribute != null, "Couldn't find an attribute for id", text);
				this.attributes.Add(attribute);
			}
			Traits component = base.GetComponent<Traits>();
			if (this.initialTraits != null)
			{
				foreach (string id2 in this.initialTraits)
				{
					Trait trait = Db.Get().traits.Get(id2);
					component.Add(trait);
				}
			}
		}

		// Token: 0x0600815C RID: 33116 RVA: 0x0033EC94 File Offset: 0x0033CE94
		public float GetPreModifiedAttributeValue(Attribute attribute)
		{
			return AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
		}

		// Token: 0x0600815D RID: 33117 RVA: 0x0033ECA4 File Offset: 0x0033CEA4
		public string GetPreModifiedAttributeFormattedValue(Attribute attribute)
		{
			float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
			return attribute.formatter.GetFormattedValue(totalValue, attribute.formatter.DeltaTimeSlice);
		}

		// Token: 0x0600815E RID: 33118 RVA: 0x0033ECD8 File Offset: 0x0033CED8
		public string GetPreModifiedAttributeDescription(Attribute attribute)
		{
			float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
			return string.Format(DUPLICANTS.ATTRIBUTES.VALUE, attribute.Name, attribute.formatter.GetFormattedValue(totalValue, GameUtil.TimeSlice.None));
		}

		// Token: 0x0600815F RID: 33119 RVA: 0x0033ED15 File Offset: 0x0033CF15
		public string GetPreModifiedAttributeToolTip(Attribute attribute)
		{
			return attribute.formatter.GetTooltip(attribute, this.GetPreModifiers(attribute), null);
		}

		// Token: 0x06008160 RID: 33120 RVA: 0x0033ED2C File Offset: 0x0033CF2C
		public List<AttributeModifier> GetPreModifiers(Attribute attribute)
		{
			List<AttributeModifier> list = new List<AttributeModifier>();
			foreach (string id in this.initialTraits)
			{
				foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(id).SelfModifiers)
				{
					if (attributeModifier.AttributeId == attribute.Id)
					{
						list.Add(attributeModifier);
					}
				}
			}
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null && component.MutationIDs != null)
			{
				foreach (string id2 in component.MutationIDs)
				{
					foreach (AttributeModifier attributeModifier2 in Db.Get().PlantMutations.Get(id2).SelfModifiers)
					{
						if (attributeModifier2.AttributeId == attribute.Id)
						{
							list.Add(attributeModifier2);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06008161 RID: 33121 RVA: 0x0033EEAC File Offset: 0x0033D0AC
		public void Serialize(BinaryWriter writer)
		{
			this.OnSerialize(writer);
		}

		// Token: 0x06008162 RID: 33122 RVA: 0x0033EEB5 File Offset: 0x0033D0B5
		public void Deserialize(IReader reader)
		{
			this.OnDeserialize(reader);
		}

		// Token: 0x06008163 RID: 33123 RVA: 0x0033EEBE File Offset: 0x0033D0BE
		public virtual void OnSerialize(BinaryWriter writer)
		{
			this.amounts.Serialize(writer);
			this.sicknesses.Serialize(writer);
		}

		// Token: 0x06008164 RID: 33124 RVA: 0x0033EED8 File Offset: 0x0033D0D8
		public virtual void OnDeserialize(IReader reader)
		{
			this.amounts.Deserialize(reader);
			this.sicknesses.Deserialize(reader);
		}

		// Token: 0x06008165 RID: 33125 RVA: 0x0033EEF2 File Offset: 0x0033D0F2
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			if (this.amounts != null)
			{
				this.amounts.Cleanup();
			}
		}

		// Token: 0x040061F4 RID: 25076
		public Amounts amounts;

		// Token: 0x040061F5 RID: 25077
		public Attributes attributes;

		// Token: 0x040061F6 RID: 25078
		public Sicknesses sicknesses;

		// Token: 0x040061F7 RID: 25079
		public List<string> initialTraits = new List<string>();

		// Token: 0x040061F8 RID: 25080
		public List<string> initialAmounts = new List<string>();

		// Token: 0x040061F9 RID: 25081
		public List<string> initialAttributes = new List<string>();
	}
}
