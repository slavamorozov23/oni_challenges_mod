using System;

// Token: 0x020005F8 RID: 1528
public class KnockKnock : Activatable
{
	// Token: 0x06002376 RID: 9078 RVA: 0x000CD06B File Offset: 0x000CB26B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
	}

	// Token: 0x06002377 RID: 9079 RVA: 0x000CD07A File Offset: 0x000CB27A
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (!this.doorAnswered)
		{
			this.workTimeRemaining += dt;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002378 RID: 9080 RVA: 0x000CD09A File Offset: 0x000CB29A
	public void AnswerDoor()
	{
		this.doorAnswered = true;
		this.workTimeRemaining = 1f;
	}

	// Token: 0x040014AB RID: 5291
	private bool doorAnswered;
}
