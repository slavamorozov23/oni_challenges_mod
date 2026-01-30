using System;
using UnityEngine;

// Token: 0x02000836 RID: 2102
public static class CameraSaveData
{
	// Token: 0x0600395D RID: 14685 RVA: 0x00140560 File Offset: 0x0013E760
	public static void Load(FastReader reader)
	{
		CameraSaveData.position = reader.ReadVector3();
		CameraSaveData.localScale = reader.ReadVector3();
		CameraSaveData.rotation = reader.ReadQuaternion();
		CameraSaveData.orthographicsSize = reader.ReadSingle();
		CameraSaveData.valid = true;
	}

	// Token: 0x04002308 RID: 8968
	public static bool valid;

	// Token: 0x04002309 RID: 8969
	public static Vector3 position;

	// Token: 0x0400230A RID: 8970
	public static Vector3 localScale;

	// Token: 0x0400230B RID: 8971
	public static Quaternion rotation;

	// Token: 0x0400230C RID: 8972
	public static float orthographicsSize;
}
