using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001021 RID: 4129
	public class AttributeConverterInstance : ModifierInstance<AttributeConverter>
	{
		// Token: 0x06008021 RID: 32801 RVA: 0x003376F1 File Offset: 0x003358F1
		public AttributeConverterInstance(GameObject game_object, AttributeConverter converter, AttributeInstance attribute_instance) : base(game_object, converter)
		{
			this.converter = converter;
			this.attributeInstance = attribute_instance;
		}

		// Token: 0x06008022 RID: 32802 RVA: 0x00337709 File Offset: 0x00335909
		public float Evaluate()
		{
			return this.converter.multiplier * this.attributeInstance.GetTotalValue() + this.converter.baseValue;
		}

		// Token: 0x06008023 RID: 32803 RVA: 0x0033772E File Offset: 0x0033592E
		public string DescriptionFromAttribute(float value, GameObject go)
		{
			return this.converter.DescriptionFromAttribute(this.Evaluate(), go);
		}

		// Token: 0x0400612E RID: 24878
		public AttributeConverter converter;

		// Token: 0x0400612F RID: 24879
		public AttributeInstance attributeInstance;
	}
}
