using GroceryStoreAPI.Models;
using GroceryStoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly string _data;       // Json data in string
        private readonly string _fullPath;   // Json file location
        private readonly ICustomerService _service;
        // Constructor: get Json file
        public CustomersController(ICustomerService service)
        {
            _service = service;
            string fileName = "database.json";
            _fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            _data = System.IO.File.ReadAllText(_fullPath);
        }

        [HttpGet("Home")]
        public ContentResult Home() 
        {
            return new ContentResult
            {
                ContentType = "text/html",
                Content = 
                "<div style='padding:25px; margin-left: 100px; font-size:16pt;'>" 
                    + "<h1>Customers APIs</h1>"
                    + "<p>API Base URL: <a href='https://localhost:44322/api/Customers/'>https://localhost:44322/api/Customers/</a></p>"
                    + "<ul style='text-align:left; padding:10px;'>"
                    +   "<li style='margin-left:50px;'>"
                    +       "<a href='https://localhost:44322/api/Customers'>Get All Customers</a>"
                    +   "</li>"
                    +   "<li style='margin-left:50px;'>"
                    +       "<a href='https://localhost:44322/api/Customers/1'>Get one Customer by id</a>"
                    +   "</li>"
                    + "</ul>"
                    + "<p>For Creating, Updating and Deleting, you can test with Postman by seting with POST, PUT and DELETE requests</p>"
                + "</div>"
            };
        }

        // Get All Customers
        [HttpGet]
        public List<Customer> GetCustomers()
        {
            var customerList = new List<Customer>();
            JObject customers = JObject.Parse(_data);
            var jarray = customers.GetValue("customers") as JArray;
            Customer customer;

            foreach (var item in jarray)
            {
                customer = new Customer();
                customer.id = item["id"].Value<int>();
                customer.name = item["name"].Value<string>();
                customerList.Add(customer);
            }

            return customerList;
        }

        // Get one by id
        [HttpGet("{id}")]
        public Customer GetCustomer(int id)
        {
            JObject customers = JObject.Parse(_data);
            var jarray = customers.GetValue("customers") as JArray;
            var customerObj = jarray.FirstOrDefault(c => c["id"].Value<int>() == id);
            Customer customer = new Customer();
            customer.id = customerObj["id"].Value<int>();
            customer.name = customerObj["name"].ToString();

            return customer;
        }

        // Create new customer
        [HttpPost]
        public Customer PostCustomer(Customer customer)
        {
            int id = customer.id;
            string name = customer.name;
            var newCustomer = "{ 'id': " + id + ", 'name': '" + name + "'}";

            try
            {
                JObject customers = JObject.Parse(_data);
                var jarray = customers.GetValue("customers") as JArray;
                var newObj = JObject.Parse(newCustomer);
                jarray.Add(newObj);

                customers["customers"] = jarray;
                string result = Newtonsoft.Json.JsonConvert.SerializeObject(customers, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(_fullPath, result);
                return customer;
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
                return null;
            }
        }

        // Update by id
        [HttpPut("{id}")]
        public ActionResult PutCustomer(int id, Customer customer)
        {
            if (id != customer.id)
            {
                return BadRequest();
            }

            try
            {
                JObject customers = JObject.Parse(_data);
                var jarray = customers.GetValue("customers") as JArray;

                foreach (var item in jarray.Where(c => c["id"].Value<int>() == id))
                {
                    item["name"] = customer.name;
                }

                customers["customers"] = jarray;
                string result = Newtonsoft.Json.JsonConvert.SerializeObject(customers, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(_fullPath, result);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        // Delete by id
        [HttpDelete("{id}")]
        public ActionResult DeleteCustomer(int id)
        {

            if (id < 1)
            {
                return BadRequest();
            }

            try
            {
                JObject customers = JObject.Parse(_data);
                var jarray = customers.GetValue("customers") as JArray;
                var customerToDeleted = jarray.FirstOrDefault(c => c["id"].Value<int>() == id);

                if (customerToDeleted == null)
                {
                    return BadRequest("Not found");
                }

                jarray.Remove(customerToDeleted);
                string result = Newtonsoft.Json.JsonConvert.SerializeObject(customers, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(_fullPath, result);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
