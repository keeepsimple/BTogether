using AspNetCoreHero.ToastNotification.Abstractions;
using BTogether.BussinessLayer.IServices;
using BTogether.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace BTogether.Web.Areas.ConfigPage.Pages
{
    [Authorize]
    public class HobbyConfigModel : PageModel
    {
        private readonly IHobbyService _hobbyService;
        private readonly ILoveService _loveService;
        private readonly UserManager<User> _userManager;
        private readonly INotyfService _notyf;

        public HobbyConfigModel(IHobbyService hobbyService, UserManager<User> userManager, ILoveService loveService, INotyfService notyf)
        {
            _hobbyService = hobbyService;
            _userManager = userManager;
            _loveService = loveService;
            _notyf = notyf;
        }

        public IEnumerable<Hobby> Hobbies { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [DisplayName("Hobby Text")]
            public string HobbyText { get; set; }

            [DisplayName("Her or His")]
            public bool HerHis { get; set; }

            public int LoveId { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            var loveCreated = _loveService.CheckUserCreateLove(userId);
            if (loveCreated)
            {
                Hobbies = await _hobbyService.GetHobbiesByUserId(userId);
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
            var loveId = await _loveService.GetLoveIdByUserId(_userManager.GetUserId(User));
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var hob = new Hobby
            {
                HerHis = Input.HerHis,
                HobbyText = Input.HobbyText,
                LoveId = loveId
            };
            var result = await _hobbyService.AddAsync(hob);
            if (result > 0)
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
            var hob = await _hobbyService.GetByIdAsync(id);
            return new JsonResult(hob);
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            var hob = await _hobbyService.GetByIdAsync(id);
            hob.HobbyText = Input.HobbyText;
            var result = await _hobbyService.UpdateAsync(hob);
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
            var hob = await _hobbyService.GetByIdAsync(id);
            return new JsonResult(hob);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var hob = await _hobbyService.GetByIdAsync(id);
            var result = await _hobbyService.DeleteAsync(hob);
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
