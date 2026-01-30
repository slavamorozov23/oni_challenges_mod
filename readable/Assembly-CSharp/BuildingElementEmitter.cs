using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006FB RID: 1787
[AddComponentMenu("KMonoBehaviour/scripts/BuildingElementEmitter")]
public class BuildingElementEmitter : KMonoBehaviour, IGameObjectEffectDescriptor, IElementEmitter, ISim200ms
{
	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06002C37 RID: 11319 RVA: 0x00101B34 File Offset: 0x000FFD34
	public float AverageEmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06002C38 RID: 11320 RVA: 0x00101B4B File Offset: 0x000FFD4B
	public float EmitRate
	{
		get
		{
			return this.emitRate;
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06002C39 RID: 11321 RVA: 0x00101B53 File Offset: 0x000FFD53
	public SimHashes Element
	{
		get
		{
			return this.element;
		}
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x00101B5B File Offset: 0x000FFD5B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		base.Subscribe<BuildingElementEmitter>(824508782, BuildingElementEmitter.OnActiveChangedDelegate);
		this.SimRegister();
	}

	// Token: 0x06002C3B RID: 11323 RVA: 0x00101B95 File Offset: 0x000FFD95
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x06002C3C RID: 11324 RVA: 0x00101BB9 File Offset: 0x000FFDB9
	private void OnActiveChanged(object data)
	{
		this.simActive = ((Operational)data).IsActive;
		this.dirty = true;
	}

	// Token: 0x06002C3D RID: 11325 RVA: 0x00101BD3 File Offset: 0x000FFDD3
	public void Sim200ms(float dt)
	{
		this.UnsafeUpdate(dt);
	}

	// Token: 0x06002C3E RID: 11326 RVA: 0x00101BDC File Offset: 0x000FFDDC
	private unsafe void UnsafeUpdate(float dt)
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimState();
		int handleIndex = Sim.GetHandleIndex(this.simHandle);
		Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[handleIndex];
		if (emittedMassInfo.mass > 0f)
		{
			Game.Instance.accumulators.Accumulate(this.accumulator, emittedMassInfo.mass);
			if (this.element == SimHashes.Oxygen)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, emittedMassInfo.mass, base.gameObject.GetProperName(), null);
			}
		}
	}

	// Token: 0x06002C3F RID: 11327 RVA: 0x00101C7C File Offset: 0x000FFE7C
	private void UpdateSimState()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		if (this.simActive)
		{
			if (this.element != (SimHashes)0 && this.emitRate > 0f)
			{
				int game_cell = Grid.PosToCell(new Vector3(base.transform.GetPosition().x + this.modifierOffset.x, base.transform.GetPosition().y + this.modifierOffset.y, 0f));
				SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, this.element, 0.2f, this.emitRate * 0.2f, this.temperature, float.MaxValue, this.emitDiseaseIdx, this.emitDiseaseCount);
			}
			this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmittingElement, this);
			return;
		}
		SimMessages.ModifyElementEmitter(this.simHandle, 0, 0, SimHashes.Vacuum, 0f, 0f, 0f, 0f, byte.MaxValue, 0);
		this.statusHandle = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, this);
	}

	// Token: 0x06002C40 RID: 11328 RVA: 0x00101DB4 File Offset: 0x000FFFB4
	private void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1)
		{
			this.simHandle = -2;
			SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(BuildingElementEmitter.OnSimRegisteredCallback), this, "BuildingElementEmitter").index, -1, -1);
		}
	}

	// Token: 0x06002C41 RID: 11329 RVA: 0x00101E0F File Offset: 0x0010000F
	private void SimUnregister()
	{
		if (this.simHandle != -1)
		{
			if (Sim.IsValidHandle(this.simHandle))
			{
				SimMessages.RemoveElementEmitter(-1, this.simHandle);
			}
			this.simHandle = -1;
		}
	}

	// Token: 0x06002C42 RID: 11330 RVA: 0x00101E3A File Offset: 0x0010003A
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((BuildingElementEmitter)data).OnSimRegistered(handle);
	}

	// Token: 0x06002C43 RID: 11331 RVA: 0x00101E48 File Offset: 0x00100048
	private void OnSimRegistered(int handle)
	{
		if (this != null)
		{
			this.simHandle = handle;
			return;
		}
		SimMessages.RemoveElementEmitter(-1, handle);
	}

	// Token: 0x06002C44 RID: 11332 RVA: 0x00101E64 File Offset: 0x00100064
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.element).tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_FIXEDTEMP, arg, GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_FIXEDTEMP, arg, GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04001A36 RID: 6710
	[SerializeField]
	public float emitRate = 0.3f;

	// Token: 0x04001A37 RID: 6711
	[SerializeField]
	[Serialize]
	public float temperature = 293f;

	// Token: 0x04001A38 RID: 6712
	[SerializeField]
	[HashedEnum]
	public SimHashes element = SimHashes.Oxygen;

	// Token: 0x04001A39 RID: 6713
	[SerializeField]
	public Vector2 modifierOffset;

	// Token: 0x04001A3A RID: 6714
	[SerializeField]
	public byte emitRange = 1;

	// Token: 0x04001A3B RID: 6715
	[SerializeField]
	public byte emitDiseaseIdx = byte.MaxValue;

	// Token: 0x04001A3C RID: 6716
	[SerializeField]
	public int emitDiseaseCount;

	// Token: 0x04001A3D RID: 6717
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001A3E RID: 6718
	private int simHandle = -1;

	// Token: 0x04001A3F RID: 6719
	private bool simActive;

	// Token: 0x04001A40 RID: 6720
	private bool dirty = true;

	// Token: 0x04001A41 RID: 6721
	private Guid statusHandle;

	// Token: 0x04001A42 RID: 6722
	private static readonly EventSystem.IntraObjectHandler<BuildingElementEmitter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<BuildingElementEmitter>(delegate(BuildingElementEmitter component, object data)
	{
		component.OnActiveChanged(data);
	});
}
