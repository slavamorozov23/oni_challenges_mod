using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000510 RID: 1296
public class EmoteReactable : Reactable
{
	// Token: 0x06001BF2 RID: 7154 RVA: 0x0009A994 File Offset: 0x00098B94
	public EmoteReactable(GameObject gameObject, HashedString id, ChoreType chore_type, int range_width = 15, int range_height = 8, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f) : base(gameObject, id, chore_type, range_width, range_height, true, globalCooldown, localCooldown, lifeSpan, max_initial_delay, ObjectLayer.NumLayers)
	{
	}

	// Token: 0x06001BF3 RID: 7155 RVA: 0x0009A9C0 File Offset: 0x00098BC0
	public EmoteReactable SetEmote(Emote emote)
	{
		this.emote = emote;
		return this;
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x0009A9CC File Offset: 0x00098BCC
	public EmoteReactable RegisterEmoteStepCallbacks(HashedString stepName, Action<GameObject> startedCb, Action<GameObject> finishedCb)
	{
		if (this.callbackHandles == null)
		{
			this.callbackHandles = new HandleVector<EmoteStep.Callbacks>.Handle[this.emote.StepCount];
		}
		int stepIndex = this.emote.GetStepIndex(stepName);
		this.callbackHandles[stepIndex] = this.emote[stepIndex].RegisterCallbacks(startedCb, finishedCb);
		return this;
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x0009AA24 File Offset: 0x00098C24
	public EmoteReactable SetExpression(Expression expression)
	{
		this.expression = expression;
		return this;
	}

	// Token: 0x06001BF6 RID: 7158 RVA: 0x0009AA2E File Offset: 0x00098C2E
	public EmoteReactable SetThought(Thought thought)
	{
		this.thought = thought;
		return this;
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x0009AA38 File Offset: 0x00098C38
	public EmoteReactable SetOverideAnimSet(string animSet)
	{
		this.overrideAnimSet = Assets.GetAnim(animSet);
		return this;
	}

	// Token: 0x06001BF8 RID: 7160 RVA: 0x0009AA4C File Offset: 0x00098C4C
	public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
	{
		if (this.reactor != null || new_reactor == null)
		{
			return false;
		}
		Navigator component = new_reactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving() && (-257 & 1 << (int)component.CurrentNavType) != 0 && this.gameObject != new_reactor;
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x0009AAB0 File Offset: 0x00098CB0
	public override void Update(float dt)
	{
		if (this.emote == null || !this.emote.IsValidStep(this.currentStep))
		{
			return;
		}
		if (this.gameObject != null && this.reactor != null)
		{
			Facing component = this.reactor.GetComponent<Facing>();
			if (component != null)
			{
				component.Face(this.gameObject.transform.GetPosition());
			}
		}
		float timeout = this.emote[this.currentStep].timeout;
		if (timeout > 0f && timeout < this.elapsed)
		{
			this.NextStep(null);
			return;
		}
		this.elapsed += dt;
	}

	// Token: 0x06001BFA RID: 7162 RVA: 0x0009AB64 File Offset: 0x00098D64
	protected override void InternalBegin()
	{
		this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
		this.emote.ApplyAnimOverrides(this.kbac, this.overrideAnimSet);
		if (this.expression != null)
		{
			this.reactor.GetComponent<FaceGraph>().AddExpression(this.expression);
		}
		if (this.thought != null)
		{
			this.reactor.GetSMI<ThoughtGraph.Instance>().AddThought(this.thought);
		}
		this.NextStep(null);
	}

	// Token: 0x06001BFB RID: 7163 RVA: 0x0009ABE4 File Offset: 0x00098DE4
	protected override void InternalEnd()
	{
		if (this.kbac != null)
		{
			this.kbac.onAnimComplete -= this.NextStep;
			this.emote.RemoveAnimOverrides(this.kbac, this.overrideAnimSet);
			this.kbac = null;
		}
		if (this.reactor != null)
		{
			if (this.expression != null)
			{
				this.reactor.GetComponent<FaceGraph>().RemoveExpression(this.expression);
			}
			if (this.thought != null)
			{
				this.reactor.GetSMI<ThoughtGraph.Instance>().RemoveThought(this.thought);
			}
		}
		this.currentStep = -1;
	}

	// Token: 0x06001BFC RID: 7164 RVA: 0x0009AC88 File Offset: 0x00098E88
	protected override void InternalCleanup()
	{
		if (this.emote == null || this.callbackHandles == null)
		{
			return;
		}
		int num = 0;
		while (this.emote.IsValidStep(num))
		{
			this.emote[num].UnregisterCallbacks(this.callbackHandles[num]);
			num++;
		}
	}

	// Token: 0x06001BFD RID: 7165 RVA: 0x0009ACDC File Offset: 0x00098EDC
	private void NextStep(HashedString finishedAnim)
	{
		if (this.emote.IsValidStep(this.currentStep) && this.emote[this.currentStep].timeout <= 0f)
		{
			this.kbac.onAnimComplete -= this.NextStep;
			if (this.callbackHandles != null)
			{
				this.emote[this.currentStep].OnStepFinished(this.callbackHandles[this.currentStep], this.reactor);
			}
		}
		this.currentStep++;
		if (!this.emote.IsValidStep(this.currentStep) || this.kbac == null)
		{
			base.End();
			return;
		}
		EmoteStep emoteStep = this.emote[this.currentStep];
		if (emoteStep.anim != HashedString.Invalid)
		{
			this.kbac.Play(emoteStep.anim, emoteStep.mode, 1f, 0f);
			if (this.kbac.IsStopped())
			{
				emoteStep.timeout = 0.25f;
			}
		}
		if (emoteStep.timeout <= 0f)
		{
			this.kbac.onAnimComplete += this.NextStep;
		}
		else
		{
			this.elapsed = 0f;
		}
		if (this.callbackHandles != null)
		{
			emoteStep.OnStepStarted(this.callbackHandles[this.currentStep], this.reactor);
		}
	}

	// Token: 0x04001079 RID: 4217
	private KBatchedAnimController kbac;

	// Token: 0x0400107A RID: 4218
	public Expression expression;

	// Token: 0x0400107B RID: 4219
	public Thought thought;

	// Token: 0x0400107C RID: 4220
	public Emote emote;

	// Token: 0x0400107D RID: 4221
	private HandleVector<EmoteStep.Callbacks>.Handle[] callbackHandles;

	// Token: 0x0400107E RID: 4222
	protected KAnimFile overrideAnimSet;

	// Token: 0x0400107F RID: 4223
	private int currentStep = -1;

	// Token: 0x04001080 RID: 4224
	private float elapsed;
}
