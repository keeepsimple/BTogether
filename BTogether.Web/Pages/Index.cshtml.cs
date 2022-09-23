using AspNetCoreHero.ToastNotification.Abstractions;
using BTogether.BussinessLayer.IServices;
using BTogether.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BTogether.Web.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILoveService _loveService;
        private readonly IStoryService _storyService;
        private readonly IHobbyService _hobbyService;
        private readonly INotyfService _notyf;

        public IndexModel(UserManager<User> userManager,
            ILoveService loveService,
            IStoryService storyService,
            IHobbyService hobbyService,
            INotyfService notyf)
        {
            _userManager = userManager;
            _loveService = loveService;
            _storyService = storyService;
            _hobbyService = hobbyService;
            _notyf = notyf;
        }

        public User UserView { get; set; }

        public Love LoveView { get; set; }

        public IEnumerable<Hobby> Hobbies { get; set; }

        public IEnumerable<Story> Stories { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public int TogetherDay { get; set; }

        public int LongDistanceLoveDay { get; set; }

        public int FindPartnerAfter { get; set; }

        public double LoveMinute { get; set; }

        public bool IsCreatedLove { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string PartnerId { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            var love = await _loveService.GetLoveByUserId(userId);
            IsCreatedLove = _loveService.CheckUserCreateLove(userId);

            if (love != null)
            {
                if (userId == love.PartnerId)
                {
                    
                    UserView = await _userManager.FindByIdAsync(love.UserId);
                    LoveView = love;
                    Hobbies = await _hobbyService.GetHobbiesByUserId(love.PartnerId);
                    Stories = await _storyService.GetStoryByLoveIdAsync(love.Id);
                    var now = DateTime.Now;
                    var startDate = LoveView.StartDate;
                    var aprtDate = (DateTime)LoveView.ApartDate;
                    TogetherDay = (now - startDate).Days;
                    LongDistanceLoveDay = (now - aprtDate).Days;
                    FindPartnerAfter = now.Year - UserView.BirthYear;
                    LoveMinute = (now - startDate).TotalMinutes;
                    return Page();
                }
                else if (IsCreatedLove)
                {
                    UserView = await _userManager.FindByIdAsync(userId);
                    LoveView = love;
                    Hobbies = await _hobbyService.GetHobbiesByUserId(userId);
                    Stories = await _storyService.GetStoryByLoveIdAsync(LoveView.Id);
                    var now = DateTime.Now;
                    var startDate = LoveView.StartDate;
                    var aprtDate = (DateTime)LoveView.ApartDate;
                    TogetherDay = (now - startDate).Days;
                    LongDistanceLoveDay = (now - aprtDate).Days;
                    FindPartnerAfter = now.Year - UserView.BirthYear;
                    LoveMinute = (now - startDate).TotalMinutes;
                    return Page();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var partner = await _userManager.FindByIdAsync(Input.PartnerId);
            if(partner == null)
            {
                _notyf.Error("This partner not exist!");
                return Page();
            }
            var love = await _loveService.GetLoveByUserId(partner.Id);
            love.PartnerName = user.Fullname;
            love.PartnerId = user.Id;
            await _loveService.UpdateAsync(love);
            _notyf.Success("Enjoy your love <3");
            return Page();
        }
    }
}