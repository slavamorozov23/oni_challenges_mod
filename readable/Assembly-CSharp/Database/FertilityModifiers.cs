using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000F16 RID: 3862
	public class FertilityModifiers : ResourceSet<FertilityModifier>
	{
		// Token: 0x06007BF6 RID: 31734 RVA: 0x00301254 File Offset: 0x002FF454
		public List<FertilityModifier> GetForTag(Tag searchTag)
		{
			List<FertilityModifier> list = new List<FertilityModifier>();
			foreach (FertilityModifier fertilityModifier in this.resources)
			{
				if (fertilityModifier.TargetTag == searchTag)
				{
					list.Add(fertilityModifier);
				}
			}
			return list;
		}
	}
}
