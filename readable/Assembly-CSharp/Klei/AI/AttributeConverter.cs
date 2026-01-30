using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001020 RID: 4128
	public class AttributeConverter : Resource
	{
		// Token: 0x0600801D RID: 32797 RVA: 0x00337600 File Offset: 0x00335800
		public AttributeConverter(string id, string name, string description, float multiplier, float base_value, Attribute attribute, IAttributeFormatter formatter = null) : base(id, name)
		{
			this.description = description;
			this.multiplier = multiplier;
			this.baseValue = base_value;
			this.attribute = attribute;
			this.formatter = formatter;
		}

		// Token: 0x0600801E RID: 32798 RVA: 0x00337631 File Offset: 0x00335831
		public AttributeConverterInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x0600801F RID: 32799 RVA: 0x00337640 File Offset: 0x00335840
		public AttributeConverterInstance Lookup(GameObject go)
		{
			AttributeConverters component = go.GetComponent<AttributeConverters>();
			if (component != null)
			{
				return component.Get(this);
			}
			return null;
		}

		// Token: 0x06008020 RID: 32800 RVA: 0x00337668 File Offset: 0x00335868
		public string DescriptionFromAttribute(float value, GameObject go)
		{
			string text;
			if (this.formatter != null)
			{
				text = this.formatter.GetFormattedValue(value, this.formatter.DeltaTimeSlice);
			}
			else if (this.attribute.formatter != null)
			{
				text = this.attribute.formatter.GetFormattedValue(value, this.attribute.formatter.DeltaTimeSlice);
			}
			else
			{
				text = GameUtil.GetFormattedSimple(value, GameUtil.TimeSlice.None, null);
			}
			if (text != null)
			{
				text = GameUtil.AddPositiveSign(text, value > 0f);
				return string.Format(this.description, text);
			}
			return null;
		}

		// Token: 0x04006129 RID: 24873
		public string description;

		// Token: 0x0400612A RID: 24874
		public float multiplier;

		// Token: 0x0400612B RID: 24875
		public float baseValue;

		// Token: 0x0400612C RID: 24876
		public Attribute attribute;

		// Token: 0x0400612D RID: 24877
		public IAttributeFormatter formatter;
	}
}
