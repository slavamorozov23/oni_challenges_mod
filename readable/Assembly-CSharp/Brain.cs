using System;
using UnityEngine;

// Token: 0x02000487 RID: 1159
[AddComponentMenu("KMonoBehaviour/scripts/Brain")]
public class Brain : KMonoBehaviour
{
	// Token: 0x0600188C RID: 6284 RVA: 0x00088A8B File Offset: 0x00086C8B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600188D RID: 6285 RVA: 0x00088A93 File Offset: 0x00086C93
	protected override void OnSpawn()
	{
		this.prefabId = base.GetComponent<KPrefabID>();
		this.choreConsumer = base.GetComponent<ChoreConsumer>();
		this.running = true;
		Components.Brains.Add(this);
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600188E RID: 6286 RVA: 0x00088AC0 File Offset: 0x00086CC0
	// (remove) Token: 0x0600188F RID: 6287 RVA: 0x00088AF8 File Offset: 0x00086CF8
	public event System.Action onPreUpdate;

	// Token: 0x06001890 RID: 6288 RVA: 0x00088B2D File Offset: 0x00086D2D
	public virtual void UpdateBrain()
	{
		SuperluminalPerf.BeginEvent("UpdateBrain", base.name);
		if (this.onPreUpdate != null)
		{
			this.onPreUpdate();
		}
		if (this.IsRunning())
		{
			this.UpdateChores();
		}
		SuperluminalPerf.EndEvent();
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x00088B66 File Offset: 0x00086D66
	private bool FindBetterChore(ref Chore.Precondition.Context context)
	{
		return this.choreConsumer.FindNextChore(ref context);
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x00088B74 File Offset: 0x00086D74
	private void UpdateChores()
	{
		if (this.prefabId.HasTag(GameTags.PreventChoreInterruption))
		{
			return;
		}
		Chore.Precondition.Context chore = default(Chore.Precondition.Context);
		if (this.FindBetterChore(ref chore))
		{
			if (this.prefabId.HasTag(GameTags.PerformingWorkRequest))
			{
				base.Trigger(1485595942, null);
				return;
			}
			this.choreConsumer.choreDriver.SetChore(chore);
		}
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x00088BD6 File Offset: 0x00086DD6
	public bool IsRunning()
	{
		return this.running && !this.suspend;
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x00088BEB File Offset: 0x00086DEB
	public void Reset(string reason)
	{
		this.Stop("Reset");
		this.running = true;
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x00088BFF File Offset: 0x00086DFF
	public void Stop(string reason)
	{
		base.GetComponent<ChoreDriver>().StopChore();
		this.running = false;
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x00088C13 File Offset: 0x00086E13
	public void Resume(string caller)
	{
		this.suspend = false;
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x00088C1C File Offset: 0x00086E1C
	public void Suspend(string caller)
	{
		this.suspend = true;
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x00088C25 File Offset: 0x00086E25
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.Stop("OnCmpDisable");
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x00088C38 File Offset: 0x00086E38
	protected override void OnCleanUp()
	{
		this.Stop("OnCleanUp");
		Components.Brains.Remove(this);
	}

	// Token: 0x04000E3D RID: 3645
	private bool running;

	// Token: 0x04000E3E RID: 3646
	private bool suspend;

	// Token: 0x04000E3F RID: 3647
	protected KPrefabID prefabId;

	// Token: 0x04000E40 RID: 3648
	protected ChoreConsumer choreConsumer;
}
