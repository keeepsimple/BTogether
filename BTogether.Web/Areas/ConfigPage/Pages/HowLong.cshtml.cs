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
    public class HowLongsModel : PageModel
    {
        private readonly ILoveService _loveService;
        private readonly UserManager<User> _userManager;
        private readonly INotyfService _notyf;

        public HowLongsModel(ILoveService loveService, UserManager<User> userManager, INotyfService notyf)
        {
            _loveService = loveService;
            _userManager = userManager;
            _notyf = notyf;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string CurrentUserId { get; set; }

        public class InputModel
        {
            [DisplayName("Started Date")]
            public DateTime StartDate { get; set; }

            [DisplayName("Apart Date")]
            public DateTime? ApartDate { get; set; }

            [DisplayName("Partner Name")]
            public string? PartnerName { get; set; }

            [DisplayName("Partner Id")]
            public string? PartnerId { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUserId = _userManager.GetUserId(User);
            var loveCreated = _loveService.CheckUserCreateLove(CurrentUserId);
            if (loveCreated)
            {
                var all = await _loveService.GetAllAsync();
                var loveExist = all.FirstOrDefault();
                Input = new InputModel
                {
                    ApartDate = loveExist.ApartDate,
                    StartDate = loveExist.StartDate,
                    PartnerId = loveExist.PartnerId,
                    PartnerName = loveExist.PartnerName
                };
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(User);
            var loveCreated = _loveService.CheckUserCreateLove(userId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (loveCreated)
            {
                var all = await _loveService.GetAllAsync();
                var loveExist = all.FirstOrDefault();
                loveExist.ApartDate = Input.ApartDate;
                loveExist.StartDate = Input.StartDate;
                loveExist.LastModify = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                var result = await _loveService.UpdateAsync(loveExist);
                if (result)
                {
                    _notyf.Success("Update successfully.");
                }
                else
                {
                    _notyf.Error("An error occured. Please try again.");
                }
            }
            else
            {
                var love = new Love
                {
                    ApartDate = Input.ApartDate,
                    StartDate = Input.StartDate,
                    UserId = _userManager.GetUserId(User),
                    LastModify = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                };
                var result = await _loveService.AddAsync(love);
                if (result > 0)
                {
                    _notyf.Success("Create successfully.");
                }
                else
                {
                    _notyf.Error("An error occured. Please try again.");
                }
            }

            return RedirectToPage();
        }
    }
}
