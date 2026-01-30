using System;
using FMOD.Studio;
using KSerialization;
using UnityEngine;

// Token: 0x0200079D RID: 1949
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicHammer : Switch
{
	// Token: 0x06003298 RID: 12952 RVA: 0x00122F84 File Offset: 0x00121184
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.switchedOn = false;
		this.UpdateVisualState(false, false);
		this.rotatable = base.GetComponent<Rotatable>();
		CellOffset rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.target_offset);
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.resonator_cell = Grid.OffsetCell(cell, rotatedCellOffset);
		base.Subscribe<LogicHammer>(-801688580, LogicHammer.OnLogicValueChangedDelegate);
		base.Subscribe<LogicHammer>(-592767678, LogicHammer.OnOperationalChangedDelegate);
		base.OnToggle += this.OnSwitchToggled;
	}

	// Token: 0x06003299 RID: 12953 RVA: 0x00123024 File Offset: 0x00121224
	private void OnSwitchToggled(bool toggled_on)
	{
		bool connected = false;
		if (this.operational.IsOperational && toggled_on)
		{
			connected = this.TriggerAudio();
			this.operational.SetActive(true, false);
		}
		else
		{
			this.operational.SetActive(false, false);
		}
		this.UpdateVisualState(connected, false);
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x0012306D File Offset: 0x0012126D
	private void OnOperationalChanged(object _)
	{
		if (this.operational.IsOperational)
		{
			this.SetState(LogicCircuitNetwork.IsBitActive(0, this.logic_value));
			return;
		}
		this.UpdateVisualState(false, false);
	}

	// Token: 0x0600329B RID: 12955 RVA: 0x00123098 File Offset: 0x00121298
	private bool TriggerAudio()
	{
		if (this.wasOn || !this.switchedOn)
		{
			return false;
		}
		string text = null;
		if (!Grid.IsValidCell(this.resonator_cell))
		{
			return false;
		}
		float num = float.NaN;
		GameObject gameObject = Grid.Objects[this.resonator_cell, 1];
		if (gameObject == null)
		{
			gameObject = Grid.Objects[this.resonator_cell, 30];
			if (gameObject == null)
			{
				gameObject = Grid.Objects[this.resonator_cell, 26];
				if (gameObject != null)
				{
					Wire component = gameObject.GetComponent<Wire>();
					if (component != null)
					{
						ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)Game.Instance.electricalConduitSystem.GetNetworkForCell(component.GetNetworkCell());
						if (electricalUtilityNetwork != null)
						{
							num = (float)electricalUtilityNetwork.allWires.Count;
						}
					}
				}
				else
				{
					gameObject = Grid.Objects[this.resonator_cell, 31];
					if (gameObject != null)
					{
						if (gameObject.GetComponent<LogicWire>() != null)
						{
							LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(this.resonator_cell);
							if (networkForCell != null)
							{
								num = (float)networkForCell.WireCount;
							}
						}
					}
					else
					{
						gameObject = Grid.Objects[this.resonator_cell, 12];
						if (gameObject != null)
						{
							Conduit component2 = gameObject.GetComponent<Conduit>();
							FlowUtilityNetwork flowUtilityNetwork = (FlowUtilityNetwork)Conduit.GetNetworkManager(ConduitType.Gas).GetNetworkForCell(component2.GetNetworkCell());
							if (flowUtilityNetwork != null)
							{
								num = (float)flowUtilityNetwork.conduitCount;
							}
						}
						else
						{
							gameObject = Grid.Objects[this.resonator_cell, 16];
							if (gameObject != null)
							{
								Conduit component3 = gameObject.GetComponent<Conduit>();
								FlowUtilityNetwork flowUtilityNetwork2 = (FlowUtilityNetwork)Conduit.GetNetworkManager(ConduitType.Liquid).GetNetworkForCell(component3.GetNetworkCell());
								if (flowUtilityNetwork2 != null)
								{
									num = (float)flowUtilityNetwork2.conduitCount;
								}
							}
							else
							{
								gameObject = Grid.Objects[this.resonator_cell, 20];
								gameObject != null;
							}
						}
					}
				}
			}
		}
		if (gameObject != null)
		{
			Building component4 = gameObject.GetComponent<BuildingComplete>();
			if (component4 != null)
			{
				text = component4.Def.PrefabID;
			}
		}
		if (text != null)
		{
			string text2 = StringFormatter.Combine(LogicHammer.SOUND_EVENT_PREFIX, text);
			text2 = GlobalAssets.GetSound(text2, true);
			if (text2 == null)
			{
				text2 = GlobalAssets.GetSound(LogicHammer.DEFAULT_NO_SOUND_EVENT, false);
			}
			Vector3 position = base.transform.position;
			position.z = 0f;
			EventInstance instance = KFMOD.BeginOneShot(text2, position, 1f);
			if (!float.IsNaN(num))
			{
				instance.setParameterByName(LogicHammer.PARAMETER_NAME, num, false);
			}
			KFMOD.EndOneShot(instance);
			return true;
		}
		return false;
	}

	// Token: 0x0600329C RID: 12956 RVA: 0x00123320 File Offset: 0x00121520
	private void UpdateVisualState(bool connected, bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				if (connected)
				{
					this.animController.Play(LogicHammer.ON_HIT_ANIMS, KAnim.PlayMode.Once);
					return;
				}
				this.animController.Play(LogicHammer.ON_MISS_ANIMS, KAnim.PlayMode.Once);
				return;
			}
			else
			{
				this.animController.Play(LogicHammer.OFF_ANIMS, KAnim.PlayMode.Once);
			}
		}
	}

	// Token: 0x0600329D RID: 12957 RVA: 0x00123390 File Offset: 0x00121590
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == LogicHammer.PORT_ID)
		{
			this.SetState(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
			this.logic_value = logicValueChanged.newValue;
		}
	}

	// Token: 0x04001EA6 RID: 7846
	protected KBatchedAnimController animController;

	// Token: 0x04001EA7 RID: 7847
	private static readonly EventSystem.IntraObjectHandler<LogicHammer> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicHammer>(delegate(LogicHammer component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001EA8 RID: 7848
	private static readonly EventSystem.IntraObjectHandler<LogicHammer> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<LogicHammer>(delegate(LogicHammer component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001EA9 RID: 7849
	public static readonly HashedString PORT_ID = new HashedString("LogicHammerInput");

	// Token: 0x04001EAA RID: 7850
	private static string PARAMETER_NAME = "hammerObjectCount";

	// Token: 0x04001EAB RID: 7851
	private static string SOUND_EVENT_PREFIX = "Hammer_strike_";

	// Token: 0x04001EAC RID: 7852
	private static string DEFAULT_NO_SOUND_EVENT = "Hammer_strike_default";

	// Token: 0x04001EAD RID: 7853
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001EAE RID: 7854
	private int resonator_cell;

	// Token: 0x04001EAF RID: 7855
	private CellOffset target_offset = new CellOffset(-1, 0);

	// Token: 0x04001EB0 RID: 7856
	private Rotatable rotatable;

	// Token: 0x04001EB1 RID: 7857
	private int logic_value;

	// Token: 0x04001EB2 RID: 7858
	private bool wasOn;

	// Token: 0x04001EB3 RID: 7859
	protected static readonly HashedString[] ON_HIT_ANIMS = new HashedString[]
	{
		"on_hit"
	};

	// Token: 0x04001EB4 RID: 7860
	protected static readonly HashedString[] ON_MISS_ANIMS = new HashedString[]
	{
		"on_miss"
	};

	// Token: 0x04001EB5 RID: 7861
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"off_pre",
		"off"
	};
}
