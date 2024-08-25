﻿namespace LibraryAPI.Entities.DTOs.PublisherAddressDTO
{
    public class PublisherAddressResponse
    {
        public int PublisherAddressId { get; set; }

        public string? Street { get; set; }

        public string City { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;
    }
}
