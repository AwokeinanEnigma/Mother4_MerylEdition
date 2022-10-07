using SunsetRhapsody.Battle;
using SunsetRhapsody.Battle.Combatants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunsetRhapsody.SOMETHING
{
    public abstract class AUXBase
    {
        internal abstract Psi.IPsi identifier { get; }

        public abstract int AUCost { get; }
        public abstract TargetingMode TargetMode { get; }
        public abstract int[] Symbols { get; }
        public abstract string QualifiedName { get; }
        public abstract string Key { get; }
        internal abstract void Initialize();
        internal abstract void Animate();
        internal abstract void Act(Combatant[] combatants);

        internal abstract void ScaleToLevel(PlayerCombatant combatant);

        internal virtual bool GetAvailiability(PlayerCombatant combantant) {
            if (combantant.Stats.PP < AUCost) {
                return false;
            }
            return true;
        }
    }
}
