using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000300 RID: 768
public class BalloonStandConfig : IEntityConfig
{
	// Token: 0x06000FAA RID: 4010 RVA: 0x0005BE00 File Offset: 0x0005A000
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(BalloonStandConfig.ID, BalloonStandConfig.ID, false);
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_balloon_receiver_kanim")
		};
		GetBalloonWorkable getBalloonWorkable = gameObject.AddOrGet<GetBalloonWorkable>();
		getBalloonWorkable.workTime = 2f;
		getBalloonWorkable.workLayer = Grid.SceneLayer.BuildingFront;
		getBalloonWorkable.overrideAnims = overrideAnims;
		getBalloonWorkable.synchronizeAnims = false;
		return gameObject;
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x0005BE5E File Offset: 0x0005A05E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x0005BE60 File Offset: 0x0005A060
	public void OnSpawn(GameObject inst)
	{
		GetBalloonWorkable component = inst.GetComponent<GetBalloonWorkable>();
		WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, component, null, true, new Action<Chore>(this.MakeNewBalloonChore), null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, true, true);
		workChore.AddPrecondition(BalloonStandConfig.HasNoBalloon, workChore);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		component.GetBalloonArtist().NextBalloonOverride();
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x0005BEDC File Offset: 0x0005A0DC
	private void MakeNewBalloonChore(Chore chore)
	{
		GetBalloonWorkable component = chore.target.GetComponent<GetBalloonWorkable>();
		WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, component, null, true, new Action<Chore>(this.MakeNewBalloonChore), null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, true, true);
		workChore.AddPrecondition(BalloonStandConfig.HasNoBalloon, workChore);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		component.GetBalloonArtist().NextBalloonOverride();
	}

	// Token: 0x04000A3E RID: 2622
	public static readonly string ID = "BalloonStand";

	// Token: 0x04000A3F RID: 2623
	private static Chore.Precondition HasNoBalloon = new Chore.Precondition
	{
		id = "HasNoBalloon",
		description = "__ Duplicant doesn't have a balloon already",
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !context.consumerState.gameObject.GetComponent<Effects>().HasEffect("HasBalloon");
		}
	};
}
