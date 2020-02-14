
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Com.DaisleyHarrison.CosmosDb.Storage.Models;

namespace Com.DaisleyHarrison.CosmosDb.Storage
{
    class Program
    {
        protected static async Task RunPOC(IServiceProvider serviceProvider)
        {
            var profileCollection = serviceProvider.GetService<IDbCollection<Profile>>();

            await profileCollection.ConnectAsync();

            var profileQuery = profileCollection.Query();

            var profileIterator = profileQuery.ToFeedIterator();

            while (profileIterator.HasMoreResults)
            {
                var item = await profileIterator.ReadNextAsync();
                var profiles = item.Resource;
                profiles.ToList().ForEach(profile =>
                {
                    Console.WriteLine($"{profile.Id}: {profile.Name}");
                });
            }

            profileCollection = serviceProvider.GetService<IDbCollection<Profile>>();
            var testProfiles = await profileCollection.ListAsync(q => q.Where(p => p.Name.Contains("dd-porto-test-")));
            testProfiles.Items.ToList().ForEach(profile =>
            {
                Console.WriteLine($"{profile.Id}: {profile.Name}");
            });


            int testProfilesCount = await profileCollection.CountAsync(q => q.Where(p => p.Name.Contains("test-")));
            Console.WriteLine($"Total test profiles: {testProfilesCount}");

            profileCollection = serviceProvider.GetService<IDbCollection<Profile>>();
            var testProfilesSkipTake = await profileCollection.ListAsync(q => q.Where(p => p.Name.Contains("test-")).Skip(2).Take(1));
            testProfilesSkipTake.Items.ToList().ForEach(profile =>
            {
                Console.WriteLine($"Skip 2, Take 1: {profile.Name}");
            });

            var taskCollection = serviceProvider.GetService<IDbCollection<TaskModel>>();
            await taskCollection.ConnectAsync();
            var taskListResponse = await taskCollection.ListAsync();
            taskListResponse.Items.ToList().ForEach(task =>
            {
                Console.WriteLine($"{task.Id}: {task.Title}");
            });

            int taskCount = await taskCollection.CountAsync(partitionKeyValue: Constants.PARTITION_KEY_NONE);
            Console.WriteLine($"Total tasks: {taskCount}");

        }

        static void Main(string[] args)
        {
            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddIniFile("appSettings.ini")
                    .Build();

                Startup startup = new Startup(configuration);

                IServiceCollection services = new ServiceCollection();

                startup.ConfigureServices(services);

                var serviceProvider = services.BuildServiceProvider();

                var pocTask = RunPOC(serviceProvider);

                Task.WaitAll(pocTask);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
    }
}
