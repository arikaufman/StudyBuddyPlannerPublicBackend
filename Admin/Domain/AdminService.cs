using plannerBackEnd.Admin.DataAccess;
using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common.DomainObjects;
using plannerBackEnd.Common.EmailManager;
using plannerBackEnd.Common.Filters.DomainObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;

namespace plannerBackEnd.Admin.Domain
{
    public class AdminService : IAdminService
    {
        private readonly IAdminDataAccessor adminDataAccessor;
        private readonly RequestContext requestContext;
        private readonly EmailSender emailSender;


        // -----------------------------------------------------------------------------

        public AdminService(
            IAdminDataAccessor adminDataAccessor,
            RequestContext requestContext,
            EmailSender emailSender
            )
        {
            this.adminDataAccessor = adminDataAccessor;
            this.requestContext = requestContext;
            this.emailSender = emailSender;
        }

        // -----------------------------------------------------------------------------

        public SmokeTest CreateSmokeTest(SmokeTest smokeTest)
        {
            return adminDataAccessor.CreateSmokeTest(smokeTest);
        }

        // -----------------------------------------------------------------------------

        public void CreateErrorLogEntry(ErrorLog errorLog)
        {
            adminDataAccessor.CreateErrorLogEntry(errorLog);
        }

        // -----------------------------------------------------------------------------

        public void CreateAccessLogEntry(AccessLog accessLog)
        {
            adminDataAccessor.CreateAccessLogEntry(accessLog);
        }

        // -----------------------------------------------------------------------------

        public void SendEmail(string emailFileExtension, string firstName, string lastName, string email, 
            string friendFirstName = "", string friendLastName = "", string friendUniversity = "")
        {
            if (email.Contains("fakeuser"))
            {

            }
            else
            {
                string bodyString = "";
                string file = "C:\\Users\\Public\\Documents\\" + emailFileExtension + ".html";
                using (var sr = new StreamReader(file))
                {
                    // Read the stream as a string, and write the string to the console.
                    bodyString += (sr.ReadToEnd());
                }

                bodyString = bodyString.Replace("{{first_name}}", firstName);
                bodyString = bodyString.Replace("{{last_name}}", lastName);

                if ( emailFileExtension == "SignUpEmail")
                {
                    EmailSender.EmailMessage message = new EmailSender.EmailMessage()
                    {
                        From = "studybuddyplannerapp@gmail.com",
                        To = email,
                        Subject = "Welcome to Study Buddy!",
                        Body = bodyString
                    };

                    emailSender.Send(message);
                }
                else if (emailFileExtension == "AmbassadorEmail")
                {
                    bodyString = bodyString.Replace("{{first_name}}", firstName);
                    bodyString = bodyString.Replace("{{last_name}}", lastName);
                    bodyString = bodyString.Replace("{{campaigncode}}", friendFirstName);

                    EmailSender.EmailMessage message = new EmailSender.EmailMessage()
                    {
                        From = "studybuddyplannerapp@gmail.com",
                        To = email,
                        Subject = "CORRECTION TO LINK: WIN Free StudyBuddy Early Access Premium!",
                        Body = bodyString
                    };

                    emailSender.Send(message);
                }
                else if (emailFileExtension == "FriendNotification")
                {
                    bodyString = bodyString.Replace("{{friend_first_name}}", friendFirstName);
                    bodyString = bodyString.Replace("{{friend_last_name}}", friendLastName);
                    bodyString = bodyString.Replace("{{friend_university}}", friendUniversity);

                    EmailSender.EmailMessage message = new EmailSender.EmailMessage()
                    {
                        From = "studybuddyplannerapp@gmail.com",
                        To = email,
                        Subject = friendFirstName + " " + friendLastName + " added you as a friend!",
                        Body = bodyString
                    };

                    emailSender.Send(message);
                }
                else if (emailFileExtension == "ResetPassword")
                {
                    bodyString = bodyString.Replace("{{first_name}}", firstName);
                    bodyString = bodyString.Replace("{{last_name}}", lastName);
                    bodyString = bodyString.Replace("{{friend_first_name}}", friendFirstName);

                    EmailSender.EmailMessage message = new EmailSender.EmailMessage()
                    {
                        From = "studybuddyplannerapp@gmail.com",
                        To = email,
                        Subject = "You have reset your password.",
                        Body = bodyString
                    };

                    emailSender.Send(message);
                }
            }
            
        }

        // SUPPORT LOG
        // -----------------------------------------------------------------------------

        public SupportLog Get(int requestedId)
        {
            return adminDataAccessor.Get(requestedId);
        }

        // -----------------------------------------------------------------------------

        public List<SupportLog> GetList()
        {
            return adminDataAccessor.GetList(); ;
        }

        // -----------------------------------------------------------------------------

        public SupportLog Create(SupportLog supportLog)
        {
            return adminDataAccessor.Create(supportLog);
        }

        // -----------------------------------------------------------------------------

        public SupportLog Update(SupportLog supportLog)
        {
            return adminDataAccessor.Update(supportLog);
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int requestedId)
        {
            return (adminDataAccessor.Delete(requestedId));
        }

        // CHARTS
        // -----------------------------------------------------------------------------
        public AdminStatistics GetListAdminStats()
        {
            return adminDataAccessor.GetListAdminStats();
        }

        // -----------------------------------------------------------------------------
        public List<UserAnalysis> GetListUsers()
        {
            if (requestContext.UserId != 269 || requestContext.UserId != 270)
            {
                return adminDataAccessor.GetListUsers();
            }
            else
            {
                { throw new AuthenticationException("Unauthorized to Access User Information"); }
            }
        }

        // -----------------------------------------------------------------------------
        public BaseFilterResponse GetListNewUsers()
        {
            return adminDataAccessor.GetListNewUsers();
        }
    }
}
