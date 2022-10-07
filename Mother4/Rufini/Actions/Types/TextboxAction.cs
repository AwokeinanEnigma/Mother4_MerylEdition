using System;
using System.Collections.Generic;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Rufini.Strings;

namespace Rufini.Actions.Types
{
	internal class TextboxAction : RufiniAction
	{
		public TextboxAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "text",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "sin",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "sout",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "lbx",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			this.context = context;
			string value = base.GetValue<string>("text");
			string value2 = StringFile.Instance.Get(value).Value;
			bool value3 = base.GetValue<bool>("sin");
			bool value4 = base.GetValue<bool>("sout");
			this.context.TextBox.OnTextboxComplete += this.ContinueAfterTextbox;
			this.context.TextBox.Reset(value2, context.Nametag, value3, value4);
			this.context.TextBox.Show();
			this.context.Player.MovementLocked = true;
            this.context.Player.InputLocked = true;
			if (this.context.ActiveNPC != null)
			{
				this.activeNpc = this.context.ActiveNPC;
				this.context.TextBox.OnTypewriterComplete += this.StopTalking;
				this.activeNpc.StartTalking();
			}
			return new ActionReturnContext
			{
				Wait = ScriptExecutor.WaitType.Event
			};
		}

		private void StopTalking()
		{
			this.activeNpc.StopTalking();
			this.context.TextBox.OnTypewriterComplete -= this.StopTalking;
			this.activeNpc = null;
		}

		private void ContinueAfterTextbox()
		{
			this.context.TextBox.Hide();
			this.context.TextBox.OnTextboxComplete -= this.ContinueAfterTextbox;
			this.context.Player.MovementLocked = false;
            this.context.Player.InputLocked = false;
			this.context.Executor.Continue();
		}

		private ExecutionContext context;

		private NPC activeNpc;
	}
}
