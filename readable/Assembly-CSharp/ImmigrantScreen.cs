using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02000C5E RID: 3166
public class ImmigrantScreen : CharacterSelectionController
{
	// Token: 0x06006022 RID: 24610 RVA: 0x00234304 File Offset: 0x00232504
	public static void DestroyInstance()
	{
		ImmigrantScreen.instance = null;
	}

	// Token: 0x17000704 RID: 1796
	// (get) Token: 0x06006023 RID: 24611 RVA: 0x0023430C File Offset: 0x0023250C
	public Telepad Telepad
	{
		get
		{
			return this.telepad;
		}
	}

	// Token: 0x06006024 RID: 24612 RVA: 0x00234314 File Offset: 0x00232514
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006025 RID: 24613 RVA: 0x0023431C File Offset: 0x0023251C
	protected override void OnSpawn()
	{
		this.activateOnSpawn = false;
		base.ConsumeMouseScroll = false;
		base.OnSpawn();
		base.IsStarterMinion = false;
		this.rejectButton.onClick += this.OnRejectAll;
		this.confirmRejectionBtn.onClick += this.OnRejectionConfirmed;
		this.cancelRejectionBtn.onClick += this.OnRejectionCancelled;
		ImmigrantScreen.instance = this;
		this.title.text = UI.IMMIGRANTSCREEN.IMMIGRANTSCREENTITLE;
		this.proceedButton.GetComponentInChildren<LocText>().text = UI.IMMIGRANTSCREEN.PROCEEDBUTTON;
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.Show(false);
	}

	// Token: 0x06006026 RID: 24614 RVA: 0x002343DC File Offset: 0x002325DC
	protected override void OnShow(bool show)
	{
		if (show)
		{
			KFMOD.PlayUISound(GlobalAssets.GetSound("Dialog_Popup", false));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot);
			MusicManager.instance.PlaySong("Music_SelectDuplicant", false);
			this.hasShown = true;
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.StopSong("Music_SelectDuplicant", true, STOP_MODE.ALLOWFADEOUT);
			}
			if (Immigration.Instance.ImmigrantsAvailable && this.hasShown)
			{
				AudioMixer.instance.Start(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot);
			}
		}
		base.OnShow(show);
	}

	// Token: 0x06006027 RID: 24615 RVA: 0x00234492 File Offset: 0x00232692
	public void DebugShuffleOptions()
	{
		this.OnRejectionConfirmed();
		Immigration.Instance.timeBeforeSpawn = 0f;
	}

	// Token: 0x06006028 RID: 24616 RVA: 0x002344A9 File Offset: 0x002326A9
	public override void OnPressBack()
	{
		if (this.rejectConfirmationScreen.activeSelf)
		{
			this.OnRejectionCancelled();
			return;
		}
		base.OnPressBack();
	}

	// Token: 0x06006029 RID: 24617 RVA: 0x002344C5 File Offset: 0x002326C5
	public override void Deactivate()
	{
		this.Show(false);
	}

	// Token: 0x0600602A RID: 24618 RVA: 0x002344CE File Offset: 0x002326CE
	public static void InitializeImmigrantScreen(Telepad telepad)
	{
		ImmigrantScreen.instance.Initialize(telepad);
		ImmigrantScreen.instance.Show(true);
	}

	// Token: 0x0600602B RID: 24619 RVA: 0x002344E8 File Offset: 0x002326E8
	private void Initialize(Telepad telepad)
	{
		this.InitializeContainers();
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.SetReshufflingState(false);
			}
		}
		this.telepad = telepad;
	}

	// Token: 0x0600602C RID: 24620 RVA: 0x00234558 File Offset: 0x00232758
	protected override void OnProceed()
	{
		this.telepad.OnAcceptDelivery(this.selectedDeliverables[0]);
		this.Show(false);
		this.containers.ForEach(delegate(ITelepadDeliverableContainer cc)
		{
			UnityEngine.Object.Destroy(cc.GetGameObject());
		});
		this.containers.Clear();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot, STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.PlaySong("Stinger_NewDuplicant", false);
	}

	// Token: 0x0600602D RID: 24621 RVA: 0x002345F4 File Offset: 0x002327F4
	private void OnRejectAll()
	{
		this.rejectConfirmationScreen.transform.SetAsLastSibling();
		this.rejectConfirmationScreen.SetActive(true);
	}

	// Token: 0x0600602E RID: 24622 RVA: 0x00234612 File Offset: 0x00232812
	private void OnRejectionCancelled()
	{
		this.rejectConfirmationScreen.SetActive(false);
	}

	// Token: 0x0600602F RID: 24623 RVA: 0x00234620 File Offset: 0x00232820
	private void OnRejectionConfirmed()
	{
		this.ClearRejectedShuffleState();
		this.rejectConfirmationScreen.SetActive(false);
		this.Show(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot, STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06006030 RID: 24624 RVA: 0x00234674 File Offset: 0x00232874
	public void ClearRejectedShuffleState()
	{
		if (this.telepad == null)
		{
			return;
		}
		this.telepad.RejectAll();
		this.containers.ForEach(delegate(ITelepadDeliverableContainer cc)
		{
			UnityEngine.Object.Destroy(cc.GetGameObject());
		});
		this.containers.Clear();
	}

	// Token: 0x04004042 RID: 16450
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004043 RID: 16451
	[SerializeField]
	private KButton rejectButton;

	// Token: 0x04004044 RID: 16452
	[SerializeField]
	private LocText title;

	// Token: 0x04004045 RID: 16453
	[SerializeField]
	private GameObject rejectConfirmationScreen;

	// Token: 0x04004046 RID: 16454
	[SerializeField]
	private KButton confirmRejectionBtn;

	// Token: 0x04004047 RID: 16455
	[SerializeField]
	private KButton cancelRejectionBtn;

	// Token: 0x04004048 RID: 16456
	public static ImmigrantScreen instance;

	// Token: 0x04004049 RID: 16457
	private Telepad telepad;

	// Token: 0x0400404A RID: 16458
	private bool hasShown;
}
