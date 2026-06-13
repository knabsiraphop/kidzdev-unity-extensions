using UnityEngine;

namespace KidzDev.Extensions
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

        // TODO: add more
    }
}
