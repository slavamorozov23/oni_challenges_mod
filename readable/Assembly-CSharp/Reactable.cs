using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000511 RID: 1297
public abstract class Reactable
{
	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06001BFE RID: 7166 RVA: 0x0009AE4E File Offset: 0x0009904E
	public bool IsValid
	{
		get
		{
			return this.partitionerEntry.IsValid();
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06001BFF RID: 7167 RVA: 0x0009AE5B File Offset: 0x0009905B
	// (set) Token: 0x06001C00 RID: 7168 RVA: 0x0009AE63 File Offset: 0x00099063
	public float creationTime { get; private set; }

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06001C01 RID: 7169 RVA: 0x0009AE6C File Offset: 0x0009906C
	public bool IsReacting
	{
		get
		{
			return this.reactor != null;
		}
	}

	// Token: 0x06001C02 RID: 7170 RVA: 0x0009AE7C File Offset: 0x0009907C
	public Reactable(GameObject gameObject, HashedString id, ChoreType chore_type, int range_width = 15, int range_height = 8, bool follow_transform = false, float globalCooldown = 0f, float localCooldown = 0f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f, ObjectLayer overrideLayer = ObjectLayer.NumLayers)
	{
		this.rangeHeight = range_height;
		this.rangeWidth = range_width;
		this.id = id;
		this.gameObject = gameObject;
		this.choreType = chore_type;
		this.globalCooldown = globalCooldown;
		this.localCooldown = localCooldown;
		this.lifeSpan = lifeSpan;
		this.initialDelay = ((max_initial_delay > 0f) ? UnityEngine.Random.Range(0f, max_initial_delay) : 0f);
		this.creationTime = GameClock.Instance.GetTime();
		ObjectLayer objectLayer = (overrideLayer == ObjectLayer.NumLayers) ? this.reactionLayer : overrideLayer;
		ReactionMonitor.Def def = gameObject.GetDef<ReactionMonitor.Def>();
		if (overrideLayer != objectLayer && def != null)
		{
			objectLayer = def.ReactionLayer;
		}
		this.reactionLayer = objectLayer;
		this.Initialize(follow_transform);
	}

	// Token: 0x06001C03 RID: 7171 RVA: 0x0009AF51 File Offset: 0x00099151
	public void Initialize(bool followTransform)
	{
		this.UpdateLocation();
		if (followTransform)
		{
			this.cellChangedHandleID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.gameObject.transform, Reactable.UpdateLocationDispatcher, this, "Reactable follow transform");
		}
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x0009AF82 File Offset: 0x00099182
	public void Begin(GameObject reactor)
	{
		this.reactor = reactor;
		this.lastTriggerTime = GameClock.Instance.GetTime();
		this.InternalBegin();
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x0009AFA4 File Offset: 0x000991A4
	public void End()
	{
		this.InternalEnd();
		if (this.reactor != null)
		{
			GameObject gameObject = this.reactor;
			this.InternalEnd();
			this.reactor = null;
			if (gameObject != null)
			{
				ReactionMonitor.Instance smi = gameObject.GetSMI<ReactionMonitor.Instance>();
				if (smi != null)
				{
					smi.StopReaction();
				}
			}
		}
	}

	// Token: 0x06001C06 RID: 7174 RVA: 0x0009AFF4 File Offset: 0x000991F4
	public bool CanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		float time = GameClock.Instance.GetTime();
		float num = time - this.creationTime;
		float num2 = time - this.lastTriggerTime;
		if (num < this.initialDelay || num2 < this.globalCooldown)
		{
			return false;
		}
		ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
		Chore chore = (component != null) ? component.choreDriver.GetCurrentChore() : null;
		if (chore == null || this.choreType.priority <= chore.choreType.priority)
		{
			return false;
		}
		int num3 = 0;
		while (this.additionalPreconditions != null && num3 < this.additionalPreconditions.Count)
		{
			if (!this.additionalPreconditions[num3](reactor, transition))
			{
				return false;
			}
			num3++;
		}
		return this.InternalCanBegin(reactor, transition);
	}

	// Token: 0x06001C07 RID: 7175 RVA: 0x0009B0AE File Offset: 0x000992AE
	public bool IsExpired()
	{
		return GameClock.Instance.GetTime() - this.creationTime > this.lifeSpan;
	}

	// Token: 0x06001C08 RID: 7176
	public abstract bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition);

	// Token: 0x06001C09 RID: 7177
	public abstract void Update(float dt);

	// Token: 0x06001C0A RID: 7178
	protected abstract void InternalBegin();

	// Token: 0x06001C0B RID: 7179
	protected abstract void InternalEnd();

	// Token: 0x06001C0C RID: 7180
	protected abstract void InternalCleanup();

	// Token: 0x06001C0D RID: 7181 RVA: 0x0009B0C9 File Offset: 0x000992C9
	public void Cleanup()
	{
		this.End();
		this.InternalCleanup();
		if (this.cellChangedHandleID != 0UL)
		{
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandleID);
		}
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x0009B100 File Offset: 0x00099300
	private void UpdateLocation()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.gameObject != null)
		{
			this.sourceCell = Grid.PosToCell(this.gameObject);
			Extents extents = new Extents(Grid.PosToXY(this.gameObject.transform.GetPosition()).x - this.rangeWidth / 2, Grid.PosToXY(this.gameObject.transform.GetPosition()).y - this.rangeHeight / 2, this.rangeWidth, this.rangeHeight);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("Reactable", this, extents, GameScenePartitioner.Instance.objectLayers[(int)this.reactionLayer], null);
		}
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x0009B1C1 File Offset: 0x000993C1
	public Reactable AddPrecondition(Reactable.ReactablePrecondition precondition)
	{
		if (this.additionalPreconditions == null)
		{
			this.additionalPreconditions = new List<Reactable.ReactablePrecondition>();
		}
		this.additionalPreconditions.Add(precondition);
		return this;
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x0009B1E3 File Offset: 0x000993E3
	public void InsertPrecondition(int index, Reactable.ReactablePrecondition precondition)
	{
		if (this.additionalPreconditions == null)
		{
			this.additionalPreconditions = new List<Reactable.ReactablePrecondition>();
		}
		index = Math.Min(index, this.additionalPreconditions.Count);
		this.additionalPreconditions.Insert(index, precondition);
	}

	// Token: 0x04001081 RID: 4225
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001082 RID: 4226
	protected GameObject gameObject;

	// Token: 0x04001083 RID: 4227
	public HashedString id;

	// Token: 0x04001084 RID: 4228
	public bool preventChoreInterruption = true;

	// Token: 0x04001085 RID: 4229
	public int sourceCell;

	// Token: 0x04001086 RID: 4230
	private int rangeWidth;

	// Token: 0x04001087 RID: 4231
	private int rangeHeight;

	// Token: 0x04001088 RID: 4232
	private ulong cellChangedHandleID;

	// Token: 0x04001089 RID: 4233
	public float globalCooldown;

	// Token: 0x0400108A RID: 4234
	public float localCooldown;

	// Token: 0x0400108B RID: 4235
	public float lifeSpan = float.PositiveInfinity;

	// Token: 0x0400108C RID: 4236
	private float lastTriggerTime = -2.1474836E+09f;

	// Token: 0x0400108D RID: 4237
	private float initialDelay;

	// Token: 0x0400108F RID: 4239
	protected GameObject reactor;

	// Token: 0x04001090 RID: 4240
	private ChoreType choreType;

	// Token: 0x04001091 RID: 4241
	protected LoggerFSS log;

	// Token: 0x04001092 RID: 4242
	private List<Reactable.ReactablePrecondition> additionalPreconditions;

	// Token: 0x04001093 RID: 4243
	private ObjectLayer reactionLayer;

	// Token: 0x04001094 RID: 4244
	private static readonly Action<object> UpdateLocationDispatcher = delegate(object obj)
	{
		Unsafe.As<Reactable>(obj).UpdateLocation();
	};

	// Token: 0x0200139F RID: 5023
	// (Invoke) Token: 0x06008C89 RID: 35977
	public delegate bool ReactablePrecondition(GameObject go, Navigator.ActiveTransition transition);
}
