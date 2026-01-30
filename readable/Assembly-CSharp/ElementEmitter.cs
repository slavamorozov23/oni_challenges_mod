using System;
using UnityEngine;

// Token: 0x02000917 RID: 2327
public class ElementEmitter : SimComponent
{
	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x060040DB RID: 16603 RVA: 0x0016F6D3 File Offset: 0x0016D8D3
	// (set) Token: 0x060040DC RID: 16604 RVA: 0x0016F6DB File Offset: 0x0016D8DB
	public bool isEmitterBlocked { get; private set; }

	// Token: 0x060040DD RID: 16605 RVA: 0x0016F6E4 File Offset: 0x0016D8E4
	protected override void OnSpawn()
	{
		this.onBlockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnEmitterBlocked), true));
		this.onUnblockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnEmitterUnblocked), true));
		base.OnSpawn();
	}

	// Token: 0x060040DE RID: 16606 RVA: 0x0016F745 File Offset: 0x0016D945
	protected override void OnCleanUp()
	{
		Game.Instance.ManualReleaseHandle(this.onBlockedHandle);
		Game.Instance.ManualReleaseHandle(this.onUnblockedHandle);
		base.OnCleanUp();
	}

	// Token: 0x060040DF RID: 16607 RVA: 0x0016F76D File Offset: 0x0016D96D
	public void SetEmitting(bool emitting)
	{
		base.SetSimActive(emitting);
	}

	// Token: 0x060040E0 RID: 16608 RVA: 0x0016F778 File Offset: 0x0016D978
	protected override void OnSimActivate()
	{
		int game_cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		if (this.outputElement.elementHash != (SimHashes)0 && this.outputElement.massGenerationRate > 0f && this.emissionFrequency > 0f)
		{
			float emit_temperature = (this.outputElement.minOutputTemperature == 0f) ? base.GetComponent<PrimaryElement>().Temperature : this.outputElement.minOutputTemperature;
			SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, this.outputElement.elementHash, this.emissionFrequency, this.outputElement.massGenerationRate, emit_temperature, this.maxPressure, this.outputElement.addedDiseaseIdx, this.outputElement.addedDiseaseCount);
		}
		if (this.showDescriptor)
		{
			this.statusHandle = base.GetComponent<KSelectable>().ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.ElementEmitterOutput, this);
		}
	}

	// Token: 0x060040E1 RID: 16609 RVA: 0x0016F894 File Offset: 0x0016DA94
	protected override void OnSimDeactivate()
	{
		int game_cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, SimHashes.Vacuum, 0f, 0f, 0f, 0f, byte.MaxValue, 0);
		if (this.showDescriptor)
		{
			this.statusHandle = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
		}
	}

	// Token: 0x060040E2 RID: 16610 RVA: 0x0016F92C File Offset: 0x0016DB2C
	public void ForceEmit(float mass, byte disease_idx, int disease_count, float temperature = -1f)
	{
		if (mass <= 0f)
		{
			return;
		}
		float temperature2 = (temperature > 0f) ? temperature : this.outputElement.minOutputTemperature;
		Element element = ElementLoader.FindElementByHash(this.outputElement.elementHash);
		if (element.IsGas || element.IsLiquid)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), this.outputElement.elementHash, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature2, disease_idx, disease_count, true, -1);
		}
		else if (element.IsSolid)
		{
			element.substance.SpawnResource(base.transform.GetPosition() + new Vector3(0f, 0.5f, 0f), mass, temperature2, disease_idx, disease_count, false, true, false);
		}
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ElementLoader.FindElementByHash(this.outputElement.elementHash).name, base.gameObject.transform, 1.5f, false);
	}

	// Token: 0x060040E3 RID: 16611 RVA: 0x0016FA28 File Offset: 0x0016DC28
	private void OnEmitterBlocked()
	{
		this.isEmitterBlocked = true;
		base.Trigger(1615168894, this);
	}

	// Token: 0x060040E4 RID: 16612 RVA: 0x0016FA3D File Offset: 0x0016DC3D
	private void OnEmitterUnblocked()
	{
		this.isEmitterBlocked = false;
		base.Trigger(-657992955, this);
	}

	// Token: 0x060040E5 RID: 16613 RVA: 0x0016FA52 File Offset: 0x0016DC52
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		Game.Instance.simComponentCallbackManager.GetItem(cb_handle);
		SimMessages.AddElementEmitter(this.maxPressure, cb_handle.index, this.onBlockedHandle.index, this.onUnblockedHandle.index);
	}

	// Token: 0x060040E6 RID: 16614 RVA: 0x0016FA8D File Offset: 0x0016DC8D
	protected override void OnSimUnregister()
	{
		ElementEmitter.StaticUnregister(this.simHandle);
	}

	// Token: 0x060040E7 RID: 16615 RVA: 0x0016FA9A File Offset: 0x0016DC9A
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveElementEmitter(-1, sim_handle);
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x0016FAB0 File Offset: 0x0016DCB0
	private void OnDrawGizmosSelected()
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(Grid.CellToPos(cell) + Vector3.right / 2f + Vector3.up / 2f, 0.2f);
	}

	// Token: 0x060040E9 RID: 16617 RVA: 0x0016FB35 File Offset: 0x0016DD35
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(ElementEmitter.StaticUnregister);
	}

	// Token: 0x04002896 RID: 10390
	[SerializeField]
	public ElementConverter.OutputElement outputElement;

	// Token: 0x04002897 RID: 10391
	[SerializeField]
	public float emissionFrequency = 1f;

	// Token: 0x04002898 RID: 10392
	[SerializeField]
	public byte emitRange = 1;

	// Token: 0x04002899 RID: 10393
	[SerializeField]
	public float maxPressure = 1f;

	// Token: 0x0400289A RID: 10394
	private Guid statusHandle = Guid.Empty;

	// Token: 0x0400289B RID: 10395
	public bool showDescriptor = true;

	// Token: 0x0400289C RID: 10396
	private HandleVector<Game.CallbackInfo>.Handle onBlockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x0400289D RID: 10397
	private HandleVector<Game.CallbackInfo>.Handle onUnblockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
}
