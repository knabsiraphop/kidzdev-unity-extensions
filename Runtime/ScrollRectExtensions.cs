using UnityEngine;
using UnityEngine.UI;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ScrollRect"/>.
    /// </summary>
    public static class ScrollRectExtensions
    {
        /// <summary>
        /// Applies <paramref name="normalizedDelta"/> to <see cref="ScrollRect.verticalNormalizedPosition"/>,
        /// clamped to <c>[0, 1]</c>. No-op when <paramref name="scrollRect"/> is <c>null</c>.
        /// </summary>
        public static void ScrollVertical(this ScrollRect scrollRect, float normalizedDelta)
        {
            if (scrollRect == null)
                return;

            scrollRect.verticalNormalizedPosition =
                Mathf.Clamp01(scrollRect.verticalNormalizedPosition + normalizedDelta);
        }

        /// <summary>
        /// Applies <paramref name="normalizedDelta"/> to <see cref="ScrollRect.horizontalNormalizedPosition"/>,
        /// clamped to <c>[0, 1]</c>. No-op when <paramref name="scrollRect"/> is <c>null</c>.
        /// </summary>
        public static void ScrollHorizontal(this ScrollRect scrollRect, float normalizedDelta)
        {
            if (scrollRect == null)
                return;

            scrollRect.horizontalNormalizedPosition =
                Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + normalizedDelta);
        }

        /// <summary>Snaps to the top of the content (<c>verticalNormalizedPosition = 1</c>).</summary>
        public static void ScrollToTop(this ScrollRect scrollRect)
        {
            if (scrollRect != null)
                scrollRect.verticalNormalizedPosition = 1f;
        }

        /// <summary>Snaps to the bottom of the content (<c>verticalNormalizedPosition = 0</c>).</summary>
        public static void ScrollToBottom(this ScrollRect scrollRect)
        {
            if (scrollRect != null)
                scrollRect.verticalNormalizedPosition = 0f;
        }

        /// <summary>Snaps to the left of the content (<c>horizontalNormalizedPosition = 0</c>).</summary>
        public static void ScrollToLeft(this ScrollRect scrollRect)
        {
            if (scrollRect != null)
                scrollRect.horizontalNormalizedPosition = 0f;
        }

        /// <summary>Snaps to the right of the content (<c>horizontalNormalizedPosition = 1</c>).</summary>
        public static void ScrollToRight(this ScrollRect scrollRect)
        {
            if (scrollRect != null)
                scrollRect.horizontalNormalizedPosition = 1f;
        }
    }
}
