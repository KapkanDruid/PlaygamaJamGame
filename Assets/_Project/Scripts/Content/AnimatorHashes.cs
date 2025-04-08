using UnityEngine;

namespace Project.Content
{
    public static class AnimatorHashes
    {
        public static readonly int SimpleAttackTrigger = Animator.StringToHash("SimpleAttack");
        public static readonly int ArealAttackTrigger = Animator.StringToHash("ArealAttack");
        public static readonly int SpikeAttackTrigger = Animator.StringToHash("SpikeAttack");
        public static readonly int RangeAttackTrigger = Animator.StringToHash("RangeAttack");
        public static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public static readonly int IsDead = Animator.StringToHash("IsDead");
        public static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
        public static readonly int IsTakingDamage = Animator.StringToHash("IsTakingDamage");
        public static readonly int TakeDamageTrigger = Animator.StringToHash("TakeDamage");
        public static readonly int DeathTrigger = Animator.StringToHash("Death");
        public static readonly int StartSceneTrigger = Animator.StringToHash("StartScene");
        public static readonly int EndSceneTrigger = Animator.StringToHash("EndScene");
        public static readonly int ShootTrigger = Animator.StringToHash("Shoot");
        public static readonly int HitTrigger = Animator.StringToHash("Hit");
        public static readonly int ShowEffectTrigger = Animator.StringToHash("ShowEffect");
        public static readonly int HintTrigger = Animator.StringToHash("Hint");
        public static readonly int IsHints = Animator.StringToHash("IsHints");
        public static readonly int ShowTitlesTrigger = Animator.StringToHash("ShowTitles");
        public static readonly int IsForceEnded = Animator.StringToHash("IsForceEnded");
    }
}