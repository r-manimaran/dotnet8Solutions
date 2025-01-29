﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreApi.Data;

public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter():base(v=>v.ToUniversalTime(),
        v=> DateTime.SpecifyKind(v,DateTimeKind.Utc))
    {

    }
}
