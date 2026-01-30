using System;

namespace KMod
{
	// Token: 0x02000FC2 RID: 4034
	[Flags]
	public enum Content : byte
	{
		// Token: 0x04005D82 RID: 23938
		LayerableFiles = 1,
		// Token: 0x04005D83 RID: 23939
		Strings = 2,
		// Token: 0x04005D84 RID: 23940
		DLL = 4,
		// Token: 0x04005D85 RID: 23941
		Translation = 8,
		// Token: 0x04005D86 RID: 23942
		Animation = 16
	}
}
