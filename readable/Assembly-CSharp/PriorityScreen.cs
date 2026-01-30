using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02000DE3 RID: 3555
public class PriorityScreen : KScreen
{
	// Token: 0x06006FC3 RID: 28611 RVA: 0x002A71F0 File Offset: 0x002A53F0
	public void InstantiateButtons(Action<PrioritySetting> on_click, bool playSelectionSound = true)
	{
		this.onClick = on_click;
		for (int i = 1; i <= 9; i++)
		{
			int num = i;
			PriorityButton priorityButton = global::Util.KInstantiateUI<PriorityButton>(this.buttonPrefab_basic.gameObject, this.buttonPrefab_basic.transform.parent.gameObject, false);
			this.buttons_basic.Add(priorityButton);
			priorityButton.playSelectionSound = playSelectionSound;
			priorityButton.onClick = this.onClick;
			priorityButton.text.text = num.ToString();
			priorityButton.priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, num);
			priorityButton.tooltip.SetSimpleTooltip(string.Format(UI.PRIORITYSCREEN.BASIC, num));
		}
		this.buttonPrefab_basic.gameObject.SetActive(false);
		this.button_emergency.playSelectionSound = playSelectionSound;
		this.button_emergency.onClick = this.onClick;
		this.button_emergency.priority = new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1);
		this.button_emergency.tooltip.SetSimpleTooltip(UI.PRIORITYSCREEN.TOP_PRIORITY);
		this.button_toggleHigh.gameObject.SetActive(false);
		this.PriorityMenuContainer.SetActive(true);
		this.button_priorityMenu.gameObject.SetActive(true);
		this.button_priorityMenu.onClick += this.PriorityButtonClicked;
		this.button_priorityMenu.GetComponent<ToolTip>().SetSimpleTooltip(UI.PRIORITYSCREEN.OPEN_JOBS_SCREEN);
		this.diagram.SetActive(false);
		this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
	}

	// Token: 0x06006FC4 RID: 28612 RVA: 0x002A7371 File Offset: 0x002A5571
	private void OnClick(PrioritySetting priority)
	{
		if (this.onClick != null)
		{
			this.onClick(priority);
		}
	}

	// Token: 0x06006FC5 RID: 28613 RVA: 0x002A7387 File Offset: 0x002A5587
	public void ShowDiagram(bool show)
	{
		this.diagram.SetActive(show);
	}

	// Token: 0x06006FC6 RID: 28614 RVA: 0x002A7395 File Offset: 0x002A5595
	public void ResetPriority()
	{
		this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
	}

	// Token: 0x06006FC7 RID: 28615 RVA: 0x002A73A5 File Offset: 0x002A55A5
	public void PriorityButtonClicked()
	{
		ManagementMenu.Instance.TogglePriorities();
	}

	// Token: 0x06006FC8 RID: 28616 RVA: 0x002A73B4 File Offset: 0x002A55B4
	private void RefreshButton(PriorityButton b, PrioritySetting priority, bool play_sound)
	{
		if (b.priority == priority)
		{
			b.toggle.Select();
			b.toggle.isOn = true;
			if (play_sound)
			{
				b.toggle.soundPlayer.Play(0);
				return;
			}
		}
		else
		{
			b.toggle.isOn = false;
		}
	}

	// Token: 0x06006FC9 RID: 28617 RVA: 0x002A7408 File Offset: 0x002A5608
	public void SetScreenPriority(PrioritySetting priority, bool play_sound = false)
	{
		if (this.lastSelectedPriority == priority)
		{
			return;
		}
		this.lastSelectedPriority = priority;
		if (priority.priority_class == PriorityScreen.PriorityClass.high)
		{
			this.button_toggleHigh.isOn = true;
		}
		else if (priority.priority_class == PriorityScreen.PriorityClass.basic)
		{
			this.button_toggleHigh.isOn = false;
		}
		for (int i = 0; i < this.buttons_basic.Count; i++)
		{
			this.buttons_basic[i].priority = new PrioritySetting(this.button_toggleHigh.isOn ? PriorityScreen.PriorityClass.high : PriorityScreen.PriorityClass.basic, i + 1);
			this.buttons_basic[i].tooltip.SetSimpleTooltip(string.Format(this.button_toggleHigh.isOn ? UI.PRIORITYSCREEN.HIGH : UI.PRIORITYSCREEN.BASIC, i + 1));
			this.RefreshButton(this.buttons_basic[i], this.lastSelectedPriority, play_sound);
		}
		this.RefreshButton(this.button_emergency, this.lastSelectedPriority, play_sound);
	}

	// Token: 0x06006FCA RID: 28618 RVA: 0x002A7509 File Offset: 0x002A5709
	public PrioritySetting GetLastSelectedPriority()
	{
		return this.lastSelectedPriority;
	}

	// Token: 0x06006FCB RID: 28619 RVA: 0x002A7514 File Offset: 0x002A5714
	public static void PlayPriorityConfirmSound(PrioritySetting priority)
	{
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Priority_Tool_Confirm", false), Vector3.zero, 1f);
		if (instance.isValid())
		{
			float num = 0f;
			if (priority.priority_class >= PriorityScreen.PriorityClass.high)
			{
				num += 10f;
			}
			if (priority.priority_class >= PriorityScreen.PriorityClass.topPriority)
			{
				num += 0f;
			}
			num += (float)priority.priority_value;
			instance.setParameterByName("priority", num, false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x04004CA0 RID: 19616
	[SerializeField]
	protected PriorityButton buttonPrefab_basic;

	// Token: 0x04004CA1 RID: 19617
	[SerializeField]
	protected GameObject EmergencyContainer;

	// Token: 0x04004CA2 RID: 19618
	[SerializeField]
	protected PriorityButton button_emergency;

	// Token: 0x04004CA3 RID: 19619
	[SerializeField]
	protected GameObject PriorityMenuContainer;

	// Token: 0x04004CA4 RID: 19620
	[SerializeField]
	protected KButton button_priorityMenu;

	// Token: 0x04004CA5 RID: 19621
	[SerializeField]
	protected KToggle button_toggleHigh;

	// Token: 0x04004CA6 RID: 19622
	[SerializeField]
	protected GameObject diagram;

	// Token: 0x04004CA7 RID: 19623
	protected List<PriorityButton> buttons_basic = new List<PriorityButton>();

	// Token: 0x04004CA8 RID: 19624
	protected List<PriorityButton> buttons_emergency = new List<PriorityButton>();

	// Token: 0x04004CA9 RID: 19625
	private PrioritySetting priority;

	// Token: 0x04004CAA RID: 19626
	private PrioritySetting lastSelectedPriority = new PrioritySetting(PriorityScreen.PriorityClass.basic, -1);

	// Token: 0x04004CAB RID: 19627
	private Action<PrioritySetting> onClick;

	// Token: 0x0200204F RID: 8271
	public enum PriorityClass
	{
		// Token: 0x040095A5 RID: 38309
		idle = -1,
		// Token: 0x040095A6 RID: 38310
		basic,
		// Token: 0x040095A7 RID: 38311
		high,
		// Token: 0x040095A8 RID: 38312
		personalNeeds,
		// Token: 0x040095A9 RID: 38313
		topPriority,
		// Token: 0x040095AA RID: 38314
		compulsory
	}
}
