﻿using System.ComponentModel.DataAnnotations;

namespace Uniqlo.ViewModels.Slider
{
    public class CreateSliderVM
    {
        [MaxLength(32, ErrorMessage = "Title length must be less than 32"), Required(ErrorMessage = "Basliq yazmaq vacibdir")]
        public string Title { get; set; }
        [MaxLength(64), Required]
        public string Subtitle { get; set; }
        public string? Link { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}