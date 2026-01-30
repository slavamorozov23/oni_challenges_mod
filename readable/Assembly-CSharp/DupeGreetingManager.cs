using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000903 RID: 2307
[AddComponentMenu("KMonoBehaviour/scripts/DupeGreetingManager")]
public class DupeGreetingManager : KMonoBehaviour, ISim200ms
{
	// Token: 0x0600401A RID: 16410 RVA: 0x00168ABB File Offset: 0x00166CBB
	protected override void OnPrefabInit()
	{
		this.candidateCells = new Dictionary<int, MinionIdentity>();
		this.activeSetups = new List<DupeGreetingManager.GreetingSetup>();
		this.cooldowns = new Dictionary<MinionIdentity, float>();
	}

	// Token: 0x0600401B RID: 16411 RVA: 0x00168AE0 File Offset: 0x00166CE0
	public void Sim200ms(float dt)
	{
		if (GameClock.Instance.GetTime() / 600f < TuningData<DupeGreetingManager.Tuning>.Get().cyclesBeforeFirstGreeting)
		{
			return;
		}
		for (int i = this.activeSetups.Count - 1; i >= 0; i--)
		{
			DupeGreetingManager.GreetingSetup greetingSetup = this.activeSetups[i];
			if (!this.ValidNavigatingMinion(greetingSetup.A.minion) || !this.ValidOppositionalMinion(greetingSetup.A.minion, greetingSetup.B.minion))
			{
				greetingSetup.A.reactable.Cleanup();
				greetingSetup.B.reactable.Cleanup();
				this.activeSetups.RemoveAt(i);
			}
		}
		this.candidateCells.Clear();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if ((!this.cooldowns.ContainsKey(minionIdentity) || GameClock.Instance.GetTime() - this.cooldowns[minionIdentity] >= 720f * TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier) && this.ValidNavigatingMinion(minionIdentity))
			{
				for (int j = 0; j <= 2; j++)
				{
					int offsetCell = this.GetOffsetCell(minionIdentity, j);
					if (this.candidateCells.ContainsKey(offsetCell) && this.ValidOppositionalMinion(minionIdentity, this.candidateCells[offsetCell]))
					{
						this.BeginNewGreeting(minionIdentity, this.candidateCells[offsetCell], offsetCell);
						break;
					}
					this.candidateCells[offsetCell] = minionIdentity;
				}
			}
		}
	}

	// Token: 0x0600401C RID: 16412 RVA: 0x00168C88 File Offset: 0x00166E88
	private int GetOffsetCell(MinionIdentity minion, int offset)
	{
		if (!minion.GetComponent<Facing>().GetFacing())
		{
			return Grid.OffsetCell(Grid.PosToCell(minion), offset, 0);
		}
		return Grid.OffsetCell(Grid.PosToCell(minion), -offset, 0);
	}

	// Token: 0x0600401D RID: 16413 RVA: 0x00168CB4 File Offset: 0x00166EB4
	private bool ValidNavigatingMinion(MinionIdentity minion)
	{
		if (minion == null)
		{
			return false;
		}
		Navigator component = minion.GetComponent<Navigator>();
		return component != null && component.IsMoving() && component.CurrentNavType == NavType.Floor;
	}

	// Token: 0x0600401E RID: 16414 RVA: 0x00168CF0 File Offset: 0x00166EF0
	private bool ValidOppositionalMinion(MinionIdentity reference_minion, MinionIdentity minion)
	{
		if (reference_minion == null)
		{
			return false;
		}
		if (minion == null)
		{
			return false;
		}
		Facing component = minion.GetComponent<Facing>();
		Facing component2 = reference_minion.GetComponent<Facing>();
		return this.ValidNavigatingMinion(minion) && component != null && component2 != null && component.GetFacing() != component2.GetFacing();
	}

	// Token: 0x0600401F RID: 16415 RVA: 0x00168D50 File Offset: 0x00166F50
	private void BeginNewGreeting(MinionIdentity minion_a, MinionIdentity minion_b, int cell)
	{
		DupeGreetingManager.GreetingSetup greetingSetup = new DupeGreetingManager.GreetingSetup();
		greetingSetup.cell = cell;
		greetingSetup.A = new DupeGreetingManager.GreetingUnit(minion_a, this.GetReactable(minion_a));
		greetingSetup.B = new DupeGreetingManager.GreetingUnit(minion_b, this.GetReactable(minion_b));
		this.activeSetups.Add(greetingSetup);
	}

	// Token: 0x06004020 RID: 16416 RVA: 0x00168D9C File Offset: 0x00166F9C
	private Reactable GetReactable(MinionIdentity minion)
	{
		if (DupeGreetingManager.emotes == null)
		{
			DupeGreetingManager.emotes = new List<Emote>
			{
				Db.Get().Emotes.Minion.Wave,
				Db.Get().Emotes.Minion.Wave_Shy,
				Db.Get().Emotes.Minion.FingerGuns
			};
		}
		Emote emote = DupeGreetingManager.emotes[UnityEngine.Random.Range(0, DupeGreetingManager.emotes.Count)];
		SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(minion.gameObject, "NavigatorPassingGreeting", Db.Get().ChoreTypes.Emote, 1000f, 20f, float.PositiveInfinity, 0f);
		selfEmoteReactable.SetEmote(emote).SetThought(Db.Get().Thoughts.Chatty);
		selfEmoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.BeginReacting), null);
		return selfEmoteReactable;
	}

	// Token: 0x06004021 RID: 16417 RVA: 0x00168E98 File Offset: 0x00167098
	private void BeginReacting(GameObject minionGO)
	{
		if (minionGO == null)
		{
			return;
		}
		MinionIdentity component = minionGO.GetComponent<MinionIdentity>();
		Vector3 vector = Vector3.zero;
		foreach (DupeGreetingManager.GreetingSetup greetingSetup in this.activeSetups)
		{
			if (greetingSetup.A.minion == component)
			{
				if (greetingSetup.B.minion != null)
				{
					vector = greetingSetup.B.minion.transform.GetPosition();
					greetingSetup.A.minion.Trigger(-594200555, greetingSetup.B.minion);
					greetingSetup.B.minion.Trigger(-594200555, greetingSetup.A.minion);
					break;
				}
				break;
			}
			else if (greetingSetup.B.minion == component)
			{
				if (greetingSetup.A.minion != null)
				{
					vector = greetingSetup.A.minion.transform.GetPosition();
					break;
				}
				break;
			}
		}
		minionGO.GetComponent<Facing>().SetFacing(vector.x < minionGO.transform.GetPosition().x);
		minionGO.GetComponent<Effects>().Add("Greeting", true);
		this.cooldowns[component] = GameClock.Instance.GetTime();
	}

	// Token: 0x040027B7 RID: 10167
	private const float COOLDOWN_TIME = 720f;

	// Token: 0x040027B8 RID: 10168
	private Dictionary<int, MinionIdentity> candidateCells;

	// Token: 0x040027B9 RID: 10169
	private List<DupeGreetingManager.GreetingSetup> activeSetups;

	// Token: 0x040027BA RID: 10170
	private Dictionary<MinionIdentity, float> cooldowns;

	// Token: 0x040027BB RID: 10171
	private static List<Emote> emotes;

	// Token: 0x020018FD RID: 6397
	public class Tuning : TuningData<DupeGreetingManager.Tuning>
	{
		// Token: 0x04007C95 RID: 31893
		public float cyclesBeforeFirstGreeting;

		// Token: 0x04007C96 RID: 31894
		public float greetingDelayMultiplier;
	}

	// Token: 0x020018FE RID: 6398
	private class GreetingUnit
	{
		// Token: 0x0600A0F6 RID: 41206 RVA: 0x003AAEE2 File Offset: 0x003A90E2
		public GreetingUnit(MinionIdentity minion, Reactable reactable)
		{
			this.minion = minion;
			this.reactable = reactable;
		}

		// Token: 0x04007C97 RID: 31895
		public MinionIdentity minion;

		// Token: 0x04007C98 RID: 31896
		public Reactable reactable;
	}

	// Token: 0x020018FF RID: 6399
	private class GreetingSetup
	{
		// Token: 0x04007C99 RID: 31897
		public int cell;

		// Token: 0x04007C9A RID: 31898
		public DupeGreetingManager.GreetingUnit A;

		// Token: 0x04007C9B RID: 31899
		public DupeGreetingManager.GreetingUnit B;
	}
}
