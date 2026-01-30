using System;
using UnityEngine;

// Token: 0x02000B36 RID: 2870
[AddComponentMenu("KMonoBehaviour/scripts/ScreenPrefabs")]
public class ScreenPrefabs : KMonoBehaviour
{
	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x060054A3 RID: 21667 RVA: 0x001EE77D File Offset: 0x001EC97D
	// (set) Token: 0x060054A4 RID: 21668 RVA: 0x001EE784 File Offset: 0x001EC984
	public static ScreenPrefabs Instance { get; private set; }

	// Token: 0x060054A5 RID: 21669 RVA: 0x001EE78C File Offset: 0x001EC98C
	protected override void OnPrefabInit()
	{
		ScreenPrefabs.Instance = this;
	}

	// Token: 0x060054A6 RID: 21670 RVA: 0x001EE794 File Offset: 0x001EC994
	public void ConfirmDoAction(string message, System.Action action, Transform parent)
	{
		((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent.gameObject)).PopupConfirmDialog(message, action, delegate
		{
		}, null, null, null, null, null, null);
	}

	// Token: 0x04003917 RID: 14615
	public ControlsScreen ControlsScreen;

	// Token: 0x04003918 RID: 14616
	public Hud HudScreen;

	// Token: 0x04003919 RID: 14617
	public HoverTextScreen HoverTextScreen;

	// Token: 0x0400391A RID: 14618
	public OverlayScreen OverlayScreen;

	// Token: 0x0400391B RID: 14619
	public TileScreen TileScreen;

	// Token: 0x0400391C RID: 14620
	public SpeedControlScreen SpeedControlScreen;

	// Token: 0x0400391D RID: 14621
	public ManagementMenu ManagementMenu;

	// Token: 0x0400391E RID: 14622
	public ToolTipScreen ToolTipScreen;

	// Token: 0x0400391F RID: 14623
	public DebugPaintElementScreen DebugPaintElementScreen;

	// Token: 0x04003920 RID: 14624
	public UserMenuScreen UserMenuScreen;

	// Token: 0x04003921 RID: 14625
	public KButtonMenu OwnerScreen;

	// Token: 0x04003922 RID: 14626
	public KButtonMenu ButtonGrid;

	// Token: 0x04003923 RID: 14627
	public NameDisplayScreen NameDisplayScreen;

	// Token: 0x04003924 RID: 14628
	public ConfirmDialogScreen ConfirmDialogScreen;

	// Token: 0x04003925 RID: 14629
	public CustomizableDialogScreen CustomizableDialogScreen;

	// Token: 0x04003926 RID: 14630
	public SpriteListDialogScreen SpriteListDialogScreen;

	// Token: 0x04003927 RID: 14631
	public InfoDialogScreen InfoDialogScreen;

	// Token: 0x04003928 RID: 14632
	public StoryMessageScreen StoryMessageScreen;

	// Token: 0x04003929 RID: 14633
	public SubSpeciesInfoScreen SubSpeciesInfoScreen;

	// Token: 0x0400392A RID: 14634
	public EventInfoScreen eventInfoScreen;

	// Token: 0x0400392B RID: 14635
	public FileNameDialog FileNameDialog;

	// Token: 0x0400392C RID: 14636
	public TagFilterScreen TagFilterScreen;

	// Token: 0x0400392D RID: 14637
	public ResearchScreen ResearchScreen;

	// Token: 0x0400392E RID: 14638
	public MessageDialogFrame MessageDialogFrame;

	// Token: 0x0400392F RID: 14639
	public ResourceCategoryScreen ResourceCategoryScreen;

	// Token: 0x04003930 RID: 14640
	public ColonyDiagnosticScreen ColonyDiagnosticScreen;

	// Token: 0x04003931 RID: 14641
	public LanguageOptionsScreen languageOptionsScreen;

	// Token: 0x04003932 RID: 14642
	public LargeImpactorSequenceUIReticle largeImpactorSequenceReticlePrefab;

	// Token: 0x04003933 RID: 14643
	public ModsScreen modsMenu;

	// Token: 0x04003934 RID: 14644
	public RailModUploadScreen RailModUploadMenu;

	// Token: 0x04003935 RID: 14645
	public GameObject GameOverScreen;

	// Token: 0x04003936 RID: 14646
	public GameObject VictoryScreen;

	// Token: 0x04003937 RID: 14647
	public GameObject StatusItemIndicatorScreen;

	// Token: 0x04003938 RID: 14648
	public GameObject CollapsableContentPanel;

	// Token: 0x04003939 RID: 14649
	public GameObject DescriptionLabel;

	// Token: 0x0400393A RID: 14650
	public LoadingOverlay loadingOverlay;

	// Token: 0x0400393B RID: 14651
	public LoadScreen LoadScreen;

	// Token: 0x0400393C RID: 14652
	public InspectSaveScreen InspectSaveScreen;

	// Token: 0x0400393D RID: 14653
	public OptionsMenuScreen OptionsScreen;

	// Token: 0x0400393E RID: 14654
	public WorldGenScreen WorldGenScreen;

	// Token: 0x0400393F RID: 14655
	public ModeSelectScreen ModeSelectScreen;

	// Token: 0x04003940 RID: 14656
	public ColonyDestinationSelectScreen ColonyDestinationSelectScreen;

	// Token: 0x04003941 RID: 14657
	public RetiredColonyInfoScreen RetiredColonyInfoScreen;

	// Token: 0x04003942 RID: 14658
	public VideoScreen VideoScreen;

	// Token: 0x04003943 RID: 14659
	public ComicViewer ComicViewer;

	// Token: 0x04003944 RID: 14660
	public GameObject OldVersionWarningScreen;

	// Token: 0x04003945 RID: 14661
	public GameObject DLCBetaWarningScreen;

	// Token: 0x04003946 RID: 14662
	[Header("Klei Items")]
	public GameObject KleiItemDropScreen;

	// Token: 0x04003947 RID: 14663
	public GameObject LockerMenuScreen;

	// Token: 0x04003948 RID: 14664
	public GameObject LockerNavigator;

	// Token: 0x04003949 RID: 14665
	[Header("Main Menu")]
	public GameObject MainMenuForVanilla;

	// Token: 0x0400394A RID: 14666
	public GameObject MainMenuForSpacedOut;

	// Token: 0x0400394B RID: 14667
	public GameObject MainMenuIntroShort;

	// Token: 0x0400394C RID: 14668
	public GameObject MainMenuHealthyGameMessage;
}
