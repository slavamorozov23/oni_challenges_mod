using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000BD9 RID: 3033
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Spawner")]
public class Spawner : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06005AE6 RID: 23270 RVA: 0x0020F003 File Offset: 0x0020D203
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SaveGame.Instance.worldGenSpawner.AddLegacySpawner(this.prefabTag, Grid.PosToCell(this));
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x04003C90 RID: 15504
	[Serialize]
	public Tag prefabTag;

	// Token: 0x04003C91 RID: 15505
	[Serialize]
	public int units = 1;
}
