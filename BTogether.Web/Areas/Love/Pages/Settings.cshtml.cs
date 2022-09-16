using BTogether.BussinessLayer.IServices;
using BTogether.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BTogether.Web.Areas.Love.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly ILoveService _loveService;
        private readonly UserManager<User> _userManager;

        public SettingsModel(ILoveService loveService, UserManager<User> userManager)
        {
            _loveService = loveService;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public DateTime StartDate { get; set; }

            public DateTime ApartDate { get; set; }

            public string? UserId { get; set; }

            public string LastModify { get; set; }

            public bool IsCreated { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            var userCreated = _loveService.CheckUserCreateLove(userId);
            Input = new InputModel
            {
                IsCreated = userCreated
            };
            return Page();
        }
    }
}
