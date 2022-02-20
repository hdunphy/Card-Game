using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public class DefenseModiferStatus : BaseStatus
    {
        [SerializeField] public int Modifier;

        public override void ApplyStatus(Monster monster)
        {
            base.ApplyStatus(monster);
            if (Mathf.Sign(monster.DefenseModifier) == Mathf.Sign(Modifier))
            {
                monster.DefenseModifier *= Modifier;
            }
            else
            {
                monster.DefenseModifier /= Modifier;
            }
        }

        public override int GetCount() => (int)Mathf.Sign(Modifier);

        public override void DoEffect(Monster monster, int count)
        {

        }
    }
}
