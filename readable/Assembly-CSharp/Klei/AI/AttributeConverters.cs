using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001022 RID: 4130
	[AddComponentMenu("KMonoBehaviour/scripts/AttributeConverters")]
	public class AttributeConverters : KMonoBehaviour
	{
		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06008024 RID: 32804 RVA: 0x00337742 File Offset: 0x00335942
		public int Count
		{
			get
			{
				return this.converters.Count;
			}
		}

		// Token: 0x06008025 RID: 32805 RVA: 0x00337750 File Offset: 0x00335950
		protected override void OnPrefabInit()
		{
			foreach (AttributeInstance attributeInstance in this.GetAttributes())
			{
				foreach (AttributeConverter converter in attributeInstance.Attribute.converters)
				{
					AttributeConverterInstance item = new AttributeConverterInstance(base.gameObject, converter, attributeInstance);
					this.converters.Add(item);
				}
			}
		}

		// Token: 0x06008026 RID: 32806 RVA: 0x003377F4 File Offset: 0x003359F4
		public AttributeConverterInstance Get(AttributeConverter converter)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in this.converters)
			{
				if (attributeConverterInstance.converter == converter)
				{
					return attributeConverterInstance;
				}
			}
			return null;
		}

		// Token: 0x06008027 RID: 32807 RVA: 0x00337850 File Offset: 0x00335A50
		public AttributeConverterInstance GetConverter(string id)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in this.converters)
			{
				if (attributeConverterInstance.converter.Id == id)
				{
					return attributeConverterInstance;
				}
			}
			return null;
		}

		// Token: 0x04006130 RID: 24880
		public List<AttributeConverterInstance> converters = new List<AttributeConverterInstance>();
	}
}
