using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000817 RID: 2071
public class TemporalTearOpener : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>
{
	// Token: 0x06003834 RID: 14388 RVA: 0x0013A804 File Offset: 0x00138A04
	private static StatusItem CreateColoniesStatusItem()
	{
		StatusItem statusItem = new StatusItem("Temporal_Tear_Opener_Insufficient_Colonies", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
		statusItem.resolveStringCallback = delegate(string str, object data)
		{
			TemporalTearOpener.Instance instance = (TemporalTearOpener.Instance)data;
			str = str.Replace("{progress}", string.Format("({0}/{1})", instance.CountColonies(), EstablishColonies.BASE_COUNT));
			return str;
		};
		return statusItem;
	}

	// Token: 0x06003835 RID: 14389 RVA: 0x0013A85C File Offset: 0x00138A5C
	private static StatusItem CreateProgressStatusItem()
	{
		StatusItem statusItem = new StatusItem("Temporal_Tear_Opener_Progress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
		statusItem.resolveStringCallback = delegate(string str, object data)
		{
			TemporalTearOpener.Instance instance = (TemporalTearOpener.Instance)data;
			str = str.Replace("{progress}", GameUtil.GetFormattedPercent(instance.GetPercentComplete(), GameUtil.TimeSlice.None));
			return str;
		};
		return statusItem;
	}

	// Token: 0x06003836 RID: 14390 RVA: 0x0013A8B4 File Offset: 0x00138AB4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.UpdateMeter();
			if (ClusterManager.Instance.GetClusterPOIManager().IsTemporalTearOpen())
			{
				smi.GoTo(this.opening_tear_finish);
				return;
			}
			smi.GoTo(this.check_requirements);
		}).PlayAnim("off");
		this.check_requirements.DefaultState(this.check_requirements.has_target).Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.GetComponent<HighEnergyParticleStorage>().receiverOpen = false;
			smi.GetComponent<KBatchedAnimController>().Play("port_close", KAnim.PlayMode.Once, 1f, 0f);
			smi.GetComponent<KBatchedAnimController>().Queue("off", KAnim.PlayMode.Loop, 1f, 0f);
		});
		this.check_requirements.has_target.ToggleStatusItem(TemporalTearOpener.s_noTargetStatus, null).UpdateTransition(this.check_requirements.has_los, (TemporalTearOpener.Instance smi, float dt) => ClusterManager.Instance.GetClusterPOIManager().IsTemporalTearRevealed(), UpdateRate.SIM_200ms, false);
		this.check_requirements.has_los.ToggleStatusItem(TemporalTearOpener.s_noLosStatus, null).UpdateTransition(this.check_requirements.enough_colonies, (TemporalTearOpener.Instance smi, float dt) => smi.HasLineOfSight(), UpdateRate.SIM_200ms, false);
		this.check_requirements.enough_colonies.ToggleStatusItem(TemporalTearOpener.s_insufficient_colonies, null).UpdateTransition(this.charging, (TemporalTearOpener.Instance smi, float dt) => smi.HasSufficientColonies(), UpdateRate.SIM_200ms, false);
		this.charging.DefaultState(this.charging.idle).ToggleStatusItem(TemporalTearOpener.s_progressStatus, (TemporalTearOpener.Instance smi) => smi).UpdateTransition(this.check_requirements.has_los, (TemporalTearOpener.Instance smi, float dt) => !smi.HasLineOfSight(), UpdateRate.SIM_200ms, false).UpdateTransition(this.check_requirements.enough_colonies, (TemporalTearOpener.Instance smi, float dt) => !smi.HasSufficientColonies(), UpdateRate.SIM_200ms, false).Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.GetComponent<HighEnergyParticleStorage>().receiverOpen = true;
			smi.GetComponent<KBatchedAnimController>().Play("port_open", KAnim.PlayMode.Once, 1f, 0f);
			smi.GetComponent<KBatchedAnimController>().Queue("inert", KAnim.PlayMode.Loop, 1f, 0f);
		});
		this.charging.idle.EventTransition(GameHashes.OnParticleStorageChanged, this.charging.consuming, (TemporalTearOpener.Instance smi) => !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
		this.charging.consuming.EventTransition(GameHashes.OnParticleStorageChanged, this.charging.idle, (TemporalTearOpener.Instance smi) => smi.GetComponent<HighEnergyParticleStorage>().IsEmpty()).UpdateTransition(this.ready, (TemporalTearOpener.Instance smi, float dt) => smi.ConsumeParticlesAndCheckComplete(dt), UpdateRate.SIM_200ms, false);
		this.ready.ToggleNotification((TemporalTearOpener.Instance smi) => new Notification(BUILDING.STATUSITEMS.TEMPORAL_TEAR_OPENER_READY.NOTIFICATION, NotificationType.Good, (List<Notification> a, object b) => BUILDING.STATUSITEMS.TEMPORAL_TEAR_OPENER_READY.NOTIFICATION_TOOLTIP, null, false, 0f, null, null, null, true, false, false));
		this.opening_tear_beam_pre.PlayAnim("working_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.opening_tear_beam);
		this.opening_tear_beam.Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.CreateBeamFX();
		}).PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(5f, this.opening_tear_finish);
		this.opening_tear_finish.PlayAnim("working_pst").Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.OpenTemporalTear();
		});
	}

	// Token: 0x04002217 RID: 8727
	private const float MIN_SUNLIGHT_EXPOSURE = 15f;

	// Token: 0x04002218 RID: 8728
	private static StatusItem s_noLosStatus = new StatusItem("Temporal_Tear_Opener_No_Los", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);

	// Token: 0x04002219 RID: 8729
	private static StatusItem s_insufficient_colonies = TemporalTearOpener.CreateColoniesStatusItem();

	// Token: 0x0400221A RID: 8730
	private static StatusItem s_noTargetStatus = new StatusItem("Temporal_Tear_Opener_No_Target", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);

	// Token: 0x0400221B RID: 8731
	private static StatusItem s_progressStatus = TemporalTearOpener.CreateProgressStatusItem();

	// Token: 0x0400221C RID: 8732
	private TemporalTearOpener.CheckRequirementsState check_requirements;

	// Token: 0x0400221D RID: 8733
	private TemporalTearOpener.ChargingState charging;

	// Token: 0x0400221E RID: 8734
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_beam_pre;

	// Token: 0x0400221F RID: 8735
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_beam;

	// Token: 0x04002220 RID: 8736
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_finish;

	// Token: 0x04002221 RID: 8737
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State ready;

	// Token: 0x020017AC RID: 6060
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400785B RID: 30811
		public float consumeRate;

		// Token: 0x0400785C RID: 30812
		public float numParticlesToOpen;
	}

	// Token: 0x020017AD RID: 6061
	private class ChargingState : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State
	{
		// Token: 0x0400785D RID: 30813
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State idle;

		// Token: 0x0400785E RID: 30814
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State consuming;
	}

	// Token: 0x020017AE RID: 6062
	private class CheckRequirementsState : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State
	{
		// Token: 0x0400785F RID: 30815
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State has_target;

		// Token: 0x04007860 RID: 30816
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State has_los;

		// Token: 0x04007861 RID: 30817
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State enough_colonies;
	}

	// Token: 0x020017AF RID: 6063
	public new class Instance : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x06009BFC RID: 39932 RVA: 0x00397E6B File Offset: 0x0039606B
		public Instance(IStateMachineTarget master, TemporalTearOpener.Def def) : base(master, def)
		{
			this.m_meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			EnterTemporalTearSequence.tearOpenerGameObject = base.gameObject;
		}

		// Token: 0x06009BFD RID: 39933 RVA: 0x00397EA8 File Offset: 0x003960A8
		protected override void OnCleanUp()
		{
			if (EnterTemporalTearSequence.tearOpenerGameObject == base.gameObject)
			{
				EnterTemporalTearSequence.tearOpenerGameObject = null;
			}
			base.OnCleanUp();
		}

		// Token: 0x06009BFE RID: 39934 RVA: 0x00397EC8 File Offset: 0x003960C8
		public bool HasLineOfSight()
		{
			Extents extents = base.GetComponent<Building>().GetExtents();
			int x = extents.x;
			int num = extents.x + extents.width - 1;
			for (int i = x; i <= num; i++)
			{
				int i2 = Grid.XYToCell(i, extents.y);
				if ((float)Grid.ExposedToSunlight[i2] < 15f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009BFF RID: 39935 RVA: 0x00397F25 File Offset: 0x00396125
		public bool HasSufficientColonies()
		{
			return this.CountColonies() >= EstablishColonies.BASE_COUNT;
		}

		// Token: 0x06009C00 RID: 39936 RVA: 0x00397F38 File Offset: 0x00396138
		public int CountColonies()
		{
			int num = 0;
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				Activatable component = Components.Telepads[i].GetComponent<Activatable>();
				if (component == null || component.IsActivated)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06009C01 RID: 39937 RVA: 0x00397F84 File Offset: 0x00396184
		public bool ConsumeParticlesAndCheckComplete(float dt)
		{
			float amount = Mathf.Min(dt * base.def.consumeRate, base.def.numParticlesToOpen - this.m_particlesConsumed);
			float num = base.GetComponent<HighEnergyParticleStorage>().ConsumeAndGet(amount);
			this.m_particlesConsumed += num;
			this.UpdateMeter();
			return this.m_particlesConsumed >= base.def.numParticlesToOpen;
		}

		// Token: 0x06009C02 RID: 39938 RVA: 0x00397FED File Offset: 0x003961ED
		public void UpdateMeter()
		{
			this.m_meter.SetPositionPercent(this.GetAmountComplete());
		}

		// Token: 0x06009C03 RID: 39939 RVA: 0x00398000 File Offset: 0x00396200
		private float GetAmountComplete()
		{
			return Mathf.Min(this.m_particlesConsumed / base.def.numParticlesToOpen, 1f);
		}

		// Token: 0x06009C04 RID: 39940 RVA: 0x0039801E File Offset: 0x0039621E
		public float GetPercentComplete()
		{
			return this.GetAmountComplete() * 100f;
		}

		// Token: 0x06009C05 RID: 39941 RVA: 0x0039802C File Offset: 0x0039622C
		public void CreateBeamFX()
		{
			Vector3 position = base.gameObject.transform.position;
			position.y += 3.25f;
			Quaternion rotation = Quaternion.Euler(-90f, 90f, 0f);
			Util.KInstantiate(EffectPrefabs.Instance.OpenTemporalTearBeam, position, rotation, base.gameObject, null, true, 0);
		}

		// Token: 0x06009C06 RID: 39942 RVA: 0x0039808A File Offset: 0x0039628A
		public void OpenTemporalTear()
		{
			ClusterManager.Instance.GetClusterPOIManager().RevealTemporalTear();
			ClusterManager.Instance.GetClusterPOIManager().OpenTemporalTear(this.GetMyWorldId());
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06009C07 RID: 39943 RVA: 0x003980B0 File Offset: 0x003962B0
		public string SidescreenButtonText
		{
			get
			{
				return BUILDINGS.PREFABS.TEMPORALTEAROPENER.SIDESCREEN.TEXT;
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06009C08 RID: 39944 RVA: 0x003980BC File Offset: 0x003962BC
		public string SidescreenButtonTooltip
		{
			get
			{
				return BUILDINGS.PREFABS.TEMPORALTEAROPENER.SIDESCREEN.TOOLTIP;
			}
		}

		// Token: 0x06009C09 RID: 39945 RVA: 0x003980C8 File Offset: 0x003962C8
		public bool SidescreenEnabled()
		{
			return this.GetCurrentState() == base.sm.ready || DebugHandler.InstantBuildMode;
		}

		// Token: 0x06009C0A RID: 39946 RVA: 0x003980E4 File Offset: 0x003962E4
		public bool SidescreenButtonInteractable()
		{
			return this.GetCurrentState() == base.sm.ready || DebugHandler.InstantBuildMode;
		}

		// Token: 0x06009C0B RID: 39947 RVA: 0x00398100 File Offset: 0x00396300
		public void OnSidescreenButtonPressed()
		{
			ConfirmDialogScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<ConfirmDialogScreen>();
			string text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_MESSAGE;
			System.Action on_confirm = delegate()
			{
				this.FireTemporalTearOpener(base.smi);
			};
			System.Action on_cancel = delegate()
			{
			};
			string configurable_text = null;
			System.Action on_configurable_clicked = null;
			string confirm_text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_CONFIRM;
			string cancel_text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_CANCEL;
			component.PopupConfirmDialog(text, on_confirm, on_cancel, configurable_text, on_configurable_clicked, UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_TITLE, confirm_text, cancel_text, null);
		}

		// Token: 0x06009C0C RID: 39948 RVA: 0x0039818C File Offset: 0x0039638C
		private void FireTemporalTearOpener(TemporalTearOpener.Instance smi)
		{
			smi.GoTo(base.sm.opening_tear_beam_pre);
		}

		// Token: 0x06009C0D RID: 39949 RVA: 0x0039819F File Offset: 0x0039639F
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06009C0E RID: 39950 RVA: 0x003981A3 File Offset: 0x003963A3
		public void SetButtonTextOverride(ButtonMenuTextOverride text)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009C0F RID: 39951 RVA: 0x003981AA File Offset: 0x003963AA
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x04007862 RID: 30818
		[Serialize]
		private float m_particlesConsumed;

		// Token: 0x04007863 RID: 30819
		private MeterController m_meter;
	}
}
