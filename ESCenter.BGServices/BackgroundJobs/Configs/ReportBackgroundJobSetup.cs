using Microsoft.Extensions.Options;
using Quartz;

namespace ESCenter.BGServices.BackgroundJobs.Configs;

public class ReportBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = new JobKey(nameof(PdfReportBackgroundJob));
        options
            .AddJob<PdfReportBackgroundJob>(builder => builder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger.ForJob(jobKey)
                .WithIdentity(nameof(PdfReportBackgroundJob))
                //.WithCronSchedule("0 0 20 * * ?")); // Run at 20:00 every day
                // run every 5 seconds for testing
                .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(5).RepeatForever()));
    }
}