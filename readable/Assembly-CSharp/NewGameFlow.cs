using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DBA RID: 3514
[AddComponentMenu("KMonoBehaviour/scripts/NewGameFlow")]
public class NewGameFlow : KMonoBehaviour
{
	// Token: 0x06006DC3 RID: 28099 RVA: 0x002998D0 File Offset: 0x00297AD0
	public void BeginFlow()
	{
		this.currentScreenIndex = -1;
		this.Next();
	}

	// Token: 0x06006DC4 RID: 28100 RVA: 0x002998DF File Offset: 0x00297ADF
	private void Next()
	{
		this.ClearCurrentScreen();
		this.currentScreenIndex++;
		this.ActivateCurrentScreen();
	}

	// Token: 0x06006DC5 RID: 28101 RVA: 0x002998FB File Offset: 0x00297AFB
	private void Previous()
	{
		this.ClearCurrentScreen();
		this.currentScreenIndex--;
		this.ActivateCurrentScreen();
	}

	// Token: 0x06006DC6 RID: 28102 RVA: 0x00299917 File Offset: 0x00297B17
	private void ClearCurrentScreen()
	{
		if (this.currentScreen != null)
		{
			this.currentScreen.Deactivate();
			this.currentScreen = null;
		}
	}

	// Token: 0x06006DC7 RID: 28103 RVA: 0x0029993C File Offset: 0x00297B3C
	private void ActivateCurrentScreen()
	{
		if (this.currentScreenIndex >= 0 && this.currentScreenIndex < this.newGameFlowScreens.Count)
		{
			NewGameFlowScreen newGameFlowScreen = Util.KInstantiateUI<NewGameFlowScreen>(this.newGameFlowScreens[this.currentScreenIndex].gameObject, base.transform.parent.gameObject, true);
			newGameFlowScreen.OnNavigateForward += this.Next;
			newGameFlowScreen.OnNavigateBackward += this.Previous;
			if (!newGameFlowScreen.IsActive() && !newGameFlowScreen.activateOnSpawn)
			{
				newGameFlowScreen.Activate();
			}
			this.currentScreen = newGameFlowScreen;
		}
	}

	// Token: 0x04004AF4 RID: 19188
	public List<NewGameFlowScreen> newGameFlowScreens;

	// Token: 0x04004AF5 RID: 19189
	private int currentScreenIndex = -1;

	// Token: 0x04004AF6 RID: 19190
	private NewGameFlowScreen currentScreen;
}
