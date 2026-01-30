using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DDF RID: 3551
public class PopFxGroup : KMonoBehaviour
{
	// Token: 0x06006FA2 RID: 28578 RVA: 0x002A65F2 File Offset: 0x002A47F2
	public void WakeUp(int key)
	{
		if (!this.isLive)
		{
			this.isLive = true;
			this.lastSpawnTimeStamp = float.MinValue;
			this.lastKeyUsed = key;
		}
	}

	// Token: 0x06006FA3 RID: 28579 RVA: 0x002A6615 File Offset: 0x002A4815
	public void Enqueue(PopFX effect)
	{
		this.spawnQueue.Enqueue(effect);
	}

	// Token: 0x06006FA4 RID: 28580 RVA: 0x002A6624 File Offset: 0x002A4824
	public void Update()
	{
		if (!this.isLive)
		{
			return;
		}
		if (!PopFXManager.Instance.Ready())
		{
			return;
		}
		if (Time.unscaledTime - this.lastSpawnTimeStamp >= 0.1f)
		{
			this.padding = ((this.padding == -1f) ? ((float)(Mathf.Min(this.spawnQueue.Count, 3) - 1) * 1f) : Mathf.Max(this.padding - 1f, 0f));
			PopFX popFX = (this.spawnQueue.Count > 0) ? this.spawnQueue.Dequeue() : null;
			if (popFX != null)
			{
				if (this.spawnPosition == PopFxGroup.INVALID_SPAWN_POSITION)
				{
					this.spawnPosition = popFX.StartPos;
				}
				popFX.Run(this.spawnPosition, Vector3.up * this.padding);
				this.lastSpawnTimeStamp = Time.unscaledTime;
				return;
			}
			this.Recycle();
		}
	}

	// Token: 0x06006FA5 RID: 28581 RVA: 0x002A6714 File Offset: 0x002A4914
	public void Recycle()
	{
		this.isLive = false;
		this.lastSpawnTimeStamp = float.MinValue;
		this.spawnPosition = PopFxGroup.INVALID_SPAWN_POSITION;
		this.padding = -1f;
		while (this.spawnQueue.Count > 0)
		{
			this.spawnQueue.Dequeue().Recycle();
		}
		PopFXManager.Instance.RecycleFxGroup(this.lastKeyUsed, this);
		this.lastKeyUsed = -1;
		base.gameObject.SetActive(false);
	}

	// Token: 0x04004C75 RID: 19573
	public static readonly Vector3 INVALID_SPAWN_POSITION = Vector3.one * -1f;

	// Token: 0x04004C76 RID: 19574
	public const float INVALID_PADDING = -1f;

	// Token: 0x04004C77 RID: 19575
	public const float SPAWN_COOLDOWN = 0.1f;

	// Token: 0x04004C78 RID: 19576
	public const float MAX_PADDING_MULTIPLIER = 2f;

	// Token: 0x04004C79 RID: 19577
	public const int MAX_ITEM_COUNT_PADDING = 3;

	// Token: 0x04004C7A RID: 19578
	public const float INDIVIDUAL_PADDING = 1f;

	// Token: 0x04004C7B RID: 19579
	public Queue<PopFX> spawnQueue = new Queue<PopFX>();

	// Token: 0x04004C7C RID: 19580
	private float padding = -1f;

	// Token: 0x04004C7D RID: 19581
	private int lastKeyUsed = -1;

	// Token: 0x04004C7E RID: 19582
	private bool isLive;

	// Token: 0x04004C7F RID: 19583
	private float lastSpawnTimeStamp = float.MinValue;

	// Token: 0x04004C80 RID: 19584
	private Vector3 spawnPosition = PopFxGroup.INVALID_SPAWN_POSITION;
}
