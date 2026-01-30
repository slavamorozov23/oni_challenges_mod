using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006F2 RID: 1778
[AddComponentMenu("KMonoBehaviour/scripts/BubbleManager")]
public class BubbleManager : KMonoBehaviour, ISim33ms, IRenderEveryTick
{
	// Token: 0x06002BEC RID: 11244 RVA: 0x000FFDE6 File Offset: 0x000FDFE6
	public static void DestroyInstance()
	{
		BubbleManager.instance = null;
	}

	// Token: 0x06002BED RID: 11245 RVA: 0x000FFDEE File Offset: 0x000FDFEE
	protected override void OnPrefabInit()
	{
		BubbleManager.instance = this;
	}

	// Token: 0x06002BEE RID: 11246 RVA: 0x000FFDF8 File Offset: 0x000FDFF8
	public void SpawnBubble(Vector2 position, Vector2 velocity, SimHashes element, float mass, float temperature)
	{
		BubbleManager.Bubble item = new BubbleManager.Bubble
		{
			position = position,
			velocity = velocity,
			element = element,
			temperature = temperature,
			mass = mass
		};
		this.bubbles.Add(item);
	}

	// Token: 0x06002BEF RID: 11247 RVA: 0x000FFE48 File Offset: 0x000FE048
	public void Sim33ms(float dt)
	{
		ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
		ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList2 = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
		foreach (BubbleManager.Bubble bubble in this.bubbles)
		{
			bubble.position += bubble.velocity * dt;
			bubble.elapsedTime += dt;
			int num = Grid.PosToCell(bubble.position);
			if (!Grid.IsVisiblyInLiquid(bubble.position) || Grid.Element[num].id == bubble.element)
			{
				pooledList2.Add(bubble);
			}
			else
			{
				pooledList.Add(bubble);
			}
		}
		foreach (BubbleManager.Bubble bubble2 in pooledList2)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(bubble2.position), bubble2.element, CellEventLogger.Instance.FallingWaterAddToSim, bubble2.mass, bubble2.temperature, byte.MaxValue, 0, true, -1);
		}
		this.bubbles.Clear();
		this.bubbles.AddRange(pooledList);
		pooledList2.Recycle();
		pooledList.Recycle();
	}

	// Token: 0x06002BF0 RID: 11248 RVA: 0x000FFFA0 File Offset: 0x000FE1A0
	public void RenderEveryTick(float dt)
	{
		ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.PooledList pooledList = ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.Allocate();
		SpriteSheetAnimator spriteSheetAnimator = SpriteSheetAnimManager.instance.GetSpriteSheetAnimator("liquid_splash1");
		foreach (BubbleManager.Bubble bubble in this.bubbles)
		{
			SpriteSheetAnimator.AnimInfo item = new SpriteSheetAnimator.AnimInfo
			{
				frame = spriteSheetAnimator.GetFrameFromElapsedTimeLooping(bubble.elapsedTime),
				elapsedTime = bubble.elapsedTime,
				pos = new Vector3(bubble.position.x, bubble.position.y, 0f),
				rotation = Quaternion.identity,
				size = Vector2.one,
				colour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
			};
			pooledList.Add(item);
		}
		pooledList.Recycle();
	}

	// Token: 0x04001A15 RID: 6677
	public static BubbleManager instance;

	// Token: 0x04001A16 RID: 6678
	private List<BubbleManager.Bubble> bubbles = new List<BubbleManager.Bubble>();

	// Token: 0x020015B7 RID: 5559
	private struct Bubble
	{
		// Token: 0x04007275 RID: 29301
		public Vector2 position;

		// Token: 0x04007276 RID: 29302
		public Vector2 velocity;

		// Token: 0x04007277 RID: 29303
		public float elapsedTime;

		// Token: 0x04007278 RID: 29304
		public int frame;

		// Token: 0x04007279 RID: 29305
		public SimHashes element;

		// Token: 0x0400727A RID: 29306
		public float temperature;

		// Token: 0x0400727B RID: 29307
		public float mass;
	}
}
