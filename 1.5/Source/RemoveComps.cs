﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace AADMod;

[SuppressMessage("ReSharper", "UnassignedField.Global")]
public class RemoveComps : DefModExtension
{
    public List<Type> compProps;
}
