using System;
using TUNING;
using UnityEngine;

// Token: 0x0200074A RID: 1866
[AddComponentMenu("KMonoBehaviour/scripts/EggCracker")]
public class EggCracker : KMonoBehaviour
{
	// Token: 0x06002F28 RID: 12072 RVA: 0x001104DC File Offset: 0x0010E6DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.refinery.choreType = Db.Get().ChoreTypes.Cook;
		this.refinery.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		ComplexFabricatorWorkable complexFabricatorWorkable = this.workable;
		complexFabricatorWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(complexFabricatorWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
	}

	// Token: 0x06002F29 RID: 12073 RVA: 0x001105C2 File Offset: 0x0010E7C2
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		UnityEngine.Object.Destroy(this.tracker);
		this.tracker = null;
	}

	// Token: 0x06002F2A RID: 12074 RVA: 0x001105DC File Offset: 0x0010E7DC
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent e)
	{
		if (e == Workable.WorkableEvent.WorkStarted)
		{
			ComplexRecipe currentWorkingOrder = this.refinery.CurrentWorkingOrder;
			if (currentWorkingOrder != null)
			{
				ComplexRecipe.RecipeElement[] ingredients = currentWorkingOrder.ingredients;
				if (ingredients.Length != 0)
				{
					ComplexRecipe.RecipeElement recipeElement = ingredients[0];
					this.display_egg = this.refinery.buildStorage.FindFirst(recipeElement.material);
					this.PositionActiveEgg();
					return;
				}
			}
		}
		else if (e == Workable.WorkableEvent.WorkCompleted)
		{
			if (this.display_egg)
			{
				this.display_egg.GetComponent<KBatchedAnimController>().Play("hatching_pst", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
		}
		else if (e == Workable.WorkableEvent.WorkStopped)
		{
			UnityEngine.Object.Destroy(this.tracker);
			this.tracker = null;
			this.display_egg = null;
		}
	}

	// Token: 0x06002F2B RID: 12075 RVA: 0x00110684 File Offset: 0x0010E884
	private void PositionActiveEgg()
	{
		if (!this.display_egg)
		{
			return;
		}
		KBatchedAnimController component = this.display_egg.GetComponent<KBatchedAnimController>();
		component.enabled = true;
		component.SetSceneLayer(Grid.SceneLayer.BuildingUse);
		KSelectable component2 = this.display_egg.GetComponent<KSelectable>();
		if (component2 != null)
		{
			component2.enabled = true;
		}
		this.tracker = this.display_egg.AddComponent<KBatchedAnimTracker>();
		this.tracker.symbol = "snapto_egg";
	}

	// Token: 0x04001BFB RID: 7163
	[MyCmpReq]
	private ComplexFabricator refinery;

	// Token: 0x04001BFC RID: 7164
	[MyCmpReq]
	private ComplexFabricatorWorkable workable;

	// Token: 0x04001BFD RID: 7165
	private KBatchedAnimTracker tracker;

	// Token: 0x04001BFE RID: 7166
	private GameObject display_egg;
}
