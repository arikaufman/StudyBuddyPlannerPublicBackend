using AutoMapper;
using Common.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using plannerBackEnd.Admin.DataAccess;
using plannerBackEnd.Admin.DataAccess.Dao;
using plannerBackEnd.Admin.Domain;
using plannerBackEnd.Common;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Common.Constants;
using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Comparatives.DataAccess;
using plannerBackEnd.Comparatives.DataAccess.Dao;
using plannerBackEnd.Comparatives.Domain;
using plannerBackEnd.Schools.DataAccess;
using plannerBackEnd.Schools.DataAccess.Dao;
using plannerBackEnd.Schools.Domain;
using plannerBackEnd.Semesters.DataAccess;
using plannerBackEnd.Semesters.DataAccess.Dao;
using plannerBackEnd.Semesters.Domain;
using plannerBackEnd.Subjects.DataAccess;
using plannerBackEnd.Subjects.DataAccess.Dao;
using plannerBackEnd.Subjects.Domain;
using plannerBackEnd.Tasks.DataAccess;
using plannerBackEnd.Tasks.DataAccess.Dao;
using plannerBackEnd.Tasks.Domain;
using plannerBackEnd.Users.DataAccess;
using plannerBackEnd.Users.DataAccess.Dao;
using plannerBackEnd.Users.Domain;
using System;
using System.Text;
using plannerBackEnd.Campaigns.DataAccess;
using plannerBackEnd.Campaigns.Domain;
using plannerBackEnd.Common.DomainObjects;
using plannerBackEnd.Common.EmailManager;
using plannerBackEnd.Feeds.DataAccess;
using plannerBackEnd.Feeds.DataAccess.Dao;
using plannerBackEnd.Feeds.Domain;
using plannerBackEnd.Friends.DataAccess;
using plannerBackEnd.Friends.DataAccess.Dao;
using plannerBackEnd.Friends.Domain;
using plannerBackEnd.Personals.DataAccess.Dao;
using plannerBackEnd.Personals.Domain;

namespace plannerBackEnd
{
    public class Startup
    {
        private const string corsPolicyName = "AllowAllOrigins";
        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment env)
        {

            // Set Configuration
            IConfigurationBuilder builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                ;


            builder.AddJsonFile($"appsettings.{Environment.MachineName}.json",
                    optional: true); // json specific for each machine that runs the app - should not be included in GIT

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        // -------------------------------------------------------------------------------------------------
        public void ConfigureServices(IServiceCollection services)
        {

            configureCors(services);
            services.AddControllers();

            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(ApplicationConstants.JwtEncryptionKey);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });

            services.AddMvc().AddNewtonsoftJson();
            services.AddAutoMapper(typeof(AutomapperProfile));
            services.AddTransient<SqlConnection, SqlConnection>();
            services.AddTransient<SqlTools, SqlTools>();
            services.AddScoped<EmailSender, EmailSender>();
            services.AddScoped<RequestContext, RequestContext>();

            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<ISchoolDataAccessor, SchoolDataAccessor>();
            services.AddScoped<SchoolDao, SchoolDao>();

            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IStripeService, StripeService>();
            services.AddScoped<IAdminDataAccessor, AdminDataAccessor>();
            services.AddScoped<AdminDao, AdminDao>();
            services.AddScoped<AdminChartsDao, AdminChartsDao>();
            services.AddScoped<SupportLogDao, SupportLogDao>();

            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<ICampaignDataAccessor, CampaignDataAccessor>();
            services.AddScoped<CampaignDao, CampaignDao>();

            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IUserProfileDataAccessor, UserProfileDataAccessor>();
            services.AddScoped<UserProfileDao, UserProfileDao>();

            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<IUserActivityDataAccessor, UserActivityDataAccessor>();
            services.AddScoped<UserActivityDao, UserActivityDao>();

            services.AddScoped<IUserBillingService, UserBillingService>();
            services.AddScoped<IUserBillingDataAccessor, UserBillingDataAccessor>();
            services.AddScoped<UserBillingDao, UserBillingDao>();

            services.AddScoped<IFriendService, FriendService>();
            services.AddScoped<IFriendDataAccessor, FriendDataAccessor>();
            services.AddScoped<FriendDao, FriendDao>();

            services.AddScoped<IFeedService, FeedService>();
            services.AddScoped<IFeedDataAccessor, FeedDataAccessor>();
            services.AddScoped<FeedDao, FeedDao>();

            services.AddScoped<ISemesterService, SemesterService>();
            services.AddScoped<ISemesterDataAccessor, SemesterDataAccessor>();
            services.AddScoped<SemesterDao, SemesterDao>();

            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ISubjectDataAccessor, SubjectDataAccessor>();
            services.AddScoped<SubjectDao, SubjectDao>();

            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskDataAccessor, TaskDataAccessor>();
            services.AddScoped<TaskDao, TaskDao>();

            services.AddScoped<ITaskSessionService, TaskSessionService>();
            services.AddScoped<ITaskSessionDataAccessor, TaskSessionDataAccessor>();
            services.AddScoped<TaskSessionDao, TaskSessionDao>();

            services.AddScoped<IComparativeService, ComparativeService>();
            services.AddScoped<IComparativeDataAccessor, ComparativeDataAccessor>();
            services.AddScoped<ComparativeChartsDao, ComparativeChartsDao>();

            services.AddScoped<IPersonalService, PersonalService>();
            services.AddScoped<IPersonalDataAccessor, PersonalDataAccessor>();
            services.AddScoped<PersonalChartsDao, PersonalChartsDao>();


            services.AddSingleton(Configuration);
            configureSwagger(services);

        }

        // -------------------------------------------------------------------------------------------------
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseMiddleware(typeof(ErrorHandler));

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.RoutePrefix = "swagger";                                 
                c.SwaggerEndpoint("/swagger/current/swagger.json", "Planner App"); 
            });

            app.UseCors(corsPolicyName);
            app.UseStaticFiles();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=admin}/{action=Post}");
            });
        }


        // -------------------------------------------------------------------------------------------------
        private void configureCors(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy(corsPolicyName,
                    builder => {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });
        }

        // -------------------------------------------------------------------------------------------------
        private void configureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c => {

                c.SwaggerDoc("current", new OpenApiInfo { Title = "Planner App", Version = "Current"});
                c.DocumentFilter<ApplySwaggerDescriptionAttribute>();
                // The line below is to make the #refs RFC3986 compliant in the swagger file
                c.CustomSchemaIds((type) =>
                    type.ToString().Replace("[", "-").Replace("]", "-").Replace("`", "-"));
            });

        }

    }
}