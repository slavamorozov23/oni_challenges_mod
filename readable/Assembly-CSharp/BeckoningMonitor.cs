using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000887 RID: 2183
public class BeckoningMonitor : GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>
{
	// Token: 0x06003C1B RID: 15387 RVA: 0x001509D0 File Offset: 0x0014EBD0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(BeckoningMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.WantsToBeckon, (BeckoningMonitor.Instance smi) => smi.IsReadyToBeckon(), null).Update(delegate(BeckoningMonitor.Instance smi, float dt)
		{
			smi.UpdateBlockedStatusItem();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x02001851 RID: 6225
	[Serializable]
	public class SongChance
	{
		// Token: 0x04007A95 RID: 31381
		public Tag meteorID;

		// Token: 0x04007A96 RID: 31382
		public string singAnimPre;

		// Token: 0x04007A97 RID: 31383
		public string singAnimLoop;

		// Token: 0x04007A98 RID: 31384
		public string singAnimPst;

		// Token: 0x04007A99 RID: 31385
		public float weight;
	}

	// Token: 0x02001852 RID: 6226
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009E89 RID: 40585 RVA: 0x003A37F3 File Offset: 0x003A19F3
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Beckoning.Id);
		}

		// Token: 0x04007A9A RID: 31386
		public List<BeckoningMonitor.SongChance> initialSongWeights;

		// Token: 0x04007A9B RID: 31387
		public float caloriesPerCycle;

		// Token: 0x04007A9C RID: 31388
		public string effectId = "MooWellFed";
	}

	// Token: 0x02001853 RID: 6227
	public new class Instance : GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>.GameInstance
	{
		// Token: 0x06009E8B RID: 40587 RVA: 0x003A382C File Offset: 0x003A1A2C
		public Instance(IStateMachineTarget master, BeckoningMonitor.Def def) : base(master, def)
		{
			this.beckoning = Db.Get().Amounts.Beckoning.Lookup(base.gameObject);
			this.InitializSongChances();
		}

		// Token: 0x06009E8C RID: 40588 RVA: 0x003A385C File Offset: 0x003A1A5C
		private void InitializSongChances()
		{
			this.songChances = new List<BeckoningMonitor.SongChance>();
			if (base.def.initialSongWeights != null)
			{
				foreach (BeckoningMonitor.SongChance songChance in base.def.initialSongWeights)
				{
					this.songChances.Add(new BeckoningMonitor.SongChance
					{
						meteorID = songChance.meteorID,
						weight = songChance.weight,
						singAnimPre = songChance.singAnimPre,
						singAnimLoop = songChance.singAnimLoop,
						singAnimPst = songChance.singAnimPst
					});
					foreach (MooSongModifier mooSongModifier in Db.Get().MooSongModifiers.GetForTag(songChance.meteorID))
					{
						mooSongModifier.ApplyFunction(this, songChance.meteorID);
					}
				}
				this.NormalizeSongsChances();
			}
		}

		// Token: 0x06009E8D RID: 40589 RVA: 0x003A397C File Offset: 0x003A1B7C
		public void AddSongChance(Tag type, float addedPercentChance)
		{
			foreach (BeckoningMonitor.SongChance songChance in this.songChances)
			{
				if (songChance.meteorID == type)
				{
					float num = Mathf.Min(1f - songChance.weight, Mathf.Max(0f - songChance.weight, addedPercentChance));
					songChance.weight += num;
				}
			}
			this.NormalizeSongsChances();
			base.master.Trigger(1105317911, this.songChances);
		}

		// Token: 0x06009E8E RID: 40590 RVA: 0x003A3A24 File Offset: 0x003A1C24
		public void NormalizeSongsChances()
		{
			float num = 0f;
			foreach (BeckoningMonitor.SongChance songChance in this.songChances)
			{
				num += songChance.weight;
			}
			foreach (BeckoningMonitor.SongChance songChance2 in this.songChances)
			{
				songChance2.weight /= num;
			}
		}

		// Token: 0x06009E8F RID: 40591 RVA: 0x003A3AC8 File Offset: 0x003A1CC8
		private bool IsSpaceVisible()
		{
			int num = Grid.PosToCell(this);
			return Grid.IsValidCell(num) && Grid.ExposedToSunlight[num] > 0;
		}

		// Token: 0x06009E90 RID: 40592 RVA: 0x003A3AF4 File Offset: 0x003A1CF4
		private bool IsBeckoningAvailable()
		{
			return base.smi.beckoning.value >= base.smi.beckoning.GetMax();
		}

		// Token: 0x06009E91 RID: 40593 RVA: 0x003A3B1B File Offset: 0x003A1D1B
		public bool IsReadyToBeckon()
		{
			return this.IsBeckoningAvailable() && this.IsSpaceVisible();
		}

		// Token: 0x06009E92 RID: 40594 RVA: 0x003A3B30 File Offset: 0x003A1D30
		public void UpdateBlockedStatusItem()
		{
			bool flag = this.IsSpaceVisible();
			if (!flag && this.IsBeckoningAvailable() && this.beckoningBlockedHandle == Guid.Empty)
			{
				this.beckoningBlockedHandle = this.kselectable.AddStatusItem(Db.Get().CreatureStatusItems.BeckoningBlocked, null);
				return;
			}
			if (flag)
			{
				this.beckoningBlockedHandle = this.kselectable.RemoveStatusItem(this.beckoningBlockedHandle, false);
			}
		}

		// Token: 0x06009E93 RID: 40595 RVA: 0x003A3BA0 File Offset: 0x003A1DA0
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent value = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>)data).value;
			EffectInstance effectInstance = this.effects.Get(base.smi.def.effectId);
			if (effectInstance == null)
			{
				effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			effectInstance.timeRemaining += value.calories / base.smi.def.caloriesPerCycle * 600f;
		}

		// Token: 0x04007A9D RID: 31389
		private AmountInstance beckoning;

		// Token: 0x04007A9E RID: 31390
		[Serialize]
		public List<BeckoningMonitor.SongChance> songChances;

		// Token: 0x04007A9F RID: 31391
		[MyCmpGet]
		private Effects effects;

		// Token: 0x04007AA0 RID: 31392
		[MyCmpGet]
		public KSelectable kselectable;

		// Token: 0x04007AA1 RID: 31393
		private Guid beckoningBlockedHandle;
	}
}
