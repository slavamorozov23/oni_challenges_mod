using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000C2C RID: 3116
public class InputModuleSwitch : MonoBehaviour
{
	// Token: 0x06005E36 RID: 24118 RVA: 0x00225EB0 File Offset: 0x002240B0
	private void Update()
	{
		if (this.lastMousePosition != Input.mousePosition && KInputManager.currentControllerIsGamepad)
		{
			KInputManager.currentControllerIsGamepad = false;
			KInputManager.InputChange.Invoke();
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			this.virtualInput.enabled = KInputManager.currentControllerIsGamepad;
			if (this.standaloneInput.enabled)
			{
				this.standaloneInput.enabled = false;
				this.ChangeInputHandler();
				return;
			}
		}
		else
		{
			this.lastMousePosition = Input.mousePosition;
			this.standaloneInput.enabled = true;
			if (this.virtualInput.enabled)
			{
				this.virtualInput.enabled = false;
				this.ChangeInputHandler();
			}
		}
	}

	// Token: 0x06005E37 RID: 24119 RVA: 0x00225F54 File Offset: 0x00224154
	private void ChangeInputHandler()
	{
		GameInputManager inputManager = Global.GetInputManager();
		for (int i = 0; i < inputManager.usedMenus.Count; i++)
		{
			if (inputManager.usedMenus[i].Equals(null))
			{
				inputManager.usedMenus.RemoveAt(i);
			}
		}
		if (inputManager.GetControllerCount() > 1)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				Cursor.visible = false;
				inputManager.GetController(1).inputHandler.TransferHandles(inputManager.GetController(0).inputHandler);
				return;
			}
			Cursor.visible = true;
			inputManager.GetController(0).inputHandler.TransferHandles(inputManager.GetController(1).inputHandler);
		}
	}

	// Token: 0x04003E9E RID: 16030
	public VirtualInputModule virtualInput;

	// Token: 0x04003E9F RID: 16031
	public StandaloneInputModule standaloneInput;

	// Token: 0x04003EA0 RID: 16032
	private Vector3 lastMousePosition;
}
