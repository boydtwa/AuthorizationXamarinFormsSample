using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using AuthXamSam.Models;
using Newtonsoft.Json;

namespace AuthXamSam.UnitTests
{
    public static class TestValues
    {
        public static readonly string CellarSummaryModelListJson =
            "[{\"CellarId\":{\"PartitionKey\":\"boydtwa@yahoo.com\",\"RowKey\":" +
            "\"1e5d14b4ff0e44bf8b33af2b56c00817\",\"TimeStamp\":" +
            "\"2020-07-21T17:07:32.6905492+00:00\"},\"Name\":\"Wine Closet\"," +
            "\"Description\":\"A Closet converted to a temperature controlled " +
            "wine cellar\",\"Capacity\":1000,\"BottleCount\":4,\"Value\":0.0," +
            "\"Bottles\":null,\"BottlesWithDetails\":null}]";

        public static readonly string InventoryReportHtml = "<html><body>FooBar</body><html>";

        public static readonly string InventoryReportHtmlFail =
            $"Error retrieving Inventory Report status code is: {HttpStatusCode.Accepted}";

        public static readonly string VintageTreeRequestNotOkResponseJson =
            "[{\"Title\":\"Service Call unsuccessful\",\"IsParent\":true,\"Count\":1}]";

        public static readonly ObservableCollection<CellarSummaryModel> CellarSummaryModelListObject =
            new ObservableCollection<CellarSummaryModel>()
            {
                new CellarSummaryModel()
                {
                    BottleCount = 4,
                    CellarId = new AzureTableKey(),
                    Name = "Wine Closet",
                    Description = "A Closet full of Wine",
                    Capacity = 1000,
                    Value = 0.0
                }
            };


        public static readonly ObservableCollection<Vintage> VintageTreeRequestNotOkResponseObject = new ObservableCollection<Vintage>(){new Vintage(){Title = "Service Call unsuccessful"}};

        public static readonly string VintageTreeRequestEmptyCellarJson =
            "[{\"Title\":\"No Wine in Cellar? Big Mistake! HUGE!\",\"IsParent\":true,\"Count\":1}]";
        public static readonly ObservableCollection<Vintage> VintageTreeRequestEmptyCellarObject = new ObservableCollection<Vintage>(){new Vintage(){Title = "No Wine in Cellar? Big Mistake! HUGE!"}};


        public static readonly string VintageTreeRequestSuccessJson =
            "[{\"Title\":\"SUCCESS\",\"IsParent\":true,\"Count\":1}]";
        public static readonly ObservableCollection<Vintage> VintageTreeRequestSuccessObject = new ObservableCollection<Vintage>(){new Vintage(){Title = "SUCCESS"}};


        public static readonly string VintageTreeRequestExceptionJson = "[{\"foo\":\"bar\"}]";
        public static readonly ObservableCollection<Vintage> VintageTreeRequestExceptionObject = new ObservableCollection<Vintage>(){new Vintage(){Title = "foobarErrorMessage"}};





    }
}
