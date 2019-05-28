﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Environment;

namespace TopsyTurvyCakes.Models
{
    public class Recipe
    {
        public long Id { get; set; }

        [Required]
        //[MinLength(5)]
        //[MaxLength(25)]
        [StringLength(100,MinimumLength =5, ErrorMessage ="The name must be between 5 and 100!")]
        public string Name { get; set; }

        [Required(ErrorMessage ="The recipe description is required")]
        public string Description { get; set; }

        [Required]
        public string Directions { get; set; }

        [Required]
        public string Ingredients { get; set; }



        public IEnumerable<string> DirectionsList
        {
            get { return (Directions ?? string.Empty).Split(NewLine); }
        }

        public IEnumerable<string> IngredientsList
        {
            get { return (Ingredients ?? string.Empty).Split(NewLine); }
        }

        #region Image

        public byte[] Image { get; set; }

        public string ImageContentType { get; set; }

        public string GetInlineImageSrc ()
        {
            if (Image == null || ImageContentType == null)
                return null;

            var base64Image = System.Convert.ToBase64String(Image);
            return $"data:{ImageContentType};base64,{base64Image}";
        }

        public void SetImage(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null)
                return;

            ImageContentType = file.ContentType;

            using (var stream = new System.IO.MemoryStream())
            {
                file.CopyTo(stream);
                Image = stream.ToArray();
            }
        }

        #endregion
    }
}
