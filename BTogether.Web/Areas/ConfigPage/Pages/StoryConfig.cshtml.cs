using AspNetCoreHero.ToastNotification.Abstractions;
using BTogether.BussinessLayer.IServices;
using BTogether.BussinessLayer.Services;
using BTogether.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BTogether.Web.Areas.ConfigPage.Pages
{
    [Authorize]
    public class StoryConfigModel : PageModel
    {
        private readonly IStoryService _storyService;
        private readonly IImageMemoryService _imageMemoryService;
        private readonly ILoveService _loveService;
        private readonly UserManager<User> _userManager;
        private readonly INotyfService _notyf;

        public StoryConfigModel(IStoryService storyService, IImageMemoryService imageMemoryService, UserManager<User> userManager, ILoveService loveService, INotyfService notyf)
        {
            _storyService = storyService;
            _imageMemoryService = imageMemoryService;
            _userManager = userManager;
            _loveService = loveService;
            _notyf = notyf;
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
            var userId = _userManager.GetUserId(User);
            var loveCreated = _loveService.CheckUserCreateLove(userId);
            if (loveCreated)
            {
                var loveId = await _loveService.GetLoveIdByUserId(_userManager.GetUserId(User));
                Stories = await _storyService.GetStoryByLoveIdAsync(loveId);
                return Page();
            }
            else
            {
                _notyf.Error("You must config How Long first to access all config");
                return RedirectToPage("/HowLong");
            }
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
            var result = await _storyService.AddAsync(story);
            if(result > 0)
            {
                _notyf.Success("Create successfully.");
            }
            else
            {
                _notyf.Error("An error occured. Please try again.");
            }
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
            var result = await _storyService.UpdateAsync(story);
            if (result)
            {
                _notyf.Success("Update successfully.");
            }
            else
            {
                _notyf.Error("An error occured. Please try again.");
            }
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
            var result = await _storyService.DeleteAsync(story);
            if (result)
            {
                _notyf.Success("Delete successfully.");
            }
            else
            {
                _notyf.Error("An error occured. Please try again.");
            }
            return RedirectToPage();
        }
    }
}
