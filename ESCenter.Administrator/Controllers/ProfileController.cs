using ESCenter.Administrator.Models;
using ESCenter.Administrator.Utilities;
using ESCenter.Application.Contracts.Users.Learners;
using ESCenter.Application.ServiceImpls.Accounts.Commands.ChangePassword;
using ESCenter.Application.ServiceImpls.Accounts.Commands.CreateUpdateBasicProfile;
using ESCenter.Application.ServiceImpls.Accounts.Queries.GetUserProfile;
using ESCenter.Application.ServiceImpls.Accounts.Queries.Login;
using ESCenter.Domain.Shared;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Controllers
{
    [Route("[controller]")]
    public class ProfileController(
        IMapper mapper,
        ISender sender,
        IWebHostEnvironment webHostEnvironment)
        : Controller
    {
        private void PackStaticListToView()
        {
            ViewData["Roles"] = EnumProvider.Roles;
            ViewData["Genders"] = EnumProvider.Genders;
            ViewData["AcademicLevels"] = EnumProvider.AcademicLevels;
        }

        [HttpGet("")]
        public async Task<IActionResult> Profile()
        {
            PackStaticListToView();

            var learnerProfile = await sender.Send(new GetUserProfileQuery());

            if (learnerProfile is { IsSuccess: true, Value: not null })
            {
                return View(new ProfileViewModel
                {
                    LearnerForProfileDto = learnerProfile.Value,
                });
            }

            return RedirectToAction("Index", "Authentication", new LoginQuery("", ""));
        }

        [HttpPost("change-avatar")]
        public async Task<IActionResult> ChoosePicture(IFormFile formFile)
        {
            await Task.CompletedTask;
            // TODO: on maintaining
            // if (formFile.Length > 0)
            // {
            //     string fileName = formFile.FileName;
            //     string path = Path.Combine(webHostEnvironment.WebRootPath + "\\temp\\", fileName);
            //     
            //     await using var fileStream = new FileStream(path, FileMode.Create);
            //     await formFile.CopyToAsync(fileStream);
            //     fileStream.Position = 0;
            //
            //     var changeAvatarCommand = new ChangeAvatarCommand(fileStream, fileName);
            //        
            //     var changePictureResult = await sender.Send(changeAvatarCommand);
            //     if (changePictureResult.IsSuccess)
            //     {
            //         HttpContext.Session.SetString("image", fileName);
            //         return Json(new { res = true, image = fileName });
            //     }
            // }

            return BadRequest();
        }

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LearnerForCreateUpdateDto userDto) //, IFormFile? formFile)
        {
            PackStaticListToView();

            if (!ModelState.IsValid)
                return Helper.RenderRazorViewToString(this, "_ProfileEdit",
                    userDto,
                    true
                );
            try
            {
                var result = await sender.Send(new CreateUpdateBasicProfileCommand(userDto));
                ViewBag.Updated = result;
                Helper.ClearTempFile(webHostEnvironment.WebRootPath);

                if (result.IsSuccess && result.Value != null)
                {
                    HttpContext.Session.SetString("name", result.Value.User.FullName);
                    LearnerForProfileDto learnerForProfileDto = mapper.Map<LearnerForProfileDto>(userDto);

                    return Helper.RenderRazorViewToString(this, "Profile", new ProfileViewModel
                    {
                        LearnerForProfileDto = learnerForProfileDto
                    });
                }
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists, " + ex.Message +
                                             "see your system administrator.");
            }

            return Helper.RenderRazorViewToString(this, "_ProfileEdit",
                userDto,
                true
            );
        }

        [HttpPost("ChangePassword")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand changePasswordCommand)
        {
            if (!ModelState.IsValid)
            {
                return Helper.RenderRazorViewToString(this, "_ChangePassword", changePasswordCommand, true);
            }

            try
            {
                var loginResult = await sender.Send(changePasswordCommand);

                if (loginResult.IsSuccess)
                {
                    return Helper.RenderRazorViewToString(this, "_ChangePassword"
                    );
                }
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists, " + ex.Message +
                                             "see your system administrator.");
            }

            return Helper.RenderRazorViewToString(this, "_ChangePassword", changePasswordCommand, true);
        }
    }
}