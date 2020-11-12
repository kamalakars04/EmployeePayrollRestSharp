using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Newtonsoft.Json;
using System;

namespace EmployeePayrollRestSharp
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;
        [TestInitialize]
        public void setup()
        {
            client = new RestClient("http://localhost:3000");
        }

        /// <summary>
        /// TC 1 Retrieves all elements from rest API.
        /// </summary>
        [TestMethod]
        public void OnCallingAPI_RetrieveAllElementsFromRestApi()
        {
            // Get the response
            IRestResponse response = GetEmployeeList();

            // Get all the elements into a list
            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(response.Content);

            // Print all elements
            employees.ForEach(employee => Console.WriteLine("Id: " + employee.id + " Name: " 
                                + employee.name + " Salary: " + employee.salary));
            // Assert
            Assert.AreEqual(3, employees.Count);
        }

        private IRestResponse GetEmployeeList()
        {
            // Request the client by giving resource url and method to perform
            IRestRequest request = new RestRequest("/Employee", Method.GET);

            // executing the request using client and saving the result in IrestResponse.
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
