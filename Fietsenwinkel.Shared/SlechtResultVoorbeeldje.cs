using System.IO;

namespace Fietsenwinkel.Shared;
public static class SlechtResultVoorbeeldje
{
    private class StandaardResult<TValueType, TErrorType>
    {
        public bool Success { get; }

        public TValueType? Value { get; }

        public TErrorType? Error { get; }

        private StandaardResult(bool success, TValueType? value, TErrorType? error)
        {
            Success = success;
            Value = value;
            Error = error;
        }

        public static StandaardResult<TValueType, TErrorType> Succeed(TValueType value) => new(true, value, default);

        public static StandaardResult<TValueType, TErrorType> Fail(TErrorType error) => new(false, default, error);
    }


    public static string AppendWords(params string[] words)
    {
        var result = AppendWordsInternal(words);

        if (result.Success)
        {
            if (result.Value == null)
            {
                throw new InvalidDataException("result is null on success");
            }

            return result.Value;
        }
        else
        {
            if (result.Error == null)
            {
                throw new InvalidDataException("result error is null on failure");
            }

            return "Error: " + result.Error;
        }
    }

    private static StandaardResult<string, string> AppendWordsInternal(params string[] words)
    {
        string accumulator = words[0];

        for (int n = 1; n < words.Length; n++)
        {
            var appendResult = Append(accumulator, words[n]);

            if (!appendResult.Success)
            {
                if (appendResult.Error == null)
                {
                    throw new InvalidDataException("append result error is null on failure");
                }

                return StandaardResult<string, string>.Fail(appendResult.Error);
            }

            if (appendResult.Value == null)
            {
                throw new InvalidDataException("append result is null on success");
            }

            accumulator = appendResult.Value;
        }

        return StandaardResult<string, string>.Succeed(accumulator);
    }


    private static StandaardResult<string, string> Append(string baseString, string addString)
    {
        if (addString == "paard")
            return StandaardResult<string, string>.Fail("Paarden zijn niet toegestaan");

        return StandaardResult<string, string>.Succeed(baseString + addString);
    }
}


