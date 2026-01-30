using System;

// Token: 0x02000C1F RID: 3103
public class WaterTrapTrail : GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>
{
	// Token: 0x06005D52 RID: 23890 RVA: 0x0021C790 File Offset: 0x0021A990
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.retracted;
		base.serializable = StateMachine.SerializeType.Never;
		this.retracted.EventHandler(GameHashes.TrapArmWorkPST, delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.loose, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.ShouldBeVisible)).Enter(delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		});
		this.loose.EventHandlerTransition(GameHashes.TagsChanged, this.retracted, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.OnTagsChangedWhenOnLooseState)).EventHandler(GameHashes.TrapCaptureCompleted, delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		}).Enter(delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		});
	}

	// Token: 0x06005D53 RID: 23891 RVA: 0x0021C88C File Offset: 0x0021AA8C
	public static bool OnTagsChangedWhenOnLooseState(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance smi2 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		if (smi2 != null)
		{
			smi2.CAPTURING_SYMBOL_NAME = WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + smi.sm.depthAvailable.Get(smi).ToString();
		}
		return WaterTrapTrail.ShouldBeInvisible(smi, tagOBJ);
	}

	// Token: 0x06005D54 RID: 23892 RVA: 0x0021C8D8 File Offset: 0x0021AAD8
	public static bool ShouldBeInvisible(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		return !WaterTrapTrail.ShouldBeVisible(smi, tagOBJ);
	}

	// Token: 0x06005D55 RID: 23893 RVA: 0x0021C8E4 File Offset: 0x0021AAE4
	public static bool ShouldBeVisible(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance smi2 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		bool isOperational = smi.IsOperational;
		bool flag = smi.HasTag(GameTags.TrapArmed);
		bool flag2 = smi2 != null && smi2.IsInsideState(smi2.sm.operational.capture) && !smi2.IsInsideState(smi2.sm.operational.capture.idle) && !smi2.IsInsideState(smi2.sm.operational.capture.release);
		bool flag3 = smi2 != null && smi2.IsInsideState(smi2.sm.operational.unarmed) && smi2.GetWorkable().WorkInPstAnimation;
		return isOperational && (flag || flag2 || flag3);
	}

	// Token: 0x06005D56 RID: 23894 RVA: 0x0021C99C File Offset: 0x0021AB9C
	public static void RefreshDepthAvailable(WaterTrapTrail.Instance smi, float dt)
	{
		bool flag = WaterTrapTrail.ShouldBeVisible(smi, null);
		int num = Grid.PosToCell(smi);
		int num2 = flag ? WaterTrapGuide.GetDepthAvailable(num, smi.gameObject) : 0;
		int num3 = 4;
		if (num2 != smi.sm.depthAvailable.Get(smi))
		{
			KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
			for (int i = 1; i <= num3; i++)
			{
				component.SetSymbolVisiblity("pipe" + i.ToString(), i <= num2);
				component.SetSymbolVisiblity(WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + i.ToString(), i == num2);
			}
			int cell = Grid.OffsetCell(num, 0, -num2);
			smi.ChangeTrapCellPosition(cell);
			WaterTrapGuide.OccupyArea(smi.gameObject, num2);
			smi.sm.depthAvailable.Set(num2, smi, false);
		}
		smi.SetRangeVisualizerOffset(new Vector2I(0, -num2));
		smi.SetRangeVisualizerVisibility(flag);
	}

	// Token: 0x04003E32 RID: 15922
	private static string CAPTURING_SYMBOL_OVERRIDE_NAME = "creatureSymbol";

	// Token: 0x04003E33 RID: 15923
	public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State retracted;

	// Token: 0x04003E34 RID: 15924
	public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State loose;

	// Token: 0x04003E35 RID: 15925
	private StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter depthAvailable = new StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter(-1);

	// Token: 0x02001DB6 RID: 7606
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001DB7 RID: 7607
	public new class Instance : GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.GameInstance
	{
		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x0600B1F0 RID: 45552 RVA: 0x003DE989 File Offset: 0x003DCB89
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x0600B1F1 RID: 45553 RVA: 0x003DE996 File Offset: 0x003DCB96
		public Lure.Instance lureSMI
		{
			get
			{
				if (this._lureSMI == null)
				{
					this._lureSMI = base.gameObject.GetSMI<Lure.Instance>();
				}
				return this._lureSMI;
			}
		}

		// Token: 0x0600B1F2 RID: 45554 RVA: 0x003DE9B7 File Offset: 0x003DCBB7
		public Instance(IStateMachineTarget master, WaterTrapTrail.Def def) : base(master, def)
		{
		}

		// Token: 0x0600B1F3 RID: 45555 RVA: 0x003DE9C1 File Offset: 0x003DCBC1
		public override void StartSM()
		{
			base.StartSM();
			this.RegisterListenersToCellChanges();
		}

		// Token: 0x0600B1F4 RID: 45556 RVA: 0x003DE9D0 File Offset: 0x003DCBD0
		private void RegisterListenersToCellChanges()
		{
			int widthInCells = base.GetComponent<BuildingComplete>().Def.WidthInCells;
			CellOffset[] array = new CellOffset[widthInCells * 4];
			for (int i = 0; i < 4; i++)
			{
				int y = -(i + 1);
				for (int j = 0; j < widthInCells; j++)
				{
					array[i * widthInCells + j] = new CellOffset(j, y);
				}
			}
			Extents extents = new Extents(Grid.PosToCell(base.transform.GetPosition()), array);
			this.partitionerEntry_solids = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnLowerCellChanged));
			this.partitionerEntry_buildings = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnLowerCellChanged));
		}

		// Token: 0x0600B1F5 RID: 45557 RVA: 0x003DEAAC File Offset: 0x003DCCAC
		private void UnregisterListenersToCellChanges()
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
		}

		// Token: 0x0600B1F6 RID: 45558 RVA: 0x003DEACE File Offset: 0x003DCCCE
		private void OnLowerCellChanged(object o)
		{
			WaterTrapTrail.RefreshDepthAvailable(base.smi, 0f);
		}

		// Token: 0x0600B1F7 RID: 45559 RVA: 0x003DEAE0 File Offset: 0x003DCCE0
		protected override void OnCleanUp()
		{
			this.UnregisterListenersToCellChanges();
			base.OnCleanUp();
		}

		// Token: 0x0600B1F8 RID: 45560 RVA: 0x003DEAEE File Offset: 0x003DCCEE
		public void SetRangeVisualizerVisibility(bool visible)
		{
			this.rangeVisualizer.RangeMax.x = (visible ? 0 : -1);
		}

		// Token: 0x0600B1F9 RID: 45561 RVA: 0x003DEB07 File Offset: 0x003DCD07
		public void SetRangeVisualizerOffset(Vector2I offset)
		{
			this.rangeVisualizer.OriginOffset = offset;
		}

		// Token: 0x0600B1FA RID: 45562 RVA: 0x003DEB15 File Offset: 0x003DCD15
		public void ChangeTrapCellPosition(int cell)
		{
			if (this.lureSMI != null)
			{
				this.lureSMI.ChangeLureCellPosition(cell);
			}
			base.gameObject.GetComponent<TrapTrigger>().SetTriggerCell(cell);
		}

		// Token: 0x04008C09 RID: 35849
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04008C0A RID: 35850
		[MyCmpGet]
		private RangeVisualizer rangeVisualizer;

		// Token: 0x04008C0B RID: 35851
		private HandleVector<int>.Handle partitionerEntry_buildings;

		// Token: 0x04008C0C RID: 35852
		private HandleVector<int>.Handle partitionerEntry_solids;

		// Token: 0x04008C0D RID: 35853
		private Lure.Instance _lureSMI;
	}
}
