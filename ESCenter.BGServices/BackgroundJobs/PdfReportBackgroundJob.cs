using System.Text;
using ESCenter.Persistence.EntityFrameworkCore;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces.Emails;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace ESCenter.BGServices.BackgroundJobs;

[DisallowConcurrentExecution] // Mark that this job can't be run in parallel
internal class PdfReportBackgroundJob(
    AppDbContext appDbContext,
    IEmailSender emailSender) : IJob
{
    private async Task<Result> GeneratePdfAsync()
    {
        // Get all subscriber's mails
        var mails = await appDbContext
            .Subscribers
            .Select(x => x.Email)
            .ToListAsync();

        // Get today's courses
        var todayCourses = await appDbContext
            .Courses
            .Where(x => x.CreationTime.Date == DateTime.Now.Date)
            .ToListAsync();

        // Select all user that is tutor and in the list mails
        var receivers = await appDbContext.Customers
            .Where(x => mails.Contains(x.Email))
            .Join(appDbContext.Tutors,
                cus => cus.Id,
                tutor => tutor.CustomerId,
                (customer, tutor) => new
                {
                    mail = customer.Email,
                    SubjectIds = tutor
                        .TutorMajors
                        .Select(x => x.SubjectId.Value)
                })
            .ToListAsync();

        // Join 


        var test = mails
            .Except(receivers.Select(x => x.mail))
            .Select(mail => new
            {
                mail,
                SubjectIds = Enumerable.Empty<int>()
            });

        receivers.AddRange(test);

        // Send
        foreach (var receiver in receivers)
        {
            var subjectIds = receiver.SubjectIds.ToList();
            var realTodayClasses = todayCourses;
            if (subjectIds.Count != 0)
            {
                // Select course related to major
                realTodayClasses = realTodayClasses
                    .OrderByDescending(x => subjectIds.Contains(x.SubjectId.Value))
                    .ToList();
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "<div class=\"section\">\r\n    <div class=\"card\">\r\n        <div class=\"card-body\">");
            foreach (var course in realTodayClasses)
            {
                stringBuilder.Append(
                    $" <h3><a asp-controller=\"Course\" asp-action=\"Detail\" asp-route-id=\"{course.Id}\">{course.Title}</a></h3>\r\n\r\n" +
                    $" <div class=\"trainer d-flex justify-content-between align-items-center\">\r\n" +
                    $" <ul class=\"row requestList\">\r\n" +
                    $"<li class=\"col-sm-4\"><i class=\"far fa-clock text-danger mr-10\"></i> <b>Created:</b> 14:10 16.05.2023</li>\r\n\r\n" +
                    $"<li class=\"col-sm-4\">\r\n" +
                    $"<i class=\"fas fa-transgender text-danger mr-10\"></i> " +
                    $"<b>Gender Requirements:</b>\r\n {course.GenderRequirement}\r\n </li>\r\n\r\n" +
                    $"<li class=\"col-sm-4\">\r\n" +
                    $"<i class=\"far fa-calendar-alt text-danger mr-10\"></i><b>Academic requirements:</b> {course.AcademicLevelRequirement}\r\n" +
                    $"</li>\r\n\r\n\r\n " +
                    $"<li class=\"col-sm-12\">\r\n " +
                    $"<i class=\"fas fa-map-marker-alt text-danger mr-10\"></i>\r\n" +
                    $"<b>Address:</b> {course.Address}\r\n" +
                    $"<a href=\"https://www.google.com/maps?q={course.Address}\" class=\"text-danger\" target=\"_blank\">\r\n" +
                    $"<small>\r\n" +
                    $"<em>(View map <i class=\"bi bi-map\"></i>)</em>\r\n " +
                    $"</small>\r\n" +
                    $"</a>\r\n " +
                    $"</li>\r\n" +
                    $"</ul>\r\n" +
                    $"</div>");

                stringBuilder.Append("\r\n        </div>\r\n    </div>\r\n</div>");
            }
            await emailSender.SendHtmlEmail("hoangle.q3@gmail.com", "EduSmart Center: Today's class",
                stringBuilder.ToString());
        }

        return Result.Success();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var result = await GeneratePdfAsync();
    }
}