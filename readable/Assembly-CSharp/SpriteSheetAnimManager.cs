using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200063D RID: 1597
[AddComponentMenu("KMonoBehaviour/scripts/SpriteSheetAnimManager")]
public class SpriteSheetAnimManager : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06002619 RID: 9753 RVA: 0x000DB368 File Offset: 0x000D9568
	public static void DestroyInstance()
	{
		SpriteSheetAnimManager.instance = null;
	}

	// Token: 0x0600261A RID: 9754 RVA: 0x000DB370 File Offset: 0x000D9570
	protected override void OnPrefabInit()
	{
		SpriteSheetAnimManager.instance = this;
	}

	// Token: 0x0600261B RID: 9755 RVA: 0x000DB378 File Offset: 0x000D9578
	protected override void OnSpawn()
	{
		for (int i = 0; i < this.sheets.Length; i++)
		{
			int key = Hash.SDBMLower(this.sheets[i].name);
			this.nameIndexMap[key] = new SpriteSheetAnimator(this.sheets[i]);
		}
	}

	// Token: 0x0600261C RID: 9756 RVA: 0x000DB3CC File Offset: 0x000D95CC
	public void Play(string name, Vector3 pos, Vector2 size, Color32 colour)
	{
		int name_hash = Hash.SDBMLower(name);
		this.Play(name_hash, pos, Quaternion.identity, size, colour);
	}

	// Token: 0x0600261D RID: 9757 RVA: 0x000DB3F0 File Offset: 0x000D95F0
	public void Play(string name, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
	{
		int name_hash = Hash.SDBMLower(name);
		this.Play(name_hash, pos, rotation, size, colour);
	}

	// Token: 0x0600261E RID: 9758 RVA: 0x000DB411 File Offset: 0x000D9611
	public void Play(int name_hash, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
	{
		this.nameIndexMap[name_hash].Play(pos, rotation, size, colour);
	}

	// Token: 0x0600261F RID: 9759 RVA: 0x000DB42F File Offset: 0x000D962F
	public void RenderEveryTick(float dt)
	{
		this.UpdateAnims(dt);
		this.Render();
	}

	// Token: 0x06002620 RID: 9760 RVA: 0x000DB440 File Offset: 0x000D9640
	public void UpdateAnims(float dt)
	{
		foreach (KeyValuePair<int, SpriteSheetAnimator> keyValuePair in this.nameIndexMap)
		{
			keyValuePair.Value.UpdateAnims(dt);
		}
	}

	// Token: 0x06002621 RID: 9761 RVA: 0x000DB49C File Offset: 0x000D969C
	public void Render()
	{
		Vector3 zero = Vector3.zero;
		foreach (KeyValuePair<int, SpriteSheetAnimator> keyValuePair in this.nameIndexMap)
		{
			keyValuePair.Value.Render();
		}
	}

	// Token: 0x06002622 RID: 9762 RVA: 0x000DB4FC File Offset: 0x000D96FC
	public SpriteSheetAnimator GetSpriteSheetAnimator(HashedString name)
	{
		return this.nameIndexMap[name.HashValue];
	}

	// Token: 0x0400167E RID: 5758
	public const float SECONDS_PER_FRAME = 0.033333335f;

	// Token: 0x0400167F RID: 5759
	[SerializeField]
	private SpriteSheet[] sheets;

	// Token: 0x04001680 RID: 5760
	private Dictionary<int, SpriteSheetAnimator> nameIndexMap = new Dictionary<int, SpriteSheetAnimator>();

	// Token: 0x04001681 RID: 5761
	public static SpriteSheetAnimManager instance;
}
