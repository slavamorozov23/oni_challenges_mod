using System;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public static class ChoreHelpers
{
	// Token: 0x0600190D RID: 6413 RVA: 0x0008BC98 File Offset: 0x00089E98
	public static GameObject CreateLocator(string name, Vector3 pos)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(ApproachableLocator.ID), null, null);
		gameObject.name = name;
		gameObject.transform.SetPosition(pos);
		gameObject.gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x0008BCD0 File Offset: 0x00089ED0
	public static GameObject CreateSleepLocator(Vector3 pos)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(SleepLocator.ID), null, null);
		gameObject.name = "SLeepLocator";
		gameObject.transform.SetPosition(pos);
		gameObject.gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x0008BD0C File Offset: 0x00089F0C
	public static void DestroyLocator(GameObject locator)
	{
		if (locator != null)
		{
			locator.gameObject.DeleteObject();
		}
	}
}
