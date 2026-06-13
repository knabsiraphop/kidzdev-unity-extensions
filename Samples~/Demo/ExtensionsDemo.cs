using System.Collections.Generic;
using KidzDev.Extensions;
using UnityEngine;

namespace KidzDev.Extensions.Samples
{
    /// <summary>
    /// Minimal demo that exercises a few of the extension methods and logs the results.
    /// Add this component to a GameObject in an empty scene and press Play.
    /// </summary>
    public class ExtensionsDemo : MonoBehaviour
    {
        private void Start()
        {
            // StringExtensions
            string title = "KidzDev Extensions Demo";
            Debug.Log($"Truncate(8): \"{title.Truncate(8)}\"");
            Debug.Log($"\"\".IsNullOrEmpty(): {string.Empty.IsNullOrEmpty()}");

            // ArrayExtensions / CollectionExtensions
            var numbers = new[] { 1, 2, 3, 4, 5 };
            Debug.Log($"Array IsNullOrEmpty(): {numbers.IsNullOrEmpty()}");
            Debug.Log($"GetRandom(): {numbers.GetRandom()}");

            // DictionaryExtensions
            var scores = new Dictionary<string, int>();
            int playerScore = scores.GetOrAdd("player", _ => 100);
            Debug.Log($"GetOrAdd(\"player\"): {playerScore}");

            // GameObjectExtensions
            gameObject.SetLayerRecursively(0);
            Debug.Log($"SetLayerRecursively(0): layer is now {gameObject.layer}");
        }
    }
}
