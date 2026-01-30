using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001052 RID: 4178
	public static class ModifiersExtensions
	{
		// Token: 0x06008167 RID: 33127 RVA: 0x0033EF36 File Offset: 0x0033D136
		public static Attributes GetAttributes(this KMonoBehaviour cmp)
		{
			return cmp.gameObject.GetAttributes();
		}

		// Token: 0x06008168 RID: 33128 RVA: 0x0033EF44 File Offset: 0x0033D144
		public static Attributes GetAttributes(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.attributes;
			}
			return null;
		}

		// Token: 0x06008169 RID: 33129 RVA: 0x0033EF69 File Offset: 0x0033D169
		public static Amounts GetAmounts(this KMonoBehaviour cmp)
		{
			if (cmp is Modifiers)
			{
				return ((Modifiers)cmp).amounts;
			}
			return cmp.gameObject.GetAmounts();
		}

		// Token: 0x0600816A RID: 33130 RVA: 0x0033EF8C File Offset: 0x0033D18C
		public static Amounts GetAmounts(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.amounts;
			}
			return null;
		}

		// Token: 0x0600816B RID: 33131 RVA: 0x0033EFB1 File Offset: 0x0033D1B1
		public static Sicknesses GetSicknesses(this KMonoBehaviour cmp)
		{
			return cmp.gameObject.GetSicknesses();
		}

		// Token: 0x0600816C RID: 33132 RVA: 0x0033EFC0 File Offset: 0x0033D1C0
		public static Sicknesses GetSicknesses(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.sicknesses;
			}
			return null;
		}
	}
}
