using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Audio;
using Carbine.Input;
using Mother4.Battle.Combatants;
using Mother4.Data;

namespace Mother4.Battle.Actions
{
	internal class PlayerBashAction : BattleAction
	{
		public PlayerBashAction(ActionParams aparams) : base(aparams)
		{
			this.combatant = (this.sender as PlayerCombatant);
			if (this.targets.Length == 1)
			{
				this.target = this.targets[0];
				this.meterDelta = default(StatSet);
				this.meterDelta.Meter = 0.013333334f;
				this.messageStack = new Stack<string>();
				this.power = 2f;
				this.comboCount = 0;
				this.state = PlayerBashAction.State.Initialize;
				return;
			}
			throw new NotImplementedException("Cannot target more than one combatant while bashing.");
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case PlayerBashAction.State.Initialize:
				this.Initialize();
				return;
			case PlayerBashAction.State.Combo:
				this.Combo();
				return;
			case PlayerBashAction.State.FinishCombo:
				this.FinishCombo();
				return;
			case PlayerBashAction.State.PopMessage:
				this.PopMessage();
				return;
			case PlayerBashAction.State.WaitForUI:
				break;
			case PlayerBashAction.State.Finish:
				this.Finish();
				break;
			default:
				return;
			}
		}

		public void ButtonPressed(InputManager sender, Button b)
		{
			if (b == Button.A)
			{
				this.bgmPosition = AudioManager.Instance.BGM.Position;
				this.buttonPressed = true;
			}
		}

		private void Initialize()
		{
			Console.WriteLine("BASHMÖDE ({0})", this.combatant.Character);
			this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			this.controller.InterfaceController.PrePlayerAttack.Play();
			this.controller.InterfaceController.PopCard(this.combatant.ID, 12);
			if (!this.controller.CombatantController.IsIdValid(this.target.ID))
			{
				Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.EnemyTeam);
				this.target = factionCombatants[Engine.Random.Next() % factionCombatants.Length];
			}
			this.statDelta = default(StatSet);
			this.comboCount = 0;
			string item = string.Format("{0} attacked!", CharacterNames.GetName(this.combatant.Character));
			this.messageStack.Push(item);
			this.state = PlayerBashAction.State.PopMessage;
			if (this.target is EnemyCombatant)
			{
				this.controller.InterfaceController.StartComboCircle(this.target as EnemyCombatant, this.sender as PlayerCombatant);
			}
		}

		private void PopMessage()
		{
			if (this.messageStack.Count > 0)
			{
				string message = this.messageStack.Pop();
				this.controller.InterfaceController.ShowMessage(message, true);
				this.state = PlayerBashAction.State.WaitForUI;
			}
		}

		private int AccumulateDamage(Combatant target, out bool smash)
		{
			int num;
			if (this.comboCount == 0)
			{
				num = -BattleCalculator.CalculatePhysicalDamage(this.power, this.combatant, target, out smash);
				this.firstHpDelta = num;
				this.statDelta.HP = num;
			}
			else
			{
				num = -BattleCalculator.CalculateComboDamage(this.power, this.combatant, target, (this.firstHpDelta == 0) ? 0 : 1, out smash);
				this.statDelta.HP = this.statDelta.HP + num;
			}
			Console.WriteLine(" hpDelta = {0}", num);
			return num;
		}

		private void Combo()
		{
			if (!(this.target is EnemyCombatant))
			{
				throw new NotImplementedException("Bashing player characters is not yet supported.");
			}
			this.lastLastComboEdge = this.lastComboEdge;
			this.lastComboEdge = this.comboEdge;
			this.comboEdge = this.controller.ComboController.IsCombo(AudioManager.Instance.BGM.Position);
			if (this.buttonPressed)
			{
				if (this.comboCount < 16)
				{
					if (this.controller.ComboController.IsCombo(this.bgmPosition) || this.comboCount == 0)
					{
						bool smash;
						int damage = this.AccumulateDamage(this.target, out smash);
						this.controller.InterfaceController.AddComboHit(damage, this.comboCount, (this.sender as PlayerCombatant).Character, this.target, smash);
						this.combatant.AlterStats(this.meterDelta);
						Console.WriteLine(" COMBO x{0}", this.comboCount + 1);
						this.comboCount++;
					}
					else
					{
						Console.WriteLine(" COMBOVER");
						this.controller.InterfaceController.StopComboCircle(false);
						this.state = PlayerBashAction.State.FinishCombo;
					}
					if (this.comboCount >= 16)
					{
						Console.WriteLine(" THE GREAT COMBONI!!");
						this.controller.InterfaceController.StopComboCircle(true);
						this.state = PlayerBashAction.State.FinishCombo;
					}
				}
				this.buttonPressed = false;
				return;
			}
		}

		private void FinishCombo()
		{
			if (this.controller.InterfaceController.IsComboCircleDone())
			{
				this.state = PlayerBashAction.State.Finish;
			}
		}

		private void Finish()
		{
			Console.WriteLine("Total hpDelta={0}", this.statDelta.HP);
			this.target.AlterStats(this.statDelta);
			if (this.combatant.Stats.Meter < 1f && this.controller.ActionCount > 0)
			{
				this.controller.InterfaceController.PopCard(this.combatant.ID, 0);
			}
			this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
			this.complete = true;
		}

		public void InteractionComplete()
		{
			if (this.state == PlayerBashAction.State.WaitForUI)
			{
				this.state = ((this.messageStack.Count == 0) ? PlayerBashAction.State.Combo : PlayerBashAction.State.PopMessage);
				return;
			}
			if (this.state == PlayerBashAction.State.Combo)
			{
				this.state = PlayerBashAction.State.FinishCombo;
			}
		}

		private const Button COMBO_BUTTON = Button.A;

		private const int MAX_COMBOS = 16;

		private const float ONE_GP = 0.013333334f;

		private const int CARD_POP_HEIGHT = 12;

		private PlayerBashAction.State state;

		private float power;

		private PlayerCombatant combatant;

		private Combatant target;

		private StatSet statDelta;

		private StatSet meterDelta;

		private int firstHpDelta;

		private Stack<string> messageStack;

		private bool buttonPressed;

		private int comboCount;

		private uint bgmPosition;

		private bool comboEdge;

		private bool lastComboEdge;

		private bool lastLastComboEdge;

		private enum State
		{
			Initialize,
			Combo,
			FinishCombo,
			PopMessage,
			WaitForUI,
			Finish
		}
	}
}
