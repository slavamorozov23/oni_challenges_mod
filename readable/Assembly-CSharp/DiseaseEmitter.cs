using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020008FA RID: 2298
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseEmitter")]
public class DiseaseEmitter : KMonoBehaviour
{
	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x06003FC7 RID: 16327 RVA: 0x001673F6 File Offset: 0x001655F6
	public float EmitRate
	{
		get
		{
			return this.emitRate;
		}
	}

	// Token: 0x06003FC8 RID: 16328 RVA: 0x00167400 File Offset: 0x00165600
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.emitDiseases != null)
		{
			this.simHandles = new int[this.emitDiseases.Length];
			for (int i = 0; i < this.simHandles.Length; i++)
			{
				this.simHandles[i] = -1;
			}
		}
		this.SimRegister();
	}

	// Token: 0x06003FC9 RID: 16329 RVA: 0x00167450 File Offset: 0x00165650
	protected override void OnCleanUp()
	{
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x06003FCA RID: 16330 RVA: 0x0016745E File Offset: 0x0016565E
	public void SetEnable(bool enable)
	{
		if (this.enableEmitter == enable)
		{
			return;
		}
		this.enableEmitter = enable;
		if (this.enableEmitter)
		{
			this.SimRegister();
			return;
		}
		this.SimUnregister();
	}

	// Token: 0x06003FCB RID: 16331 RVA: 0x00167488 File Offset: 0x00165688
	private void OnCellChanged()
	{
		if (this.simHandles == null || !this.enableEmitter)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		if (Grid.IsValidCell(cell))
		{
			for (int i = 0; i < this.emitDiseases.Length; i++)
			{
				if (Sim.IsValidHandle(this.simHandles[i]))
				{
					SimMessages.ModifyDiseaseEmitter(this.simHandles[i], cell, this.emitRange, this.emitDiseases[i], this.emitRate, this.emitCount);
				}
			}
		}
	}

	// Token: 0x06003FCC RID: 16332 RVA: 0x00167500 File Offset: 0x00165700
	private void SimRegister()
	{
		if (this.simHandles == null || !this.enableEmitter)
		{
			return;
		}
		this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, DiseaseEmitter.OnCellChangedDispatcher, "DiseaseEmitter.Modify", null);
		for (int i = 0; i < this.simHandles.Length; i++)
		{
			if (this.simHandles[i] == -1)
			{
				this.simHandles[i] = -2;
				SimMessages.AddDiseaseEmitter(Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(DiseaseEmitter.OnSimRegisteredCallback), this, "DiseaseEmitter").index);
			}
		}
	}

	// Token: 0x06003FCD RID: 16333 RVA: 0x00167598 File Offset: 0x00165798
	private void SimUnregister()
	{
		if (this.simHandles == null)
		{
			return;
		}
		for (int i = 0; i < this.simHandles.Length; i++)
		{
			if (Sim.IsValidHandle(this.simHandles[i]))
			{
				SimMessages.RemoveDiseaseEmitter(-1, this.simHandles[i]);
			}
			this.simHandles[i] = -1;
		}
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
	}

	// Token: 0x06003FCE RID: 16334 RVA: 0x001675F7 File Offset: 0x001657F7
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((DiseaseEmitter)data).OnSimRegistered(handle);
	}

	// Token: 0x06003FCF RID: 16335 RVA: 0x00167608 File Offset: 0x00165808
	private void OnSimRegistered(int handle)
	{
		bool flag = false;
		if (this != null)
		{
			for (int i = 0; i < this.simHandles.Length; i++)
			{
				if (this.simHandles[i] == -2)
				{
					this.simHandles[i] = handle;
					flag = true;
					break;
				}
			}
			this.OnCellChanged();
		}
		if (!flag)
		{
			SimMessages.RemoveDiseaseEmitter(-1, handle);
		}
	}

	// Token: 0x06003FD0 RID: 16336 RVA: 0x0016765C File Offset: 0x0016585C
	public void SetDiseases(List<Disease> diseases)
	{
		this.emitDiseases = new byte[diseases.Count];
		for (int i = 0; i < diseases.Count; i++)
		{
			this.emitDiseases[i] = Db.Get().Diseases.GetIndex(diseases[i].id);
		}
	}

	// Token: 0x0400277D RID: 10109
	[Serialize]
	public float emitRate = 1f;

	// Token: 0x0400277E RID: 10110
	[Serialize]
	public byte emitRange;

	// Token: 0x0400277F RID: 10111
	[Serialize]
	public int emitCount;

	// Token: 0x04002780 RID: 10112
	[Serialize]
	public byte[] emitDiseases;

	// Token: 0x04002781 RID: 10113
	public int[] simHandles;

	// Token: 0x04002782 RID: 10114
	[Serialize]
	private bool enableEmitter;

	// Token: 0x04002783 RID: 10115
	private ulong cellChangedHandlerID;

	// Token: 0x04002784 RID: 10116
	private static Action<object> OnCellChangedDispatcher = delegate(object obj)
	{
		Unsafe.As<DiseaseEmitter>(obj).OnCellChanged();
	};
}
