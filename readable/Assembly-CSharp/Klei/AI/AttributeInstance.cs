using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001023 RID: 4131
	[DebuggerDisplay("{Attribute.Id}")]
	public class AttributeInstance : ModifierInstance<Attribute>
	{
		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06008029 RID: 32809 RVA: 0x003378CB File Offset: 0x00335ACB
		public string Id
		{
			get
			{
				return this.Attribute.Id;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x0600802A RID: 32810 RVA: 0x003378D8 File Offset: 0x00335AD8
		public string Name
		{
			get
			{
				return this.Attribute.Name;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x0600802B RID: 32811 RVA: 0x003378E5 File Offset: 0x00335AE5
		public string Description
		{
			get
			{
				return this.Attribute.Description;
			}
		}

		// Token: 0x0600802C RID: 32812 RVA: 0x003378F2 File Offset: 0x00335AF2
		public float GetBaseValue()
		{
			return this.Attribute.BaseValue;
		}

		// Token: 0x0600802D RID: 32813 RVA: 0x00337900 File Offset: 0x00335B00
		public float GetTotalDisplayValue()
		{
			float num = this.Attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != this.Modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num3];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
				else
				{
					num2 += attributeModifier.Value;
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x0600802E RID: 32814 RVA: 0x00337974 File Offset: 0x00335B74
		public float GetTotalValue()
		{
			float num = this.Attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != this.Modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num3];
				if (!attributeModifier.UIOnly)
				{
					if (!attributeModifier.IsMultiplier)
					{
						num += attributeModifier.Value;
					}
					else
					{
						num2 += attributeModifier.Value;
					}
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x0600802F RID: 32815 RVA: 0x003379F0 File Offset: 0x00335BF0
		public static float GetTotalDisplayValue(Attribute attribute, List<AttributeModifier> modifiers)
		{
			float num = attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = modifiers[num3];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
				else
				{
					num2 += attributeModifier.Value;
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x06008030 RID: 32816 RVA: 0x00337A54 File Offset: 0x00335C54
		public static float GetTotalValue(Attribute attribute, List<AttributeModifier> modifiers)
		{
			float num = attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = modifiers[num3];
				if (!attributeModifier.UIOnly)
				{
					if (!attributeModifier.IsMultiplier)
					{
						num += attributeModifier.Value;
					}
					else
					{
						num2 += attributeModifier.Value;
					}
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x06008031 RID: 32817 RVA: 0x00337AC0 File Offset: 0x00335CC0
		public float GetModifierContribution(AttributeModifier testModifier)
		{
			if (!testModifier.IsMultiplier)
			{
				return testModifier.Value;
			}
			float num = this.Attribute.BaseValue;
			for (int num2 = 0; num2 != this.Modifiers.Count; num2++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num2];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
			}
			return num * testModifier.Value;
		}

		// Token: 0x06008032 RID: 32818 RVA: 0x00337B24 File Offset: 0x00335D24
		public AttributeInstance(GameObject game_object, Attribute attribute) : base(game_object, attribute)
		{
			DebugUtil.Assert(attribute != null);
			this.Attribute = attribute;
		}

		// Token: 0x06008033 RID: 32819 RVA: 0x00337B3E File Offset: 0x00335D3E
		public void Add(AttributeModifier modifier)
		{
			this.Modifiers.Add(modifier);
			if (this.OnDirty != null)
			{
				this.OnDirty();
			}
		}

		// Token: 0x06008034 RID: 32820 RVA: 0x00337B60 File Offset: 0x00335D60
		public void Remove(AttributeModifier modifier)
		{
			int i = 0;
			while (i < this.Modifiers.Count)
			{
				if (this.Modifiers[i] == modifier)
				{
					this.Modifiers.RemoveAt(i);
					if (this.OnDirty != null)
					{
						this.OnDirty();
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06008035 RID: 32821 RVA: 0x00337BB2 File Offset: 0x00335DB2
		public void ClearModifiers()
		{
			if (this.Modifiers.Count > 0)
			{
				this.Modifiers.Clear();
				if (this.OnDirty != null)
				{
					this.OnDirty();
				}
			}
		}

		// Token: 0x06008036 RID: 32822 RVA: 0x00337BE0 File Offset: 0x00335DE0
		public string GetDescription()
		{
			return string.Format(DUPLICANTS.ATTRIBUTES.VALUE, this.Name, this.GetFormattedValue());
		}

		// Token: 0x06008037 RID: 32823 RVA: 0x00337BFD File Offset: 0x00335DFD
		public string GetFormattedValue()
		{
			return this.Attribute.formatter.GetFormattedAttribute(this);
		}

		// Token: 0x06008038 RID: 32824 RVA: 0x00337C10 File Offset: 0x00335E10
		public string GetAttributeValueTooltip()
		{
			return this.Attribute.GetTooltip(this);
		}

		// Token: 0x04006131 RID: 24881
		public Attribute Attribute;

		// Token: 0x04006132 RID: 24882
		public System.Action OnDirty;

		// Token: 0x04006133 RID: 24883
		public ArrayRef<AttributeModifier> Modifiers;

		// Token: 0x04006134 RID: 24884
		public bool hide;
	}
}
