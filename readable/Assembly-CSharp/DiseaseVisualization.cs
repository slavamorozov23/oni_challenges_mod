using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ADF RID: 2783
public class DiseaseVisualization : ScriptableObject
{
	// Token: 0x060050E9 RID: 20713 RVA: 0x001D54F8 File Offset: 0x001D36F8
	public DiseaseVisualization.Info GetInfo(HashedString id)
	{
		foreach (DiseaseVisualization.Info info in this.info)
		{
			if (id == info.name)
			{
				return info;
			}
		}
		return default(DiseaseVisualization.Info);
	}

	// Token: 0x040035F9 RID: 13817
	public Sprite overlaySprite;

	// Token: 0x040035FA RID: 13818
	public List<DiseaseVisualization.Info> info = new List<DiseaseVisualization.Info>();

	// Token: 0x02001C26 RID: 7206
	[Serializable]
	public struct Info
	{
		// Token: 0x0600ACAD RID: 44205 RVA: 0x003CD915 File Offset: 0x003CBB15
		public Info(string name)
		{
			this.name = name;
			this.overlayColourName = "germFoodPoisoning";
		}

		// Token: 0x0400870B RID: 34571
		public string name;

		// Token: 0x0400870C RID: 34572
		public string overlayColourName;
	}
}
