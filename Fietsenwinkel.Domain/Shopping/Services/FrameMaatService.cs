﻿using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Domain.Shopping.Services;

internal static class FrameMaatService
{
    public static Result<(FrameMaat min, FrameMaat max), ErrorCodeList> DetermineSizesFor(int height) =>
        Result.Combine(
            DetermineMin(height),
            DetermineMax(height)).Map(
                onSuccess: (min, max) => Result<(FrameMaat min, FrameMaat max), ErrorCodeList>.Succeed((min, max)),
                    onFailure: Result<(FrameMaat min, FrameMaat max), ErrorCodeList>.Fail);

    private static Result<FrameMaat, ErrorCodeList> DetermineMin(int height) =>
        FrameMaat.Create(height switch
        {
            <= 164 => 48,
            <= 167 => 49,
            <= 170 => 50,
            <= 173 => 51,
            <= 176 => 52,
            <= 179 => 53,
            <= 182 => 54,
            <= 185 => 55,
            <= 188 => 56,
            <= 190 => 57,
            <= 192 => 58,
            <= 195 => 59,
            <= 197 => 60,
            <= 199 => 61,
            <= 202 => 62,
            <= 204 => 63,
            <= 206 => 64,
            <= 208 => 65,
            _ => 66
        });

    private static Result<FrameMaat, ErrorCodeList> DetermineMax(int height) =>
        FrameMaat.Create(height switch
        {
            >= 195 => 66,
            >= 192 => 65,
            >= 190 => 64,
            >= 188 => 63,
            >= 185 => 62,
            >= 182 => 61,
            >= 179 => 60,
            >= 176 => 59,
            >= 173 => 58,
            >= 170 => 57,
            >= 167 => 56,
            >= 165 => 55,
            >= 163 => 54,
            >= 161 => 53,
            >= 159 => 52,
            >= 157 => 51,
            >= 155 => 50,
            >= 153 => 49,
            _ => 48
        });
}