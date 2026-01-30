using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Token: 0x0200066D RID: 1645
public class TransitionDriver
{
	// Token: 0x170001CB RID: 459
	// (get) Token: 0x060027C6 RID: 10182 RVA: 0x000E4348 File Offset: 0x000E2548
	private Action<object> onAnimCompleteBinding
	{
		get
		{
			if (this.onAnimComplete_ == null)
			{
				this.onAnimComplete_ = new Action<object>(this.OnAnimComplete);
			}
			return this.onAnimComplete_;
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x060027C7 RID: 10183 RVA: 0x000E436A File Offset: 0x000E256A
	public Navigator.ActiveTransition GetTransition
	{
		get
		{
			return this.transition;
		}
	}

	// Token: 0x060027C8 RID: 10184 RVA: 0x000E4372 File Offset: 0x000E2572
	public TransitionDriver(Navigator navigator)
	{
		this.log = new LoggerFS("TransitionDriver", 35);
	}

	// Token: 0x060027C9 RID: 10185 RVA: 0x000E43AC File Offset: 0x000E25AC
	public void BeginTransition(Navigator navigator, NavGrid.Transition transition, float defaultSpeed)
	{
		Navigator.ActiveTransition activeTransition = TransitionDriver.TransitionPool.Get();
		activeTransition.Init(transition, defaultSpeed);
		this.BeginTransition(navigator, activeTransition);
	}

	// Token: 0x060027CA RID: 10186 RVA: 0x000E43D4 File Offset: 0x000E25D4
	private void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		bool flag = this.interruptOverrideStack.Count != 0;
		foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
		{
			if (!flag || !(overrideLayer is TransitionDriver.InterruptOverrideLayer))
			{
				overrideLayer.BeginTransition(navigator, transition);
			}
		}
		this.navigator = navigator;
		this.transition = transition;
		this.isComplete = false;
		Grid.SceneLayer sceneLayer = navigator.sceneLayer;
		if (transition.navGridTransition.start == NavType.Tube || transition.navGridTransition.end == NavType.Tube)
		{
			sceneLayer = Grid.SceneLayer.BuildingUse;
		}
		else if (transition.navGridTransition.start == NavType.Solid && transition.navGridTransition.end == NavType.Solid)
		{
			sceneLayer = Grid.SceneLayer.FXFront;
			navigator.animController.SetSceneLayer(sceneLayer);
		}
		else if (transition.navGridTransition.start == NavType.Solid || transition.navGridTransition.end == NavType.Solid)
		{
			navigator.animController.SetSceneLayer(sceneLayer);
		}
		int target_cell = Grid.OffsetCell(Grid.PosToCell(navigator), transition.x, transition.y);
		this.targetPos = this.GetTargetPosition(transition.navGridTransition, target_cell, sceneLayer);
		if (transition.isLooping)
		{
			KAnimControllerBase animController = navigator.animController;
			animController.PlaySpeedMultiplier = transition.animSpeed;
			bool flag2 = transition.preAnim != "";
			bool flag3 = animController.CurrentAnim != null && animController.CurrentAnim.name == transition.anim;
			if (flag2 && animController.CurrentAnim != null && animController.CurrentAnim.name == transition.preAnim)
			{
				animController.ClearQueue();
				animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
			else if (flag3)
			{
				if (animController.PlayMode != KAnim.PlayMode.Loop)
				{
					animController.ClearQueue();
					animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
			else if (flag2)
			{
				animController.Play(transition.preAnim, KAnim.PlayMode.Once, 1f, 0f);
				animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
			else
			{
				animController.Play(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}
		else if (transition.anim != null)
		{
			KBatchedAnimController animController2 = navigator.animController;
			animController2.PlaySpeedMultiplier = transition.animSpeed;
			animController2.Play(transition.anim, KAnim.PlayMode.Once, 1f, 0f);
			navigator.Unsubscribe(this.onAnimCompleteHandle);
			this.onAnimCompleteHandle = navigator.Subscribe(-1061186183, this.onAnimCompleteBinding);
		}
		if (transition.navGridTransition.y != 0)
		{
			if (transition.navGridTransition.start == NavType.RightWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.y < 0);
			}
			else if (transition.navGridTransition.start == NavType.LeftWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.y > 0);
			}
		}
		if (transition.navGridTransition.x != 0)
		{
			if (transition.navGridTransition.start == NavType.Ceiling)
			{
				navigator.facing.SetFacing(transition.navGridTransition.x > 0);
			}
			else if (transition.navGridTransition.start != NavType.LeftWall && transition.navGridTransition.start != NavType.RightWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.x < 0);
			}
		}
		this.brain = navigator.GetComponent<Brain>();
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x000E4770 File Offset: 0x000E2970
	private Vector3 GetTargetPosition(NavGrid.Transition trans, int target_cell, Grid.SceneLayer layer)
	{
		if (trans.useXOffset)
		{
			if (trans.x < 0)
			{
				return Grid.CellToPosRBC(target_cell, layer);
			}
			if (trans.x > 0)
			{
				return Grid.CellToPosLBC(target_cell, layer);
			}
		}
		return Grid.CellToPosCBC(target_cell, layer);
	}

	// Token: 0x060027CC RID: 10188 RVA: 0x000E47A4 File Offset: 0x000E29A4
	public void UpdateTransition(float dt)
	{
		if (this.navigator == null)
		{
			return;
		}
		foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
		{
			bool flag = this.interruptOverrideStack.Count != 0;
			bool flag2 = overrideLayer is TransitionDriver.InterruptOverrideLayer;
			if (!flag || !flag2 || this.interruptOverrideStack.Peek() == overrideLayer)
			{
				overrideLayer.UpdateTransition(this.navigator, this.transition);
			}
		}
		if (!this.isComplete && this.transition.isCompleteCB != null)
		{
			this.isComplete = this.transition.isCompleteCB();
		}
		if (this.brain != null)
		{
			bool flag3 = this.isComplete;
		}
		if (this.transition.isLooping)
		{
			float speed = this.transition.speed;
			Vector3 position = this.navigator.transform.GetPosition();
			int num = Grid.PosToCell(position);
			if (this.transition.x > 0)
			{
				position.x += dt * speed;
				if (position.x > this.targetPos.x)
				{
					this.isComplete = true;
				}
			}
			else if (this.transition.x < 0)
			{
				position.x -= dt * speed;
				if (position.x < this.targetPos.x)
				{
					this.isComplete = true;
				}
			}
			else
			{
				position.x = this.targetPos.x;
			}
			if (this.transition.y > 0)
			{
				position.y += dt * speed;
				if (position.y > this.targetPos.y)
				{
					this.isComplete = true;
				}
			}
			else if (this.transition.y < 0)
			{
				position.y -= dt * speed;
				if (position.y < this.targetPos.y)
				{
					this.isComplete = true;
				}
			}
			else
			{
				position.y = this.targetPos.y;
			}
			this.navigator.transform.SetPosition(position);
			int num2 = Grid.PosToCell(position);
			if (num2 != num)
			{
				this.navigator.BoxingTrigger<int>(915392638, num2);
			}
		}
		if (this.isComplete)
		{
			this.isComplete = false;
			Navigator navigator = this.navigator;
			navigator.SetCurrentNavType(this.transition.end);
			navigator.transform.SetPosition(this.targetPos);
			this.EndTransition();
			navigator.AdvancePath(true);
		}
	}

	// Token: 0x060027CD RID: 10189 RVA: 0x000E4A44 File Offset: 0x000E2C44
	public void EndTransition()
	{
		if (this.navigator != null)
		{
			this.interruptOverrideStack.Clear();
			foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
			{
				overrideLayer.EndTransition(this.navigator, this.transition);
			}
			this.navigator.animController.PlaySpeedMultiplier = 1f;
			this.navigator.Unsubscribe(ref this.onAnimCompleteHandle);
			if (this.brain != null)
			{
				this.brain.Resume("move_handler");
			}
			TransitionDriver.TransitionPool.Release(this.transition);
			this.transition = null;
			this.navigator = null;
			this.brain = null;
		}
	}

	// Token: 0x060027CE RID: 10190 RVA: 0x000E4B28 File Offset: 0x000E2D28
	private void OnAnimComplete(object data)
	{
		if (this.navigator != null)
		{
			this.navigator.Unsubscribe(ref this.onAnimCompleteHandle);
		}
		this.isComplete = true;
	}

	// Token: 0x060027CF RID: 10191 RVA: 0x000E4B50 File Offset: 0x000E2D50
	public static Navigator.ActiveTransition SwapTransitionWithEmpty(Navigator.ActiveTransition src)
	{
		Navigator.ActiveTransition activeTransition = TransitionDriver.TransitionPool.Get();
		activeTransition.Copy(src);
		src.Copy(TransitionDriver.emptyTransition);
		return activeTransition;
	}

	// Token: 0x0400175B RID: 5979
	private static Navigator.ActiveTransition emptyTransition = new Navigator.ActiveTransition();

	// Token: 0x0400175C RID: 5980
	public static ObjectPool<Navigator.ActiveTransition> TransitionPool = new ObjectPool<Navigator.ActiveTransition>(() => new Navigator.ActiveTransition(), null, null, null, false, 128, 10000);

	// Token: 0x0400175D RID: 5981
	private Stack<TransitionDriver.InterruptOverrideLayer> interruptOverrideStack = new Stack<TransitionDriver.InterruptOverrideLayer>(8);

	// Token: 0x0400175E RID: 5982
	private Navigator.ActiveTransition transition;

	// Token: 0x0400175F RID: 5983
	private Navigator navigator;

	// Token: 0x04001760 RID: 5984
	private Vector3 targetPos;

	// Token: 0x04001761 RID: 5985
	private bool isComplete;

	// Token: 0x04001762 RID: 5986
	private Brain brain;

	// Token: 0x04001763 RID: 5987
	public List<TransitionDriver.OverrideLayer> overrideLayers = new List<TransitionDriver.OverrideLayer>();

	// Token: 0x04001764 RID: 5988
	private LoggerFS log;

	// Token: 0x04001765 RID: 5989
	private Action<object> onAnimComplete_;

	// Token: 0x04001766 RID: 5990
	private int onAnimCompleteHandle = -1;

	// Token: 0x02001534 RID: 5428
	public class OverrideLayer
	{
		// Token: 0x06009285 RID: 37509 RVA: 0x0037408F File Offset: 0x0037228F
		public OverrideLayer(Navigator navigator)
		{
		}

		// Token: 0x06009286 RID: 37510 RVA: 0x00374097 File Offset: 0x00372297
		public virtual void Destroy()
		{
		}

		// Token: 0x06009287 RID: 37511 RVA: 0x00374099 File Offset: 0x00372299
		public virtual void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		// Token: 0x06009288 RID: 37512 RVA: 0x0037409B File Offset: 0x0037229B
		public virtual void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		// Token: 0x06009289 RID: 37513 RVA: 0x0037409D File Offset: 0x0037229D
		public virtual void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}
	}

	// Token: 0x02001535 RID: 5429
	public class InterruptOverrideLayer : TransitionDriver.OverrideLayer
	{
		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x0600928A RID: 37514 RVA: 0x0037409F File Offset: 0x0037229F
		protected bool InterruptInProgress
		{
			get
			{
				return this.originalTransition != null;
			}
		}

		// Token: 0x0600928B RID: 37515 RVA: 0x003740AA File Offset: 0x003722AA
		public InterruptOverrideLayer(Navigator navigator) : base(navigator)
		{
			this.driver = navigator.transitionDriver;
		}

		// Token: 0x0600928C RID: 37516 RVA: 0x003740BF File Offset: 0x003722BF
		public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			this.driver.interruptOverrideStack.Push(this);
			this.originalTransition = TransitionDriver.SwapTransitionWithEmpty(transition);
		}

		// Token: 0x0600928D RID: 37517 RVA: 0x003740E0 File Offset: 0x003722E0
		public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			if (!this.IsOverrideComplete())
			{
				return;
			}
			this.driver.interruptOverrideStack.Pop();
			transition.Copy(this.originalTransition);
			TransitionDriver.TransitionPool.Release(this.originalTransition);
			this.originalTransition = null;
			this.EndTransition(navigator, transition);
			this.driver.BeginTransition(navigator, transition);
		}

		// Token: 0x0600928E RID: 37518 RVA: 0x0037413F File Offset: 0x0037233F
		public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			base.EndTransition(navigator, transition);
			if (this.originalTransition == null)
			{
				return;
			}
			TransitionDriver.TransitionPool.Release(this.originalTransition);
			this.originalTransition = null;
		}

		// Token: 0x0600928F RID: 37519 RVA: 0x00374169 File Offset: 0x00372369
		protected virtual bool IsOverrideComplete()
		{
			return this.originalTransition != null && this.driver.interruptOverrideStack.Count != 0 && this.driver.interruptOverrideStack.Peek() == this;
		}

		// Token: 0x04007100 RID: 28928
		protected Navigator.ActiveTransition originalTransition;

		// Token: 0x04007101 RID: 28929
		protected TransitionDriver driver;
	}
}
