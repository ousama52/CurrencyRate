using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyProject.Controllers.Model;
using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly CurrencyContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString;

        public HomeController(CurrencyContext context, ILogger<HomeController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("SqlServerDb") ?? throw new InvalidOperationException("Connection string 'SqlServerDb' not found.");
        }

        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateCurrency(CurrencyDto currencyDto)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO currencys (currency, exchangetime, exchangerate) VALUES (@currency, @exchangetime, @exchangerate)";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add("@currency", SqlDbType.VarChar, 100).Value = currencyDto.CurrencyN;
                        cmd.Parameters.Add("@exchangetime", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.AddWithValue("@exchangerate", currencyDto.Exchangerate);
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Currency", $"An error occurred while creating the currency: {ex.Message}");
                return View("Create");
            }
        }

        [HttpGet]
        private List<Currency> GetCurrenciesFromDatabase()
        {
            var currencies = new List<Currency>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, currency, exchangetime, exchangerate FROM currencys";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var currency = new Currency
                            {
                                Id = reader.GetInt32(0),
                                CurrencyN = reader.GetString(1),
                                Exchangetime = reader.GetDateTime(2),
                                Exchangerate = reader.GetDecimal(3)
                            };
                            currencies.Add(currency);
                        }
                    }
                }
            }

            return currencies;
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Index()
        {
            var viewModel = new CurrencyViewModel
            {
                Currencies = GetCurrenciesFromDatabase()
            };

            ViewData["Title"] = "Home Page";
            return View(viewModel);
        }
    }
}
