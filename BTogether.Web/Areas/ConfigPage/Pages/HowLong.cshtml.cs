using BTogether.BussinessLayer.IServices;
using BTogether.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace BTogether.Web.Areas.ConfigPage.Pages
{
    public class HowLongsModel : PageModel
    {
        private readonly ILoveService _loveService;
        private readonly UserManager<User> _userManager;

        public HowLongsModel(ILoveService loveService, UserManager<User> userManager)
        {
            _loveService = loveService;
            _userManager = userManager;
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
                loveExist.PartnerName = Input.PartnerName;
                loveExist.PartnerId = Input.PartnerId;
                loveExist.LastModify = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                await _loveService.UpdateAsync(loveExist);
            }
            else
            {
                var love = new Love
                {
                    ApartDate = Input.ApartDate,
                    StartDate = Input.StartDate,
                    PartnerName = Input.PartnerName,
                    UserId = _userManager.GetUserId(User),
                    PartnerId = Input.PartnerId,
                    LastModify = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                };
                await _loveService.AddAsync(love);
            }

            return RedirectToPage();
        }
    }
}
