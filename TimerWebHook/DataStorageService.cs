using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimerWebHook.Logic.Model;

public class DataStorageService : IHostedService
{
    private List<TimerItem>? dataList;
    private readonly string filePath = "data.json";

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Load data on application startup
        LoadData();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Save data on application shutdown
        SaveData();
        return Task.CompletedTask;
    }


    public List<TimerItem> GetDataList()
    {
        LoadData();
        return dataList;
    }

    public void AddData(TimerItem newData)
    {
        // Add the new data to the list
        dataList.Add(newData);

        // Save the updated data
        SaveData();
    }

    private void LoadData()
    {
        try
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                dataList = JsonConvert.DeserializeObject<List<TimerItem>>(json);
            }
            else
            {
                dataList = new List<TimerItem>();
            }
        }
        catch (Exception ex)
        {
            // Handle exception (e.g., log it)
        }
    }

    private void SaveData()
    {
        try
        {
            var json = JsonConvert.SerializeObject(dataList);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            // Handle exception (e.g., log it)
        }
    }
}