using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000B49 RID: 2889
public abstract class SimComponent : KMonoBehaviour, ISim200ms
{
	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x06005507 RID: 21767 RVA: 0x001F02B4 File Offset: 0x001EE4B4
	public bool IsSimActive
	{
		get
		{
			return this.simActive;
		}
	}

	// Token: 0x06005508 RID: 21768 RVA: 0x001F02BC File Offset: 0x001EE4BC
	protected virtual void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
	}

	// Token: 0x06005509 RID: 21769 RVA: 0x001F02BE File Offset: 0x001EE4BE
	protected virtual void OnSimRegistered()
	{
	}

	// Token: 0x0600550A RID: 21770 RVA: 0x001F02C0 File Offset: 0x001EE4C0
	protected virtual void OnSimActivate()
	{
	}

	// Token: 0x0600550B RID: 21771 RVA: 0x001F02C2 File Offset: 0x001EE4C2
	protected virtual void OnSimDeactivate()
	{
	}

	// Token: 0x0600550C RID: 21772 RVA: 0x001F02C4 File Offset: 0x001EE4C4
	protected virtual void OnSimUnregister()
	{
	}

	// Token: 0x0600550D RID: 21773
	protected abstract Action<int> GetStaticUnregister();

	// Token: 0x0600550E RID: 21774 RVA: 0x001F02C6 File Offset: 0x001EE4C6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600550F RID: 21775 RVA: 0x001F02CE File Offset: 0x001EE4CE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SimRegister();
	}

	// Token: 0x06005510 RID: 21776 RVA: 0x001F02DC File Offset: 0x001EE4DC
	protected override void OnCleanUp()
	{
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x06005511 RID: 21777 RVA: 0x001F02EA File Offset: 0x001EE4EA
	public void SetSimActive(bool active)
	{
		this.simActive = active;
		this.dirty = true;
	}

	// Token: 0x06005512 RID: 21778 RVA: 0x001F02FA File Offset: 0x001EE4FA
	public void Sim200ms(float dt)
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimState();
	}

	// Token: 0x06005513 RID: 21779 RVA: 0x001F0310 File Offset: 0x001EE510
	private void UpdateSimState()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		if (this.simActive)
		{
			this.OnSimActivate();
			return;
		}
		this.OnSimDeactivate();
	}

	// Token: 0x06005514 RID: 21780 RVA: 0x001F0338 File Offset: 0x001EE538
	private void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1)
		{
			this.simHandle = -2;
			Action<int> static_unregister = this.GetStaticUnregister();
			HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle = Game.Instance.simComponentCallbackManager.Add(delegate(int handle, object data)
			{
				SimComponent.OnSimRegistered(this, handle, static_unregister);
			}, this, "SimComponent.SimRegister");
			this.OnSimRegister(cb_handle);
		}
	}

	// Token: 0x06005515 RID: 21781 RVA: 0x001F03A0 File Offset: 0x001EE5A0
	private void SimUnregister()
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			this.OnSimUnregister();
		}
		this.simHandle = -1;
	}

	// Token: 0x06005516 RID: 21782 RVA: 0x001F03BC File Offset: 0x001EE5BC
	private static void OnSimRegistered(SimComponent instance, int handle, Action<int> static_unregister)
	{
		if (instance != null)
		{
			instance.simHandle = handle;
			instance.OnSimRegistered();
			return;
		}
		static_unregister(handle);
	}

	// Token: 0x06005517 RID: 21783 RVA: 0x001F03DC File Offset: 0x001EE5DC
	[Conditional("ENABLE_LOGGER")]
	protected void Log(string msg)
	{
	}

	// Token: 0x0400396E RID: 14702
	[SerializeField]
	protected int simHandle = -1;

	// Token: 0x0400396F RID: 14703
	private bool simActive = true;

	// Token: 0x04003970 RID: 14704
	private bool dirty = true;
}
