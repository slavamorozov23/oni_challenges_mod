using System;
using System.Collections.Generic;

// Token: 0x02000660 RID: 1632
public class WorkingToiletTracker : WorldTracker
{
	// Token: 0x06002790 RID: 10128 RVA: 0x000E3110 File Offset: 0x000E1310
	public WorkingToiletTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x000E311C File Offset: 0x000E131C
	public override void UpdateData()
	{
		int num = 0;
		using (IEnumerator<IUsable> enumerator = Components.Toilets.WorldItemsEnumerate(base.WorldID, true).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsUsable())
				{
					num++;
				}
			}
		}
		base.AddPoint((float)num);
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x000E3184 File Offset: 0x000E1384
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
