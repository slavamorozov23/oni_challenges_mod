using System;

// Token: 0x02000EDA RID: 3802
public static class WorldGenLogger
{
	// Token: 0x060079B2 RID: 31154 RVA: 0x002ED2D2 File Offset: 0x002EB4D2
	public static void LogException(string message, string stack)
	{
		Debug.LogError(message + "\n" + stack);
	}
}
