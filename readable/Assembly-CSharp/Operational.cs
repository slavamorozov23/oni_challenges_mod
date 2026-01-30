using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000619 RID: 1561
[AddComponentMenu("KMonoBehaviour/scripts/Operational")]
public class Operational : KMonoBehaviour
{
	// Token: 0x1700018E RID: 398
	// (get) Token: 0x060024C1 RID: 9409 RVA: 0x000D368F File Offset: 0x000D188F
	// (set) Token: 0x060024C2 RID: 9410 RVA: 0x000D3697 File Offset: 0x000D1897
	public bool IsFunctional { get; private set; }

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x060024C3 RID: 9411 RVA: 0x000D36A0 File Offset: 0x000D18A0
	// (set) Token: 0x060024C4 RID: 9412 RVA: 0x000D36A8 File Offset: 0x000D18A8
	public bool IsOperational { get; private set; }

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x060024C5 RID: 9413 RVA: 0x000D36B1 File Offset: 0x000D18B1
	// (set) Token: 0x060024C6 RID: 9414 RVA: 0x000D36B9 File Offset: 0x000D18B9
	public bool IsActive { get; private set; }

	// Token: 0x060024C7 RID: 9415 RVA: 0x000D36C2 File Offset: 0x000D18C2
	[OnSerializing]
	private void OnSerializing()
	{
		this.AddTimeData(this.IsActive);
		this.activeStartTime = GameClock.Instance.GetTime();
		this.inactiveStartTime = GameClock.Instance.GetTime();
	}

	// Token: 0x060024C8 RID: 9416 RVA: 0x000D36F0 File Offset: 0x000D18F0
	protected override void OnPrefabInit()
	{
		this.UpdateFunctional();
		this.UpdateOperational();
		base.Subscribe<Operational>(-1661515756, Operational.OnNewBuildingDelegate);
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
	}

	// Token: 0x060024C9 RID: 9417 RVA: 0x000D372C File Offset: 0x000D192C
	public void OnNewBuilding(object data)
	{
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.creationTime > 0f)
		{
			this.inactiveStartTime = component.creationTime;
			this.activeStartTime = component.creationTime;
		}
	}

	// Token: 0x060024CA RID: 9418 RVA: 0x000D3765 File Offset: 0x000D1965
	public bool IsOperationalType(Operational.Flag.Type type)
	{
		if (type == Operational.Flag.Type.Functional)
		{
			return this.IsFunctional;
		}
		return this.IsOperational;
	}

	// Token: 0x060024CB RID: 9419 RVA: 0x000D3778 File Offset: 0x000D1978
	public void SetFlag(Operational.Flag flag, bool value)
	{
		bool flag2 = false;
		if (this.Flags.TryGetValue(flag, out flag2))
		{
			if (flag2 != value)
			{
				this.Flags[flag] = value;
				base.Trigger(187661686, flag);
			}
		}
		else
		{
			this.Flags[flag] = value;
			base.Trigger(187661686, flag);
		}
		if (flag.FlagType == Operational.Flag.Type.Functional && value != this.IsFunctional)
		{
			this.UpdateFunctional();
		}
		if (value != this.IsOperational)
		{
			this.UpdateOperational();
		}
	}

	// Token: 0x060024CC RID: 9420 RVA: 0x000D37F8 File Offset: 0x000D19F8
	public bool GetFlag(Operational.Flag flag)
	{
		bool result = false;
		this.Flags.TryGetValue(flag, out result);
		return result;
	}

	// Token: 0x060024CD RID: 9421 RVA: 0x000D3818 File Offset: 0x000D1A18
	private void UpdateFunctional()
	{
		bool isFunctional = true;
		foreach (KeyValuePair<Operational.Flag, bool> keyValuePair in this.Flags)
		{
			if (keyValuePair.Key.FlagType == Operational.Flag.Type.Functional && !keyValuePair.Value)
			{
				isFunctional = false;
				break;
			}
		}
		this.IsFunctional = isFunctional;
		base.Trigger(-1852328367, BoxedBools.Box(this.IsFunctional));
	}

	// Token: 0x060024CE RID: 9422 RVA: 0x000D38A0 File Offset: 0x000D1AA0
	private void UpdateOperational()
	{
		Dictionary<Operational.Flag, bool>.Enumerator enumerator = this.Flags.GetEnumerator();
		bool flag = true;
		while (enumerator.MoveNext())
		{
			KeyValuePair<Operational.Flag, bool> keyValuePair = enumerator.Current;
			if (!keyValuePair.Value)
			{
				flag = false;
				break;
			}
		}
		if (flag != this.IsOperational)
		{
			this.IsOperational = flag;
			if (!this.IsOperational)
			{
				this.SetActive(false, false);
			}
			if (this.IsOperational)
			{
				base.GetComponent<KPrefabID>().AddTag(GameTags.Operational, false);
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Operational);
			}
			base.Trigger(-592767678, BoxedBools.Box(this.IsOperational));
			Game.Instance.Trigger(-809948329, base.gameObject);
		}
	}

	// Token: 0x060024CF RID: 9423 RVA: 0x000D3951 File Offset: 0x000D1B51
	public void SetActive(bool value, bool force_ignore = false)
	{
		if (this.IsActive != value)
		{
			this.AddTimeData(value);
			base.Trigger(824508782, this);
			Game.Instance.Trigger(-809948329, base.gameObject);
		}
	}

	// Token: 0x060024D0 RID: 9424 RVA: 0x000D3984 File Offset: 0x000D1B84
	private void AddTimeData(bool value)
	{
		float num = this.IsActive ? this.activeStartTime : this.inactiveStartTime;
		float time = GameClock.Instance.GetTime();
		float num2 = time - num;
		if (this.IsActive)
		{
			this.activeTime += num2;
		}
		else
		{
			this.inactiveTime += num2;
		}
		this.IsActive = value;
		if (this.IsActive)
		{
			this.activeStartTime = time;
			return;
		}
		this.inactiveStartTime = time;
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x000D39FC File Offset: 0x000D1BFC
	public void OnNewDay(object data)
	{
		this.AddTimeData(this.IsActive);
		this.uptimeData.Add(this.activeTime / 600f);
		while (this.uptimeData.Count > this.MAX_DATA_POINTS)
		{
			this.uptimeData.RemoveAt(0);
		}
		this.activeTime = 0f;
		this.inactiveTime = 0f;
	}

	// Token: 0x060024D2 RID: 9426 RVA: 0x000D3A64 File Offset: 0x000D1C64
	public float GetCurrentCycleUptime()
	{
		if (this.IsActive)
		{
			float num = this.IsActive ? this.activeStartTime : this.inactiveStartTime;
			float num2 = GameClock.Instance.GetTime() - num;
			return (this.activeTime + num2) / GameClock.Instance.GetTimeSinceStartOfCycle();
		}
		return this.activeTime / GameClock.Instance.GetTimeSinceStartOfCycle();
	}

	// Token: 0x060024D3 RID: 9427 RVA: 0x000D3AC2 File Offset: 0x000D1CC2
	public float GetLastCycleUptime()
	{
		if (this.uptimeData.Count > 0)
		{
			return this.uptimeData[this.uptimeData.Count - 1];
		}
		return 0f;
	}

	// Token: 0x060024D4 RID: 9428 RVA: 0x000D3AF0 File Offset: 0x000D1CF0
	public float GetUptimeOverCycles(int num_cycles)
	{
		if (this.uptimeData.Count > 0)
		{
			int num = Mathf.Min(this.uptimeData.Count, num_cycles);
			float num2 = 0f;
			for (int i = num - 1; i >= 0; i--)
			{
				num2 += this.uptimeData[i];
			}
			return num2 / (float)num;
		}
		return 0f;
	}

	// Token: 0x060024D5 RID: 9429 RVA: 0x000D3B4A File Offset: 0x000D1D4A
	public bool MeetsRequirements(Operational.State stateRequirement)
	{
		switch (stateRequirement)
		{
		case Operational.State.Operational:
			return this.IsOperational;
		case Operational.State.Functional:
			return this.IsFunctional;
		case Operational.State.Active:
			return this.IsActive;
		}
		return true;
	}

	// Token: 0x060024D6 RID: 9430 RVA: 0x000D3B7A File Offset: 0x000D1D7A
	public static GameHashes GetEventForState(Operational.State state)
	{
		if (state == Operational.State.Operational)
		{
			return GameHashes.OperationalChanged;
		}
		if (state == Operational.State.Functional)
		{
			return GameHashes.FunctionalChanged;
		}
		return GameHashes.ActiveChanged;
	}

	// Token: 0x04001588 RID: 5512
	[Serialize]
	public float inactiveStartTime;

	// Token: 0x04001589 RID: 5513
	[Serialize]
	public float activeStartTime;

	// Token: 0x0400158A RID: 5514
	[Serialize]
	private List<float> uptimeData = new List<float>();

	// Token: 0x0400158B RID: 5515
	[Serialize]
	private float activeTime;

	// Token: 0x0400158C RID: 5516
	[Serialize]
	private float inactiveTime;

	// Token: 0x0400158D RID: 5517
	private int MAX_DATA_POINTS = 5;

	// Token: 0x0400158E RID: 5518
	public Dictionary<Operational.Flag, bool> Flags = new Dictionary<Operational.Flag, bool>();

	// Token: 0x0400158F RID: 5519
	private static readonly EventSystem.IntraObjectHandler<Operational> OnNewBuildingDelegate = new EventSystem.IntraObjectHandler<Operational>(delegate(Operational component, object data)
	{
		component.OnNewBuilding(data);
	});

	// Token: 0x020014EB RID: 5355
	public enum State
	{
		// Token: 0x04006FFB RID: 28667
		Operational,
		// Token: 0x04006FFC RID: 28668
		Functional,
		// Token: 0x04006FFD RID: 28669
		Active,
		// Token: 0x04006FFE RID: 28670
		None
	}

	// Token: 0x020014EC RID: 5356
	public class Flag
	{
		// Token: 0x06009184 RID: 37252 RVA: 0x003715D5 File Offset: 0x0036F7D5
		public Flag(string name, Operational.Flag.Type type)
		{
			this.Name = name;
			this.FlagType = type;
		}

		// Token: 0x06009185 RID: 37253 RVA: 0x003715EB File Offset: 0x0036F7EB
		public static Operational.Flag.Type GetFlagType(Operational.State operationalState)
		{
			switch (operationalState)
			{
			case Operational.State.Operational:
			case Operational.State.Active:
				return Operational.Flag.Type.Requirement;
			case Operational.State.Functional:
				return Operational.Flag.Type.Functional;
			}
			throw new InvalidOperationException("Can not convert NONE state to an Operational Flag Type");
		}

		// Token: 0x04006FFF RID: 28671
		public string Name;

		// Token: 0x04007000 RID: 28672
		public Operational.Flag.Type FlagType;

		// Token: 0x020028A7 RID: 10407
		public enum Type
		{
			// Token: 0x0400B315 RID: 45845
			Requirement,
			// Token: 0x0400B316 RID: 45846
			Functional
		}
	}
}
