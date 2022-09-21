using BTogether.BussinessLayer.IServices;
using BTogether.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace BTogether.Web.Areas.ConfigPage.Pages
{
    public class HobbyConfigModel : PageModel
    {
        private readonly IHobbyService _hobbyService;
        private readonly ILoveService _loveService;
        private readonly UserManager<User> _userManager;

        public HobbyConfigModel(IHobbyService hobbyService, UserManager<User> userManager, ILoveService loveService)
        {
            _hobbyService = hobbyService;
            _userManager = userManager;
            _loveService = loveService;
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
            var loveCreated = _loveService.CheckUserCreateLove(_userManager.GetUserId(User));
            if (loveCreated)
            {
                Hobbies = await _hobbyService.GetHobbiesByUserId(_userManager.GetUserId(User));
                return Page();
            }
            else
            {
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
            await _hobbyService.AddAsync(hob);
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
            hob.HerHis = Input.HerHis;
            hob.HobbyText = Input.HobbyText;
            await _hobbyService.UpdateAsync(hob);
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
            await _hobbyService.DeleteAsync(hob);
            return RedirectToPage();
        }
    }
}
