using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02000DDE RID: 3550
public class PopFXManager : KScreen
{
	// Token: 0x06006F95 RID: 28565 RVA: 0x002A62B8 File Offset: 0x002A44B8
	public static void DestroyInstance()
	{
		PopFXManager.Instance = null;
	}

	// Token: 0x06006F96 RID: 28566 RVA: 0x002A62C0 File Offset: 0x002A44C0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PopFXManager.Instance = this;
		this.Prefab_PopFxGroup = new GameObject("Prefab_PopFxGroup");
		this.Prefab_PopFxGroup.AddComponent<PopFxGroup>();
		this.Prefab_PopFxGroup.transform.SetParent(this.Prefab_PopFX.transform.parent);
	}

	// Token: 0x06006F97 RID: 28567 RVA: 0x002A6318 File Offset: 0x002A4518
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ready = true;
		if (GenericGameSettings.instance.disablePopFx)
		{
			return;
		}
		for (int i = 0; i < 20; i++)
		{
			PopFxGroup item = this.CreatePopFxGroup();
			PopFX item2 = this.CreatePopFX();
			this.Pool.Add(item2);
			this.GroupPool.Add(item);
		}
	}

	// Token: 0x06006F98 RID: 28568 RVA: 0x002A6372 File Offset: 0x002A4572
	public bool Ready()
	{
		return this.ready;
	}

	// Token: 0x06006F99 RID: 28569 RVA: 0x002A637A File Offset: 0x002A457A
	public PopFX SpawnFX(Sprite mainIcon, string text, Transform target_transform, float lifetime = 1.5f, bool track_target = false)
	{
		return this.SpawnFX(mainIcon, text, target_transform, Vector3.zero, lifetime, track_target, false);
	}

	// Token: 0x06006F9A RID: 28570 RVA: 0x002A6390 File Offset: 0x002A4590
	public PopFX SpawnFX(Sprite mainIcon, string text, Transform target_transform, Vector3 offset, float lifetime = 1.5f, bool track_target = false, bool force_spawn = false)
	{
		return this.SpawnFX(mainIcon, null, text, target_transform, offset, lifetime, true, track_target, force_spawn);
	}

	// Token: 0x06006F9B RID: 28571 RVA: 0x002A63B0 File Offset: 0x002A45B0
	public PopFX SpawnFX(Sprite mainIcon, Sprite secondaryIcon, string text, Transform target_transform, Vector3 offset, float lifetime = 1.5f, bool selfAdjustPositionIfInGroup = true, bool track_target = false, bool force_spawn = false)
	{
		if (GenericGameSettings.instance.disablePopFx)
		{
			return null;
		}
		if (Game.IsQuitting())
		{
			return null;
		}
		Vector3 vector = offset;
		if (target_transform != null)
		{
			vector += target_transform.GetPosition();
		}
		int num = Grid.PosToCell(vector);
		if (!force_spawn && (!Grid.IsValidCell(num) || !Grid.IsVisible(num) || (CameraController.Instance != null && !CameraController.Instance.IsVisiblePosExtended(vector))))
		{
			return null;
		}
		PopFX orCreatePopFX = this.GetOrCreatePopFX(mainIcon, secondaryIcon, text, target_transform, offset, selfAdjustPositionIfInGroup, lifetime, track_target);
		PopFxGroup popFxGroup;
		if (!this.AliveGroups.TryGetValue(num, out popFxGroup) || popFxGroup == null)
		{
			if (this.GroupPool.Count > 0)
			{
				popFxGroup = this.GroupPool[0];
				this.GroupPool[0].gameObject.SetActive(true);
				this.GroupPool.RemoveAt(0);
			}
			else
			{
				popFxGroup = this.CreatePopFxGroup();
				popFxGroup.gameObject.SetActive(true);
			}
			this.AliveGroups.Add(num, popFxGroup);
		}
		popFxGroup.Enqueue(orCreatePopFX);
		popFxGroup.WakeUp(num);
		return orCreatePopFX;
	}

	// Token: 0x06006F9C RID: 28572 RVA: 0x002A64C4 File Offset: 0x002A46C4
	private PopFX GetOrCreatePopFX(Sprite mainIcon, Sprite secondaryIcon, string text, Transform target_transform, Vector3 offset, bool selfAdjustPositionIfInGroup = true, float lifetime = 1.5f, bool track_target = false)
	{
		PopFX popFX;
		if (this.Pool.Count > 0)
		{
			popFX = this.Pool[0];
			this.Pool[0].Setup(mainIcon, secondaryIcon, text, target_transform, offset, selfAdjustPositionIfInGroup, lifetime, track_target);
			this.Pool.RemoveAt(0);
		}
		else
		{
			popFX = this.CreatePopFX();
			popFX.Setup(mainIcon, secondaryIcon, text, target_transform, offset, selfAdjustPositionIfInGroup, lifetime, track_target);
		}
		return popFX;
	}

	// Token: 0x06006F9D RID: 28573 RVA: 0x002A6535 File Offset: 0x002A4735
	private PopFX CreatePopFX()
	{
		bool activeInHierarchy = this.Prefab_PopFX.gameObject.activeInHierarchy;
		GameObject gameObject = Util.KInstantiate(this.Prefab_PopFX, base.gameObject, "Pooled_PopFX");
		gameObject.transform.localScale = Vector3.one;
		return gameObject.GetComponent<PopFX>();
	}

	// Token: 0x06006F9E RID: 28574 RVA: 0x002A6573 File Offset: 0x002A4773
	private PopFxGroup CreatePopFxGroup()
	{
		GameObject gameObject = Util.KInstantiate(this.Prefab_PopFxGroup, base.gameObject, "Pooled_PopFxGroup");
		gameObject.transform.localScale = Vector3.one;
		return gameObject.GetComponent<PopFxGroup>();
	}

	// Token: 0x06006F9F RID: 28575 RVA: 0x002A65A0 File Offset: 0x002A47A0
	public void RecycleFX(PopFX fx)
	{
		this.Pool.Add(fx);
	}

	// Token: 0x06006FA0 RID: 28576 RVA: 0x002A65AE File Offset: 0x002A47AE
	public void RecycleFxGroup(int key, PopFxGroup fx)
	{
		this.AliveGroups.Remove(key);
		this.GroupPool.Add(fx);
	}

	// Token: 0x04004C69 RID: 19561
	public static PopFXManager Instance;

	// Token: 0x04004C6A RID: 19562
	private GameObject Prefab_PopFxGroup;

	// Token: 0x04004C6B RID: 19563
	public GameObject Prefab_PopFX;

	// Token: 0x04004C6C RID: 19564
	public List<PopFX> Pool = new List<PopFX>();

	// Token: 0x04004C6D RID: 19565
	public List<PopFxGroup> GroupPool = new List<PopFxGroup>();

	// Token: 0x04004C6E RID: 19566
	public Dictionary<int, PopFxGroup> AliveGroups = new Dictionary<int, PopFxGroup>();

	// Token: 0x04004C6F RID: 19567
	public Sprite sprite_Plus;

	// Token: 0x04004C70 RID: 19568
	public Sprite sprite_Negative;

	// Token: 0x04004C71 RID: 19569
	public Sprite sprite_Resource;

	// Token: 0x04004C72 RID: 19570
	public Sprite sprite_Building;

	// Token: 0x04004C73 RID: 19571
	public Sprite sprite_Research;

	// Token: 0x04004C74 RID: 19572
	private bool ready;
}
