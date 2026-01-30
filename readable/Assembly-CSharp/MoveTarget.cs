using System;
using UnityEngine;

// Token: 0x02000A5E RID: 2654
[AddComponentMenu("KMonoBehaviour/scripts/MoveTarget")]
public class MoveTarget : KMonoBehaviour
{
	// Token: 0x06004D22 RID: 19746 RVA: 0x001C0D50 File Offset: 0x001BEF50
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
	}
}
