using System;
using System.Collections.Generic;

namespace VehicleRentingApi.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? CustomerCity { get; set; }

    public string? MobileNo { get; set; }

    public string? Email { get; set; }

}
