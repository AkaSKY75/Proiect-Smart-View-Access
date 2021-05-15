using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotnetnewmvc.Models;
using System.IO;
using Microsoft.ClearScript.V8;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;

namespace dotnetnewmvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.failed_login = 0;
            return View();
        }

        private string SendHttpRequest(string link, StringContent content, string method)
        {
            HttpClient http = new HttpClient();
            HttpResponseMessage response = null;
            switch (method) {
                case "POST":    response = http.PostAsync(link, content).Result;
                                break;
                case "GET":     response = http.GetAsync(link).Result;
                                break;
                case "PATCH":   response = http.PatchAsync(link, content).Result;
                                break;
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            if (form["email"].ToString() != "" && form["password"].ToString() != "")
            {
                string values = "{ \"structuredQuery\": { \"select\": {\"fields\": [{\"fieldPath\": \"name\"}]}, \"where\": {\"compositeFilter\": { \"op\": \"AND\", \"filters\": [{ \"fieldFilter\": {\"field\": {\"fieldPath\": \"email\"}, \"op\": \"EQUAL\", \"value\": {\"stringValue\": \"" + form["email"]+ "\"}}}, { \"fieldFilter\": {\"field\": {\"fieldPath\": \"parola\"}, \"op\": \"EQUAL\", \"value\": {\"stringValue\": \"" + form["password"] + "\"}}}]}}, \"from\": [{\"collectionId\": \"Angajat\"}]}}";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                var response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                var jresponse = JArray.Parse(response).Children()["document"];

                if (jresponse.Count() == 0)
                {
                    ViewBag.failed_login = 1;
                    return View();
                }
                else
                {
                    jresponse = jresponse.First()["name"];

                    string id = jresponse.ToString().Substring(jresponse.ToString().LastIndexOf('/') + 1);

                    response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Administrator/" + id, null, "GET");

                    json = JObject.Parse(response);

                    ViewBag.uid = id;

                    if (json["name"] != null)
                    {
                        return View("Index_Administrator");
                    }
                    else
                    {
                        values = "{ \"structuredQuery\": { \"select\": {\"fields\": [{\"fieldPath\": \"avatar\"}]}, \"from\":[{\"collectionId\":\"Angajat\"}], \"where\": {\"fieldFilter\": {\"field\":{\"fieldPath\": \"__name__\"}, \"op\":\"EQUAL\", \"value\": {\"referenceValue\":\"projects/smartviewacces/databases/(default)/documents/Angajat/" + id+"\"}}}}}";
                        json = JObject.Parse(values);
                        content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                        response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                        jresponse = JArray.Parse(response).Children()["document"];
                        if (jresponse.First()["fields"]["avatar"]["stringValue"].ToString() == "")
                        {
                            byte[] filebytes = System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory+"/wwwroot/static/profile.png"));
                            ViewBag.Image = "data:image/png;base64,"+Convert.ToBase64String(filebytes);
                        }
                        else
                            ViewBag.Image = jresponse.First()["fields"]["avatar"]["stringValue"];
                        string[] months = new string[] { "Ianuarie", "Februarie", "Martie", "Aprilie", "Mai", "Iunie", "Iulie", "August", "Septembrie", "Octombrie", "Noiembrie", "Decembrie" };
                        ViewBag.CurrentMonth = DateTime.Now.Month;
                        ViewBag.Months = months;
                        values = "{ \"structuredQuery\": {\"select\": {\"fields\": [{\"fieldPath\": \"Intrare\"}]}, \"from\": [{\"collectionId\": \"Pontaj\"}], \"orderBy\": [{\"field\": {\"fieldPath\": \"Intrare\"}, \"direction\": \"DESCENDING\"}], \"where\": {\"compositeFilter\": {\"filters\": [{\"fieldFilter\":{\"field\":{\"fieldPath\": \"id\"}, \"op\": \"EQUAL\", \"value\": {\"referenceValue\":\"projects/smartviewacces/databases/(default)/documents/Angajat/"+id+ "\"}}}, {\"fieldFilter\":{\"field\":{\"fieldPath\": \"Intrare\"}, \"op\": \"GREATER_THAN_OR_EQUAL\", \"value\": {\"timestampValue\":\""+DateTime.Now.AddDays(-DateTime.Now.Day).ToString("yyyy-MM-ddT")+"21:00:00.00Z"+"\"}}}], \"op\":\"AND\"}}}}";
                        json = JObject.Parse(values);
                        content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                        response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                        jresponse = JArray.Parse(response).Children()["document"];
                        List<DateTime> Intrari = new List<DateTime>();
                        for (int i = 0; i < jresponse.Count(); i++)
                        {
                            Intrari.Add(DateTime.Parse(jresponse.ElementAt(i)["fields"]["Intrare"]["timestampValue"].ToString()).AddHours(3));
                        }
                        values = "{ \"structuredQuery\": {\"select\": {\"fields\": [{\"fieldPath\": \"Iesire\"}]}, \"from\": [{\"collectionId\": \"Pontaj\"}], \"orderBy\": [{\"field\": {\"fieldPath\": \"Iesire\"}, \"direction\": \"DESCENDING\"}], \"where\": {\"compositeFilter\": {\"filters\": [{\"fieldFilter\":{\"field\":{\"fieldPath\": \"id\"}, \"op\": \"EQUAL\", \"value\": {\"referenceValue\":\"projects/smartviewacces/databases/(default)/documents/Angajat/" + id + "\"}}}, {\"fieldFilter\":{\"field\":{\"fieldPath\": \"Iesire\"}, \"op\": \"GREATER_THAN_OR_EQUAL\", \"value\": {\"timestampValue\":\"" + DateTime.Now.AddDays(-DateTime.Now.Day).ToString("yyyy-MM-ddT") + "21:00:00.00Z" + "\"}}}], \"op\":\"AND\"}}}}";
                        json = JObject.Parse(values);
                        content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                        response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                        jresponse = JArray.Parse(response).Children()["document"];
                        List<DateTime> Iesiri = new List<DateTime>();
                        List<string> Ore = new List<string>();
                        int total = 0;
                        for (int i = 0; i < jresponse.Count(); i++)
                        {
                            Iesiri.Add(DateTime.Parse(jresponse.ElementAt(i)["fields"]["Iesire"]["timestampValue"].ToString()).AddHours(3));
                            Ore.Add(Convert.ToInt32(Math.Floor((Iesiri[i]-Intrari[i]).TotalMinutes/60)).ToString() +":"+ Convert.ToInt32(Math.Floor((Iesiri[i] - Intrari[i]).TotalMinutes % 60)).ToString());
                            total += Convert.ToInt32((Iesiri[i] - Intrari[i]).TotalHours);
                        }
                        ViewBag.Count = jresponse.Count();
                        ViewBag.Total = total;
                        ViewBag.Intrari = Intrari;
                        ViewBag.Iesiri = Iesiri;
                        ViewBag.Ore = Ore;
                        return View("Index_Angajat");
                    }
                }

                //return View("Firebase", new Angajat(form["email"].ToString(), form["password"].ToString()));
            }
            else if(form["uid"].ToString() != "" && form["change_profile_pic"].ToString() != "")
            {
                string values = "{ \"fields\": { \"avatar\": {\"stringValue\": \"" + form["change_profile_pic"].ToString() + "\"}}}";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                string response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Angajat/"+form["uid"].ToString()+"?updateMask.fieldPaths=avatar&key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "PATCH");
                return Json(new {Message = "success" });
            }
            else if (form["change_month"].ToString() != "" && form["change_year"].ToString() != "")
            {
                string month = form["change_month"].ToString();

                if (month.Length == 1)
                    month = "0" + month;
                DateTime begin = DateTime.Parse("01/" + month + "/"+form["change_year"].ToString());
                DateTime end = begin.AddMonths(1).AddSeconds(-1);
                string values = "{ \"structuredQuery\": { \"from\": [ { \"collectionId\": \"Pontaj\" } ], \"select\": { \"fields\": [ { \"fieldPath\": \"Intrare\" } ] }, \"orderBy\": [ { \"field\": { \"fieldPath\": \"Intrare\" }, \"direction\": \"DESCENDING\" } ], \"where\": { \"compositeFilter\": { \"filters\": [ { \"fieldFilter\": { \"field\": { \"fieldPath\": \"id\" }, \"op\": \"EQUAL\", \"value\": { \"referenceValue\": \"projects/smartviewacces/databases/(default)/documents/Angajat/"+form["uid"]+"\" } } }, { \"fieldFilter\": { \"field\": { \"fieldPath\": \"Intrare\" }, \"op\": \"GREATER_THAN_OR_EQUAL\", \"value\": { \"timestampValue\": \""+begin.AddHours(-3).ToString("yyyy-MM-ddTHH:mm:ssZ")+"\" } } }, { \"fieldFilter\": { \"field\": { \"fieldPath\": \"Intrare\" }, \"op\": \"LESS_THAN_OR_EQUAL\", \"value\": { \"timestampValue\": \""+end.AddHours(-3).ToString("yyyy-MM-ddTHH:mm:ssZ")+"\" } } } ], \"op\": \"AND\" } } } }";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                string response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                IEnumerable<JToken> jresponse = JArray.Parse(response).Children()["document"];
                List<DateTime> intrari = new List<DateTime>();
                for(int i = 0; i < jresponse.Count(); i++)
                {
                    intrari.Add(DateTime.Parse(jresponse.ElementAt(i)["fields"]["Intrare"]["timestampValue"].ToString()).AddHours(3));
                }
                values = "{ \"structuredQuery\": { \"from\": [ { \"collectionId\": \"Pontaj\" } ], \"select\": { \"fields\": [ { \"fieldPath\": \"Iesire\" } ] }, \"orderBy\": [ { \"field\": { \"fieldPath\": \"Iesire\" }, \"direction\": \"DESCENDING\" } ], \"where\": { \"compositeFilter\": { \"filters\": [ { \"fieldFilter\": { \"field\": { \"fieldPath\": \"id\" }, \"op\": \"EQUAL\", \"value\": { \"referenceValue\": \"projects/smartviewacces/databases/(default)/documents/Angajat/" + form["uid"] + "\" } } }, { \"fieldFilter\": { \"field\": { \"fieldPath\": \"Iesire\" }, \"op\": \"GREATER_THAN_OR_EQUAL\", \"value\": { \"timestampValue\": \"" + begin.AddHours(-3).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\" } } }, { \"fieldFilter\": { \"field\": { \"fieldPath\": \"Iesire\" }, \"op\": \"LESS_THAN_OR_EQUAL\", \"value\": { \"timestampValue\": \"" + end.AddHours(-3).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\" } } } ], \"op\": \"AND\" } } } }";
                json = JObject.Parse(values);
                content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                jresponse = JArray.Parse(response).Children()["document"];
                List<DateTime> iesiri = new List<DateTime>();
                List<string> ore = new List<string>();
                int total = 0;
                for (int i = 0; i < jresponse.Count(); i++)
                {
                    iesiri.Add(DateTime.Parse(jresponse.ElementAt(i)["fields"]["Iesire"]["timestampValue"].ToString()).AddHours(3));
                }
                for(int i = 0; i < jresponse.Count(); i++)
                {
                    ore.Add(Convert.ToInt32(Math.Floor((iesiri[i] - intrari[i]).TotalMinutes / 60)).ToString() + ":" + Convert.ToInt32(Math.Floor((iesiri[i] - intrari[i]).TotalMinutes % 60)).ToString());
                    total += Convert.ToInt32((iesiri[i]-intrari[i]).TotalHours);
                }
                return Json(new { Intrari = intrari, Iesiri = iesiri, Ore = ore, Total = total });
            }
            else if(form["administrator_menu_item"].ToString() != "")
            {
                switch(form["administrator_menu_item"].ToString())
                {
                    case "1":   return View("Administrator_Menu_1");
                    case "2":   return View("Administrator_Menu_2");
                    case "3":   return View("Administrator_Menu_3");
                    case "4":   return View("Administrator_Menu_4");
                }
                return View("Index_Administrator");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
