using DomainModelEditor;
using DomainModelEditor.Models;
using DomainModelEditor.Navigation;
using DomainModelEditor.ViewModels;
using DomainModelEditor.Views;
using DomainModelEditor.Data;
using DomainModelEditor.Data.Contract;
using DomainModelEditor.Data.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Windows;

namespace interview_assessment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;
        public static IServiceProvider ServiceProvider { get; private set; }
        public App()
        {

            host = Host.CreateDefaultBuilder()  
        .ConfigureAppConfiguration((context, builder) =>
        {
            // Add other configuration files...
            builder.AddJsonFile("appsettings.json", optional: true);
        }).ConfigureServices((context, services) =>
        {
            ConfigureServices(context.Configuration, services);
        })
        .ConfigureLogging(logging =>
        {
            // Add other loggers...
        })
        .Build();

            ServiceProvider = host.Services;
        }
        private void ConfigureServices(IConfiguration configuration,IServiceCollection services)
        {
            
            //Configuration 
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

            string connectionString = GetSqlLiteConnectionString();
            services.AddDbContext<EntityContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            // Add NavigationService for the application.
            services.AddScoped<INavigationService, NavigationService>(serviceProvider =>
            {
                var navigationService = new NavigationService(serviceProvider);
                navigationService.Configure(WindowsNames.Main, typeof(MainWindow));
                navigationService.Configure(WindowsNames.AddEntity, typeof(AddEntityDialog));
                navigationService.Configure(WindowsNames.AddAttribute, typeof(AddAttributeDialog));
                navigationService.Configure(WindowsNames.AddEntityAttribute, typeof(EntityAttributeDialog));
                return navigationService;
            });

            // Register all the Windows of the applications.
            services.AddScoped<MainWindow>();
            services.AddTransient<AddEntityDialog>();
            services.AddTransient<AddAttributeDialog>();
            services.AddTransient<EntityAttributeDialog>();

            // Register all ViewModels.
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<AddEntityDialogViewModel>();
            services.AddTransient<AddAttributeDialogViewModel>();
            services.AddTransient<EntityAttributeDialogViewModel>();

            //Register all Repositories & services
            services.AddScoped<IEntityRepository, EntityRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }

        private string GetSqlLiteConnectionString()
        {
            string dbPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, @"DomainModelEditor.Data\DB\entities.sqlite");
            var builder = new SqliteConnectionStringBuilder("");
            builder.DataSource = dbPath;
            var connectionString = builder.ToString();
            return connectionString;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();

            var navigationService = ServiceProvider.GetRequiredService<INavigationService>();
            await navigationService.ShowAsync(WindowsNames.Main);

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (host)
            {
                await host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
        private void Application_DispatcherUnhandledException(object sender,
          System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occured. Please inform the admin."
              + Environment.NewLine + e.Exception.Message, "Unexpected error");

            e.Handled = true;
        }
    }
}
