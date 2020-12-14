using Core.Converters;
using Core.DTO;
using Core.Entities;
using Core.Identity;
using Core.Interfaces;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("CatalogConnection")));

            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(
                opts =>
                {
                    opts.Password.RequiredLength = 6;   // ����������� �����
                    opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
                    opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
                    opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
                    opts.Password.RequireDigit = false; // ��������� �� �����
                })
                .AddEntityFrameworkStores<IdentityContext>();

            services.AddTransient<IUnitOfWork, EFUnitOfWork>();

            services.AddTransient<IGenreService, GenreService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IFeedBackService, FeedBackService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IOrderService, OrderService>();

            services.AddTransient<IConverter<Genre, GenreDTO>, ConverterGenre>();
            services.AddTransient<IConverter<Book, BookDTO>, ConverterBook>();
            services.AddTransient<IConverter<FeedBack, FeedBackDTO>, ConverterFeedBack>();
            services.AddTransient<IConverter<Review, ReviewDTO>, ConverterIReview>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
