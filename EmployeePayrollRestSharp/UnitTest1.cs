using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

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
            Assert.AreEqual(10, employees.Count);
        }

        private IRestResponse GetEmployeeList()
        {
            // Request the client by giving resource url and method to perform
            IRestRequest request = new RestRequest("/Employee", Method.GET);

            // executing the request using client and saving the result in IrestResponse.
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// TC 2 Givens the employee on post should return added employee.
        /// TC 3 Add Multiple employees
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            //adding multiple employees to list
            List<Employee> MultipleEmployeeList = new List<Employee>();
            MultipleEmployeeList.Add(new Employee { name = "Raja", salary = 15421 });
            MultipleEmployeeList.Add(new Employee { name = "Rani", salary = 52300 });
            MultipleEmployeeList.ForEach(employeeData =>
            {
                // Arrange
                RestRequest request = new RestRequest("/Employee", Method.POST);

                // Create jObject for adding data for name and salary, id auto increments
                JObject jObject = new JObject();
                jObject.Add("name", employeeData.name);
                jObject.Add("salary", employeeData.salary);

                // parameters are passed as body hence "request body" call is made, in parameter type
                request.AddParameter("application/json", jObject, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

                // Assert
                // code will be 201 for posting data
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);

                // Derserializing object for assert and checking test case
                Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(employeeData.name, dataResponse.name);
                Console.WriteLine(response.Content);
            });
        }

        /// <summary>
        /// TC 4 Givens the employee on update should return updated employee.
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnUpdate_ShouldReturnUpdatedEmployee()
        {
            // Making a request for a particular employee to be updated
            RestRequest request = new RestRequest("Employee/5", Method.PUT);

            // Add data to JSON object
            JObject jobject = new JObject();
            jobject.Add("name", "Clark");
            jobject.Add("salary", 550000);

            // adding parameters in request
            request.AddParameter("application/json", jobject, ParameterType.RequestBody);

            // Execute the request
            IRestResponse response = client.Execute(request);

            // checking status code of response
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);

            // Deserializing the recieved data
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);

            // Asserting for salary
            Assert.AreEqual(dataResponse.salary, 550000);
        }

        /// <summary>
        /// TC 5 Givens the employee on delete should return success status.
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnDelete_ShouldReturnSuccessStatus()
        {
            // request for deleting elements from json 
            RestRequest request = new RestRequest("Employee/4", Method.DELETE);

            //executing request using rest client
            IRestResponse response = client.Execute(request);

            // checking status codes.
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }
    }
}
