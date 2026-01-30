using System;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DF8 RID: 3576
[AddComponentMenu("KMonoBehaviour/scripts/RocketThrustWidget")]
public class RocketThrustWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06007111 RID: 28945 RVA: 0x002B252E File Offset: 0x002B072E
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x06007112 RID: 28946 RVA: 0x002B2530 File Offset: 0x002B0730
	public void Draw(CommandModule commandModule)
	{
		if (this.rectTransform == null)
		{
			this.rectTransform = this.graphBar.gameObject.GetComponent<RectTransform>();
		}
		this.commandModule = commandModule;
		this.totalWidth = this.rectTransform.rect.width;
		this.UpdateGraphDotPos(commandModule);
	}

	// Token: 0x06007113 RID: 28947 RVA: 0x002B2588 File Offset: 0x002B0788
	private void UpdateGraphDotPos(CommandModule rocket)
	{
		this.totalWidth = this.rectTransform.rect.width;
		float num = Mathf.Lerp(0f, this.totalWidth, rocket.rocketStats.GetTotalMass() / this.maxMass);
		num = Mathf.Clamp(num, 0f, this.totalWidth);
		this.graphDot.rectTransform.SetLocalPosition(new Vector3(num, 0f, 0f));
		this.graphDotText.text = "-" + Util.FormatWholeNumber(rocket.rocketStats.GetTotalThrust() - rocket.rocketStats.GetRocketMaxDistance()) + "km";
	}

	// Token: 0x06007114 RID: 28948 RVA: 0x002B263C File Offset: 0x002B083C
	private void Update()
	{
		if (this.mouseOver)
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = this.graphBar.gameObject.GetComponent<RectTransform>();
			}
			Vector3 position = this.rectTransform.GetPosition();
			Vector2 size = this.rectTransform.rect.size;
			float num = KInputManager.GetMousePos().x - position.x + size.x / 2f;
			num = Mathf.Clamp(num, 0f, this.totalWidth);
			this.hoverMarker.rectTransform.SetLocalPosition(new Vector3(num, 0f, 0f));
			float num2 = Mathf.Lerp(0f, this.maxMass, num / this.totalWidth);
			float totalThrust = this.commandModule.rocketStats.GetTotalThrust();
			float rocketMaxDistance = this.commandModule.rocketStats.GetRocketMaxDistance();
			this.hoverTooltip.SetSimpleTooltip(string.Concat(new string[]
			{
				UI.STARMAP.ROCKETWEIGHT.MASS,
				GameUtil.GetFormattedMass(num2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"),
				"\n",
				UI.STARMAP.ROCKETWEIGHT.MASSPENALTY,
				Util.FormatWholeNumber(ROCKETRY.CalculateMassWithPenalty(num2)),
				UI.UNITSUFFIXES.DISTANCE.KILOMETER,
				"\n\n",
				UI.STARMAP.ROCKETWEIGHT.CURRENTMASS,
				GameUtil.GetFormattedMass(this.commandModule.rocketStats.GetTotalMass(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"),
				"\n",
				UI.STARMAP.ROCKETWEIGHT.CURRENTMASSPENALTY,
				Util.FormatWholeNumber(totalThrust - rocketMaxDistance),
				UI.UNITSUFFIXES.DISTANCE.KILOMETER
			}));
		}
	}

	// Token: 0x06007115 RID: 28949 RVA: 0x002B27F5 File Offset: 0x002B09F5
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.mouseOver = true;
		this.hoverMarker.SetAlpha(1f);
	}

	// Token: 0x06007116 RID: 28950 RVA: 0x002B280E File Offset: 0x002B0A0E
	public void OnPointerExit(PointerEventData eventData)
	{
		this.mouseOver = false;
		this.hoverMarker.SetAlpha(0f);
	}

	// Token: 0x04004E03 RID: 19971
	public Image graphBar;

	// Token: 0x04004E04 RID: 19972
	public Image graphDot;

	// Token: 0x04004E05 RID: 19973
	public LocText graphDotText;

	// Token: 0x04004E06 RID: 19974
	public Image hoverMarker;

	// Token: 0x04004E07 RID: 19975
	public ToolTip hoverTooltip;

	// Token: 0x04004E08 RID: 19976
	public RectTransform markersContainer;

	// Token: 0x04004E09 RID: 19977
	public Image markerTemplate;

	// Token: 0x04004E0A RID: 19978
	private RectTransform rectTransform;

	// Token: 0x04004E0B RID: 19979
	private float maxMass = 20000f;

	// Token: 0x04004E0C RID: 19980
	private float totalWidth = 5f;

	// Token: 0x04004E0D RID: 19981
	private bool mouseOver;

	// Token: 0x04004E0E RID: 19982
	public CommandModule commandModule;
}
