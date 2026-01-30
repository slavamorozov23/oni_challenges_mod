using System;
using System.Collections.Generic;

// Token: 0x02000A79 RID: 2681
public static class OffsetTable
{
	// Token: 0x06004E01 RID: 19969 RVA: 0x001C616C File Offset: 0x001C436C
	public static CellOffset[][] Mirror(CellOffset[][] table)
	{
		List<CellOffset[]> list = new List<CellOffset[]>();
		foreach (CellOffset[] array in table)
		{
			list.Add(array);
			if (array[0].x != 0)
			{
				CellOffset[] array2 = new CellOffset[array.Length];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = array[j];
					array2[j].x = -array2[j].x;
				}
				list.Add(array2);
			}
		}
		return list.ToArray();
	}
}
