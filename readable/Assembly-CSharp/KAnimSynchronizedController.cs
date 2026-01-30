using System;
using UnityEngine;

// Token: 0x0200054F RID: 1359
public class KAnimSynchronizedController
{
	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06001DDD RID: 7645 RVA: 0x000A1D44 File Offset: 0x0009FF44
	// (set) Token: 0x06001DDE RID: 7646 RVA: 0x000A1D4C File Offset: 0x0009FF4C
	public string Postfix
	{
		get
		{
			return this.postfix;
		}
		set
		{
			this.postfix = value;
		}
	}

	// Token: 0x06001DDF RID: 7647 RVA: 0x000A1D58 File Offset: 0x0009FF58
	public KAnimSynchronizedController(KAnimControllerBase controller, Grid.SceneLayer layer, string postfix)
	{
		this.controller = controller;
		this.Postfix = postfix;
		GameObject gameObject = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, controller.gameObject, null);
		gameObject.name = controller.name + postfix;
		this.synchronizedController = gameObject.GetComponent<KAnimControllerBase>();
		this.synchronizedController.AnimFiles = controller.AnimFiles;
		gameObject.SetActive(true);
		this.synchronizedController.initialAnim = controller.initialAnim + postfix;
		this.synchronizedController.defaultAnim = this.synchronizedController.initialAnim;
		Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(layer) - 0.1f);
		gameObject.transform.SetLocalPosition(position);
		this.link = new KAnimLink(controller, this.synchronizedController);
		this.Dirty();
		KAnimSynchronizer synchronizer = controller.GetSynchronizer();
		synchronizer.Add(this);
		synchronizer.SyncController(this);
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x000A1E48 File Offset: 0x000A0048
	public void Enable(bool enable)
	{
		this.synchronizedController.enabled = enable;
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x000A1E56 File Offset: 0x000A0056
	public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (this.synchronizedController.enabled && this.synchronizedController.HasAnimation(anim_name))
		{
			this.synchronizedController.Play(anim_name, mode, speed, time_offset);
		}
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x000A1E84 File Offset: 0x000A0084
	public void Dirty()
	{
		if (this.synchronizedController == null)
		{
			return;
		}
		this.synchronizedController.Offset = this.controller.Offset;
		this.synchronizedController.Pivot = this.controller.Pivot;
		this.synchronizedController.Rotation = this.controller.Rotation;
		this.synchronizedController.FlipX = this.controller.FlipX;
		this.synchronizedController.FlipY = this.controller.FlipY;
	}

	// Token: 0x0400117A RID: 4474
	private KAnimControllerBase controller;

	// Token: 0x0400117B RID: 4475
	public KAnimControllerBase synchronizedController;

	// Token: 0x0400117C RID: 4476
	private KAnimLink link;

	// Token: 0x0400117D RID: 4477
	private string postfix;
}
