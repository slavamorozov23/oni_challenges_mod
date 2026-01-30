using System;
using UnityEngine;

// Token: 0x02000B2C RID: 2860
public class SceneInitializerLoader : MonoBehaviour
{
	// Token: 0x06005415 RID: 21525 RVA: 0x001EB230 File Offset: 0x001E9430
	private void Awake()
	{
		Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		KMonoBehaviour.isLoadingScene = false;
		Singleton<StateMachineManager>.Instance.Clear();
		Util.KInstantiate(this.sceneInitializer, null, null);
		if (SceneInitializerLoader.ReportDeferredError != null && SceneInitializerLoader.deferred_error.IsValid)
		{
			SceneInitializerLoader.ReportDeferredError(SceneInitializerLoader.deferred_error);
			SceneInitializerLoader.deferred_error = default(SceneInitializerLoader.DeferredError);
		}
	}

	// Token: 0x040038CB RID: 14539
	public SceneInitializer sceneInitializer;

	// Token: 0x040038CC RID: 14540
	public static SceneInitializerLoader.DeferredError deferred_error;

	// Token: 0x040038CD RID: 14541
	public static SceneInitializerLoader.DeferredErrorDelegate ReportDeferredError;

	// Token: 0x02001C95 RID: 7317
	public struct DeferredError
	{
		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x0600AE14 RID: 44564 RVA: 0x003D25BE File Offset: 0x003D07BE
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.msg);
			}
		}

		// Token: 0x04008891 RID: 34961
		public string msg;

		// Token: 0x04008892 RID: 34962
		public string stack_trace;
	}

	// Token: 0x02001C96 RID: 7318
	// (Invoke) Token: 0x0600AE16 RID: 44566
	public delegate void DeferredErrorDelegate(SceneInitializerLoader.DeferredError deferred_error);
}
