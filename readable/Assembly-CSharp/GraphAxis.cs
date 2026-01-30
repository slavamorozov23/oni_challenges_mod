using System;

// Token: 0x02000D18 RID: 3352
[Serializable]
public struct GraphAxis
{
	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x060067CF RID: 26575 RVA: 0x00272840 File Offset: 0x00270A40
	public float range
	{
		get
		{
			return this.max_value - this.min_value;
		}
	}

	// Token: 0x04004734 RID: 18228
	public string name;

	// Token: 0x04004735 RID: 18229
	public float min_value;

	// Token: 0x04004736 RID: 18230
	public float max_value;

	// Token: 0x04004737 RID: 18231
	public float guide_frequency;
}
