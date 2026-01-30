using System;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02000D5A RID: 3418
public class KleiPermitDioramaVis_JoyResponseBalloon : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060069B7 RID: 27063 RVA: 0x00280289 File Offset: 0x0027E489
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060069B8 RID: 27064 RVA: 0x00280294 File Offset: 0x0027E494
	public void ConfigureSetup()
	{
		this.minionUI.transform.localScale = Vector3.one * 0.7f;
		this.minionUI.transform.localPosition = new Vector3(this.minionUI.transform.localPosition.x - 73f, this.minionUI.transform.localPosition.y - 152f + 8f, this.minionUI.transform.localPosition.z);
	}

	// Token: 0x060069B9 RID: 27065 RVA: 0x00280326 File Offset: 0x0027E526
	public void ConfigureWith(PermitResource permit)
	{
		this.ConfigureWith(Option.Some<BalloonArtistFacadeResource>((BalloonArtistFacadeResource)permit));
	}

	// Token: 0x060069BA RID: 27066 RVA: 0x0028033C File Offset: 0x0027E53C
	public void ConfigureWith(Option<BalloonArtistFacadeResource> permit)
	{
		KleiPermitDioramaVis_JoyResponseBalloon.<>c__DisplayClass10_0 CS$<>8__locals1 = new KleiPermitDioramaVis_JoyResponseBalloon.<>c__DisplayClass10_0();
		CS$<>8__locals1.permit = permit;
		KBatchedAnimController component = this.minionUI.SpawnedAvatar.GetComponent<KBatchedAnimController>();
		CS$<>8__locals1.minionSymbolOverrider = this.minionUI.SpawnedAvatar.GetComponent<SymbolOverrideController>();
		this.minionUI.SetMinion(this.specificPersonality.UnwrapOrElse(() => (from p in Db.Get().Personalities.GetAll(true, true)
		where p.joyTrait == "BalloonArtist"
		select p).GetRandom<Personality>(), null));
		if (!this.didAddAnims)
		{
			this.didAddAnims = true;
			component.AddAnimOverrides(Assets.GetAnim("anim_interacts_balloon_artist_kanim"), 0f);
		}
		component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
		CS$<>8__locals1.<ConfigureWith>g__DisplayNextBalloon|3();
		Updater[] array = new Updater[2];
		array[0] = Updater.WaitForSeconds(1.3f);
		int num = 1;
		Func<Updater>[] array2 = new Func<Updater>[2];
		array2[0] = (() => Updater.WaitForSeconds(1.618f));
		array2[1] = (() => Updater.Do(new System.Action(base.<ConfigureWith>g__DisplayNextBalloon|3)));
		array[num] = Updater.Loop(array2);
		this.QueueUpdater(Updater.Series(array));
	}

	// Token: 0x060069BB RID: 27067 RVA: 0x0028047D File Offset: 0x0027E67D
	public void SetMinion(Personality personality)
	{
		this.specificPersonality = personality;
		if (base.gameObject.activeInHierarchy)
		{
			this.minionUI.SetMinion(personality);
		}
	}

	// Token: 0x060069BC RID: 27068 RVA: 0x002804A4 File Offset: 0x0027E6A4
	private void QueueUpdater(Updater updater)
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.RunUpdater(updater);
			return;
		}
		this.updaterToRunOnStart = updater;
	}

	// Token: 0x060069BD RID: 27069 RVA: 0x002804C7 File Offset: 0x0027E6C7
	private void RunUpdater(Updater updater)
	{
		if (this.updaterRoutine != null)
		{
			base.StopCoroutine(this.updaterRoutine);
			this.updaterRoutine = null;
		}
		this.updaterRoutine = base.StartCoroutine(updater);
	}

	// Token: 0x060069BE RID: 27070 RVA: 0x002804F6 File Offset: 0x0027E6F6
	private void OnEnable()
	{
		if (this.updaterToRunOnStart.IsSome())
		{
			this.RunUpdater(this.updaterToRunOnStart.Unwrap());
			this.updaterToRunOnStart = Option.None;
		}
	}

	// Token: 0x040048AD RID: 18605
	private const int FRAMES_TO_MAKE_BALLOON_IN_ANIM = 39;

	// Token: 0x040048AE RID: 18606
	private const float SECONDS_TO_MAKE_BALLOON_IN_ANIM = 1.3f;

	// Token: 0x040048AF RID: 18607
	private const float SECONDS_BETWEEN_BALLOONS = 1.618f;

	// Token: 0x040048B0 RID: 18608
	[SerializeField]
	private UIMinion minionUI;

	// Token: 0x040048B1 RID: 18609
	private bool didAddAnims;

	// Token: 0x040048B2 RID: 18610
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x040048B3 RID: 18611
	private const int TARGET_OVERRIDE_PRIORITY = 0;

	// Token: 0x040048B4 RID: 18612
	private Option<Personality> specificPersonality;

	// Token: 0x040048B5 RID: 18613
	private Option<PermitResource> lastConfiguredPermit;

	// Token: 0x040048B6 RID: 18614
	private Option<Updater> updaterToRunOnStart;

	// Token: 0x040048B7 RID: 18615
	private Coroutine updaterRoutine;
}
