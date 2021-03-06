﻿using Autofac;
using Autofac.Extras.Quartz;
using DependencyInjecionResolver;
using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using Quartz;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.Quartz;
using Topshelf.ServiceConfigurators;

namespace ServerStatus
{

    public static class ServiceGlobalVariable
    {
        public static bool SignalRHubExists { get; set; }
        public static bool SignalRHubExists1 { get; set; }

        public static string ServiceName { get; set; }

    }
    class Program
    {
        static readonly ILog s_log = LogManager.GetLogger(typeof(Program));
        static IContainer _container;

        static int Main(string[] args)
        {
            ServiceGlobalVariable.ServiceName = "HRAMServerStatusChecker";

            Console.WriteLine("This sample demostrates how to integrate Quartz, TopShelf and Autofac.");
            s_log.Info("Starting...");
            try
            {
                _container = ConfigureContainer(new ContainerBuilder()).Build();

                ScheduleJobServiceConfiguratorExtensions.SchedulerFactory = () => _container.Resolve<IScheduler>();

                HostFactory.Run(conf =>
                {
                    conf.SetServiceName(ServiceGlobalVariable.ServiceName);
                    conf.SetDisplayName("Quartz.Net integration for Autofac");

                    conf.UseAutofacContainer(_container);

                    conf.Service<ServiceCore>(svc =>
                    {
                        svc.ConstructUsingAutofacContainer();
                        svc.WhenStarted(o => o.Start());
                        svc.WhenStopped(o =>
                        {
                            o.Stop();
                            _container.Dispose();
                        });
                        ConfigureScheduler(svc);
                        ConfigureScheduler1(svc);
                    });


                });

                s_log.Info("Shutting down...");
                log4net.LogManager.Shutdown();
                return 0;
            }

            catch (Exception ex)
            {
                s_log.Fatal("Unhandled exception", ex);
                log4net.LogManager.Shutdown();
                return 1;
            }
        }

        static void ConfigureScheduler(ServiceConfigurator<ServiceCore> svc)
        {
            svc.ScheduleQuartzJob(q =>
            {
                q.WithJob(JobBuilder.Create<ServerStatusCheckerJob>()
                   .WithIdentity("Heartbeat1", "Maintenance1")
                   .Build);
                q.AddTrigger(() => TriggerBuilder.Create()
                    .WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(300)).Build());
            });
        }

        static void ConfigureScheduler1(ServiceConfigurator<ServiceCore> svc)
        {
            svc.ScheduleQuartzJob(q =>
            {
                q.WithJob(JobBuilder.Create<ServiceHeartBeatJob>()
                   .WithIdentity("Heartbeat", "Maintenance")
                   .Build);
                q.AddTrigger(() => TriggerBuilder.Create()
                    .WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(10)).Build());
            });
        }

        internal static ContainerBuilder ConfigureContainer(ContainerBuilder cb)
        {
            // configure and register Quartz
            var schedulerConfig = new NameValueCollection {
                {"quartz.threadPool.threadCount", "3"},
                {"quartz.threadPool.threadNamePrefix", "SchedulerWorker"},
                {"quartz.scheduler.threadName", "Scheduler"}
            };

            cb.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = c => schedulerConfig
            });
            cb.RegisterModule(new QuartzAutofacJobsModule(typeof(ServerStatusCheckerJob).Assembly));

            RegisterComponents(cb);
            return cb;
        }

        internal static void RegisterComponents(ContainerBuilder cb)
        {
            // register Service instance
            cb.RegisterType<ServiceCore>().AsSelf();
            // register dependencies
            cb.RegisterType<HeartbeatService>().As<IHeartbeatService>();
            cb.RegisterModule(new ServiceDIContainer());
        }
    }
  
}
