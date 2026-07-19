using UnityEngine;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Animator"/>.
    /// </summary>
    public static class AnimatorExtensions
    {
        /// <summary>
        /// Restarts the animator's current state on <paramref name="layer"/> from the beginning
        /// (normalized time 0). No-op when <paramref name="animator"/> is <c>null</c> or has no
        /// <see cref="RuntimeAnimatorController"/> assigned.
        /// </summary>
        public static void RestartCurrentState(this Animator animator, int layer = 0)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return;

            int stateHash = animator.GetCurrentAnimatorStateInfo(layer).fullPathHash;
            animator.Play(stateHash, layer, 0f);
        }

        /// <summary>
        /// Resets the animator to its default pose and immediately applies it, so the change is
        /// visible without waiting for the next frame. No-op when <paramref name="animator"/> is <c>null</c>.
        /// </summary>
        public static void ResetToDefault(this Animator animator)
        {
            if (animator == null)
                return;

            animator.Rebind();
            animator.Update(0f);
        }
    }
}
