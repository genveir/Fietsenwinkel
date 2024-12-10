using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Shared;
public static class ResultVoorbeeldje
{
    public static string AppendWords(params string[] words)
    {
        return AppendWordsInternal(words).Map(
            onSuccess: s => s,
            onFailure: e => "Error: " + e);
    }

    private static Result<string, string> AppendWordsInternal(params string[] words)
    {
        var result = Result<string, string>.Succeed(words[0]);

        for (int n = 1; n < words.Length; n++)
        {
            result = result.Map(
                onSuccess: accumulator => Append(accumulator, words[n]),
                onFailure: Result<string, string>.Fail);
        }

        return result;
    }

    private static Result<string, string> Append(string baseString, string addString)
    {
        if (addString == "paard")
            return Result<string, string>.Fail("Paarden zijn niet toegestaan");

        return Result<string, string>.Succeed(baseString + addString);
    }
}
