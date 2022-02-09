using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenU_BL.Models;
using MenU_API.DataTransferObjects;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using MenU_API.Sevices;
using System.IO;

namespace MenU_API.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        MenUContext context;

        public AccountsController(MenUContext context) { this.context = context; }

        
        // This Method logs in a user using an authentication token
        [Route("TokenLogin")]
        [HttpGet]
        public AccountDTO Login([FromQuery] string token)
        {
            Account acc = null;
            try
            {
                acc = context.Login(token);
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception e)
            {
                //Log Error
            }

            if (acc != null)
            {
                AccountDTO userDTO = new AccountDTO(acc);

                HttpContext.Session.SetObject("user", userDTO);

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return userDTO;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }
        
        // This Method logs in a user using credentials (Item1 => username and Item2 => password)
        [Route("LoginCredentials")]
        [HttpPost]
        public AccountDTO Login([FromBody] Credentials credentials )
        {
            Account acc = null;


            try 
            {
                // Get salt and No. of iterations from DB
                string salt = context.GetSalt(credentials.username);
                int iterations = context.GetIterations(credentials.username);
                // Hashes the password
                if(salt != null && iterations != -1)
                {
                    string hashedPassword = GeneralProcessing.PlainTextToHashedPassword(credentials.password, salt, iterations);
                    acc = context.Login(credentials.username, hashedPassword);
                }
                
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                //Log Error
            }
            
            

            if (acc != null)
            {
                AccountDTO userDTO = new AccountDTO(acc);

                HttpContext.Session.SetObject("user", userDTO);

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return userDTO;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                return null;
            }
        }
        
        // This method generates an authentication token, saves it in the database and returns it to the client
        [Route("CreateToken")]
        [HttpGet]
        public string CreateToken()
        {
            try
            {
                bool isUnique = false;
                string token = "";
                while (!isUnique)
                {
                    token = GeneralProcessing.GenerateAlphanumerical(16);
                    if (!context.TokenExists(token))
                        isUnique = true;
                }    
            
                AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
                if(userDTO != null)
                {
                        context.SaveToken(userDTO.AccountId, token);
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return token;
                }
                else
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception e) 
            { 
                //Log Error
            }
            return "";
        }

        // This method logs the user out and removes it's auth token if one exists
        [Route("LogOut")]
        [HttpGet]
        public void LogOut()
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            if (userDTO != null)
            {
                HttpContext.Session.Clear();
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }
        }
        
        // This method adds the specified user to the database
        [Route("SignUp")]
        [HttpPost]
        public bool SignUp([FromBody] AccountDTO acc)
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            string salt = "";
            int iterations = GeneralProcessing.GenerateCryptoRandomINT(1000, 100000);
            string hashed = acc.Pass;
            try
            {
                bool isUnique = false;
                while (!isUnique)
                {
                    salt = GeneralProcessing.GenerateAlphanumerical(8);
                    if (!context.SaltExists(salt))
                        isUnique = true;
                }
                hashed = GeneralProcessing.PlainTextToHashedPassword(hashed, salt, iterations);
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception e)
            {
                //Log Error
            }

            if (userDTO == null)
            {
                Account newAcc = new Account()
                {
                    FirstName = acc.FirstName,
                    LastName = acc.LastName,
                    Username = acc.Username,
                    Email = acc.Email,
                    Pass = hashed,
                    Salt = salt,
                    Iterations = iterations,
                    DateOfBirth = acc.DateOfBirth,
                    AccountType = acc.AccountType,
                    AccountStatus = acc.AccountStatus
                };

                try
                {
                    bool exists = context.Exists(acc.Username, acc.Email);
                    if (!exists)
                    {
                        context.AddAccount(newAcc);
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return true;
                    }
                    else
                        Response.StatusCode = Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                }
                catch { Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError; }     
            }
            else
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
         
            return false;
        }

        //This method generates a random unique salt and returns it
        [Route("GenerateSalt")]
        [HttpGet]
        public string GenerateSalt()
        {
            try
            {
                string salt = "";
                bool isUnique = false;
                while (!isUnique)
                {
                    salt = GeneralProcessing.GenerateAlphanumerical(8);
                    if (!context.SaltExists(salt))
                        isUnique = true;
                }
                if(salt != "")
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK; 
                else
                    Response.StatusCode = (int)System.Net.HttpStatusCode.NoContent;

                return salt;
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception e)
            {
                //Log Error
            }
            return "";

        }


        [Route("UpdateAccountInfo")]
        [HttpPost]
        public bool UpdateAccountInfo([FromBody] Account targetAccount )
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            try
            {
                if (userDTO != null && userDTO.AccountId == targetAccount.AccountId)
                {
                    List<string> userInfo = new List<string>();
                    userInfo.Add(targetAccount.Username);
                    userInfo.Add(targetAccount.FirstName);
                    userInfo.Add(targetAccount.LastName);
                    try
                    {
                        Account updated = context.Login(userDTO.Username, targetAccount.Pass);
                        updated = updated.UpdateUser(userInfo, context);
                        HttpContext.Session.SetObject("user", new AccountDTO(updated));
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return true;
                    }
                    catch (UniqueKeyInUseException)
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.UnprocessableEntity;
                    } 
                }
                else
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }
            catch { Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError; }
            return false;
        }

        [Route("GetDefaultPfps")]
        [HttpGet]
        public List<string> GetDefaultPfps()
        {
            List<string> urls = Directory.GetFiles(@"wwwroot\imgs\pfp\default").ToList();
            urls = urls.Select(x => x.Substring(8)).ToList();
            if(urls.Count > 0)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return urls;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
                
        }

        [Route("ChangePass")]
        [HttpPost]
        public bool ChangePass(Credentials creds)
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            try
            {
                if (userDTO != null && userDTO.AccountId == creds.id)
                {
                    string salt = "";
                    int iterations = GeneralProcessing.GenerateCryptoRandomINT(1000, 100000);
                    string hashed = creds.password;
                    bool isUnique = false;
                    while (!isUnique)
                    {
                        salt = GeneralProcessing.GenerateAlphanumerical(8);
                        if (!context.SaltExists(salt))
                            isUnique = true;
                    }
                    hashed = GeneralProcessing.PlainTextToHashedPassword(hashed, salt, iterations);
                    Account loggedIn = context.Login(userDTO.Username, userDTO.Pass);
                    loggedIn.ChangePass(iterations, salt, hashed);
                    context.SaveChanges();
                    HttpContext.Session.SetObject("user", new AccountDTO(loggedIn));
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return true;

                }
                else
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            return false;
        }

        [Route("AddRestaurant")]
        [HttpPost]
        public bool AddRestaurant([FromBody] RestaurantDTO r)
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            try
            {
                if (userDTO != null && userDTO.AccountType == 2 && userDTO.AccountId == r.Restaurant.OwnerId)
                {
                    List<DishDTO> dishDTOs = r.Dishes.ToList();
                    List<Dish> dishes = new List<Dish>();
                    foreach (DishDTO dish in dishDTOs)
                    {
                        Dish d = dish.Dish;
                        d.DishTags = dish.Tags;
                        d.AllergenInDishes = dish.AllergenInDishes;
                        dishes.Add(d);
                    }

                    bool hasWorked = context.AddDishes(dishes);

                    Restaurant restaurant = r.Restaurant;
                    restaurant.RestaurantTags = r.RestaurantTags.ToList();
                    hasWorked = hasWorked && context.UpdateRestaurant(restaurant);

                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return hasWorked;
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                }
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            }
            return false;
        }


    }
}
