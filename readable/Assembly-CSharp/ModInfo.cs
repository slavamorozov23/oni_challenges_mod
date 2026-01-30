using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// Token: 0x02000353 RID: 851
[Serializable]
public struct ModInfo
{
	// Token: 0x04000B2A RID: 2858
	[JsonConverter(typeof(StringEnumConverter))]
	public ModInfo.Source source;

	// Token: 0x04000B2B RID: 2859
	[JsonConverter(typeof(StringEnumConverter))]
	public ModInfo.ModType type;

	// Token: 0x04000B2C RID: 2860
	public string assetID;

	// Token: 0x04000B2D RID: 2861
	public string assetPath;

	// Token: 0x04000B2E RID: 2862
	public bool enabled;

	// Token: 0x04000B2F RID: 2863
	public bool markedForDelete;

	// Token: 0x04000B30 RID: 2864
	public bool markedForUpdate;

	// Token: 0x04000B31 RID: 2865
	public string description;

	// Token: 0x04000B32 RID: 2866
	public ulong lastModifiedTime;

	// Token: 0x02001239 RID: 4665
	public enum Source
	{
		// Token: 0x04006729 RID: 26409
		Local,
		// Token: 0x0400672A RID: 26410
		Steam,
		// Token: 0x0400672B RID: 26411
		Rail
	}

	// Token: 0x0200123A RID: 4666
	public enum ModType
	{
		// Token: 0x0400672D RID: 26413
		WorldGen,
		// Token: 0x0400672E RID: 26414
		Scenario,
		// Token: 0x0400672F RID: 26415
		Mod
	}
}
