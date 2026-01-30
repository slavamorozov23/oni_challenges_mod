using System;

// Token: 0x02000A44 RID: 2628
[Serializable]
public struct SicknessExposureInfo
{
	// Token: 0x06004CA6 RID: 19622 RVA: 0x001BD880 File Offset: 0x001BBA80
	public SicknessExposureInfo(string id, string infection_source_info)
	{
		this.sicknessID = id;
		this.sourceInfo = infection_source_info;
	}

	// Token: 0x040032F9 RID: 13049
	public string sicknessID;

	// Token: 0x040032FA RID: 13050
	public string sourceInfo;
}
