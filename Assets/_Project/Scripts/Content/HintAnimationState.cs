using UnityEngine;

namespace Project.Content.Spawners
{
    public class HintAnimationState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(AnimatorHashes.IsHints, true);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(AnimatorHashes.IsHints, false);
        }
    }
}
