using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000805 RID: 2053
public class StickerBomb : StateMachineComponent<StickerBomb.StatesInstance>
{
	// Token: 0x06003743 RID: 14147 RVA: 0x001370D0 File Offset: 0x001352D0
	protected override void OnSpawn()
	{
		if (this.stickerName.IsNullOrWhiteSpace())
		{
			global::Debug.LogError("Missing sticker db entry for " + this.stickerType);
		}
		else
		{
			DbStickerBomb dbStickerBomb = Db.GetStickerBombs().Get(this.stickerName);
			base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
			{
				dbStickerBomb.animFile
			});
		}
		this.cellOffsets = StickerBomb.BuildCellOffsets(base.transform.GetPosition());
		base.smi.destroyTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.STICKER_DURATION;
		base.smi.StartSM();
		Extents extents = base.GetComponent<OccupyArea>().GetExtents();
		Extents extents2 = new Extents(extents.x - 1, extents.y - 1, extents.width + 2, extents.height + 2);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("StickerBomb.OnSpawn", base.gameObject, extents2, GameScenePartitioner.Instance.objectLayers[2], new Action<object>(this.OnFoundationCellChanged));
		base.OnSpawn();
	}

	// Token: 0x06003744 RID: 14148 RVA: 0x001371D8 File Offset: 0x001353D8
	[OnDeserialized]
	public void OnDeserialized()
	{
		if (this.stickerName.IsNullOrWhiteSpace() && !this.stickerType.IsNullOrWhiteSpace())
		{
			string[] array = this.stickerType.Split('_', StringSplitOptions.None);
			if (array.Length == 2)
			{
				this.stickerName = array[1];
			}
		}
	}

	// Token: 0x06003745 RID: 14149 RVA: 0x0013721D File Offset: 0x0013541D
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x00137235 File Offset: 0x00135435
	private void OnFoundationCellChanged(object data)
	{
		if (!StickerBomb.CanPlaceSticker(this.cellOffsets))
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06003747 RID: 14151 RVA: 0x00137250 File Offset: 0x00135450
	public static List<int> BuildCellOffsets(Vector3 position)
	{
		List<int> list = new List<int>();
		bool flag = position.x % 1f < 0.5f;
		bool flag2 = position.y % 1f > 0.5f;
		int num = Grid.PosToCell(position);
		list.Add(num);
		if (flag)
		{
			list.Add(Grid.CellLeft(num));
			if (flag2)
			{
				list.Add(Grid.CellAbove(num));
				list.Add(Grid.CellUpLeft(num));
			}
			else
			{
				list.Add(Grid.CellBelow(num));
				list.Add(Grid.CellDownLeft(num));
			}
		}
		else
		{
			list.Add(Grid.CellRight(num));
			if (flag2)
			{
				list.Add(Grid.CellAbove(num));
				list.Add(Grid.CellUpRight(num));
			}
			else
			{
				list.Add(Grid.CellBelow(num));
				list.Add(Grid.CellDownRight(num));
			}
		}
		return list;
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x00137320 File Offset: 0x00135520
	public static bool CanPlaceSticker(List<int> offsets)
	{
		using (List<int>.Enumerator enumerator = offsets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (Grid.IsCellOpenToSpace(enumerator.Current))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06003749 RID: 14153 RVA: 0x00137374 File Offset: 0x00135574
	public void SetStickerType(string newStickerType)
	{
		if (newStickerType == null)
		{
			newStickerType = "sticker";
		}
		DbStickerBomb randomSticker = Db.GetStickerBombs().GetRandomSticker();
		this.stickerName = randomSticker.Id;
		this.stickerType = string.Format("{0}_{1}", newStickerType, randomSticker.Id);
		base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
		{
			randomSticker.animFile
		});
	}

	// Token: 0x040021AB RID: 8619
	[Serialize]
	public string stickerType;

	// Token: 0x040021AC RID: 8620
	[Serialize]
	public string stickerName;

	// Token: 0x040021AD RID: 8621
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040021AE RID: 8622
	private List<int> cellOffsets;

	// Token: 0x0200178E RID: 6030
	public class StatesInstance : GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.GameInstance
	{
		// Token: 0x06009B81 RID: 39809 RVA: 0x003957C9 File Offset: 0x003939C9
		public StatesInstance(StickerBomb master) : base(master)
		{
		}

		// Token: 0x06009B82 RID: 39810 RVA: 0x003957D2 File Offset: 0x003939D2
		public string GetStickerAnim(string type)
		{
			return string.Format("{0}_{1}", type, base.master.stickerType);
		}

		// Token: 0x04007804 RID: 30724
		[Serialize]
		public float destroyTime;
	}

	// Token: 0x0200178F RID: 6031
	public class States : GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb>
	{
		// Token: 0x06009B83 RID: 39811 RVA: 0x003957EC File Offset: 0x003939EC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Transition(this.destroy, (StickerBomb.StatesInstance smi) => GameClock.Instance.GetTime() >= smi.destroyTime, UpdateRate.SIM_200ms).DefaultState(this.idle);
			this.idle.PlayAnim((StickerBomb.StatesInstance smi) => smi.GetStickerAnim("idle"), KAnim.PlayMode.Once).ScheduleGoTo((StickerBomb.StatesInstance smi) => (float)UnityEngine.Random.Range(20, 30), this.sparkle);
			this.sparkle.PlayAnim((StickerBomb.StatesInstance smi) => smi.GetStickerAnim("sparkle"), KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
			this.destroy.Enter(delegate(StickerBomb.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master);
			});
		}

		// Token: 0x04007805 RID: 30725
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State destroy;

		// Token: 0x04007806 RID: 30726
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State sparkle;

		// Token: 0x04007807 RID: 30727
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State idle;
	}
}
