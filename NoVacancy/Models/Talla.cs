﻿using System.ComponentModel.DataAnnotations;

namespace NoVacancy.Models
{
    public class Talla
    {

        [Key]
        public int idTalla { get; set; }

        [Required]
        public string? nombre { get; set; }

    }
}
