using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Project.Content
{
    public static class AnimatorExtensions
    {
        public static async UniTask WaitForCurrentAnimationStateAsync(this Animator animator, CancellationToken cancellationToken)
        {
            await UniTask.WaitForFixedUpdate(cancellationToken);

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            await UniTask.WaitForSeconds(stateInfo.length, cancellationToken: cancellationToken);
        }
    }
}
