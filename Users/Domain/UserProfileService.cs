using Microsoft.IdentityModel.Tokens;
using plannerBackEnd.Admin.Domain;
using plannerBackEnd.Common.Constants;
using plannerBackEnd.Common.Utilities;
using plannerBackEnd.Users.DataAccess;
using plannerBackEnd.Users.Domain.DomainObjects;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using plannerBackEnd.Common.DomainObjects;
using System.Collections.Generic;
using System.Linq;
using plannerBackEnd.Semesters.Domain;
using plannerBackEnd.Semesters.Domain.DomainObjects;

namespace plannerBackEnd.Users.Domain
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileDataAccessor userProfileDataAccessor;
        private readonly IStripeService stripeService;
        private readonly ISemesterService semesterService; 
        private readonly IUserBillingService userBillingService;
        private readonly RequestContext requestContext;
        private readonly IAdminService adminService;


        // -----------------------------------------------------------------------------

        public UserProfileService(
            IUserProfileDataAccessor userProfileDataAccessor,
            IStripeService stripeService,
            ISemesterService semesterService,
            IUserBillingService userBillingService,
            RequestContext requestContext,
            IAdminService adminService
        )
        {
            this.userProfileDataAccessor = userProfileDataAccessor;
            this.stripeService = stripeService;
            this.semesterService = semesterService;
            this.userBillingService = userBillingService;
            this.requestContext = requestContext;
            this.adminService = adminService;
        }

        // -----------------------------------------------------------------------------

        public UserProfile Get(int requestedId)
        {
            SemesterFilterRequest filter = new SemesterFilterRequest(){UserId = requestedId};
            UserProfile userProfile = userProfileDataAccessor.Get(requestedId);
                userProfile.UserBilling = userBillingService.GetByUserId(requestedId);
                userProfile.Password = EncryptionTools.DecryptString(ApplicationConstants.SecretsEncryptionKey, userProfile.Password);
                userProfile.Semesters = new List<Semester>(semesterService.GetList(filter));
                return userProfile;
        }

        // -----------------------------------------------------------------------------

        public UserProfile Get(string email, bool limit = false)
        {
            UserProfile userProfile = new UserProfile();
            if (limit == false)
            {
                userProfile = userProfileDataAccessor.Get(email);
                userProfile.Password = EncryptionTools.DecryptString(ApplicationConstants.SecretsEncryptionKey, userProfile.Password);
                userProfile.UserBilling = userBillingService.GetByUserId(userProfile.Id);
            }
            else
            {
                UserProfile temporaryProfile = userProfileDataAccessor.Get(email);
                userProfile.Email = temporaryProfile.Email;
                userProfile.Id = temporaryProfile.Id;
                userProfile.FirstName = temporaryProfile.FirstName;
                userProfile.LastName = temporaryProfile.LastName;
            }
            return userProfile;
        }

        // -----------------------------------------------------------------------------

        public List<UserProfile> GetList(UserProfileFilterRequest filterRequest)
        {
            List<UserProfile> userProfiles = userProfileDataAccessor.GetList(filterRequest);

            //CODE TO SEND MASS
            /*foreach (UserProfile user in userProfiles)
            {
                string campaignCode = user.Id.ToString() + "N20";
                adminService.SendEmail("AmbassadorEmail", user.FirstName, user.LastName, user.Email, campaignCode);
                
            }*/

            return userProfiles;
        }

        // -----------------------------------------------------------------------------

        public UserProfile Create(UserProfile userProfile)
        {
                userProfile.Password = EncryptionTools.EncryptString(ApplicationConstants.SecretsEncryptionKey, userProfile.Password);

                UserProfile userProfileReturn = userProfileDataAccessor.Create(userProfile);

                UserBilling userBillingToCreate = new UserBilling()
                {
                    UserId = userProfileReturn.Id,
                    StripeCustomerId = stripeService.CreateCustomer(userProfileReturn.Email).Id
                };

                userProfileReturn.UserBilling = userBillingService.Create(userBillingToCreate);
                userProfileReturn.Password = EncryptionTools.DecryptString(ApplicationConstants.SecretsEncryptionKey, userProfile.Password);

                adminService.SendEmail("SignUpEmail", userProfile.FirstName, userProfile.LastName, userProfile.Email);

                return userProfileReturn;
            }

        // -----------------------------------------------------------------------------

        public UserProfile Update(UserProfile userProfile)
        {
            UserProfile oldProfile = Get(userProfile.Id);
                if (userProfile.Password != EncryptionTools.EncryptString(ApplicationConstants.SecretsEncryptionKey, oldProfile.Password))
                {
                    userProfile.Password = EncryptionTools.EncryptString(ApplicationConstants.SecretsEncryptionKey, userProfile.Password);
                }

                if (DateTime.Compare(userProfile.TokenExpiry, oldProfile.TokenExpiry) < 0)
                {
                    userProfile.TokenExpiry = oldProfile.TokenExpiry;
                }

            UserProfile userProfileReturn = userProfileDataAccessor.Update(userProfile);
            userProfileReturn.UserBilling = userBillingService.GetByUserId(userProfileReturn.Id);
            userProfileReturn.Password = EncryptionTools.DecryptString(ApplicationConstants.SecretsEncryptionKey, userProfile.Password);

            SemesterFilterRequest filter = new SemesterFilterRequest() { UserId = userProfile.Id };
            userProfileReturn.Semesters = new List<Semester>(semesterService.GetList(filter));

            return userProfileReturn;
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int requestedId)
        {
            return (userProfileDataAccessor.Delete(requestedId));
        }

        // -----------------------------------------------------------------------------
        public void ResetPassword(string email)
        {
            Random random = new Random();
            var builder = new StringBuilder(10);
            const int lettersOffset = 26; // A...Z or a..z: length=26  
            char offset = 'A';
            for (var i = 0; i < 10; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            string newPassword = builder.ToString();
            UserProfile userProfile = Get(email);
            userProfile.Password = newPassword;
            Update(userProfile);

            adminService.SendEmail("ResetPassword", userProfile.FirstName, userProfile.LastName, userProfile.Email, EncryptionTools.DecryptString(ApplicationConstants.SecretsEncryptionKey, userProfile.Password));

        }

        // -----------------------------------------------------------------------------
        public UserToAuthenticateResponse Authenticate(UserToAuthenticate userToAuthenticate)
        {
            try
            {
                UserToAuthenticateResponse returnValue = new UserToAuthenticateResponse();
                UserProfile user = userProfileDataAccessor.Get(userToAuthenticate.Email);
                //string pass = EncryptionTools.DecryptString(ApplicationConstants.SecretsEncryptionKey, "DulDTYdXbd+wqttUDyKpxw==");
                if (userToAuthenticate.Password != EncryptionTools.DecryptString(ApplicationConstants.SecretsEncryptionKey, user.Password))
                {
                    throw new AuthenticationException("Password is incorrect") {Data = {{"User", user.Id}}};
                }
                else
                {
                    returnValue.UserName = user.UserName;
                    returnValue.FirstName = user.FirstName;
                    returnValue.LastName = user.LastName;
                    returnValue.Id = user.Id;
                    returnValue.Token = generateJwtToken(user);
                }

                return returnValue;
            }
            catch
            {
                throw;
            }
        }

        // -----------------------------------------------------------------------------
        private string generateJwtToken(UserProfile userProfile)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ApplicationConstants.JwtEncryptionKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userProfile.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userProfile.TokenExpiry = tokenDescriptor.Expires.Value;
            Update(userProfile);
            return tokenHandler.WriteToken(token);
        }
    }
}