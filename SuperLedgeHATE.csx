EnsureDataLoaded();

static Random rng = new Random();

static void Shuffle<T>(this IList<T> list)
{
    int n = list.Count;
    while (n > 1)
    {
        n--;
        int k = rng.Next(n + 1);

        T value = list[k];
        list[k] = list[n];
        list[n] = value;
    }
}

void StringSwap(int n, int k)
{
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

static void ShuffleOnlySelected<T>(this IList<T> list, IList<int> selected, Action<int, int> swapFunc)
{
    int n = selected.Count;
    while (n > 1)
    {
        n--;
        int k = rng.Next(n + 1);

        swapFunc(selected[n], selected[k]);

        int idx = selected[k];
        selected[k] = selected[n];
        selected[n] = idx;
    }
}

static void ShuffleOnlySelected<T>(this IList<T> list, IList<int> selected)
{
    list.ShuffleOnlySelected(selected, (n, k) => {
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
    });
}

static void SelectSome(this IList<int> list, float amountToKeep)
{
    int toRemove = (int)(list.Count * (1 - amountToKeep));
    for (int i = 0; i < toRemove; i++)
        list.RemoveAt(rng.Next(list.Count));
}

List<int> tiny = new List<int>();
List<int> small = new List<int>();
List<int> characterLike = new List<int>();
List<int> big = new List<int>();
for (int i = 0; i < Data.Sprites.Count; i++)
{
    var sprite = Data.Sprites[i];
    if (sprite.Name.Content.StartsWith("bg_"))
        continue;
    if (sprite.Name.Content.StartsWith("spr_chizuru"))
        continue; // Sorry but corrupting Chizuru makes it kinda unplayable
    if (sprite.Width < 50 && sprite.Height < 50)
        tiny.Add(i);
    else if (sprite.Width < 50 && sprite.Height < 100)
        characterLike.Add(i);
    else if (sprite.Width < 100 && sprite.Height < 100)
        small.Add(i);
    else if (sprite.Width < 500 && sprite.Height < 500)
        big.Add(i);
}
Data.Sprites.ShuffleOnlySelected(tiny);
Data.Sprites.ShuffleOnlySelected(small);
Data.Sprites.ShuffleOnlySelected(characterLike);
Data.Sprites.ShuffleOnlySelected(big);

Data.Sounds.Shuffle();

List<int> en_fonts = new List<int>();
List<int> ja_fonts = new List<int>();
List<int> foreign_fonts = new List<int>();
for (int i = 0; i < Data.Fonts.Count; i++)
{
    if (Data.Fonts[i].Name.Content.Contains("japanese"))
        ja_fonts.Add(i);
    else if (Data.Fonts[i].Name.Content.Contains("foreign"))
        foreign_fonts.Add(i);
    else
        en_fonts.Add(i);
}
Data.Fonts.ShuffleOnlySelected(en_fonts);
Data.Fonts.ShuffleOnlySelected(foreign_fonts);
Data.Fonts.ShuffleOnlySelected(ja_fonts);

// lazy and slow approach
List<string> names = new List<string>();
List<int> strings = new List<int>();
for (int i = 0; i < Data.Variables.Count; i++)
{
    names.Add(Data.Variables[i].Name.Content);
}
for (int i = 0; i < Data.Functions.Count; i++)
{
    names.Add(Data.Functions[i].Name.Content);
}
for (int i = 0; i < Data.Options.Constants.Count; i++)
{
    names.Add(Data.Options.Constants[i].Name.Content);
}

for (int i = 0; i < Data.Strings.Count; i++)
{
    // not adding underscores to some because of "background13" and "timeline4"
    if (!names.Exists(e => e.EndsWith(Data.Strings[i].Content)) && Data.Strings[i].Content.StartsWithArray(new string[] {"gml_", "rm_", "obj_", "snd_", "time", "fnt_", "scr_", "path_", "back", "spr_"})) {
        strings.Add(i);
    }
}

Data.Strings.ShuffleOnlySelected(strings, StringSwap);

ScriptMessage("done");