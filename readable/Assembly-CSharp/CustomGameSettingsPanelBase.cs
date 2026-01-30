using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CEB RID: 3307
public abstract class CustomGameSettingsPanelBase : MonoBehaviour
{
	// Token: 0x06006617 RID: 26135 RVA: 0x00266D2B File Offset: 0x00264F2B
	public virtual void Init()
	{
	}

	// Token: 0x06006618 RID: 26136 RVA: 0x00266D2D File Offset: 0x00264F2D
	public virtual void Uninit()
	{
	}

	// Token: 0x06006619 RID: 26137 RVA: 0x00266D2F File Offset: 0x00264F2F
	private void OnEnable()
	{
		this.isDirty = true;
	}

	// Token: 0x0600661A RID: 26138 RVA: 0x00266D38 File Offset: 0x00264F38
	private void Update()
	{
		if (this.isDirty)
		{
			this.isDirty = false;
			this.Refresh();
		}
	}

	// Token: 0x0600661B RID: 26139 RVA: 0x00266D4F File Offset: 0x00264F4F
	protected void AddWidget(CustomGameSettingWidget widget)
	{
		widget.onSettingChanged += this.OnWidgetChanged;
		this.widgets.Add(widget);
	}

	// Token: 0x0600661C RID: 26140 RVA: 0x00266D6F File Offset: 0x00264F6F
	private void OnWidgetChanged(CustomGameSettingWidget widget)
	{
		this.isDirty = true;
	}

	// Token: 0x0600661D RID: 26141 RVA: 0x00266D78 File Offset: 0x00264F78
	public virtual void Refresh()
	{
		foreach (CustomGameSettingWidget customGameSettingWidget in this.widgets)
		{
			customGameSettingWidget.Refresh();
		}
	}

	// Token: 0x040045A4 RID: 17828
	protected List<CustomGameSettingWidget> widgets = new List<CustomGameSettingWidget>();

	// Token: 0x040045A5 RID: 17829
	private bool isDirty;
}
