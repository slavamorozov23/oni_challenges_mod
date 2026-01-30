using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000EAD RID: 3757
public class TextLinkHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x06007875 RID: 30837 RVA: 0x002E4D34 File Offset: 0x002E2F34
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (!this.text.AllowLinks)
		{
			return;
		}
		int num = TMP_TextUtilities.FindIntersectingLink(this.text, KInputManager.GetMousePos(), null);
		if (num != -1)
		{
			string text = CodexCache.FormatLinkID(this.text.textInfo.linkInfo[num].GetLinkID());
			if (this.overrideLinkAction == null || this.overrideLinkAction(text))
			{
				if (!CodexCache.entries.ContainsKey(text))
				{
					SubEntry subEntry = CodexCache.FindSubEntry(text);
					if (subEntry == null || subEntry.disabled)
					{
						text = "PAGENOTFOUND";
					}
				}
				else if (CodexCache.entries[text].disabled)
				{
					text = "PAGENOTFOUND";
				}
				if (!ManagementMenu.Instance.codexScreen.gameObject.activeInHierarchy)
				{
					ManagementMenu.Instance.ToggleCodex();
				}
				ManagementMenu.Instance.codexScreen.ChangeArticle(text, true, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			}
		}
	}

	// Token: 0x06007876 RID: 30838 RVA: 0x002E4E22 File Offset: 0x002E3022
	private void Update()
	{
		this.CheckMouseOver();
		if (TextLinkHandler.hoveredText == this && this.text.AllowLinks)
		{
			PlayerController.Instance.ActiveTool.SetLinkCursor(this.hoverLink);
		}
	}

	// Token: 0x06007877 RID: 30839 RVA: 0x002E4E59 File Offset: 0x002E3059
	private void OnEnable()
	{
		this.CheckMouseOver();
	}

	// Token: 0x06007878 RID: 30840 RVA: 0x002E4E61 File Offset: 0x002E3061
	private void OnDisable()
	{
		this.ClearState();
	}

	// Token: 0x06007879 RID: 30841 RVA: 0x002E4E69 File Offset: 0x002E3069
	private void Awake()
	{
		this.text = base.GetComponent<LocText>();
		if (this.text.AllowLinks && !this.text.raycastTarget)
		{
			this.text.raycastTarget = true;
		}
	}

	// Token: 0x0600787A RID: 30842 RVA: 0x002E4E9D File Offset: 0x002E309D
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetMouseOver();
	}

	// Token: 0x0600787B RID: 30843 RVA: 0x002E4EA5 File Offset: 0x002E30A5
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ClearState();
	}

	// Token: 0x0600787C RID: 30844 RVA: 0x002E4EB0 File Offset: 0x002E30B0
	private void ClearState()
	{
		if (this == null || this.Equals(null))
		{
			return;
		}
		if (TextLinkHandler.hoveredText == this)
		{
			if (this.hoverLink && PlayerController.Instance != null && PlayerController.Instance.ActiveTool != null)
			{
				PlayerController.Instance.ActiveTool.SetLinkCursor(false);
			}
			TextLinkHandler.hoveredText = null;
			this.hoverLink = false;
		}
	}

	// Token: 0x0600787D RID: 30845 RVA: 0x002E4F24 File Offset: 0x002E3124
	public void CheckMouseOver()
	{
		if (this.text == null)
		{
			return;
		}
		if (TMP_TextUtilities.FindIntersectingLink(this.text, KInputManager.GetMousePos(), null) != -1)
		{
			this.SetMouseOver();
			this.hoverLink = true;
			return;
		}
		if (TextLinkHandler.hoveredText == this)
		{
			this.hoverLink = false;
		}
	}

	// Token: 0x0600787E RID: 30846 RVA: 0x002E4F76 File Offset: 0x002E3176
	private void SetMouseOver()
	{
		if (TextLinkHandler.hoveredText != null && TextLinkHandler.hoveredText != this)
		{
			TextLinkHandler.hoveredText.hoverLink = false;
		}
		TextLinkHandler.hoveredText = this;
	}

	// Token: 0x040053E9 RID: 21481
	private static TextLinkHandler hoveredText;

	// Token: 0x040053EA RID: 21482
	[MyCmpGet]
	private LocText text;

	// Token: 0x040053EB RID: 21483
	private bool hoverLink;

	// Token: 0x040053EC RID: 21484
	public Func<string, bool> overrideLinkAction;
}
