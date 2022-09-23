using AspNetCoreHero.ToastNotification.Abstractions;
using BTogether.BussinessLayer.IServices;
using BTogether.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace BTogether.Web.Areas.ConfigPage.Pages
{
    [Authorize]
    public class AddImageToStoryModel : PageModel
    {
        private readonly IStoryService _storyService;
        private readonly IImageMemoryService _imageMemoryService;
        private readonly UserManager<User> _userManager;
        private IWebHostEnvironment _environment;
        private readonly INotyfService _notyf;

        public AddImageToStoryModel(IStoryService storyService, IImageMemoryService imageMemoryService, UserManager<User> userManager, IWebHostEnvironment environment, INotyfService notyf)
        {
            _storyService = storyService;
            _imageMemoryService = imageMemoryService;
            _userManager = userManager;
            _environment = environment;
            _notyf = notyf;
        }

        [Required]
        [DataType(DataType.Upload)]
        [CheckFileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public string ReturnUrl { get; set; }

        public Story StoryView { get; set; }

        public string UserId { get; set; }

        public IEnumerable<ImageMemory> ImageMemories { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Url { get; set; }

            public int StoryId { get; set; }

            public string Description { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int storyId, string returnUrl = null)
        {
            UserId = _userManager.GetUserId(User);
            StoryView = await _storyService.GetByIdAsync(storyId);
            ImageMemories = await _imageMemoryService.GetImageMemoriesByStoryIdAsync(storyId);
            Input = new InputModel
            {
                StoryId = storyId
            };
            returnUrl ??= Url.Content("~/ConfigPage/AddImageToStory?storyId=" + StoryView.Id);
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/ConfigPage/AddImageToStory?storyId=" + Input.StoryId);
            if (FileUpload != null)
            {
                string folderPath = _environment.WebRootPath + "\\images\\" + _userManager.GetUserId(User);
                if (Directory.Exists(folderPath))
                {
                    var file = Path.Combine(_environment.WebRootPath, folderPath, FileUpload.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await FileUpload.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    Directory.CreateDirectory(folderPath);
                    var file = Path.Combine(_environment.WebRootPath, folderPath, FileUpload.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await FileUpload.CopyToAsync(fileStream);
                    }
                }
            }

            var imageMem = new ImageMemory
            {
                StoryId = Input.StoryId,
                UserId = _userManager.GetUserId(User),
                Url = FileUpload.FileName,
                Description = Input.Description
            };

            var result = await _imageMemoryService.AddAsync(imageMem);
            if (result > 0)
            {
                _notyf.Success("Create successfully.");
            }
            else
            {
                _notyf.Error("An error occured. Please try again.");
            }

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> OnGetEditAsync(int id)
        {
            var image = await _imageMemoryService.GetByIdAsync(id);
            return new JsonResult(image);
        }

        public async Task<IActionResult> OnPostEditAsync(int id, string returnUrl = null)
        {
            var image = await _imageMemoryService.GetByIdAsync(id);
            returnUrl ??= Url.Content("~/ConfigPage/AddImageToStory?storyId=" + image.StoryId);
            image.Description = Input.Description;
            var result = await _imageMemoryService.UpdateAsync(image);
            if (result)
            {
                _notyf.Success("Update successfully.");
            }
            else
            {
                _notyf.Error("An error occured. Please try again.");
            }
            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            var image = await _imageMemoryService.GetByIdAsync(id);
            return new JsonResult(image);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, string returnUrl = null)
        {
            var image = await _imageMemoryService.GetByIdAsync(id);
            returnUrl ??= Url.Content("~/ConfigPage/AddImageToStory?storyId=" + image.StoryId);
            string folderPath = _environment.WebRootPath + "\\images\\" + image.UserId + "\\" + image.Url;
            System.IO.File.Delete(folderPath);
            var result = await _imageMemoryService.DeleteAsync(image);
            if (result)
            {
                _notyf.Success("Delete successfully.");
            }
            else
            {
                _notyf.Error("An error occured. Please try again.");
            }
            return LocalRedirect(returnUrl);
        }
    }
}
