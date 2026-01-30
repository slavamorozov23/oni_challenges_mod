using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000785 RID: 1925
public class LadderBed : GameStateMachine<LadderBed, LadderBed.Instance, IStateMachineTarget, LadderBed.Def>
{
	// Token: 0x0600311E RID: 12574 RVA: 0x0011B7BA File Offset: 0x001199BA
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
	}

	// Token: 0x04001D70 RID: 7536
	public static string lightBedShakeSoundPath = GlobalAssets.GetSound("LadderBed_LightShake", false);

	// Token: 0x04001D71 RID: 7537
	public static string noDupeBedShakeSoundPath = GlobalAssets.GetSound("LadderBed_Shake", false);

	// Token: 0x04001D72 RID: 7538
	public static string LADDER_BED_COUNT_BELOW_PARAMETER = "bed_count";

	// Token: 0x020016A2 RID: 5794
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007578 RID: 30072
		public CellOffset[] offsets;
	}

	// Token: 0x020016A3 RID: 5795
	public new class Instance : GameStateMachine<LadderBed, LadderBed.Instance, IStateMachineTarget, LadderBed.Def>.GameInstance
	{
		// Token: 0x06009806 RID: 38918 RVA: 0x00386F58 File Offset: 0x00385158
		public Instance(IStateMachineTarget master, LadderBed.Def def) : base(master, def)
		{
			ScenePartitionerLayer scenePartitionerLayer = GameScenePartitioner.Instance.objectLayers[40];
			this.m_cell = Grid.PosToCell(master.gameObject);
			foreach (CellOffset offset in def.offsets)
			{
				int cell = Grid.OffsetCell(this.m_cell, offset);
				if (Grid.IsValidCell(this.m_cell) && Grid.IsValidCell(cell))
				{
					this.m_partitionEntires.Add(GameScenePartitioner.Instance.Add("LadderBed.Constructor", base.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnMoverChanged)));
					this.OnMoverChanged(null);
				}
			}
			AttachableBuilding attachable = this.m_attachable;
			attachable.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(attachable.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentChanged));
			this.OnAttachmentChanged(null);
			base.Subscribe(-717201811, new Action<object>(this.OnSleepDisturbedByMovement));
			master.GetComponent<KAnimControllerBase>().GetLayering().GetLink().syncTint = false;
		}

		// Token: 0x06009807 RID: 38919 RVA: 0x00387070 File Offset: 0x00385270
		private void OnSleepDisturbedByMovement(object obj)
		{
			base.GetComponent<KAnimControllerBase>().Play("interrupt_light", KAnim.PlayMode.Once, 1f, 0f);
			EventInstance instance = SoundEvent.BeginOneShot(LadderBed.lightBedShakeSoundPath, base.smi.transform.GetPosition(), 1f, false);
			instance.setParameterByName(LadderBed.LADDER_BED_COUNT_BELOW_PARAMETER, (float)this.numBelow, false);
			SoundEvent.EndOneShot(instance);
		}

		// Token: 0x06009808 RID: 38920 RVA: 0x003870DA File Offset: 0x003852DA
		private void OnAttachmentChanged(object data)
		{
			this.numBelow = AttachableBuilding.CountAttachedBelow(this.m_attachable);
		}

		// Token: 0x06009809 RID: 38921 RVA: 0x003870F0 File Offset: 0x003852F0
		private void OnMoverChanged(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable != null && pickupable.gameObject != null && pickupable.KPrefabID.HasTag(GameTags.BaseMinion) && pickupable.GetComponent<Navigator>().CurrentNavType == NavType.Ladder)
			{
				if (this.m_sleepable.worker == null)
				{
					base.GetComponent<KAnimControllerBase>().Play("interrupt_light_nodupe", KAnim.PlayMode.Once, 1f, 0f);
					EventInstance instance = SoundEvent.BeginOneShot(LadderBed.noDupeBedShakeSoundPath, base.smi.transform.GetPosition(), 1f, false);
					instance.setParameterByName(LadderBed.LADDER_BED_COUNT_BELOW_PARAMETER, (float)this.numBelow, false);
					SoundEvent.EndOneShot(instance);
					return;
				}
				if (pickupable.gameObject != this.m_sleepable.worker.gameObject)
				{
					this.m_sleepable.worker.Trigger(-717201811, null);
				}
			}
		}

		// Token: 0x0600980A RID: 38922 RVA: 0x003871EC File Offset: 0x003853EC
		protected override void OnCleanUp()
		{
			foreach (HandleVector<int>.Handle handle in this.m_partitionEntires)
			{
				GameScenePartitioner.Instance.Free(ref handle);
			}
			AttachableBuilding attachable = this.m_attachable;
			attachable.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachable.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentChanged));
			base.OnCleanUp();
		}

		// Token: 0x04007579 RID: 30073
		private List<HandleVector<int>.Handle> m_partitionEntires = new List<HandleVector<int>.Handle>();

		// Token: 0x0400757A RID: 30074
		private int m_cell;

		// Token: 0x0400757B RID: 30075
		[MyCmpGet]
		private Ownable m_ownable;

		// Token: 0x0400757C RID: 30076
		[MyCmpGet]
		private Sleepable m_sleepable;

		// Token: 0x0400757D RID: 30077
		[MyCmpGet]
		private AttachableBuilding m_attachable;

		// Token: 0x0400757E RID: 30078
		private int numBelow;
	}
}
