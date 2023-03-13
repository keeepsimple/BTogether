// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using AspNetCoreHero.ToastNotification.Abstractions;
using BTogether.BussinessLayer.IServices;
using BTogether.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenAI_API;
using OpenAI_API.Completions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BTogether.Web.Areas.Identity.Pages.Account.Manage {
    [Authorize]
    public class IndexModel : PageModel {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IWebHostEnvironment _environment;
        private readonly INotyfService _notyf;
        private readonly ILoveService _loveService;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IWebHostEnvironment environment,
            INotyfService notyf,
            ILoveService loveService) {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _notyf = notyf;
            _loveService = loveService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        public string UserId { get; set; }

        public string Email { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        [DataType(DataType.Upload)]
        [CheckFileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [BindProperty]
        public IFormFile FileUpload { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>

            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Required]
            [Display(Name = "Birth Year")]
            public int BirthYear { get; set; }

            [DisplayName("Image display in Home Page")]
            public string ImageUrl { get; set; }

            [DisplayName("This text will translate by OpenAI to 50 languages. It will take time to translate please be patient!!!")]
            public string DisplayText { get; set; }
        }

        private async Task LoadAsync(User user) {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;
            Email = user.Email;

            Input = new InputModel {
                FullName = user.Fullname,
                BirthYear = user.BirthYear,
                ImageUrl = user.HomeImage,
                DisplayText = user.DisplayText
            };
        }

        public async Task<IActionResult> OnGetAsync() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            UserId = user.Id;

            await LoadAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid) {
                await LoadAsync(user);
                return Page();
            }

            if (FileUpload != null) {
                string folderPath = _environment.WebRootPath + "\\images\\" + _userManager.GetUserId(User);
                if (Directory.Exists(folderPath)) {
                    var file = Path.Combine(_environment.WebRootPath, folderPath, FileUpload.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create)) {
                        await FileUpload.CopyToAsync(fileStream);
                    }
                    if (user.HomeImage.Equals(FileUpload.FileName)) {
                        System.IO.File.Delete(folderPath + "\\" + user.HomeImage);
                    }
                    user.HomeImage = FileUpload.FileName;

                }
                else {
                    Directory.CreateDirectory(folderPath);
                    var file = Path.Combine(_environment.WebRootPath, folderPath, FileUpload.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create)) {
                        await FileUpload.CopyToAsync(fileStream);
                    }
                    if (user.HomeImage.Equals(FileUpload.FileName)) {
                        System.IO.File.Delete(folderPath + "\\" + user.HomeImage);
                    }
                    user.HomeImage = FileUpload.FileName;
                }
            }

            user.BirthYear = Input.BirthYear;
            user.Fullname = Input.FullName;

            var list = new List<string>();
            var text = "";
            var apikey = "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";
            var openAi = new OpenAIAPI(apikey);
            var completion = new CompletionRequest();
            completion.Prompt = $"Translate \"{Input.DisplayText}\" to 50 languages separated by comma";
            completion.Model = OpenAI_API.Models.Model.DavinciText;
            completion.MaxTokens = 4000;
            var resultOpenAI = openAi.Completions.CreateCompletionAsync(completion);
            if (resultOpenAI != null) {
                foreach (var item in resultOpenAI.Result.Completions) {
                    text = item.Text;
                }
            }

            foreach (var item in text.Split(',')) {
                list.Add(item.Substring(item.IndexOf(':') + 1));
            }
            user.DisplayText = String.Join(',', list);

            var love = await _loveService.GetLoveByUserId(user.Id);
            if (love != null && love.PartnerId == user.Id) {
                love.PartnerName = Input.FullName;
                await _loveService.UpdateAsync(love);
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) {
                _notyf.Success("Update successfully.");
            }
            else {
                _notyf.Error("An error occurred. Please try again.");
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
