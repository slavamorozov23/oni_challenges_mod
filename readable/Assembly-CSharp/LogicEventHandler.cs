using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020009EE RID: 2542
internal class LogicEventHandler : ILogicEventReceiver, ILogicNetworkConnection, ILogicUIElement, IUniformGridObject
{
	// Token: 0x06004A16 RID: 18966 RVA: 0x001AD5AA File Offset: 0x001AB7AA
	public LogicEventHandler(int cell, Action<int, int> on_value_changed, Action<int, bool> on_connection_changed, LogicPortSpriteType sprite_type)
	{
		this.cell = cell;
		this.onValueChanged = on_value_changed;
		this.onConnectionChanged = on_connection_changed;
		this.spriteType = sprite_type;
	}

	// Token: 0x06004A17 RID: 18967 RVA: 0x001AD5D0 File Offset: 0x001AB7D0
	public void ReceiveLogicEvent(int value)
	{
		this.TriggerAudio(value);
		int arg = this.value;
		this.value = value;
		this.onValueChanged(value, arg);
	}

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x06004A18 RID: 18968 RVA: 0x001AD5FF File Offset: 0x001AB7FF
	public int Value
	{
		get
		{
			return this.value;
		}
	}

	// Token: 0x06004A19 RID: 18969 RVA: 0x001AD607 File Offset: 0x001AB807
	public int GetLogicUICell()
	{
		return this.cell;
	}

	// Token: 0x06004A1A RID: 18970 RVA: 0x001AD60F File Offset: 0x001AB80F
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x06004A1B RID: 18971 RVA: 0x001AD617 File Offset: 0x001AB817
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06004A1C RID: 18972 RVA: 0x001AD629 File Offset: 0x001AB829
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06004A1D RID: 18973 RVA: 0x001AD63B File Offset: 0x001AB83B
	public int GetLogicCell()
	{
		return this.cell;
	}

	// Token: 0x06004A1E RID: 18974 RVA: 0x001AD644 File Offset: 0x001AB844
	private void TriggerAudio(int new_value)
	{
		LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(this.cell);
		SpeedControlScreen instance = SpeedControlScreen.Instance;
		if (networkForCell != null && new_value != this.value && instance != null && !instance.IsPaused)
		{
			if (KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation) && KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) != 1 && OverlayScreen.Instance.GetMode() != OverlayModes.Logic.ID)
			{
				return;
			}
			string name = "Logic_Building_Toggle";
			if (!CameraController.Instance.IsAudibleSound(Grid.CellToPosCCC(this.cell, Grid.SceneLayer.BuildingFront)))
			{
				return;
			}
			LogicCircuitNetwork.LogicSoundPair logicSoundPair = new LogicCircuitNetwork.LogicSoundPair();
			Dictionary<int, LogicCircuitNetwork.LogicSoundPair> logicSoundRegister = LogicCircuitNetwork.logicSoundRegister;
			int id = networkForCell.id;
			if (!logicSoundRegister.ContainsKey(id))
			{
				logicSoundRegister.Add(id, logicSoundPair);
			}
			else
			{
				logicSoundPair.playedIndex = logicSoundRegister[id].playedIndex;
				logicSoundPair.lastPlayed = logicSoundRegister[id].lastPlayed;
			}
			if (logicSoundPair.playedIndex < 2)
			{
				logicSoundRegister[id].playedIndex = logicSoundPair.playedIndex + 1;
			}
			else
			{
				logicSoundRegister[id].playedIndex = 0;
				logicSoundRegister[id].lastPlayed = Time.time;
			}
			float num = (Time.time - logicSoundPair.lastPlayed) / 3f;
			EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name, false), Grid.CellToPos(this.cell), 1f);
			instance2.setParameterByName("logic_volumeModifer", num, false);
			instance2.setParameterByName("wireCount", (float)(networkForCell.WireCount % 24), false);
			instance2.setParameterByName("enabled", (float)new_value, false);
			KFMOD.EndOneShot(instance2);
		}
	}

	// Token: 0x06004A1F RID: 18975 RVA: 0x001AD7F4 File Offset: 0x001AB9F4
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
		if (this.onConnectionChanged != null)
		{
			this.onConnectionChanged(this.cell, connected);
		}
	}

	// Token: 0x04003130 RID: 12592
	private int cell;

	// Token: 0x04003131 RID: 12593
	private int value;

	// Token: 0x04003132 RID: 12594
	private Action<int, int> onValueChanged;

	// Token: 0x04003133 RID: 12595
	private Action<int, bool> onConnectionChanged;

	// Token: 0x04003134 RID: 12596
	private LogicPortSpriteType spriteType;
}
