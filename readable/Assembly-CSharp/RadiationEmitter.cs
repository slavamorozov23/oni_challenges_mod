using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000AC2 RID: 2754
public class RadiationEmitter : SimComponent
{
	// Token: 0x0600500A RID: 20490 RVA: 0x001D1103 File Offset: 0x001CF303
	protected override void OnSpawn()
	{
		this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, RadiationEmitter.RefreshDispatcher, this, "RadiationEmitter.OnSpawn");
		base.OnSpawn();
	}

	// Token: 0x0600500B RID: 20491 RVA: 0x001D112C File Offset: 0x001CF32C
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
		base.OnCleanUp();
	}

	// Token: 0x0600500C RID: 20492 RVA: 0x001D1144 File Offset: 0x001CF344
	public void SetEmitting(bool emitting)
	{
		base.SetSimActive(emitting);
	}

	// Token: 0x0600500D RID: 20493 RVA: 0x001D114D File Offset: 0x001CF34D
	public int GetEmissionCell()
	{
		return Grid.PosToCell(base.transform.GetPosition() + this.emissionOffset);
	}

	// Token: 0x0600500E RID: 20494 RVA: 0x001D116C File Offset: 0x001CF36C
	public void Refresh()
	{
		int emissionCell = this.GetEmissionCell();
		if (this.radiusProportionalToRads)
		{
			this.SetRadiusProportionalToRads();
		}
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
	}

	// Token: 0x0600500F RID: 20495 RVA: 0x001D11CC File Offset: 0x001CF3CC
	private void SetRadiusProportionalToRads()
	{
		this.emitRadiusX = (short)Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128);
		this.emitRadiusY = (short)Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128);
	}

	// Token: 0x06005010 RID: 20496 RVA: 0x001D1220 File Offset: 0x001CF420
	protected override void OnSimActivate()
	{
		int emissionCell = this.GetEmissionCell();
		if (this.radiusProportionalToRads)
		{
			this.SetRadiusProportionalToRads();
		}
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
	}

	// Token: 0x06005011 RID: 20497 RVA: 0x001D1280 File Offset: 0x001CF480
	protected override void OnSimDeactivate()
	{
		int emissionCell = this.GetEmissionCell();
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, 0, 0, 0f, 0f, 0f, 0f, 0f, this.emitType);
	}

	// Token: 0x06005012 RID: 20498 RVA: 0x001D12C4 File Offset: 0x001CF4C4
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		Game.Instance.simComponentCallbackManager.GetItem(cb_handle);
		int emissionCell = this.GetEmissionCell();
		SimMessages.AddRadiationEmitter(cb_handle.index, emissionCell, 0, 0, 0f, 0f, 0f, 0f, 0f, this.emitType);
	}

	// Token: 0x06005013 RID: 20499 RVA: 0x001D1317 File Offset: 0x001CF517
	protected override void OnSimUnregister()
	{
		RadiationEmitter.StaticUnregister(this.simHandle);
	}

	// Token: 0x06005014 RID: 20500 RVA: 0x001D1324 File Offset: 0x001CF524
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveRadiationEmitter(-1, sim_handle);
	}

	// Token: 0x06005015 RID: 20501 RVA: 0x001D1338 File Offset: 0x001CF538
	private void OnDrawGizmosSelected()
	{
		int emissionCell = this.GetEmissionCell();
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(Grid.CellToPos(emissionCell) + Vector3.right / 2f + Vector3.up / 2f, 0.2f);
	}

	// Token: 0x06005016 RID: 20502 RVA: 0x001D138C File Offset: 0x001CF58C
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(RadiationEmitter.StaticUnregister);
	}

	// Token: 0x04003570 RID: 13680
	public bool radiusProportionalToRads;

	// Token: 0x04003571 RID: 13681
	[SerializeField]
	public short emitRadiusX = 4;

	// Token: 0x04003572 RID: 13682
	[SerializeField]
	public short emitRadiusY = 4;

	// Token: 0x04003573 RID: 13683
	[SerializeField]
	public float emitRads = 10f;

	// Token: 0x04003574 RID: 13684
	[SerializeField]
	public float emitRate = 1f;

	// Token: 0x04003575 RID: 13685
	[SerializeField]
	public float emitSpeed = 1f;

	// Token: 0x04003576 RID: 13686
	[SerializeField]
	public float emitDirection;

	// Token: 0x04003577 RID: 13687
	[SerializeField]
	public float emitAngle = 360f;

	// Token: 0x04003578 RID: 13688
	[SerializeField]
	public RadiationEmitter.RadiationEmitterType emitType;

	// Token: 0x04003579 RID: 13689
	[SerializeField]
	public Vector3 emissionOffset = Vector3.zero;

	// Token: 0x0400357A RID: 13690
	private ulong cellChangedHandlerID;

	// Token: 0x0400357B RID: 13691
	private static readonly Action<object> RefreshDispatcher = delegate(object obj)
	{
		Unsafe.As<RadiationEmitter>(obj).Refresh();
	};

	// Token: 0x02001C08 RID: 7176
	public enum RadiationEmitterType
	{
		// Token: 0x040086C5 RID: 34501
		Constant,
		// Token: 0x040086C6 RID: 34502
		Pulsing,
		// Token: 0x040086C7 RID: 34503
		PulsingAveraged,
		// Token: 0x040086C8 RID: 34504
		SimplePulse,
		// Token: 0x040086C9 RID: 34505
		RadialBeams,
		// Token: 0x040086CA RID: 34506
		Attractor
	}
}
