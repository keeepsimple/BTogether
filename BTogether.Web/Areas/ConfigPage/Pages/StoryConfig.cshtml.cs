using BTogether.BussinessLayer.IServices;
using BTogether.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BTogether.Web.Areas.ConfigPage.Pages
{
    public class StoryConfigModel : PageModel
    {
        private readonly IStoryService _storyService;
        private readonly IImageMemoryService _imageMemoryService;
        private readonly ILoveService _loveService;
        private readonly UserManager<User> _userManager;

        public StoryConfigModel(IStoryService storyService, IImageMemoryService imageMemoryService, UserManager<User> userManager, ILoveService loveService)
        {
            _storyService = storyService;
            _imageMemoryService = imageMemoryService;
            _userManager = userManager;
            _loveService = loveService;
        }

        public IEnumerable<Story> Stories { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Title { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var loveId = await _loveService.GetLoveIdByUserId(_userManager.GetUserId(User));
            Stories = await _storyService.GetStoryByLoveIdAsync(loveId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var loveId = await _loveService.GetLoveIdByUserId(_userManager.GetUserId(User));
            var story = new Story
            {
                Title = Input.Title,
                LoveId = loveId
            };
            await _storyService.AddAsync(story);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetEditAsync(int id)
        {
            var story = await _storyService.GetByIdAsync(id);
            return new JsonResult(story);
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            var story = await _storyService.GetByIdAsync(id);
            story.Title = Input.Title;
            await _storyService.UpdateAsync(story);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            var story = await _storyService.GetByIdAsync(id);
            return new JsonResult(story);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var story = await _storyService.GetByIdAsync(id);
            await _storyService.DeleteAsync(story);
            return RedirectToPage();
        }
    }
}
