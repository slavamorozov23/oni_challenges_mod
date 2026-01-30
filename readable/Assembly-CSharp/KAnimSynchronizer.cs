using System;
using System.Collections.Generic;

// Token: 0x02000550 RID: 1360
public class KAnimSynchronizer
{
	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x000A1F0E File Offset: 0x000A010E
	// (set) Token: 0x06001DE4 RID: 7652 RVA: 0x000A1F16 File Offset: 0x000A0116
	public string IdleAnim
	{
		get
		{
			return this.idle_anim;
		}
		set
		{
			this.idle_anim = value;
		}
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x000A1F1F File Offset: 0x000A011F
	public KAnimSynchronizer(KAnimControllerBase master_controller)
	{
		this.masterController = master_controller;
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x000A1F4F File Offset: 0x000A014F
	private void Clear(KAnimControllerBase controller)
	{
		controller.Play(this.IdleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x000A1F6D File Offset: 0x000A016D
	public void Add(KAnimControllerBase controller)
	{
		this.Targets.Add(controller);
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x000A1F7B File Offset: 0x000A017B
	public void Remove(KAnimControllerBase controller)
	{
		this.Clear(controller);
		this.Targets.Remove(controller);
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x000A1F91 File Offset: 0x000A0191
	public void RemoveWithoutIdleAnim(KAnimControllerBase controller)
	{
		this.Targets.Remove(controller);
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x000A1FA0 File Offset: 0x000A01A0
	private void Clear(KAnimSynchronizedController controller)
	{
		controller.Play(this.IdleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06001DEB RID: 7659 RVA: 0x000A1FBE File Offset: 0x000A01BE
	public void Add(KAnimSynchronizedController controller)
	{
		this.SyncedControllers.Add(controller);
	}

	// Token: 0x06001DEC RID: 7660 RVA: 0x000A1FCC File Offset: 0x000A01CC
	public void Remove(KAnimSynchronizedController controller)
	{
		this.Clear(controller);
		this.SyncedControllers.Remove(controller);
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x000A1FE4 File Offset: 0x000A01E4
	public void Clear()
	{
		foreach (KAnimControllerBase kanimControllerBase in this.Targets)
		{
			if (!(kanimControllerBase == null) && kanimControllerBase.AnimFiles != null)
			{
				this.Clear(kanimControllerBase);
			}
		}
		this.Targets.Clear();
		foreach (KAnimSynchronizedController kanimSynchronizedController in this.SyncedControllers)
		{
			if (!(kanimSynchronizedController.synchronizedController == null) && kanimSynchronizedController.synchronizedController.AnimFiles != null)
			{
				this.Clear(kanimSynchronizedController);
			}
		}
		this.SyncedControllers.Clear();
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x000A20BC File Offset: 0x000A02BC
	public void Sync(KAnimControllerBase controller)
	{
		if (this.masterController == null)
		{
			return;
		}
		if (controller == null)
		{
			return;
		}
		KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
		if (currentAnim != null && !string.IsNullOrEmpty(controller.defaultAnim) && !controller.HasAnimation(currentAnim.name))
		{
			controller.Play(controller.defaultAnim, KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (currentAnim == null)
		{
			return;
		}
		KAnim.PlayMode mode = this.masterController.GetMode();
		float playSpeed = this.masterController.GetPlaySpeed();
		float elapsedTime = this.masterController.GetElapsedTime();
		controller.Play(currentAnim.name, mode, playSpeed, elapsedTime);
		Facing component = controller.GetComponent<Facing>();
		if (component != null)
		{
			float num = component.transform.GetPosition().x;
			num += (this.masterController.FlipX ? -0.5f : 0.5f);
			component.Face(num);
			return;
		}
		controller.FlipX = this.masterController.FlipX;
		controller.FlipY = this.masterController.FlipY;
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x000A21DC File Offset: 0x000A03DC
	public void SyncController(KAnimSynchronizedController controller)
	{
		if (this.masterController == null)
		{
			return;
		}
		if (controller == null)
		{
			return;
		}
		KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
		string s = (currentAnim != null) ? (currentAnim.name + controller.Postfix) : string.Empty;
		if (!string.IsNullOrEmpty(controller.synchronizedController.defaultAnim) && !controller.synchronizedController.HasAnimation(s))
		{
			controller.Play(controller.synchronizedController.defaultAnim, KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (currentAnim == null)
		{
			return;
		}
		KAnim.PlayMode mode = this.masterController.GetMode();
		float playSpeed = this.masterController.GetPlaySpeed();
		float elapsedTime = this.masterController.GetElapsedTime();
		controller.Play(s, mode, playSpeed, elapsedTime);
		Facing component = controller.synchronizedController.GetComponent<Facing>();
		if (component != null)
		{
			float num = component.transform.GetPosition().x;
			num += (this.masterController.FlipX ? -0.5f : 0.5f);
			component.Face(num);
			return;
		}
		controller.synchronizedController.FlipX = this.masterController.FlipX;
		controller.synchronizedController.FlipY = this.masterController.FlipY;
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x000A2324 File Offset: 0x000A0524
	public void Sync()
	{
		for (int i = 0; i < this.Targets.Count; i++)
		{
			KAnimControllerBase controller = this.Targets[i];
			this.Sync(controller);
		}
		for (int j = 0; j < this.SyncedControllers.Count; j++)
		{
			KAnimSynchronizedController controller2 = this.SyncedControllers[j];
			this.SyncController(controller2);
		}
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x000A2388 File Offset: 0x000A0588
	public void SyncTime()
	{
		float elapsedTime = this.masterController.GetElapsedTime();
		for (int i = 0; i < this.Targets.Count; i++)
		{
			this.Targets[i].SetElapsedTime(elapsedTime);
		}
		for (int j = 0; j < this.SyncedControllers.Count; j++)
		{
			this.SyncedControllers[j].synchronizedController.SetElapsedTime(elapsedTime);
		}
	}

	// Token: 0x0400117E RID: 4478
	private string idle_anim = "idle_default";

	// Token: 0x0400117F RID: 4479
	private KAnimControllerBase masterController;

	// Token: 0x04001180 RID: 4480
	private List<KAnimControllerBase> Targets = new List<KAnimControllerBase>();

	// Token: 0x04001181 RID: 4481
	private List<KAnimSynchronizedController> SyncedControllers = new List<KAnimSynchronizedController>();
}
