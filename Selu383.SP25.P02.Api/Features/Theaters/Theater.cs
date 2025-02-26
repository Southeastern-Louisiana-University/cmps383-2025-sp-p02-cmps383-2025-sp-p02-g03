﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Selu383.SP25.P02.Api.Features.Identity;

namespace Selu383.SP25.P02.Api.Features.Theaters
{
    public class Theater
    {
        public int Id { get; set; }
        [MaxLength(120)]
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int SeatCount { get; set; }
        public int? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public User? Manager { get; set; }
    }
}
