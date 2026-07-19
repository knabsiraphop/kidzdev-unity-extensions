using UnityEngine;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Camera"/> culling-mask layer visibility.
    /// </summary>
    public static class CameraExtensions
    {
        /// <summary>Shows or hides <paramref name="layer"/> in this camera's culling mask.</summary>
        public static void SetLayerVisible(this Camera camera, int layer, bool visible)
        {
            if (layer < 0 || layer > 31)
                return;

            camera.cullingMask = visible
                ? camera.cullingMask | (1 << layer)
                : camera.cullingMask & ~(1 << layer);
        }

        /// <summary>
        /// Shows or hides the layer named <paramref name="layerName"/> in this camera's culling mask.
        /// No-op when the layer name does not resolve to a valid layer.
        /// </summary>
        public static void SetLayerVisible(this Camera camera, string layerName, bool visible)
        {
            camera.SetLayerVisible(LayerMask.NameToLayer(layerName), visible);
        }

        /// <summary>Toggles whether <paramref name="layer"/> is included in this camera's culling mask.</summary>
        public static void ToggleLayer(this Camera camera, int layer)
        {
            if (layer < 0 || layer > 31)
                return;

            camera.cullingMask ^= 1 << layer;
        }

        /// <summary>
        /// Toggles whether the layer named <paramref name="layerName"/> is included in this camera's
        /// culling mask. No-op when the layer name does not resolve to a valid layer.
        /// </summary>
        public static void ToggleLayer(this Camera camera, string layerName)
        {
            camera.ToggleLayer(LayerMask.NameToLayer(layerName));
        }

        /// <summary>Returns <c>true</c> when <paramref name="layer"/> is included in this camera's culling mask.</summary>
        public static bool IsLayerVisible(this Camera camera, int layer)
        {
            if (layer < 0 || layer > 31)
                return false;

            return (camera.cullingMask & (1 << layer)) != 0;
        }
    }
}
