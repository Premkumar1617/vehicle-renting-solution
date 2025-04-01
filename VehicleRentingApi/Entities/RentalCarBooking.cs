using System;
using System.Collections.Generic;

namespace VehicleRentingApi.Entities;

public partial class RentalCarBooking
{
    public int BookingId { get; set; }

    public int CustId { get; set; }

    public int CarId { get; set; }

    public DateTime BookingDate { get; set; }

    public int? Discount { get; set; }

    public double? TotalBillAmount { get; set; }

    public string? BookingUid { get; set; }
}
