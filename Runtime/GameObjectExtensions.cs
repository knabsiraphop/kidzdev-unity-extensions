using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="GameObject"/>.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>Sets the layer on this GameObject and every descendant in its hierarchy.</summary>
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;

            foreach (Transform child in gameObject.transform)
                child.gameObject.SetLayerRecursively(layer);
        }

        /// <summary>
        /// Returns the component of type <typeparamref name="T"/>, adding one to the GameObject
        /// when it does not already have it.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent<T>(out var component)
                ? component
                : gameObject.AddComponent<T>();
        }

        /// <summary>
        /// Destroys every child of this GameObject. Uses <c>DestroyImmediate</c> outside of play mode
        /// so it also works from editor tooling, and <c>Destroy</c> at runtime.
        /// </summary>
        public static void DestroyChildren(this GameObject gameObject)
        {
            Transform transform = gameObject.transform;

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = transform.GetChild(i).gameObject;

                if (Application.isPlaying)
                    Object.Destroy(child);
                else
                    Object.DestroyImmediate(child);
            }
        }

        /// <summary>
        /// Destroys this GameObject. Uses <c>DestroyImmediate</c> outside of play mode so it also works from
        /// editor tooling, and <c>Destroy</c> at runtime. Mirrors <see cref="DestroyChildren"/>.
        /// </summary>
        public static void DestroySelf(this GameObject gameObject)
        {
            if (Application.isPlaying)
                Object.Destroy(gameObject);
            else
                Object.DestroyImmediate(gameObject);
        }

        /// <summary>
        /// Returns <c>true</c> when this GameObject's layer is included in <paramref name="mask"/>. Wraps the
        /// error-prone <c>(mask &amp; (1 &lt;&lt; layer)) != 0</c> bit test.
        /// </summary>
        public static bool IsInLayerMask(this GameObject gameObject, LayerMask mask)
        {
            return (mask.value & (1 << gameObject.layer)) != 0;
        }

        /// <summary>
        /// Returns the full hierarchy path of this GameObject (e.g. <c>"Canvas/Panel/Button"</c>), useful for
        /// debug logging. The path is rooted at the topmost ancestor and does not include the scene name.
        /// </summary>
        public static string GetPath(this GameObject gameObject)
        {
            Transform current = gameObject.transform;
            var builder = new StringBuilder(current.name);

            while (current.parent != null)
            {
                current = current.parent;
                builder.Insert(0, '/').Insert(0, current.name);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Forces an immediate UGUI layout rebuild on this GameObject's <see cref="RectTransform"/>, so layout
        /// reflects content changes within the same frame instead of waiting for the next layout pass. No-op
        /// when the GameObject has no <see cref="RectTransform"/>.
        /// </summary>
        public static void RefreshLayout(this GameObject gameObject)
        {
            if (gameObject.transform is RectTransform rectTransform)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}
