using BTogether.BussinessLayer.IServices;
using BTogether.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace BTogether.Web.Areas.ConfigPage.Pages
{
    public class AddImageToStoryModel : PageModel
    {
        private readonly IStoryService _storyService;
        private readonly IImageMemoryService _imageMemoryService;
        private readonly UserManager<User> _userManager;
        private IWebHostEnvironment _environment;

        public AddImageToStoryModel(IStoryService storyService, IImageMemoryService imageMemoryService, UserManager<User> userManager, IWebHostEnvironment environment)
        {
            _storyService = storyService;
            _imageMemoryService = imageMemoryService;
            _userManager = userManager;
            _environment = environment;
        }

        [Required]
        [DataType(DataType.Upload)]
        [CheckFileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public Story StoryView { get; set; }

        public string UserId { get; set; }

        public IEnumerable<ImageMemory> ImageMemories { get; set; }

        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Description { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int storyId)
        {
            UserId = _userManager.GetUserId(User);
            StoryView = await _storyService.GetByIdAsync(storyId);
            ImageMemories = await _imageMemoryService.GetImageMemoriesByStoryIdAsync(storyId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (FileUpload != null)
            {
                string folderPath = _environment.WebRootPath +"\\"+ UserId;
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
                StoryId = StoryView.Id,
                Url = FileUpload.FileName,
                Description = Input.Description
            };

            await _imageMemoryService.AddAsync(imageMem);

            return RedirectToPage();
        }
    }
}
