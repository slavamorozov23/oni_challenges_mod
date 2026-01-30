using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000CDE RID: 3294
public class ColorSet : ScriptableObject
{
	// Token: 0x060065C8 RID: 26056 RVA: 0x0026569C File Offset: 0x0026389C
	private void Init()
	{
		if (this.namedLookup == null)
		{
			this.namedLookup = new Dictionary<string, Color32>();
			foreach (FieldInfo fieldInfo in typeof(ColorSet).GetFields())
			{
				if (fieldInfo.FieldType == typeof(Color32))
				{
					this.namedLookup[fieldInfo.Name] = (Color32)fieldInfo.GetValue(this);
				}
			}
		}
	}

	// Token: 0x060065C9 RID: 26057 RVA: 0x00265712 File Offset: 0x00263912
	public Color32 GetColorByName(string name)
	{
		this.Init();
		return this.namedLookup[name];
	}

	// Token: 0x060065CA RID: 26058 RVA: 0x00265726 File Offset: 0x00263926
	public void RefreshLookup()
	{
		this.namedLookup = null;
		this.Init();
	}

	// Token: 0x060065CB RID: 26059 RVA: 0x00265735 File Offset: 0x00263935
	public bool IsDefaultColorSet()
	{
		return Array.IndexOf<ColorSet>(GlobalAssets.Instance.colorSetOptions, this) == 0;
	}

	// Token: 0x04004509 RID: 17673
	public string settingName;

	// Token: 0x0400450A RID: 17674
	[Header("Logic")]
	public Color32 logicOn;

	// Token: 0x0400450B RID: 17675
	public Color32 logicOff;

	// Token: 0x0400450C RID: 17676
	public Color32 logicDisconnected;

	// Token: 0x0400450D RID: 17677
	public Color32 logicOnText;

	// Token: 0x0400450E RID: 17678
	public Color32 logicOffText;

	// Token: 0x0400450F RID: 17679
	public Color32 logicOnSidescreen;

	// Token: 0x04004510 RID: 17680
	public Color32 logicOffSidescreen;

	// Token: 0x04004511 RID: 17681
	[Header("Decor")]
	public Color32 decorPositive;

	// Token: 0x04004512 RID: 17682
	public Color32 decorNegative;

	// Token: 0x04004513 RID: 17683
	public Color32 decorBaseline;

	// Token: 0x04004514 RID: 17684
	public Color32 decorHighlightPositive;

	// Token: 0x04004515 RID: 17685
	public Color32 decorHighlightNegative;

	// Token: 0x04004516 RID: 17686
	[Header("Crop Overlay")]
	public Color32 cropHalted;

	// Token: 0x04004517 RID: 17687
	public Color32 cropGrowing;

	// Token: 0x04004518 RID: 17688
	public Color32 cropGrown;

	// Token: 0x04004519 RID: 17689
	[Header("Harvest Overlay")]
	public Color32 harvestEnabled;

	// Token: 0x0400451A RID: 17690
	public Color32 harvestDisabled;

	// Token: 0x0400451B RID: 17691
	[Header("Gameplay Events")]
	public Color32 eventPositive;

	// Token: 0x0400451C RID: 17692
	public Color32 eventNegative;

	// Token: 0x0400451D RID: 17693
	public Color32 eventNeutral;

	// Token: 0x0400451E RID: 17694
	[Header("Notifications")]
	public Color32 NotificationNormal;

	// Token: 0x0400451F RID: 17695
	public Color32 NotificationNormalBG;

	// Token: 0x04004520 RID: 17696
	public Color32 NotificationBad;

	// Token: 0x04004521 RID: 17697
	public Color32 NotificationBadBG;

	// Token: 0x04004522 RID: 17698
	public Color32 NotificationEvent;

	// Token: 0x04004523 RID: 17699
	public Color32 NotificationEventBG;

	// Token: 0x04004524 RID: 17700
	public Color32 NotificationMessage;

	// Token: 0x04004525 RID: 17701
	public Color32 NotificationMessageBG;

	// Token: 0x04004526 RID: 17702
	public Color32 NotificationMessageImportant;

	// Token: 0x04004527 RID: 17703
	public Color32 NotificationMessageImportantBG;

	// Token: 0x04004528 RID: 17704
	public Color32 NotificationTutorial;

	// Token: 0x04004529 RID: 17705
	public Color32 NotificationTutorialBG;

	// Token: 0x0400452A RID: 17706
	[Header("PrioritiesScreen")]
	public Color32 PrioritiesNeutralColor;

	// Token: 0x0400452B RID: 17707
	public Color32 PrioritiesLowColor;

	// Token: 0x0400452C RID: 17708
	public Color32 PrioritiesHighColor;

	// Token: 0x0400452D RID: 17709
	[Header("Info Screen Status Items")]
	public Color32 statusItemBad;

	// Token: 0x0400452E RID: 17710
	public Color32 statusItemEvent;

	// Token: 0x0400452F RID: 17711
	public Color32 statusItemMessageImportant;

	// Token: 0x04004530 RID: 17712
	[Header("Germ Overlay")]
	public Color32 germFoodPoisoning;

	// Token: 0x04004531 RID: 17713
	public Color32 germPollenGerms;

	// Token: 0x04004532 RID: 17714
	public Color32 germSlimeLung;

	// Token: 0x04004533 RID: 17715
	public Color32 germZombieSpores;

	// Token: 0x04004534 RID: 17716
	public Color32 germRadiationSickness;

	// Token: 0x04004535 RID: 17717
	[Header("Room Overlay")]
	public Color32 roomNone;

	// Token: 0x04004536 RID: 17718
	public Color32 roomFood;

	// Token: 0x04004537 RID: 17719
	public Color32 roomSleep;

	// Token: 0x04004538 RID: 17720
	public Color32 roomRecreation;

	// Token: 0x04004539 RID: 17721
	public Color32 roomBathroom;

	// Token: 0x0400453A RID: 17722
	public Color32 roomHospital;

	// Token: 0x0400453B RID: 17723
	public Color32 roomIndustrial;

	// Token: 0x0400453C RID: 17724
	public Color32 roomAgricultural;

	// Token: 0x0400453D RID: 17725
	public Color32 roomScience;

	// Token: 0x0400453E RID: 17726
	public Color32 roomBionic;

	// Token: 0x0400453F RID: 17727
	public Color32 roomPark;

	// Token: 0x04004540 RID: 17728
	[Header("Power Overlay")]
	public Color32 powerConsumer;

	// Token: 0x04004541 RID: 17729
	public Color32 powerGenerator;

	// Token: 0x04004542 RID: 17730
	public Color32 powerBuildingDisabled;

	// Token: 0x04004543 RID: 17731
	public Color32 powerCircuitUnpowered;

	// Token: 0x04004544 RID: 17732
	public Color32 powerCircuitSafe;

	// Token: 0x04004545 RID: 17733
	public Color32 powerCircuitStraining;

	// Token: 0x04004546 RID: 17734
	public Color32 powerCircuitOverloading;

	// Token: 0x04004547 RID: 17735
	[Header("Light Overlay")]
	public Color32 lightOverlay;

	// Token: 0x04004548 RID: 17736
	[Header("Conduit Overlay")]
	public Color32 conduitNormal;

	// Token: 0x04004549 RID: 17737
	public Color32 conduitInsulated;

	// Token: 0x0400454A RID: 17738
	public Color32 conduitRadiant;

	// Token: 0x0400454B RID: 17739
	[Header("Temperature Overlay")]
	public Color32 temperatureThreshold0;

	// Token: 0x0400454C RID: 17740
	public Color32 temperatureThreshold1;

	// Token: 0x0400454D RID: 17741
	public Color32 temperatureThreshold2;

	// Token: 0x0400454E RID: 17742
	public Color32 temperatureThreshold3;

	// Token: 0x0400454F RID: 17743
	public Color32 temperatureThreshold4;

	// Token: 0x04004550 RID: 17744
	public Color32 temperatureThreshold5;

	// Token: 0x04004551 RID: 17745
	public Color32 temperatureThreshold6;

	// Token: 0x04004552 RID: 17746
	public Color32 temperatureThreshold7;

	// Token: 0x04004553 RID: 17747
	public Color32 heatflowThreshold0;

	// Token: 0x04004554 RID: 17748
	public Color32 heatflowThreshold1;

	// Token: 0x04004555 RID: 17749
	public Color32 heatflowThreshold2;

	// Token: 0x04004556 RID: 17750
	private Dictionary<string, Color32> namedLookup;
}
