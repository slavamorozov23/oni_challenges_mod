using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000813 RID: 2067
public class Teleporter : KMonoBehaviour
{
	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x060037F3 RID: 14323 RVA: 0x00139AB5 File Offset: 0x00137CB5
	// (set) Token: 0x060037F4 RID: 14324 RVA: 0x00139ABD File Offset: 0x00137CBD
	[Serialize]
	public int teleporterID { get; private set; }

	// Token: 0x060037F5 RID: 14325 RVA: 0x00139AC6 File Offset: 0x00137CC6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060037F6 RID: 14326 RVA: 0x00139ACE File Offset: 0x00137CCE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Teleporters.Add(this);
		this.SetTeleporterID(0);
		base.Subscribe<Teleporter>(-801688580, Teleporter.OnLogicValueChangedDelegate);
	}

	// Token: 0x060037F7 RID: 14327 RVA: 0x00139AFC File Offset: 0x00137CFC
	private void OnLogicValueChanged(object data)
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		List<int> list = new List<int>();
		int num = 0;
		int num2 = Mathf.Min(this.ID_LENGTH, component.inputPorts.Count);
		for (int i = 0; i < num2; i++)
		{
			int logicUICell = component.inputPorts[i].GetLogicUICell();
			LogicCircuitNetwork networkForCell = logicCircuitManager.GetNetworkForCell(logicUICell);
			int item = (networkForCell != null) ? networkForCell.OutputValue : 1;
			list.Add(item);
		}
		foreach (int num3 in list)
		{
			num = (num << 1 | num3);
		}
		this.SetTeleporterID(num);
	}

	// Token: 0x060037F8 RID: 14328 RVA: 0x00139BCC File Offset: 0x00137DCC
	protected override void OnCleanUp()
	{
		Components.Teleporters.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060037F9 RID: 14329 RVA: 0x00139BDF File Offset: 0x00137DDF
	public bool HasTeleporterTarget()
	{
		return this.FindTeleportTarget() != null;
	}

	// Token: 0x060037FA RID: 14330 RVA: 0x00139BED File Offset: 0x00137DED
	public bool IsValidTeleportTarget(Teleporter from_tele)
	{
		return from_tele.teleporterID == this.teleporterID && this.operational.IsOperational;
	}

	// Token: 0x060037FB RID: 14331 RVA: 0x00139C0C File Offset: 0x00137E0C
	public Teleporter FindTeleportTarget()
	{
		List<Teleporter> list = new List<Teleporter>();
		foreach (object obj in Components.Teleporters)
		{
			Teleporter teleporter = (Teleporter)obj;
			if (teleporter.IsValidTeleportTarget(this) && teleporter != this)
			{
				list.Add(teleporter);
			}
		}
		Teleporter result = null;
		if (list.Count > 0)
		{
			result = list.GetRandom<Teleporter>();
		}
		return result;
	}

	// Token: 0x060037FC RID: 14332 RVA: 0x00139C94 File Offset: 0x00137E94
	public void SetTeleporterID(int ID)
	{
		this.teleporterID = ID;
		foreach (object obj in Components.Teleporters)
		{
			((Teleporter)obj).Trigger(-1266722732, null);
		}
	}

	// Token: 0x060037FD RID: 14333 RVA: 0x00139CF8 File Offset: 0x00137EF8
	public void SetTeleportTarget(Teleporter target)
	{
		this.teleportTarget.Set(target);
	}

	// Token: 0x060037FE RID: 14334 RVA: 0x00139D08 File Offset: 0x00137F08
	public void TeleportObjects()
	{
		Teleporter teleporter = this.teleportTarget.Get();
		int widthInCells = base.GetComponent<Building>().Def.WidthInCells;
		int num = base.GetComponent<Building>().Def.HeightInCells - 1;
		Vector3 position = base.transform.GetPosition();
		if (teleporter != null)
		{
			ListPool<ScenePartitionerEntry, Teleporter>.PooledList pooledList = ListPool<ScenePartitionerEntry, Teleporter>.Allocate();
			GameScenePartitioner.Instance.GatherEntries((int)position.x - widthInCells / 2 + 1, (int)position.y - num / 2 + 1, widthInCells, num, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
			int cell = Grid.PosToCell(teleporter);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				GameObject gameObject = (scenePartitionerEntry.obj as Pickupable).gameObject;
				Vector3 vector = gameObject.transform.GetPosition() - position;
				MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
				if (component != null)
				{
					new EmoteChore(component.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_portal_kanim", Telepad.PortalBirthAnim, null);
				}
				else
				{
					vector += Vector3.up;
				}
				gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move) + vector);
			}
			pooledList.Recycle();
		}
		TeleportalPad.StatesInstance smi = this.teleportTarget.Get().GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.doTeleport.Trigger(smi);
		this.teleportTarget.Set(null);
	}

	// Token: 0x04002202 RID: 8706
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002204 RID: 8708
	[Serialize]
	public Ref<Teleporter> teleportTarget = new Ref<Teleporter>();

	// Token: 0x04002205 RID: 8709
	public int ID_LENGTH = 4;

	// Token: 0x04002206 RID: 8710
	private static readonly EventSystem.IntraObjectHandler<Teleporter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Teleporter>(delegate(Teleporter component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}
