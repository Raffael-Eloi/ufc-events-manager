//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Calendar.v3;
//using Google.Apis.Calendar.v3.Data;
//using Google.Apis.Services;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using UFC.Events.Manager.API.Entities;

namespace UFC.Events.Manager.API.Repositories;

public sealed class EventSenderRepo : IEventSenderRepo
{
    //private static readonly string[] Scopes = [CalendarService.Scope.Calendar];

    private readonly IConfiguration _configuration;

    public EventSenderRepo(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    //public async Task CreateCalendarEventAsync(CalendarEvent calendarEvent)
    //{
    //    string serviceAccountJson = _configuration["GoogleCalendar:ServiceAccountKeyJson"];
    //
    //    string calendarId = _configuration["GoogleCalendar:CalendarId"] ?? "primary";
    //
    //    ServiceAccountCredential specificCredential = CredentialFactory.FromJson<ServiceAccountCredential>(serviceAccountJson);
    //    GoogleCredential credential = specificCredential.ToGoogleCredential().CreateScoped(Scopes);
    //
    //    CalendarService service = new(new BaseClientService.Initializer
    //    {
    //        HttpClientInitializer = credential,
    //        ApplicationName = "UFC Events Manager"
    //    });
    //
    //    Event googleEvent = new()
    //    {
    //        Summary = calendarEvent.Title,
    //        Description = calendarEvent.Description,
    //        Start = new EventDateTime { DateTimeDateTimeOffset = calendarEvent.StartTime },
    //        End = new EventDateTime { DateTimeDateTimeOffset = calendarEvent.EndTime },
    //        //Attendees = calendarEvent.SendTo
    //        //    .Select(email => new EventAttendee { Email = email })
    //        //    .ToList()
    //        Attendees = [new EventAttendee { Email = "email@gmail.com" }]
    //    };
    //
    //    Event test = await service.Events.Insert(googleEvent, calendarId).ExecuteAsync();
    //}

    public async Task CreateCalendarEventAsync(CalendarEvent calendarEvent)
    {
        string host = _configuration["Smtp:Host"] ?? "smtp.gmail.com";
        int port = int.Parse(_configuration["Smtp:Port"] ?? "587");
        string username = _configuration["Smtp:Username"]!;
        string password = _configuration["Smtp:Password"]!;

        string ics = BuildIcs(calendarEvent, username);

        using SmtpClient client = new(host, port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(username, password)
        };

        calendarEvent.SendTo = calendarEvent.SendTo.Any() ? calendarEvent.SendTo : ["email@gmail.com"];

        foreach (string email in calendarEvent.SendTo)
        {
            using MailMessage message = new(username, email)
            {
                Subject = calendarEvent.Title,
                Body = calendarEvent.Description
            };

            using MemoryStream stream = new(Encoding.UTF8.GetBytes(ics));
            message.Attachments.Add(new Attachment(stream, new ContentType("text/calendar; method=PUBLISH; charset=UTF-8"))
            {
                Name = "invite.ics"
            });

            await client.SendMailAsync(message);
        }
    }

    private static string BuildIcs(CalendarEvent calendarEvent, string organizerEmail)
    {
        const string format = "yyyyMMdd'T'HHmmss'Z'";

        return string.Join("\r\n",
            "BEGIN:VCALENDAR",
            "VERSION:2.0",
            "PRODID:-//UFC Events Manager//EN",
            "METHOD:PUBLISH",
            "BEGIN:VEVENT",
            $"UID:{Guid.NewGuid()}@ufc-events-manager",
            $"DTSTAMP:{DateTime.UtcNow.ToString(format)}",
            $"DTSTART:{calendarEvent.StartTime.ToUniversalTime().ToString(format)}",
            $"DTEND:{calendarEvent.EndTime.ToUniversalTime().ToString(format)}",
            $"SUMMARY:{Escape(calendarEvent.Title)}",
            $"DESCRIPTION:{Escape(calendarEvent.Description)}",
            $"ORGANIZER:mailto:{organizerEmail}",
            "END:VEVENT",
            "END:VCALENDAR",
            "");
    }

    private static string Escape(string text) =>
        text.Replace("\\", "\\\\").Replace(";", "\\;").Replace(",", "\\,").Replace("\r\n", "\\n").Replace("\n", "\\n");
}
