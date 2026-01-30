using System;
using UnityEngine;

// Token: 0x02000A00 RID: 2560
[AddComponentMenu("KMonoBehaviour/scripts/Meter")]
public class Meter : KMonoBehaviour
{
	// Token: 0x02001A6C RID: 6764
	public enum Offset
	{
		// Token: 0x0400819C RID: 33180
		Infront,
		// Token: 0x0400819D RID: 33181
		Behind,
		// Token: 0x0400819E RID: 33182
		UserSpecified,
		// Token: 0x0400819F RID: 33183
		NoChange
	}
}
