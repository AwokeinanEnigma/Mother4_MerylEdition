using System;
using System.Diagnostics;
using Mother4.Battle.Combatants;
using Mother4.Data;

namespace Mother4.Battle.Actions
{
    internal class PlayerDeathAction : BattleAction
    {
        public PlayerDeathAction(ActionParams aparams) : base(aparams)
        {

            this.target = ((this.targets.Length > 0) ? ((PlayerCombatant)this.targets[0]) : null);
            this.damage = this.target.Stats.HP - 1;
            this.message = string.Format("{0} got hurt and collapsed!!", CharacterNames.GetName(this.target.Character));//, this.damage);
        }

        protected override void UpdateAction()
        {
            base.UpdateAction();
            switch (this.state)
            {
                case PlayerDeathAction.State.Initialize:
                    //Console.WriteLine($"count: {controller.ActionCount}) ");
                    controller.RemoveCombatantActions(this.target);
                    //Console.WriteLine($"count2zd: {controller.ActionCount}) ");

                    this.controller.InterfaceController.OnTextboxComplete += this.OnTextboxComplete;
                    this.controller.InterfaceController.ShowMessage(this.message, true);
                    target.AddStatusEffect(StatusEffect.Unconscious, 500);
                    this.state = PlayerDeathAction.State.WaitForUI;
                    return;
                case PlayerDeathAction.State.WaitForUI:
                    break;
                case PlayerDeathAction.State.Finish:
                {
                    this.controller.InterfaceController.OnTextboxComplete -= this.OnTextboxComplete;
                    this.complete = true;
                    break;
                }
                default:
                    return;
            }
        }

        private void OnTextboxComplete()
        {
            this.state = PlayerDeathAction.State.Finish;
        }

        private PlayerDeathAction.State state;

        private string message;

        private PlayerCombatant target;

        private int damage;

        private enum State
        {
            Initialize,
            WaitForUI,
            Finish
        }
    }
}