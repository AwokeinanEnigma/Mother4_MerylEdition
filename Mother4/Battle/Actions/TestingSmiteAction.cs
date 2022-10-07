using System;
using Mother4.Battle.Combatants;
using Mother4.Data;

namespace Mother4.Battle.Actions
{
    internal class TestingSmiteAction : BattleAction
    {
        public TestingSmiteAction(ActionParams aparams) : base(aparams)
        {
            this.target = ((this.targets.Length > 0) ? ((PlayerCombatant)this.targets[0]) : null);
            this.damage = this.target.Stats.HP - 1;
            this.message = string.Format("You pressed a button and {0} took {1} HP of damage[p:30]. Are you happy with yourself?", CharacterNames.GetName(this.target.Character), this.damage);
        }

        protected override void UpdateAction()
        {
            base.UpdateAction();
            switch (this.state)
            {
                case TestingSmiteAction.State.Initialize:
                    this.controller.InterfaceController.OnTextboxComplete += this.OnTextboxComplete;
                    this.controller.InterfaceController.ShowMessage(this.message, false);
                    this.state = TestingSmiteAction.State.WaitForUI;
                    return;
                case TestingSmiteAction.State.WaitForUI:
                    break;
                case TestingSmiteAction.State.Finish:
                {
                    this.controller.InterfaceController.OnTextboxComplete -= this.OnTextboxComplete;
                    StatSet statChange = new StatSet
                    {
                        HP = -this.damage
                    };
                    this.target.AlterStats(statChange);
                    this.complete = true;
                    break;
                }
                default:
                    return;
            }
        }

        private void OnTextboxComplete()
        {
            this.state = TestingSmiteAction.State.Finish;
        }

        private TestingSmiteAction.State state;

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