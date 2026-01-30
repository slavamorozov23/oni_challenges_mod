using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000E43 RID: 3651
public class HighEnergyParticleDirectionSideScreen : SideScreenContent
{
	// Token: 0x060073C6 RID: 29638 RVA: 0x002C3467 File Offset: 0x002C1667
	public override string GetTitle()
	{
		return UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.TITLE;
	}

	// Token: 0x060073C7 RID: 29639 RVA: 0x002C3474 File Offset: 0x002C1674
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.Buttons.Count; i++)
		{
			KButton button = this.Buttons[i];
			button.onClick += delegate()
			{
				int num = this.Buttons.IndexOf(button);
				if (this.activeButton != null)
				{
					this.activeButton.isInteractable = true;
				}
				button.isInteractable = false;
				this.activeButton = button;
				if (this.target != null)
				{
					this.target.Direction = EightDirectionUtil.AngleToDirection(num * 45);
					Game.Instance.ForceOverlayUpdate(true);
					this.Refresh();
				}
			};
		}
	}

	// Token: 0x060073C8 RID: 29640 RVA: 0x002C34D3 File Offset: 0x002C16D3
	public override int GetSideScreenSortOrder()
	{
		return 10;
	}

	// Token: 0x060073C9 RID: 29641 RVA: 0x002C34D8 File Offset: 0x002C16D8
	public override bool IsValidForTarget(GameObject target)
	{
		HighEnergyParticleRedirector component = target.GetComponent<HighEnergyParticleRedirector>();
		bool flag = component != null;
		if (flag)
		{
			flag = (flag && component.directionControllable);
		}
		bool flag2 = target.GetComponent<HighEnergyParticleSpawner>() != null || target.GetComponent<ManualHighEnergyParticleSpawner>() != null || target.GetComponent<DevHEPSpawner>() != null;
		return (flag || flag2) && target.GetComponent<IHighEnergyParticleDirection>() != null;
	}

	// Token: 0x060073CA RID: 29642 RVA: 0x002C3540 File Offset: 0x002C1740
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IHighEnergyParticleDirection>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain IHighEnergyParticleDirection component");
			return;
		}
		this.Refresh();
	}

	// Token: 0x060073CB RID: 29643 RVA: 0x002C357C File Offset: 0x002C177C
	private void Refresh()
	{
		int directionIndex = EightDirectionUtil.GetDirectionIndex(this.target.Direction);
		if (directionIndex >= 0 && directionIndex < this.Buttons.Count)
		{
			this.Buttons[directionIndex].SignalClick(KKeyCode.Mouse0);
		}
		else
		{
			if (this.activeButton)
			{
				this.activeButton.isInteractable = true;
			}
			this.activeButton = null;
		}
		this.directionLabel.SetText(string.Format(UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.SELECTED_DIRECTION, this.directionStrings[directionIndex]));
	}

	// Token: 0x0400500D RID: 20493
	private IHighEnergyParticleDirection target;

	// Token: 0x0400500E RID: 20494
	public List<KButton> Buttons;

	// Token: 0x0400500F RID: 20495
	private KButton activeButton;

	// Token: 0x04005010 RID: 20496
	public LocText directionLabel;

	// Token: 0x04005011 RID: 20497
	private string[] directionStrings = new string[]
	{
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_N,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_NW,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_W,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_SW,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_S,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_SE,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_E,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_NE
	};
}
