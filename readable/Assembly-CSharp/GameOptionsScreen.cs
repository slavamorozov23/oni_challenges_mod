using System;
using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;

// Token: 0x02000D12 RID: 3346
public class GameOptionsScreen : KModalButtonMenu
{
	// Token: 0x06006788 RID: 26504 RVA: 0x002709B3 File Offset: 0x0026EBB3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006789 RID: 26505 RVA: 0x002709BC File Offset: 0x0026EBBC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitConfiguration.Init();
		if (SaveGame.Instance != null)
		{
			this.saveConfiguration.ToggleDisabledContent(true);
			this.saveConfiguration.Init();
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			this.saveConfiguration.ToggleDisabledContent(false);
		}
		this.resetTutorialButton.onClick += this.OnTutorialReset;
		if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.controlsButton.gameObject.SetActive(false);
		}
		else
		{
			this.controlsButton.onClick += this.OnKeyBindings;
		}
		this.sandboxButton.onClick += this.OnUnlockSandboxMode;
		this.doneButton.onClick += this.Deactivate;
		this.closeButton.onClick += this.Deactivate;
		if (this.defaultToCloudSaveToggle != null)
		{
			this.RefreshCloudSaveToggle();
			this.defaultToCloudSaveToggle.GetComponentInChildren<KButton>().onClick += this.OnDefaultToCloudSaveToggle;
		}
		if (this.cloudSavesPanel != null)
		{
			this.cloudSavesPanel.SetActive(SaveLoader.GetCloudSavesAvailable());
		}
		this.cameraSpeedSlider.minValue = 1f;
		this.cameraSpeedSlider.maxValue = 20f;
		this.cameraSpeedSlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnCameraSpeedValueChanged(Mathf.FloorToInt(val));
		});
		this.cameraSpeedSlider.value = this.CameraSpeedToSlider(KPlayerPrefs.GetFloat("CameraSpeed"));
		this.RefreshCameraSliderLabel();
	}

	// Token: 0x0600678A RID: 26506 RVA: 0x00270B60 File Offset: 0x0026ED60
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (SaveGame.Instance != null)
		{
			this.savePanel.SetActive(true);
			this.saveConfiguration.Show(show);
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			this.savePanel.SetActive(false);
		}
		if (!KPlayerPrefs.HasKey("CameraSpeed"))
		{
			CameraController.SetDefaultCameraSpeed();
		}
	}

	// Token: 0x0600678B RID: 26507 RVA: 0x00270BC8 File Offset: 0x0026EDC8
	private float CameraSpeedToSlider(float prefsValue)
	{
		return prefsValue * 10f;
	}

	// Token: 0x0600678C RID: 26508 RVA: 0x00270BD1 File Offset: 0x0026EDD1
	private void OnCameraSpeedValueChanged(int sliderValue)
	{
		KPlayerPrefs.SetFloat("CameraSpeed", (float)sliderValue / 10f);
		this.RefreshCameraSliderLabel();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(75424175, null);
		}
	}

	// Token: 0x0600678D RID: 26509 RVA: 0x00270C08 File Offset: 0x0026EE08
	private void RefreshCameraSliderLabel()
	{
		this.cameraSpeedSliderLabel.text = string.Format(UI.FRONTEND.GAME_OPTIONS_SCREEN.CAMERA_SPEED_LABEL, (KPlayerPrefs.GetFloat("CameraSpeed") * 10f * 10f).ToString());
	}

	// Token: 0x0600678E RID: 26510 RVA: 0x00270C4D File Offset: 0x0026EE4D
	private void OnDefaultToCloudSaveToggle()
	{
		SaveLoader.SetCloudSavesDefault(!SaveLoader.GetCloudSavesDefault());
		this.RefreshCloudSaveToggle();
	}

	// Token: 0x0600678F RID: 26511 RVA: 0x00270C64 File Offset: 0x0026EE64
	private void RefreshCloudSaveToggle()
	{
		bool cloudSavesDefault = SaveLoader.GetCloudSavesDefault();
		this.defaultToCloudSaveToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(cloudSavesDefault);
	}

	// Token: 0x06006790 RID: 26512 RVA: 0x00270C97 File Offset: 0x0026EE97
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006791 RID: 26513 RVA: 0x00270CBC File Offset: 0x0026EEBC
	private void OnTutorialReset()
	{
		ConfirmDialogScreen component = base.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		component.PopupConfirmDialog(UI.FRONTEND.OPTIONS_SCREEN.RESET_TUTORIAL_WARNING, delegate
		{
			Tutorial.ResetHiddenTutorialMessages();
		}, delegate
		{
		}, null, null, null, null, null, null);
		component.Activate();
	}

	// Token: 0x06006792 RID: 26514 RVA: 0x00270D3C File Offset: 0x0026EF3C
	private void OnUnlockSandboxMode()
	{
		ConfirmDialogScreen component = base.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		string text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.UNLOCK_SANDBOX_WARNING;
		System.Action on_confirm = delegate()
		{
			SaveGame.Instance.sandboxEnabled = true;
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			this.Deactivate();
		};
		System.Action on_cancel = delegate()
		{
			string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
			string path = SaveGame.Instance.BaseName + UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND + ".sav";
			SaveLoader.Instance.Save(Path.Combine(savePrefixAndCreateFolder, path), false, false);
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			this.Deactivate();
		};
		string confirm_text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM;
		string cancel_text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM_SAVE_BACKUP;
		component.PopupConfirmDialog(text, on_confirm, on_cancel, UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CANCEL, delegate
		{
		}, null, confirm_text, cancel_text, null);
		component.Activate();
	}

	// Token: 0x06006793 RID: 26515 RVA: 0x00270DD3 File Offset: 0x0026EFD3
	private void OnKeyBindings()
	{
		base.ActivateChildScreen(this.inputBindingsScreenPrefab.gameObject);
	}

	// Token: 0x06006794 RID: 26516 RVA: 0x00270DE8 File Offset: 0x0026EFE8
	private void SetSandboxModeActive(bool active)
	{
		this.sandboxButton.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(active);
		this.sandboxButton.isInteractable = !active;
		this.sandboxButton.gameObject.GetComponentInParent<CanvasGroup>().alpha = (active ? 0.5f : 1f);
	}

	// Token: 0x040046E7 RID: 18151
	[SerializeField]
	private SaveConfigurationScreen saveConfiguration;

	// Token: 0x040046E8 RID: 18152
	[SerializeField]
	private UnitConfigurationScreen unitConfiguration;

	// Token: 0x040046E9 RID: 18153
	[SerializeField]
	private KButton resetTutorialButton;

	// Token: 0x040046EA RID: 18154
	[SerializeField]
	private KButton controlsButton;

	// Token: 0x040046EB RID: 18155
	[SerializeField]
	private KButton sandboxButton;

	// Token: 0x040046EC RID: 18156
	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	// Token: 0x040046ED RID: 18157
	[SerializeField]
	private KButton doneButton;

	// Token: 0x040046EE RID: 18158
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040046EF RID: 18159
	[SerializeField]
	private GameObject cloudSavesPanel;

	// Token: 0x040046F0 RID: 18160
	[SerializeField]
	private GameObject defaultToCloudSaveToggle;

	// Token: 0x040046F1 RID: 18161
	[SerializeField]
	private GameObject savePanel;

	// Token: 0x040046F2 RID: 18162
	[SerializeField]
	private InputBindingsScreen inputBindingsScreenPrefab;

	// Token: 0x040046F3 RID: 18163
	[SerializeField]
	private KSlider cameraSpeedSlider;

	// Token: 0x040046F4 RID: 18164
	[SerializeField]
	private LocText cameraSpeedSliderLabel;

	// Token: 0x040046F5 RID: 18165
	private const int cameraSliderNotchScale = 10;

	// Token: 0x040046F6 RID: 18166
	public const string PREFS_KEY_CAMERA_SPEED = "CameraSpeed";
}
