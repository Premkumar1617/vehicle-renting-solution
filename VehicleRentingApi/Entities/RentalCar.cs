using System;
using System.Collections.Generic;

namespace VehicleRentingApi.Entities;

public partial class RentalCar
{
    public int CarId { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public int Year { get; set; }

    public string Color { get; set; } = null!;

    public double DailyRate { get; set; }

    public string? CarImage { get; set; }

    public string? RegNo { get; set; }

}
