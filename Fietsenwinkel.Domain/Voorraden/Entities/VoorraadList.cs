﻿using Fietsenwinkel.Domain.Filialen.Entities;

namespace Fietsenwinkel.Domain.Voorraden.Entities;
public record VoorraadList(FiliaalId FiliaalId, VoorraadListEntry[] VoorraadEntries);