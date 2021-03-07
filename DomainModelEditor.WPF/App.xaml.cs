using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using DomainModelEditor;
using DomainModelEditor.Data.Abstractions;
using DomainModelEditor.Data.SqlServer;
using DomainModelEditor.Data.SqlServer.Repositories;
using DomainModelEditor.Data.SqlServer.Services;
using DomainModelEditor.Models;
using DomainModelEditor.Navigation;
using DomainModelEditor.Views;
using DomainModelEditor.WPF.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace interview_assessment
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    // Add other configuration files...
                    builder.AddJsonFile("appsettings.json", true);
                }).ConfigureServices((context, services) => { ConfigureServices(context.Configuration, services); })
                .ConfigureLogging(logging =>
                {
                    // Add other loggers...
                })
                .Build();

            ServiceProvider = _host.Services;
        }

        public static IServiceProvider ServiceProvider { get; private set; }

        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            //Configuration 
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

            var connectionString = GetSqlLiteConnectionString();
            services.AddDbContext<EntityContext>(options => { options.UseSqlite(connectionString); });

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
            var dbPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                @"DomainModelEditor.Data\DB\entities.sqlite");
            var builder = new SqliteConnectionStringBuilder("") {DataSource = dbPath};
            var connectionString = builder.ToString();
            return connectionString;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var navigationService = ServiceProvider.GetRequiredService<INavigationService>();
            await navigationService.ShowAsync(WindowsNames.Main);

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }

        private void Application_DispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occurred. Please inform the admin."
                            + Environment.NewLine + e.Exception.Message, "Unexpected error");

            e.Handled = true;
        }
    }
}