using System;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public class SelfEmoteReactable : EmoteReactable
{
	// Token: 0x06001C1B RID: 7195 RVA: 0x0009B480 File Offset: 0x00099680
	public SelfEmoteReactable(GameObject gameObject, HashedString id, ChoreType chore_type, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f) : base(gameObject, id, chore_type, 3, 3, globalCooldown, localCooldown, lifeSpan, max_initial_delay)
	{
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x0009B4A0 File Offset: 0x000996A0
	public override bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		if (reactor != this.gameObject)
		{
			return false;
		}
		Navigator component = reactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving();
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x0009B4D8 File Offset: 0x000996D8
	public void PairEmote(EmoteChore emoteChore)
	{
		this.chore = emoteChore;
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x0009B4E4 File Offset: 0x000996E4
	protected override void InternalEnd()
	{
		if (this.chore != null && this.chore.driver != null)
		{
			this.chore.PairReactable(null);
			this.chore.Cancel("Reactable ended");
			this.chore = null;
		}
		base.InternalEnd();
	}

	// Token: 0x04001099 RID: 4249
	private EmoteChore chore;
}
