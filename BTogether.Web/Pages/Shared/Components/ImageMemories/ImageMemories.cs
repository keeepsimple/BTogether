using BTogether.BussinessLayer.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BTogether.Web.Pages.Shared.Components.ImageMemories
{
    public class ImageMemories : ViewComponent
    {
        private readonly IImageMemoryService _imageMemoryService;

        public ImageMemories(IImageMemoryService imageMemoryService)
        {
            _imageMemoryService = imageMemoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var images = await _imageMemoryService.GetImageMemoriesByStoryIdAsync(id);
            return View(images);
        }
    }
}
