using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using KidzDev.Unity.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace KidzDev.Unity.Extensions.Samples
{
    /// <summary>
    /// Exercises every extension method in the package and reports the result of each call — both to the
    /// Console (<see cref="Debug.Log"/>) and to an on-screen <see cref="OnGUI"/> readout. Open the bundled
    /// <c>Demo.unity</c> scene (or add this component to a GameObject in an empty scene) and press Play.
    /// </summary>
    public class ExtensionsDemo : MonoBehaviour
    {
        private enum SampleEnum { Alpha, Beta, Gamma }

        private readonly List<string> _output      = new List<string>();
        private readonly List<string> _outputRight = new List<string>();
        private Vector2 _scroll;
        private Vector2 _scrollRight;

        private IEnumerator Start()
        {
            StringSamples();
            CollectionSamples();
            DictionarySamples();
            IntSamples();
            FloatSamples();
            DoubleSamples();
            LongSamples();
            DateTimeOffsetSamples();
            TimeSpanSamples();
            AnimatorSamples();
            CameraSamples();
            ScrollRectSamples();
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

            Log("\"Danger\".WithColor(Color.red)", "Danger".WithColor(Color.red));
            Log("\"a\\r\\nb\\rc\".NormalizeNewlines()", "a\r\nb\rc".NormalizeNewlines().Replace("\n", "\\n"));

            bool enumOk = "beta".TryToEnum(out SampleEnum parsedEnum);
            Log("\"beta\".TryToEnum(out SampleEnum e)", $"{enumOk} (e={parsedEnum})");
            Log("\"nope\".ToEnumOrDefault(SampleEnum.Gamma)", "nope".ToEnumOrDefault(SampleEnum.Gamma));
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

            var seededShuffle = new List<int>(nums);
            seededShuffle.Shuffle(new System.Random(42));
            Log("nums.Shuffle(new Random(42))", string.Join(",", seededShuffle));

            Log("nums.FindIndex(n => n > 25)", nums.FindIndex(n => n > 25));
            Log("nums.FindIndex(n => n > 999)", nums.FindIndex(n => n > 999));
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

            var stacks = new Dictionary<string, int> { ["buff"] = 3 };
            stacks.Increment("buff", -3, removeIfZero: true);
            Log("stacks.Increment(\"buff\", -3, removeIfZero:true)", stacks.ContainsKey("buff") ? "still present" : "removed");
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
            Log("(1500).ToAbbreviatedString()", (1500).ToAbbreviatedString());
            Log("(2500000).ToAbbreviatedString(0)", (2500000).ToAbbreviatedString(0));
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
            Log("1_750_000_000L.ToDateTimeOffsetFromUnixSeconds()", 1_750_000_000L.ToDateTimeOffsetFromUnixSeconds().ToIso8601String());
            Log("1_750_000_000_000L.ToDateTimeOffsetFromUnixMilliseconds()", 1_750_000_000_000L.ToDateTimeOffsetFromUnixMilliseconds().ToIso8601String());
            Log("1_234_567_890L.ToAbbreviatedString()", 1_234_567_890L.ToAbbreviatedString());
            Log("(-2500L).ToAbbreviatedString(0)", (-2500L).ToAbbreviatedString(0));
            Log("0L.ToAbbreviatedString()", 0L.ToAbbreviatedString());
        }

        // ── DateTimeOffset ──────────────────────────────────────────────────

        private void DateTimeOffsetSamples()
        {
            SectionRight("DateTimeOffsetExtensions");

            // Fixed instant: 2026-06-21 14:30:00 UTC+7
            var now = new DateTimeOffset(2026, 6, 21, 14, 30, 0, TimeSpan.FromHours(7));
            var sameDay = new DateTimeOffset(2026, 6, 21, 23, 59, 0, TimeSpan.FromHours(7));
            var nextDay = new DateTimeOffset(2026, 6, 22, 0, 0, 0, TimeSpan.FromHours(7));
            var eventStart = new DateTimeOffset(2026, 6, 20, 0, 0, 0, TimeSpan.FromHours(7));
            var eventEnd = new DateTimeOffset(2026, 6, 25, 0, 0, 0, TimeSpan.FromHours(7));

            LogRight("now.IsBetween(eventStart, eventEnd)", now.IsBetween(eventStart, eventEnd));
            LogRight("now.IsSameDay(sameDay)", now.IsSameDay(sameDay));
            LogRight("now.IsSameDay(nextDay)", now.IsSameDay(nextDay));
            LogRight("now.StartOfDay()", now.StartOfDay().ToIso8601String());
            LogRight("now.EndOfDay()", now.EndOfDay().ToIso8601String());
            LogRight("now.StartOfWeek(Monday)", now.StartOfWeek(DayOfWeek.Monday).ToIso8601String());
            LogRight("now.StartOfWeek(Sunday)", now.StartOfWeek(DayOfWeek.Sunday).ToIso8601String());
            LogRight("now.StartOfMonth()", now.StartOfMonth().ToIso8601String());
            LogRight("now.EndOfMonth()", now.EndOfMonth().ToIso8601String());
            LogRight("now.ToIso8601String()", now.ToIso8601String());

            // Relative time — use live UtcNow so results are meaningful
            var utcNow = DateTimeOffset.UtcNow;
            var fiveMinAgo  = utcNow.AddMinutes(-5);
            var tomorrow    = utcNow.AddDays(1);
            var twoHrsLater = utcNow.AddHours(2);

            SectionRight("— Relative");
            LogRight("(UtcNow-5m).ToRelativeString()", fiveMinAgo.ToRelativeString());
            LogRight("tomorrow.ToRelativeString()", tomorrow.ToRelativeString());
            LogRight("(UtcNow+2h).ToRelativeString()", twoHrsLater.ToRelativeString());

            var rt = fiveMinAgo.ToRelativeTime();
            LogRight("(UtcNow-5m) rt.Unit", rt.Unit);
            LogRight("(UtcNow-5m) rt.Count", rt.Count);
            LogRight("(UtcNow-5m) rt.IsFuture", rt.IsFuture);

            // Friendly absolute — invariant vs culture-specific
            SectionRight("— Friendly date");
            LogRight("now.ToFriendlyDateString()", now.ToFriendlyDateString());
            LogRight("now.ToFriendlyDateString(th-TH)", now.ToFriendlyDateString(new CultureInfo("th-TH")));
            LogRight("now.ToFriendlyDateTimeString()", now.ToFriendlyDateTimeString());
        }

        // ── TimeSpan ────────────────────────────────────────────────────────

        private void TimeSpanSamples()
        {
            SectionRight("TimeSpanExtensions");

            var overHour   = TimeSpan.FromSeconds(3909);   // 1h 5m 9s
            var underHour  = TimeSpan.FromSeconds(150);    // 2m 30s
            var twoAndHalf = new TimeSpan(2, 3, 30, 0);   // 2d 3h 30m
            var negative   = TimeSpan.FromMinutes(-90);

            SectionRight("— Clock");
            LogRight("(1h 5m 9s).ToClockString()", overHour.ToClockString());
            LogRight("(2m 30s).ToClockString()", underHour.ToClockString());
            LogRight("Zero.ToClockString()", TimeSpan.Zero.ToClockString());

            SectionRight("— Compact");
            LogRight("(2d 3h 30m).ToCompactString()", twoAndHalf.ToCompactString());
            LogRight("(1h 5m 9s).ToCompactString()", overHour.ToCompactString());
            LogRight("(2m 30s).ToCompactString()", underHour.ToCompactString());
            LogRight("Zero.ToCompactString()", TimeSpan.Zero.ToCompactString());
            LogRight("(-90m).ToCompactString()", negative.ToCompactString());

            SectionRight("— Range");
            LogRight("(2m 30s).Clamp(1m, 5m)", underHour.Clamp(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5)).ToCompactString());
            LogRight("(2m 30s).IsBetween(1m, 5m)", underHour.IsBetween(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5)));
        }

        // ── Animator ────────────────────────────────────────────────────────

        private void AnimatorSamples()
        {
            Section("AnimatorExtensions");

            var animatorGo = new GameObject("SampleAnimator", typeof(Animator));
            animatorGo.transform.SetParent(transform);
            var animator = animatorGo.GetComponent<Animator>();

            animator.ResetToDefault();
            Log("animator.ResetToDefault()", "ok (no exception)");

            animator.RestartCurrentState();
            Log("animator.RestartCurrentState() (no controller)", "no-op, ok");
        }

        // ── Camera ──────────────────────────────────────────────────────────

        private void CameraSamples()
        {
            Section("CameraExtensions");

            var cameraGo = new GameObject("SampleCamera", typeof(Camera));
            cameraGo.transform.SetParent(transform);
            var camera = cameraGo.GetComponent<Camera>();

            camera.SetLayerVisible(0, false);
            Log("camera.SetLayerVisible(0, false)", camera.IsLayerVisible(0));

            camera.SetLayerVisible(0, true);
            Log("camera.SetLayerVisible(0, true)", camera.IsLayerVisible(0));

            camera.ToggleLayer(0);
            Log("camera.ToggleLayer(0)", camera.IsLayerVisible(0));

            camera.SetLayerVisible("NoSuchLayer", true);
            Log("camera.SetLayerVisible(\"NoSuchLayer\", true)", "no-op, ok");

            camera.ToggleLayer("Default");
            Log("camera.ToggleLayer(\"Default\")", camera.IsLayerVisible(0));
        }

        // ── ScrollRect ──────────────────────────────────────────────────────

        private void ScrollRectSamples()
        {
            Section("ScrollRectExtensions");

            var scrollGo = new GameObject("SampleScrollRect", typeof(RectTransform), typeof(ScrollRect));
            scrollGo.transform.SetParent(transform, false);
            var viewportRect = (RectTransform)scrollGo.transform;
            viewportRect.sizeDelta = new Vector2(200f, 200f);
            var scrollRect = scrollGo.GetComponent<ScrollRect>();

            // Content must be larger than the viewport in both axes for normalized-position math to
            // have a real range to work with (an equal-size content always reports position 0).
            var content = new GameObject("Content", typeof(RectTransform));
            var contentRect = (RectTransform)content.transform;
            contentRect.SetParent(scrollGo.transform, false);
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(0f, 1f);
            contentRect.pivot = new Vector2(0f, 1f);
            contentRect.sizeDelta = new Vector2(600f, 600f);
            scrollRect.content = contentRect;

            scrollRect.ScrollToBottom();
            Log("scrollRect.ScrollToBottom()", scrollRect.verticalNormalizedPosition);

            scrollRect.ScrollVertical(1.5f);
            Log("scrollRect.ScrollVertical(1.5) (clamped)", scrollRect.verticalNormalizedPosition);

            scrollRect.ScrollToLeft();
            Log("scrollRect.ScrollToLeft()", scrollRect.horizontalNormalizedPosition);

            scrollRect.ScrollHorizontal(-0.5f);
            Log("scrollRect.ScrollHorizontal(-0.5) (clamped)", scrollRect.horizontalNormalizedPosition);

            scrollRect.ScrollToTop();
            Log("scrollRect.ScrollToTop()", scrollRect.verticalNormalizedPosition);

            scrollRect.ScrollToRight();
            Log("scrollRect.ScrollToRight()", scrollRect.horizontalNormalizedPosition);
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

        private void SectionRight(string name)
        {
            _outputRight.Add(string.Empty);
            _outputRight.Add("── " + name + " ──");
        }

        private void Log(string label, object value)
        {
            string line = $"{label} = {value}";
            _output.Add(line);
            Debug.Log("[ExtensionsDemo] " + line);
        }

        private void LogRight(string label, object value)
        {
            string line = $"{label} = {value}";
            _outputRight.Add(line);
            Debug.Log("[ExtensionsDemo] " + line);
        }

        private void OnGUI()
        {
            const float padding  = 10f;
            const float colWidth = 480f;
            float height = Screen.height - padding * 2f;

            // Left column — String / Collection / Dictionary / Int / Float / Double / Long / GameObject
            GUILayout.BeginArea(new Rect(padding, padding, colWidth, height), GUI.skin.box);
            GUILayout.Label("◀  Core extensions  (String · Collection · Dictionary · Int · Float · Double · Long · GameObject)");
            _scroll = GUILayout.BeginScrollView(_scroll);
            foreach (string line in _output)
                GUILayout.Label(line);
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            // Right column — DateTimeOffset / TimeSpan
            float rightX = padding + colWidth + padding;
            GUILayout.BeginArea(new Rect(rightX, padding, colWidth, height), GUI.skin.box);
            GUILayout.Label("▶  Date & time extensions  (DateTimeOffset · TimeSpan)");
            _scrollRight = GUILayout.BeginScrollView(_scrollRight);
            foreach (string line in _outputRight)
                GUILayout.Label(line);
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}
