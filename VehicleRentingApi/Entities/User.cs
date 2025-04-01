﻿using System;
using System.Collections.Generic;

namespace VehicleRentingApi.Entities;

public partial class User
{
    public byte Id { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string Code { get; set; } = null!;
}
