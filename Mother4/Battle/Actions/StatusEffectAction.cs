using System;
using Carbine.Audio;
using Mother4.Battle.Combatants;
using Mother4.Data;
using SFML.Graphics;

namespace Mother4.Battle.Actions
{
	internal class StatusEffectAction : BattleAction
	{
		public StatusEffectAction(ActionParams aparams) : base(aparams)
		{
			if (this.sender is PlayerCombatant)
			{
				this.senderName = CharacterNames.GetName((this.sender as PlayerCombatant).Character);
			}
			else if (this.sender is EnemyCombatant)
			{
				this.senderName = (this.sender as EnemyCombatant).Enemy.PlayerFriendlyName;
			}
			else
			{
				this.senderName = string.Empty;
			}
			if (this.targets != null && this.targets[0] is PlayerCombatant)
			{
				this.targetName = CharacterNames.GetName((this.targets[0] as PlayerCombatant).Character);
			}
			else if (this.targets != null && this.targets[0] is EnemyCombatant)
			{
				this.targetName = (this.targets[0] as EnemyCombatant).Enemy.PlayerFriendlyName;
			}
			else
			{
				this.targetName = string.Empty;
			}
			if (this.sender is EnemyCombatant)
			{
				this.senderArticle = (this.sender as EnemyCombatant).Enemy.Article;
			}
			else
			{
				this.senderArticle = string.Empty;
			}
			if (this.targets != null && this.targets[0] is EnemyCombatant)
			{
				this.targetArticle = (this.targets[0] as EnemyCombatant).Enemy.Article;
			}
			else
			{
				this.targetArticle = string.Empty;
			}
			if (this.sender is PlayerCombatant)
			{
				this.actionStartSound = this.controller.InterfaceController.PrePlayerAttack;
			}
			else if (this.sender is EnemyCombatant)
			{
				this.actionStartSound = this.controller.InterfaceController.PreEnemyAttack;
			}
			this.effect = (StatusEffectInstance)aparams.data[0];
			this.state = StatusEffectAction.State.Initialize;
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case StatusEffectAction.State.Initialize:
				this.Initialize();
				return;
			case StatusEffectAction.State.WaitForUI:
				this.WaitForUI();
				return;
			case StatusEffectAction.State.Finish:
				this.Finish();
				return;
			default:
				return;
			}
		}

		protected virtual void Initialize()
		{
			if (this.message == null)
			{
				throw new InvalidOperationException("StatusEffectAction message is null.");
			}
			this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
			this.controller.InterfaceController.ShowMessage(this.message, false);
			this.actionStartSound.Play();
			if (this.sender is PlayerCombatant)
			{
				this.controller.InterfaceController.PopCard(this.sender.ID, 12);
			}
			else if (this.sender is EnemyCombatant)
			{
				this.controller.InterfaceController.FlashEnemy(this.sender as EnemyCombatant, Color.Black, 8, 2);
			}
			this.sender.DecrementStatusEffect(this.effect.Type);
			this.state = StatusEffectAction.State.WaitForUI;
		}

		protected virtual void WaitForUI()
		{
		}

		protected virtual void Finish()
		{
			this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
			if (this.sender is PlayerCombatant)
			{
				this.controller.InterfaceController.PopCard(this.sender.ID, 0);
			}
			this.complete = true;
		}

		protected virtual void InteractionComplete()
		{
			this.state = StatusEffectAction.State.Finish;
		}

		private const int EFFECT_TYPE_INDEX = 0;

		private const int CARD_POP_HEIGHT = 12;

		private const bool USE_BUTTON = false;

		private const int BLINK_DURATION = 8;

		private const int BLINK_COUNT = 2;

		private StatusEffectAction.State state;

		protected string message;

		protected string senderName;

		protected string targetName;

		protected string senderArticle;

		protected string targetArticle;

		protected CarbineSound actionStartSound;

		protected readonly StatusEffectInstance effect;

		private enum State
		{
			Initialize,
			WaitForUI,
			Finish
		}
	}
}
