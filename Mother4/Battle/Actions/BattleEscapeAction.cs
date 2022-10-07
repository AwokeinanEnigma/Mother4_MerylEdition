using System;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using SFML.Graphics;

namespace Mother4.Battle.Actions
{
	internal class BattleEscapeAction : BattleAction
	{
		public BattleEscapeAction(ActionParams aparams) : base(aparams)
		{
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			this.controller.InterfaceController.RemoveAllModifiers();
			Console.WriteLine("YOU RAN AWAY!");
			ITransition transition = new ColorFadeTransition(1f, Color.Black);
			transition.Blocking = true;
			SceneManager.Instance.Transition = transition;
			SceneManager.Instance.Pop();
			this.complete = true;
		}
	}
}
