using UnityEngine;

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
    }
}
