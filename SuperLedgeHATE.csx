EnsureDataLoaded();

static int seed;

static string path = "./seed";
if (File.Exists(path)) {
	using (StreamReader reader = new StreamReader(path)) {
		seed = Convert.ToInt32(reader.ReadLine());
		if (seed != 0) {
			ScriptMessage("using seed from ./seed...");
		} else {
			seed = Guid.NewGuid().GetHashCode();
		}
	}
} else {
	seed = Guid.NewGuid().GetHashCode();
}

static Random rng = new Random(seed);

static void Shuffle<T>(this IList<T> list) {
	int n = list.Count;
	while (n > 1) {
		n--;
		int k = rng.Next(n + 1);

		T value = list[k];
		list[k] = list[n];
		list[n] = value;
	}
}

void StringSwap(int n, int k) {
	string value = Data.Strings[k].Content;
	Data.Strings[k].Content = Data.Strings[n].Content;
	Data.Strings[n].Content = value;
}

static bool StartsWithArray(this string str, string[] with) {
	foreach (string member in with) {
		if (str.StartsWith(member)) {
			return false;
		}
	}
	return true;
}

static void ShuffleOnlySelected<T>(this IList<T> list, IList<int> selected, Action<int, int> swapFunc) {
	int n = selected.Count;
	while (n > 1) {
		n--;
		int k = rng.Next(n + 1);

		swapFunc(selected[n], selected[k]);

		int idx = selected[k];
		selected[k] = selected[n];
		selected[n] = idx;
	}
}

static void ShuffleOnlySelected<T>(this IList<T> list, IList<int> selected) {
	list.ShuffleOnlySelected(selected,(n, k) => {
		T value = list[k];
		list[k] = list[n];
		list[n] = value;
	});
}

/*List<int> sprites = new List<int>();
for(int i = 0; i < Data.Sprites.Count; i++)
{
	var sprite = Data.Sprites[i];
	if(sprite.Name.Content.StartsWith("spr_chizuru"))
		continue;
	else sprites.Add(i);
}
Data.Sprites.ShuffleOnlySelected(sprites);
*/

Data.Sprites.Shuffle();
Data.Sounds.Shuffle();
Data.Fonts.Shuffle();

// kinda slow
List<string> names = new List<string>();
List<int> strings = new List<int>();
for (int i = 0; i < Data.Variables.Count; i++) {
	string item = Data.Variables[i].Name.Content;
	if (!names.Contains(item))
		names.Add(item);
}
for (int i = 0; i < Data.Functions.Count; i++) {
	string item = Data.Functions[i].Name.Content;
	if (!names.Contains(item))
		names.Add(item);
}
for (int i = 0; i < Data.Options.Constants.Count; i++) {
	string item = Data.Options.Constants[i].Name.Content;
	if (!names.Contains(item))
		names.Add(item);
}

for (int i = 0; i < Data.Strings.Count; i++) {
	// not adding underscores to some because of "background13" and "timeline4"
	if (!names.Exists(e => e.EndsWith(Data.Strings[i].Content)) && Data.Strings[i].Content.StartsWithArray(new string[] { "gml_", "rm_", "obj_", "snd_", "time", "fnt_", "scr_", "path_", "back", "spr_", "soulOfC", "SuperLedgehop" })) {
		strings.Add(i);
	}
}

Data.Strings.ShuffleOnlySelected(strings, StringSwap);

using(StreamWriter writer = new StreamWriter("./lastSeed")) {
	writer.WriteLine(Convert.ToString(seed));
}

ScriptMessage(String.Format("done\n\nseed: {0}\nseed is also saved to ./lastSeed", seed));