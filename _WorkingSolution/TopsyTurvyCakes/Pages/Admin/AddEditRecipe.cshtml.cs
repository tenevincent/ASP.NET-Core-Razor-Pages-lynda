using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopsyTurvyCakes.Models;

namespace TopsyTurvyCakes.Pages.Admin
{

    [Authorize]
    public class AddEditRecipeModel : PageModel
    {
        private IRecipesService _recipesService;

        public AddEditRecipeModel(IRecipesService recipesService)
        {
            this._recipesService = recipesService;
        }

        [FromRoute]
        public long? Id { get; set; }

        public bool IsNewRecipe
        {
            get { return Id == null; }
        }


        [BindProperty]
        public Recipe Recipe { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        public async Task OnGetAsync()
        {
            this.Recipe = await _recipesService.FindAsync(this.Id.GetValueOrDefault()) ?? new Recipe();
              
        }


        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var recipe = await _recipesService.FindAsync(Id.GetValueOrDefault())
                ?? new Recipe();

            recipe.Name = Recipe.Name;
            recipe.Description = Recipe.Description;
            recipe.Ingredients = Recipe.Ingredients;
            recipe.Directions = Recipe.Directions;

            if (Image != null)
            {
                using (var stream = new System.IO.MemoryStream())
                {
                    await Image.CopyToAsync(stream);

                    recipe.Image = stream.ToArray();
                    recipe.ImageContentType = Image.ContentType;
                }
            }

            await _recipesService.SaveAsync(recipe);
            return RedirectToPage("/Recipe", new { id = recipe.Id });
        }



        public async Task<IActionResult> OnPostDelete()
        {
            await _recipesService.DeleteAsync(Id.Value);
            return RedirectToPage("/Index");
        }




    }
}