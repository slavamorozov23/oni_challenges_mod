using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001053 RID: 4179
	[AddComponentMenu("KMonoBehaviour/scripts/PrefabAttributeModifiers")]
	public class PrefabAttributeModifiers : KMonoBehaviour
	{
		// Token: 0x0600816D RID: 33133 RVA: 0x0033EFE5 File Offset: 0x0033D1E5
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
		}

		// Token: 0x0600816E RID: 33134 RVA: 0x0033EFED File Offset: 0x0033D1ED
		public void AddAttributeDescriptor(AttributeModifier modifier)
		{
			this.descriptors.Add(modifier);
		}

		// Token: 0x0600816F RID: 33135 RVA: 0x0033EFFB File Offset: 0x0033D1FB
		public void RemovePrefabAttribute(AttributeModifier modifier)
		{
			this.descriptors.Remove(modifier);
		}

		// Token: 0x040061FA RID: 25082
		public List<AttributeModifier> descriptors = new List<AttributeModifier>();
	}
}
