using System.Collections;
using System.Collections.Generic;
using KidzDev.Unity.Extensions;
using UnityEngine;

namespace KidzDev.Unity.Extensions.Samples
{
    /// <summary>
    /// Exercises every extension method in the package and reports the result of each call — both to the
    /// Console (<see cref="Debug.Log"/>) and to an on-screen <see cref="OnGUI"/> readout. Open the bundled
    /// <c>Demo.unity</c> scene (or add this component to a GameObject in an empty scene) and press Play.
    /// </summary>
    public class ExtensionsDemo : MonoBehaviour
    {
        private readonly List<string> _output = new List<string>();
        private Vector2 _scroll;

        private IEnumerator Start()
        {
            StringSamples();
            CollectionSamples();
            DictionarySamples();
            IntSamples();
            FloatSamples();
            DoubleSamples();
            LongSamples();
            yield return GameObjectSamples();
        }

        // ── string ──────────────────────────────────────────────────────────

        private void StringSamples()
        {
            Section("StringExtensions");

            const string title = "kidzdev extensions demo";
            Log("\"kidzdev…demo\".Truncate(12)", title.Truncate(12));
            Log("\"\".Or(\"(empty)\")", string.Empty.Or("(empty)"));
            Log("\"KIDZDEV\".EqualsIgnoreCase(\"kidzdev\")", "KIDZDEV".EqualsIgnoreCase("kidzdev"));
            Log("\"Hello World\".ContainsIgnoreCase(\"WORLD\")", "Hello World".ContainsIgnoreCase("WORLD"));
            Log("\"AddressableKey\".StartsWithIgnoreCase(\"address\")", "AddressableKey".StartsWithIgnoreCase("address"));
            Log("\"image.PNG\".EndsWithIgnoreCase(\".png\")", "image.PNG".EndsWithIgnoreCase(".png"));
            Log("\"hello world\".ToTitleCase()", "hello world".ToTitleCase());
            Log("\"<b>Bold</b> text\".StripRichText()", "<b>Bold</b> text".StripRichText());

            bool intOk = "7".TryToInt(out int parsedInt);
            Log("\"7\".TryToInt(out i)", $"{intOk} (i={parsedInt})");
            Log("\"NaN\".ToIntOrDefault(-1)", "NaN".ToIntOrDefault(-1));

            bool floatOk = "3.14".TryToFloat(out float parsedFloat);
            Log("\"3.14\".TryToFloat(out f)", $"{floatOk} (f={parsedFloat})");
            Log("\"oops\".ToFloatOrDefault(1.5f)", "oops".ToFloatOrDefault(1.5f));

            bool colorOk = "#FF8800".TryToColor(out Color parsedColor);
            Log("\"#FF8800\".TryToColor(out c)", $"{colorOk} (#{ColorUtility.ToHtmlStringRGB(parsedColor)})");
            Log("\"#nope\".ToColor(Color.black)", "#" + ColorUtility.ToHtmlStringRGB("#nope".ToColor(Color.black)));

            Log("\"save_slot_1\".GetStableHashCode()", "save_slot_1".GetStableHashCode());
        }

        // ── collections ─────────────────────────────────────────────────────

        private void CollectionSamples()
        {
            Section("CollectionExtensions");

            int[] missing = null;
            Log("((int[])null).IsNullOrEmpty()", missing.IsNullOrEmpty());

            var nums = new List<int> { 10, 20, 30, 40, 50 };
            Log("nums.IsValidIndex(3)", nums.IsValidIndex(3));
            Log("nums.IsValidIndex(9)", nums.IsValidIndex(9));
            Log("nums.GetRandom()", nums.GetRandom());
            Log("new List<int>().GetRandomOrDefault()", new List<int>().GetRandomOrDefault());

            var shuffled = new List<int>(nums);
            shuffled.Shuffle();
            Log("nums.Shuffle()", string.Join(",", shuffled));

            var toSwap = new List<int> { 1, 2, 3 };
            toSwap.Swap(0, 2);
            Log("[1,2,3].Swap(0,2)", string.Join(",", toSwap));

            var bag = new List<int>(nums);
            Log("nums.PopRandom()", bag.PopRandom());

            var words = new[] { "addressable", "io", "scope" };
            Log("words.MinBy(w => w.Length)", words.MinBy(w => w.Length));
            Log("words.MaxBy(w => w.Length)", words.MaxBy(w => w.Length));
        }

        // ── dictionaries ────────────────────────────────────────────────────

        private void DictionarySamples()
        {
            Section("DictionaryExtensions");

            var scores = new Dictionary<string, int>();
            Log("scores.GetOrAdd(\"p1\", _ => 100)", scores.GetOrAdd("p1", _ => 100));

            var buckets = new Dictionary<string, List<int>>();
            buckets.GetOrAdd("a").Add(1);
            buckets.GetOrAdd("a").Add(2);
            Log("buckets.GetOrAdd(\"a\").Add(...)", string.Join(",", buckets["a"]));

            var counts = new Dictionary<string, int>();
            counts.Increment("hit");
            counts.Increment("hit", 5);
            Log("counts.Increment(\"hit\") ×{1,5}", counts["hit"]);

            scores.AddOrUpdate("p1", 1, (_, v) => v + 10);
            Log("scores.AddOrUpdate(\"p1\", 1, +10)", scores["p1"]);
        }

        // ── int ─────────────────────────────────────────────────────────────

        private void IntSamples()
        {
            Section("IntExtensions");

            Log("(-1).Wrap(5)", (-1).Wrap(5));
            Log("(7).Wrap(5)", (7).Wrap(5));
            Log("(5).IsBetween(1, 10)", (5).IsBetween(1, 10));
            Log("(1234567).ToThousandsString()", (1234567).ToThousandsString());
            Log("(22).ToOrdinal()", (22).ToOrdinal());
            Log("(13).ToOrdinal()", (13).ToOrdinal());
        }

        // ── float ───────────────────────────────────────────────────────────

        private void FloatSamples()
        {
            Section("FloatExtensions");

            Log("(0.1f + 0.2f).Approximately(0.3f)", (0.1f + 0.2f).Approximately(0.3f));
            Log("1e-7f.IsZero()", 1e-7f.IsZero());
            Log("1.5f.Clamp01()", 1.5f.Clamp01());
            Log("5f.Remap(0,10, 0,100)", 5f.Remap(0f, 10f, 0f, 100f));
            Log("7.3f.Snap(0.5f)", 7.3f.Snap(0.5f));
            Log("2.6f.RoundToInt()", 2.6f.RoundToInt());
            Log("2.6f.FloorToInt()", 2.6f.FloorToInt());
            Log("2.1f.CeilToInt()", 2.1f.CeilToInt());
            Log("0.5f.IsBetween(0, 1)", 0.5f.IsBetween(0f, 1f));
            Log("12345.678f.ToThousandsString(2)", 12345.678f.ToThousandsString(2));
        }

        // ── double ──────────────────────────────────────────────────────────

        private void DoubleSamples()
        {
            Section("DoubleExtensions");

            Log("1.5.Clamp01()", 1.5d.Clamp01());
            Log("0.25.Remap(0,1, 0,360)", 0.25d.Remap(0d, 1d, 0d, 360d));
            Log("50.0.IsBetween(0, 100)", 50d.IsBetween(0d, 100d));
            Log("9876543.21.ToThousandsString(2)", 9876543.21d.ToThousandsString(2));
        }

        // ── long ────────────────────────────────────────────────────────────

        private void LongSamples()
        {
            Section("LongExtensions");

            Log("1500000L.IsBetween(0, 2000000)", 1500000L.IsBetween(0L, 2000000L));
            Log("9876543210L.ToThousandsString()", 9876543210L.ToThousandsString());
            Log("1572864L.ToFileSizeString()", 1572864L.ToFileSizeString());
            Log("(5L<<30).ToFileSizeString()", (5L << 30).ToFileSizeString());
        }

        // ── GameObject ──────────────────────────────────────────────────────

        private IEnumerator GameObjectSamples()
        {
            Section("GameObjectExtensions");

            gameObject.SetLayerRecursively(0);
            Log("gameObject.SetLayerRecursively(0)", "layer=" + gameObject.layer);

            var probe = new GameObject("Probe");
            probe.transform.SetParent(transform);
            Log("probe.GetPath()", probe.GetPath());

            var collider = probe.GetOrAddComponent<BoxCollider>();
            Log("probe.GetOrAddComponent<BoxCollider>()", collider != null);

            LayerMask mask = 1 << gameObject.layer;
            Log("gameObject.IsInLayerMask(mask)", gameObject.IsInLayerMask(mask));

            var rect = new GameObject("Rect", typeof(RectTransform));
            rect.transform.SetParent(transform, false);
            rect.RefreshLayout();
            Log("rect.RefreshLayout()", "ok (no exception)");

            // Object.Destroy is deferred to end of frame in play mode, so wait a frame before reading results.
            var parent = new GameObject("ToClear");
            parent.transform.SetParent(transform);
            for (int i = 0; i < 3; i++)
                new GameObject("child" + i).transform.SetParent(parent.transform);
            int before = parent.transform.childCount;
            parent.DestroyChildren();
            yield return null;
            Log("parent.DestroyChildren() (3 kids)", $"{before} -> {parent.transform.childCount}");

            var disposable = new GameObject("Disposable");
            disposable.DestroySelf();
            yield return null;
            Log("disposable.DestroySelf()", disposable == null ? "destroyed" : "still alive");
        }

        // ── helpers ─────────────────────────────────────────────────────────

        private void Section(string name)
        {
            _output.Add(string.Empty);
            _output.Add("── " + name + " ──");
        }

        private void Log(string label, object value)
        {
            string line = $"{label} = {value}";
            _output.Add(line);
            Debug.Log("[ExtensionsDemo] " + line);
        }

        private void OnGUI()
        {
            float width = Mathf.Min(480f, Screen.width - 20f);
            GUILayout.BeginArea(new Rect(10f, 10f, width, Screen.height - 20f), GUI.skin.box);
            GUILayout.Label("KidzDev Extensions Demo — results (also in Console)");
            _scroll = GUILayout.BeginScrollView(_scroll);
            foreach (string line in _output)
                GUILayout.Label(line);
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}
