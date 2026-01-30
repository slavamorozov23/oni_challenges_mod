using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000552 RID: 1362
[AddComponentMenu("KMonoBehaviour/scripts/KBatchedAnimEventToggler")]
public class KBatchedAnimEventToggler : KMonoBehaviour
{
	// Token: 0x06001E3B RID: 7739 RVA: 0x000A3F30 File Offset: 0x000A2130
	protected override void OnPrefabInit()
	{
		Vector3 position = this.eventSource.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
		int layer = LayerMask.NameToLayer("Default");
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			entry.controller.transform.SetPosition(position);
			entry.controller.SetLayer(layer);
			entry.controller.gameObject.SetActive(false);
		}
		int hash = Hash.SDBMLower(this.enableEvent);
		int hash2 = Hash.SDBMLower(this.disableEvent);
		base.Subscribe(this.eventSource, hash, new Action<object>(this.Enable));
		base.Subscribe(this.eventSource, hash2, new Action<object>(this.Disable));
	}

	// Token: 0x06001E3C RID: 7740 RVA: 0x000A4020 File Offset: 0x000A2220
	protected override void OnSpawn()
	{
		this.animEventHandler = base.GetComponentInParent<AnimEventHandler>();
	}

	// Token: 0x06001E3D RID: 7741 RVA: 0x000A4030 File Offset: 0x000A2230
	private void Enable(object data)
	{
		this.StopAll();
		HashedString context = this.animEventHandler.GetContext();
		if (!context.IsValid)
		{
			return;
		}
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			if (entry.context == context)
			{
				entry.controller.gameObject.SetActive(true);
				entry.controller.Play(entry.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}
	}

	// Token: 0x06001E3E RID: 7742 RVA: 0x000A40D8 File Offset: 0x000A22D8
	private void Disable(object data)
	{
		this.StopAll();
	}

	// Token: 0x06001E3F RID: 7743 RVA: 0x000A40E0 File Offset: 0x000A22E0
	private void StopAll()
	{
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			entry.controller.StopAndClear();
			entry.controller.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400119E RID: 4510
	[SerializeField]
	public GameObject eventSource;

	// Token: 0x0400119F RID: 4511
	[SerializeField]
	public string enableEvent;

	// Token: 0x040011A0 RID: 4512
	[SerializeField]
	public string disableEvent;

	// Token: 0x040011A1 RID: 4513
	[SerializeField]
	public List<KBatchedAnimEventToggler.Entry> entries;

	// Token: 0x040011A2 RID: 4514
	private AnimEventHandler animEventHandler;

	// Token: 0x020013EE RID: 5102
	[Serializable]
	public struct Entry
	{
		// Token: 0x04006CB6 RID: 27830
		public string anim;

		// Token: 0x04006CB7 RID: 27831
		public HashedString context;

		// Token: 0x04006CB8 RID: 27832
		public KBatchedAnimController controller;
	}
}
