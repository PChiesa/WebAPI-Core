﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Magento
{
    public class MagentoApi : IMagentoApi
    {
        const string API_URL = "https://vsmagentoapi.azurewebsites.net/";
        //const string API_URL = "http://localhost:50569/";

        public async Task<User> AuthenticateUser(User user)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(API_URL + "api/Magento/AuthenticateUser", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

                return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<User> GetUserInfo(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(API_URL + $"api/Magento/GetUserInfo/{id}");
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
                else
                    return null;
            }
        }

        public async Task<User> RegisterUser(User user)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(API_URL + "api/Magento/RegisterUser", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

                return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
            }
        }
    }
}