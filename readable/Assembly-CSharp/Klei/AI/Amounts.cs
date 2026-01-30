using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200101E RID: 4126
	public class Amounts : Modifications<Amount, AmountInstance>
	{
		// Token: 0x0600800D RID: 32781 RVA: 0x003373D2 File Offset: 0x003355D2
		public Amounts(GameObject go) : base(go, null)
		{
		}

		// Token: 0x0600800E RID: 32782 RVA: 0x003373DC File Offset: 0x003355DC
		public float GetValue(string amount_id)
		{
			return base.Get(amount_id).value;
		}

		// Token: 0x0600800F RID: 32783 RVA: 0x003373EA File Offset: 0x003355EA
		public void SetValue(string amount_id, float value)
		{
			base.Get(amount_id).value = value;
		}

		// Token: 0x06008010 RID: 32784 RVA: 0x003373F9 File Offset: 0x003355F9
		public override AmountInstance Add(AmountInstance instance)
		{
			instance.Activate();
			return base.Add(instance);
		}

		// Token: 0x06008011 RID: 32785 RVA: 0x00337408 File Offset: 0x00335608
		public override void Remove(AmountInstance instance)
		{
			instance.Deactivate();
			base.Remove(instance);
		}

		// Token: 0x06008012 RID: 32786 RVA: 0x00337418 File Offset: 0x00335618
		public void Cleanup()
		{
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Deactivate();
			}
		}
	}
}
