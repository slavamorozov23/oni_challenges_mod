using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000B2B RID: 2859
public class SceneInitializer : MonoBehaviour
{
	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x0600540D RID: 21517 RVA: 0x001EB07C File Offset: 0x001E927C
	// (set) Token: 0x0600540E RID: 21518 RVA: 0x001EB083 File Offset: 0x001E9283
	public static SceneInitializer Instance { get; private set; }

	// Token: 0x0600540F RID: 21519 RVA: 0x001EB08C File Offset: 0x001E928C
	private void Awake()
	{
		Localization.SwapToLocalizedFont();
		string environmentVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
		string text = Application.dataPath + Path.DirectorySeparatorChar.ToString() + "Plugins";
		if (!environmentVariable.Contains(text))
		{
			Environment.SetEnvironmentVariable("PATH", environmentVariable + Path.PathSeparator.ToString() + text, EnvironmentVariableTarget.Process);
		}
		SceneInitializer.Instance = this;
		this.PreLoadPrefabs();
	}

	// Token: 0x06005410 RID: 21520 RVA: 0x001EB0FB File Offset: 0x001E92FB
	private void OnDestroy()
	{
		SceneInitializer.Instance = null;
	}

	// Token: 0x06005411 RID: 21521 RVA: 0x001EB104 File Offset: 0x001E9304
	private void PreLoadPrefabs()
	{
		foreach (GameObject gameObject in this.preloadPrefabs)
		{
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, gameObject.transform.GetPosition(), Quaternion.identity, base.gameObject, null, true, 0);
			}
		}
	}

	// Token: 0x06005412 RID: 21522 RVA: 0x001EB17C File Offset: 0x001E937C
	public void NewSaveGamePrefab()
	{
		if (this.prefab_NewSaveGame != null && SaveGame.Instance == null)
		{
			Util.KInstantiate(this.prefab_NewSaveGame, base.gameObject, null);
		}
	}

	// Token: 0x06005413 RID: 21523 RVA: 0x001EB1AC File Offset: 0x001E93AC
	public void PostLoadPrefabs()
	{
		foreach (GameObject gameObject in this.prefabs)
		{
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, base.gameObject, null);
			}
		}
	}

	// Token: 0x040038C5 RID: 14533
	public const int MAXDEPTH = -30000;

	// Token: 0x040038C6 RID: 14534
	public const int SCREENDEPTH = -1000;

	// Token: 0x040038C8 RID: 14536
	public GameObject prefab_NewSaveGame;

	// Token: 0x040038C9 RID: 14537
	public List<GameObject> preloadPrefabs = new List<GameObject>();

	// Token: 0x040038CA RID: 14538
	public List<GameObject> prefabs = new List<GameObject>();
}
