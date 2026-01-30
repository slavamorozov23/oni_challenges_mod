using System;
using Database;
using UnityEngine;

// Token: 0x0200076F RID: 1903
[AddComponentMenu("KMonoBehaviour/Workable/GetBalloonWorkable")]
public class GetBalloonWorkable : Workable
{
	// Token: 0x06003067 RID: 12391 RVA: 0x00117888 File Offset: 0x00115A88
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = null;
		this.workingStatusItem = null;
		this.workAnims = GetBalloonWorkable.GET_BALLOON_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			GetBalloonWorkable.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			GetBalloonWorkable.PST_ANIM
		};
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x001178EC File Offset: 0x00115AEC
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
		if (balloonOverride.animFile.IsNone())
		{
			worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", Assets.GetAnim("balloon_anim_kanim").GetData().build.GetSymbol("body"), 0);
			return;
		}
		worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", balloonOverride.symbol.Unwrap(), 0);
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x00117988 File Offset: 0x00115B88
	protected override void OnCompleteWork(WorkerBase worker)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EquippableBalloon"), worker.transform.GetPosition());
		gameObject.GetComponent<Equippable>().Assign(worker.GetComponent<MinionIdentity>());
		gameObject.GetComponent<Equippable>().isEquipped = true;
		gameObject.SetActive(true);
		base.OnCompleteWork(worker);
		BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
		this.balloonArtist.GiveBalloon(balloonOverride);
		gameObject.GetComponent<EquippableBalloon>().SetBalloonOverride(balloonOverride);
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x00117A02 File Offset: 0x00115C02
	public override Vector3 GetFacingTarget()
	{
		return this.balloonArtist.master.transform.GetPosition();
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x00117A19 File Offset: 0x00115C19
	public void SetBalloonArtist(BalloonArtistChore.StatesInstance chore)
	{
		this.balloonArtist = chore;
	}

	// Token: 0x0600306C RID: 12396 RVA: 0x00117A22 File Offset: 0x00115C22
	public BalloonArtistChore.StatesInstance GetBalloonArtist()
	{
		return this.balloonArtist;
	}

	// Token: 0x04001CCD RID: 7373
	private static readonly HashedString[] GET_BALLOON_ANIMS = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04001CCE RID: 7374
	private static readonly HashedString PST_ANIM = new HashedString("working_pst");

	// Token: 0x04001CCF RID: 7375
	private BalloonArtistChore.StatesInstance balloonArtist;

	// Token: 0x04001CD0 RID: 7376
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x04001CD1 RID: 7377
	private const int TARGET_OVERRIDE_PRIORITY = 0;
}
