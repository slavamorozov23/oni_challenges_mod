using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000E54 RID: 3668
public class MonumentSideScreen : SideScreenContent
{
	// Token: 0x06007454 RID: 29780 RVA: 0x002C69C8 File Offset: 0x002C4BC8
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MonumentPart>() != null;
	}

	// Token: 0x06007455 RID: 29781 RVA: 0x002C69D8 File Offset: 0x002C4BD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.debugVictoryButton.onClick += delegate()
		{
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Thriving.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
			GameScheduler.Instance.Schedule("ForceCheckAchievements", 0.1f, delegate(object data)
			{
				Game.Instance.Trigger(395452326, null);
			}, null, null);
		};
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPartResource.Part.Top);
		this.flipButton.onClick += delegate()
		{
			this.target.GetComponent<Rotatable>().Rotate();
		};
	}

	// Token: 0x06007456 RID: 29782 RVA: 0x002C6A54 File Offset: 0x002C4C54
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target = target.GetComponent<MonumentPart>();
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPartResource.Part.Top);
		this.GenerateStateButtons();
	}

	// Token: 0x06007457 RID: 29783 RVA: 0x002C6AA4 File Offset: 0x002C4CA4
	public void GenerateStateButtons()
	{
		for (int i = this.buttons.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.buttons[i]);
		}
		this.buttons.Clear();
		using (List<MonumentPartResource>.Enumerator enumerator = Db.GetMonumentParts().GetParts(this.target.part).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MonumentPartResource state = enumerator.Current;
				GameObject gameObject = Util.KInstantiateUI(this.stateButtonPrefab, this.buttonContainer.gameObject, true);
				string state2 = state.State;
				string symbolName = state.SymbolName;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.target.SetState(state.Id);
				};
				this.buttons.Add(gameObject);
				gameObject.GetComponent<KButton>().fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(state.AnimFile, state2, false, symbolName);
			}
		}
	}

	// Token: 0x04005070 RID: 20592
	private MonumentPart target;

	// Token: 0x04005071 RID: 20593
	public KButton debugVictoryButton;

	// Token: 0x04005072 RID: 20594
	public KButton flipButton;

	// Token: 0x04005073 RID: 20595
	public GameObject stateButtonPrefab;

	// Token: 0x04005074 RID: 20596
	private List<GameObject> buttons = new List<GameObject>();

	// Token: 0x04005075 RID: 20597
	[SerializeField]
	private RectTransform buttonContainer;
}
