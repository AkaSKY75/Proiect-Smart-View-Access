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
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;

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

        private string SendHttpRequest(string link, HttpContent content, string method)
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
                string id = "";
                byte[] password = Encoding.ASCII.GetBytes(form["password"].ToString());
                SHA256 sha = SHA256.Create();
                byte[] result;
                StringBuilder builder = new StringBuilder();
                result = sha.ComputeHash(password);
                for (int i = 0; i < result.Length; i++)
                {
                    builder.Append(result[i].ToString("x2"));
                }
                string values = "{ \"structuredQuery\": { \"select\": {\"fields\": [{\"fieldPath\": \"name\"}]}, \"where\": {\"compositeFilter\": { \"op\": \"AND\", \"filters\": [{ \"fieldFilter\": {\"field\": {\"fieldPath\": \"email\"}, \"op\": \"EQUAL\", \"value\": {\"stringValue\": \"" + form["email"] + "\"}}}, { \"fieldFilter\": {\"field\": {\"fieldPath\": \"parola\"}, \"op\": \"EQUAL\", \"value\": {\"stringValue\": \"" + builder + "\"}}}]}}, \"from\": [{\"collectionId\": \"Angajat\"}]}}";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                string response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                IEnumerable<JToken> jresponse = JArray.Parse(response).Children()["document"];
                if (jresponse.Count() > 0)
                    id = jresponse.First()["name"].ToString();
                values = "{ \"structuredQuery\": { \"select\": {\"fields\": [{\"fieldPath\": \"name\"}]}, \"where\": {\"compositeFilter\": { \"op\": \"AND\", \"filters\": [{ \"fieldFilter\": {\"field\": {\"fieldPath\": \"email_firma\"}, \"op\": \"EQUAL\", \"value\": {\"stringValue\": \"" + form["email"] + "\"}}}, { \"fieldFilter\": {\"field\": {\"fieldPath\": \"parola\"}, \"op\": \"EQUAL\", \"value\": {\"stringValue\": \"" + builder + "\"}}}]}}, \"from\": [{\"collectionId\": \"Angajat\"}]}}";
                json = JObject.Parse(values);
                content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                jresponse = JArray.Parse(response).Children()["document"];
                if (jresponse.Count() > 0)
                    id = jresponse.First()["name"].ToString();
                if (id == "")
                {
                    ViewBag.failed_login = 1;
                    return View();
                }
                else
                {
                    id = id.Substring(62);
                    ViewBag.uid = id;

                    values = "{ \"structuredQuery\": { \"select\": {\"fields\": [{\"fieldPath\": \"avatar\"}]}, \"from\":[{\"collectionId\":\"Angajat\"}], \"where\": {\"fieldFilter\": {\"field\":{\"fieldPath\": \"__name__\"}, \"op\":\"EQUAL\", \"value\": {\"referenceValue\":\"projects/smartviewacces/databases/(default)/documents/Angajat/" + id + "\"}}}}}";
                    json = JObject.Parse(values);
                    content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                    response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                    jresponse = JArray.Parse(response).Children()["document"];
                    if (jresponse.First()["fields"]["avatar"]["stringValue"].ToString() == "")
                    {
                        byte[] filebytes = System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory + "/wwwroot/static/profile.png"));
                        ViewBag.Image = "data:image/png;base64," + Convert.ToBase64String(filebytes);
                    }
                    else
                    {
                        string path = jresponse.First()["fields"]["avatar"]["stringValue"].ToString();
                        path = path.Substring(1);
                        response = SendHttpRequest("https://firebasestorage.googleapis.com/v0/b/smartviewacces.appspot.com/o?name=" + path + "&key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", null, "GET");
                        ViewBag.Image = "https://firebasestorage.googleapis.com/v0/b/smartviewacces.appspot.com/o/" + path.Replace("/", "%2F") + "?alt=media&token=" + JObject.Parse(response)["downloadTokens"].ToString();
                    }

                    values = "{ \"structuredQuery\": { \"select\": {\"fields\": [{\"fieldPath\": \"id\"}]}, \"from\":[{\"collectionId\":\"Administrator\"}], \"where\": {\"fieldFilter\": {\"field\":{\"fieldPath\": \"id\"}, \"op\":\"EQUAL\", \"value\": {\"referenceValue\":\"projects/smartviewacces/databases/(default)/documents/Angajat/" + id + "\"}}}}}";
                    json = JObject.Parse(values);
                    content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                    response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                    jresponse = JArray.Parse(response).Children()["document"];

                    if (jresponse.Count() != 0)
                    {
                        return View("Index_Administrator");
                    }
                    else
                    {

                        string[] months = new string[] { "Ianuarie", "Februarie", "Martie", "Aprilie", "Mai", "Iunie", "Iulie", "August", "Septembrie", "Octombrie", "Noiembrie", "Decembrie" };
                        ViewBag.CurrentMonth = DateTime.Now.Month;
                        ViewBag.Months = months;
                        values = "{ \"structuredQuery\": {\"select\": {\"fields\": [{\"fieldPath\": \"Intrare\"}, {\"fieldPath\": \"Iesire\"}]}, \"from\": [{\"collectionId\": \"Pontaj\"}], \"orderBy\": [{\"field\": {\"fieldPath\": \"Intrare\"}, \"direction\": \"DESCENDING\"}], \"where\": {\"compositeFilter\": {\"filters\": [{\"fieldFilter\":{\"field\":{\"fieldPath\": \"id\"}, \"op\": \"EQUAL\", \"value\": {\"referenceValue\":\"projects/smartviewacces/databases/(default)/documents/Angajat/" + id + "\"}}}, {\"fieldFilter\":{\"field\":{\"fieldPath\": \"Intrare\"}, \"op\": \"GREATER_THAN_OR_EQUAL\", \"value\": {\"stringValue\":\"" + DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyyMMdd") + "000000" + "\"}}}], \"op\":\"AND\"}}}}";
                        json = JObject.Parse(values);
                        content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                        response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                        jresponse = JArray.Parse(response).Children()["document"];
                        List<DateTime> Intrari = new List<DateTime>();
                        List<DateTime> Iesiri = new List<DateTime>();
                        List<string> Ore = new List<string>();
                        int total = 0;
                        int count = 0;
                        for (int i = 0; i < jresponse.Count(); i++)
                        {
                            if (jresponse.ElementAt(i)["fields"]["Iesire"]["stringValue"].ToString() != "-1")
                            {
                                Intrari.Add(DateTime.ParseExact(jresponse.ElementAt(i)["fields"]["Intrare"]["stringValue"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture));
                                Iesiri.Add(DateTime.ParseExact(jresponse.ElementAt(i)["fields"]["Iesire"]["stringValue"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture));
                                Ore.Add(Convert.ToInt32(Math.Floor((Iesiri[count] - Intrari[count]).TotalMinutes / 60)).ToString() + ":" + Convert.ToInt32(Math.Floor((Iesiri[count] - Intrari[count]).TotalMinutes % 60)).ToString());
                                total += Convert.ToInt32((Iesiri[count] - Intrari[count]).TotalHours);
                                count++;
                            }
                        }
                        ViewBag.Count = count;
                        ViewBag.Total = total;
                        ViewBag.Intrari = Intrari;
                        ViewBag.Iesiri = Iesiri;
                        ViewBag.Ore = Ore;
                        return View("Index_Angajat");
                    }
                }

                //return View("Firebase", new Angajat(form["email"].ToString(), form["password"].ToString()));
            }
            else if (form["uid"].ToString() != "" && form["change_profile_pic"].ToString() != "")
            {
                /*string values = "{ \"fields\": { \"avatar\": {\"stringValue\": \"" + form["change_profile_pic"].ToString() + "\"}}}";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                string response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Angajat/" + form["uid"].ToString() + "?updateMask.fieldPaths=avatar&key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "PATCH");*/
                /*Debug.WriteLine(form["change_profile_pic"].ToString().Substring(22));
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(form["change_profile_pic"].ToString().Substring(22)));
                Firebase_Image(ms);
                ms.Close();*/
                /*https://firebasestorage.googleapis.com/v0/b/smartviewacces.appspot.com/o/a7UNRVlmYa7ZS6KjYChe%2FAvatar%2Favatar.jpeg?alt=media&token=9807ca30-34b6-44da-a5ed-78ac4d2cc7ef*/
                string bytes = form["change_profile_pic"].ToString().Substring(form["change_profile_pic"].ToString().LastIndexOf(",") + 1);
                string extension = form["change_profile_pic"].ToString().Substring(11, form["change_profile_pic"].ToString().LastIndexOf(";") - 11);
                ByteArrayContent byte_content = new ByteArrayContent(Convert.FromBase64String(bytes));
                string response = SendHttpRequest("https://firebasestorage.googleapis.com/v0/b/smartviewacces.appspot.com/o?name=" + form["uid"].ToString() + "%2FAvatar%2Favatar." + extension + "&key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", byte_content, "POST");
                JToken jresponse = JObject.Parse(response)["downloadTokens"];
                string values = "{ \"fields\": { \"avatar\": {\"stringValue\": \"/" + form["uid"].ToString() + "/Avatar/avatar." + extension + "\"}}}";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Angajat/" + form["uid"].ToString() + "?updateMask.fieldPaths=avatar&key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "PATCH");
                return Json(new { Message = "success" });
            }
            else if (form["change_month"].ToString() != "" && form["change_year"].ToString() != "")
            {
                string month = form["change_month"].ToString();

                if (month.Length == 1)
                    month = "0" + month;
                DateTime begin = DateTime.Parse("01/" + month + "/" + form["change_year"].ToString());
                DateTime end = begin.AddMonths(1).AddSeconds(-1);
                string values;
                if (form["uid"].ToString() != "")
                    values = "{ \"structuredQuery\": { \"from\": [ { \"collectionId\": \"Pontaj\" } ], \"select\": { \"fields\": [ { \"fieldPath\": \"Intrare\" }, { \"fieldPath\": \"Iesire\" } ] }, \"orderBy\": [ { \"field\": { \"fieldPath\": \"Intrare\" }, \"direction\": \"DESCENDING\" } ], \"where\": { \"compositeFilter\": { \"filters\": [ { \"fieldFilter\": { \"field\": { \"fieldPath\": \"id\" }, \"op\": \"EQUAL\", \"value\": { \"referenceValue\": \"projects/smartviewacces/databases/(default)/documents/Angajat/" + form["uid"] + "\" } } }, { \"fieldFilter\": { \"field\": { \"fieldPath\": \"Intrare\" }, \"op\": \"GREATER_THAN_OR_EQUAL\", \"value\": { \"stringValue\": \"" + begin.ToString("yyyyMMddHHmmss") + "\" } } }, { \"fieldFilter\": { \"field\": { \"fieldPath\": \"Intrare\" }, \"op\": \"LESS_THAN_OR_EQUAL\", \"value\": { \"stringValue\": \"" + end.ToString("yyyyMMddHHmmss") + "\" } } } ], \"op\": \"AND\" } } } }";
                else
                    values = "{ \"structuredQuery\": { \"from\": [ { \"collectionId\": \"Pontaj\" } ], \"select\": { \"fields\": [ { \"fieldPath\": \"Intrare\" }, { \"fieldPath\": \"Iesire\" }, { \"fieldPath\": \"id\"} ] }, \"orderBy\": [ { \"field\": { \"fieldPath\": \"Intrare\" }, \"direction\": \"DESCENDING\" } ], \"where\": { \"compositeFilter\": { \"filters\": [ { \"fieldFilter\": { \"field\": { \"fieldPath\": \"Intrare\" }, \"op\": \"GREATER_THAN_OR_EQUAL\", \"value\": { \"stringValue\": \"" + begin.ToString("yyyyMMddHHmmss") + "\" } } }, { \"fieldFilter\": { \"field\": { \"fieldPath\": \"Intrare\" }, \"op\": \"LESS_THAN_OR_EQUAL\", \"value\": { \"stringValue\": \"" + end.ToString("yyyyMMddHHmmss") + "\" } } } ], \"op\": \"AND\" } } } }";
                JObject json = JObject.Parse(values);
                //Debug.WriteLine(json);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                string response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                IEnumerable<JToken> jresponse = JArray.Parse(response).Children()["document"];
                IEnumerable<JToken> angajati;

                List<DateTime> intrari = new List<DateTime>();
                List<DateTime> iesiri = new List<DateTime>();
                List<string> angajat = new List<string>();
                List<string> ore = new List<string>();
                int count = 0;
                int total = 0;
                for (int i = 0; i < jresponse.Count(); i++)
                {
                    if (jresponse.ElementAt(i)["fields"]["Iesire"]["stringValue"].ToString() != "-1")
                    {
                        if (jresponse.ElementAt(i)["fields"]["id"] != null)
                        {
                            values = "{ \"structuredQuery\": {\"select\": {\"fields\": [{\"fieldPath\": \"nume\"}, {\"fieldPath\": \"prenume\"}]}, \"from\": [{\"collectionId\": \"Angajat\"}], \"where\": {\"fieldFilter\":{\"field\":{\"fieldPath\": \"__name__\"}, \"op\": \"EQUAL\", \"value\": {\"referenceValue\":\"" + jresponse.ElementAt(i)["fields"]["id"]["referenceValue"] + "\"}}}}}";
                            json = JObject.Parse(values);
                            content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                            response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                            angajati = JArray.Parse(response).Children()["document"];
                            angajat.Add(angajati.First()["fields"]["nume"]["stringValue"] + " " + angajati.First()["fields"]["prenume"]["stringValue"]);
                        }
                        intrari.Add(DateTime.ParseExact(jresponse.ElementAt(i)["fields"]["Intrare"]["stringValue"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture));
                        iesiri.Add(DateTime.ParseExact(jresponse.ElementAt(i)["fields"]["Iesire"]["stringValue"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture));
                        ore.Add(Convert.ToInt32(Math.Floor((iesiri[count] - intrari[count]).TotalMinutes / 60)).ToString() + ":" + Convert.ToInt32(Math.Floor((iesiri[count] - intrari[count]).TotalMinutes % 60)).ToString());
                        total += Convert.ToInt32((iesiri[count] - intrari[count]).TotalHours);
                        count++;
                    }
                }
                return Json(new { Intrari = intrari, Iesiri = iesiri, Ore = ore, Total = total, Angajat = angajat });
            }
            else if (form["insert_user_cnp"].ToString() != "" && form["insert_user_email_firma"].ToString() != "" && form["insert_user_nume"].ToString() != "" && form["insert_user_prenume"].ToString() != "" && form["insert_user_departament"].ToString() != "" && form["insert_user_etaj"].ToString() != "" && form["insert_user_birou"].ToString() != "" && form["insert_user_loc"].ToString() != "")
            {
                Random rnd = new Random();
                byte[] chars = new byte[] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 56, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122 };
                byte[] password = new byte[10];
                for (int i = 0; i < 10; i++)
                    password[i] = chars[rnd.Next(chars.Length)];

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("AccessSPI@no-reply.com");
                message.To.Add(new MailAddress(form["insert_user_email_firma"].ToString()));
                if (form["insert_user_email"].ToString() != "")
                {
                    message.To.Add(new MailAddress(form["insert_user_email_firma"].ToString()));
                }
                message.Subject = "Contul dvs. AccessSPI";
                message.IsBodyHtml = false;
                message.Body = "Pentru a vă putea loga fie pe aplicație, fie pe browser, folosiți parola " + Encoding.ASCII.GetString(password) + " asociată acestui email.";
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("bossuldam@yahoo.com", "Mm0204199910");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

                SHA256 sha = SHA256.Create();
                byte[] result;
                StringBuilder builder = new StringBuilder();
                result = sha.ComputeHash(password);
                for (int i = 0; i < result.Length; i++)
                {
                    builder.Append(result[i].ToString("x2"));
                }
                string values = "{ \"fields\": { \"email\": {\"stringValue\": \"" + form["insert_user_email"].ToString() + "\"}, \"email_firma\": {\"stringValue\": \"" + form["insert_user_email_firma"].ToString() + "\"}, \"avatar\":{\"stringValue\":\"\"}, \"parola\":{\"stringValue\":\"" + builder + "\"}, \"cnp\": {\"stringValue\": \"" + form["insert_user_cnp"].ToString() + "\"}, \"nume\": {\"stringValue\": \"" + form["insert_user_nume"].ToString() + "\"}, \"prenume\": {\"stringValue\": \"" + form["insert_user_prenume"].ToString() + "\"}, \"numar_inmatriculare\": {\"stringValue\": \"" + form["insert_user_numar_inmatriculare"].ToString() + "\"}, \"departament\": {\"stringValue\": \"" + form["insert_user_departament"].ToString() + "\"}, \"etaj\": {\"integerValue\": \"" + form["insert_user_etaj"].ToString() + "\"}, \"birou\": {\"integerValue\": \"" + form["insert_user_birou"].ToString() + "\"}, \"loc\": {\"integerValue\": \"" + form["insert_user_loc"].ToString() + "\"}}}";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                string response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Angajat/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                JToken jresponse = JObject.Parse(response)["name"];
                string uid = jresponse.ToString().Substring(62);
                return Json(new { Message = uid });
            }
            else if (form["search_email"].ToString() != "")
            {
                string message = "success";
                string values = "{ \"structuredQuery\": { \"select\": {\"fields\": [{\"fieldPath\": \"name\"}]}, \"from\": [{\"collectionId\": \"Angajat\"}], \"where\": {\"fieldFilter\": {\"field\": {\"fieldPath\": \"email\"}, \"op\": \"EQUAL\", \"value\": {\"stringValue\": \"" + form["search_email"] + "\"}}}}}";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                string response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                IEnumerable<JToken> jresponse = JArray.Parse(response).Children()["document"];
                if (jresponse.Count() != 0)
                    message = "error";
                values = "{ \"structuredQuery\": { \"select\": {\"fields\": [{\"fieldPath\": \"name\"}]}, \"from\": [{\"collectionId\": \"Angajat\"}], \"where\": {\"fieldFilter\": {\"field\": {\"fieldPath\": \"email_firma\"}, \"op\": \"EQUAL\", \"value\": {\"stringValue\": \"" + form["search_email"] + "\"}}}}}";
                json = JObject.Parse(values);
                content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                jresponse = JArray.Parse(response).Children()["document"];
                if (jresponse.Count() != 0)
                    message = "error";
                return Json(new { Message = message });

            }
            else if (form["update_user"].ToString() != "" && form["field_name"].ToString() != "" && form["field_value"].ToString() != "")
            {
                string type = "";
                switch (form["field_name"])
                {
                    case "cnp":
                        type = "stringValue";
                        break;
                    case "email":
                        type = "stringValue";
                        break;
                    case "email_firma":
                        type = "stringValue";
                        break;
                    case "nume":
                        type = "stringValue";
                        break;
                    case "prenume":
                        type = "stringValue";
                        break;
                    case "numar_inmatriculare":
                        type = "stringValue";
                        break;
                    case "departament":
                        type = "stringValue";
                        break;
                    case "etaj":
                        type = "integerValue";
                        break;
                    case "birou":
                        type = "integerValue";
                        break;
                    case "loc":
                        type = "integerValue";
                        break;
                }
                string values;
                JObject json;
                StringContent content;
                string response;
                values = "{ \"fields\": { \"" + form["field_name"] + "\": {\"" + type + "\": \"" + form["field_value"].ToString() + "\"}}}";
                json = JObject.Parse(values);
                content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Angajat/" + form["update_user"].ToString() + "?updateMask.fieldPaths=" + form["field_name"] + "&key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "PATCH");

                return Json(new { Message = "success" });
            }
            else if (form["set_parking_lots"].ToString() != "")
            {
                string values = "{ \"fields\": { \"locuri_parcare_max\": {\"integerValue\": \"" + form["set_parking_lots"].ToString() + "\"}}}";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                //Debug.WriteLine(id);
                string response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Cladire/Cladire_SmartView?updateMask.fieldPaths=locuri_parcare_max&key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "PATCH");
            }
            else if (form["request_config"].ToString() != "")
            {
                string values = "{ \"structuredQuery\": { \"select\":{\"fields\":[{\"fieldPath\":\"pereti\"}, {\"fieldPath\":\"mese\"}]}, \"from\":[{\"collectionId\":\"Etaj\"}], \"where\": {\"fieldFilter\": {\"field\":{\"fieldPath\": \"index\"}, \"op\":\"EQUAL\", \"value\": {\"integerValue\":\"" + form["request_config"] + "\"}}}}}";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                string response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Cladire/Cladire_SmartView:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                IEnumerable<JToken> jresponse = JArray.Parse(response).Children()["document"];
                if (jresponse.Count() == 0)
                    return Json(new { Walls = "", Desks = "" });
                return Json(new { Walls = JArray.Parse(response).Children()["document"].First()["fields"]["pereti"]["stringValue"].ToString(), Desks = JArray.Parse(response).Children()["document"].First()["fields"]["mese"]["stringValue"].ToString() });

            }
            else if (form["floor_update"].ToString() != "" && form["walls"].ToString() != "" && form["desks"].ToString() != "")
            {
                string id = "";
                string values = "{ \"structuredQuery\": { \"from\":[{\"collectionId\":\"Etaj\"}], \"where\": {\"fieldFilter\": {\"field\":{\"fieldPath\": \"index\"}, \"op\":\"EQUAL\", \"value\": {\"integerValue\":\"" + form["floor_update"] + "\"}}}}}";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                string response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Cladire/Cladire_SmartView:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                IEnumerable<JToken> jresponse = JArray.Parse(response).Children()["document"];
                if (jresponse.Count() == 0)
                {
                    //query = "{ \"fields\": { \"data\": {\"stringValue\": \"" + DateTime.Today.ToString("yyyyMMdd") + "000000" + "\"}}}";

                    values = "{ \"fields\": { \"index\": {\"integerValue\": \"" + form["floor_update"].ToString() + "\"}, \"pereti\":{\"stringValue\":\"" + form["walls"].ToString() + "\"}, \"mese\":{\"stringValue\":\"" + form["desks"].ToString() + "\"}}}";
                    json = JObject.Parse(values);
                    content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                    response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Cladire/Cladire_SmartView/Etaj/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                    id = JObject.Parse(response)["name"].ToString();
                }
                else
                    id = jresponse.First()["name"].ToString();
                values = "{ \"fields\": { \"pereti\": {\"stringValue\": \"" + form["walls"].ToString() + "\"}, \"mese\": {\"stringValue\": \"" + form["desks"].ToString() + "\"}}}";

                json = JObject.Parse(values);
                content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                //Debug.WriteLine(id);
                response = SendHttpRequest("https://firestore.googleapis.com/v1/" + id + "?updateMask.fieldPaths=pereti&updateMask.fieldPaths=mese&key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "PATCH");

            }
            else if (form["uid"].ToString() != "" && form["administrator_menu_item"].ToString() != "")
            {
                string values = "{ \"structuredQuery\": { \"select\": {\"fields\": [{\"fieldPath\": \"avatar\"}]}, \"from\":[{\"collectionId\":\"Angajat\"}], \"where\": {\"fieldFilter\": {\"field\":{\"fieldPath\": \"__name__\"}, \"op\":\"EQUAL\", \"value\": {\"referenceValue\":\"projects/smartviewacces/databases/(default)/documents/Angajat/" + form["uid"] + "\"}}}}}";
                JObject json = JObject.Parse(values);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                string response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                IEnumerable<JToken> jresponse = JArray.Parse(response).Children()["document"];
                if (jresponse.First()["fields"]["avatar"]["stringValue"].ToString() == "")
                {
                    byte[] filebytes = System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory + "/wwwroot/static/profile.png"));
                    ViewBag.Image = "data:image/png;base64," + Convert.ToBase64String(filebytes);
                }
                else
                {
                    string path = jresponse.First()["fields"]["avatar"]["stringValue"].ToString();
                    path = path.Substring(1);
                    response = SendHttpRequest("https://firebasestorage.googleapis.com/v0/b/smartviewacces.appspot.com/o?name=" + path + "&key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", null, "GET");
                    ViewBag.Image = "https://firebasestorage.googleapis.com/v0/b/smartviewacces.appspot.com/o/" + path.Replace("/", "%2F") + "?alt=media&token=" + JObject.Parse(response)["downloadTokens"].ToString();
                }
                ViewBag.uid = form["uid"].ToString();
                switch (form["administrator_menu_item"].ToString())
                {
                    case "1":
                        string[] months = new string[] { "Ianuarie", "Februarie", "Martie", "Aprilie", "Mai", "Iunie", "Iulie", "August", "Septembrie", "Octombrie", "Noiembrie", "Decembrie" };
                        ViewBag.CurrentMonth = DateTime.Now.Month;
                        ViewBag.Months = months;
                        values = "{ \"structuredQuery\": {\"select\": {\"fields\": [{\"fieldPath\": \"Intrare\"}, {\"fieldPath\": \"Iesire\"}, {\"fieldPath\": \"id\"}]}, \"from\": [{\"collectionId\": \"Pontaj\"}], \"orderBy\": [{\"field\": {\"fieldPath\": \"Intrare\"}, \"direction\": \"DESCENDING\"}], \"where\": {\"compositeFilter\": {\"filters\": [{\"fieldFilter\":{\"field\":{\"fieldPath\": \"Intrare\"}, \"op\": \"GREATER_THAN_OR_EQUAL\", \"value\": {\"stringValue\":\"" + DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyyMMdd") + "000000" + "\"}}}], \"op\":\"AND\"}}}}";
                        json = JObject.Parse(values);
                        content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                        response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                        jresponse = JArray.Parse(response).Children()["document"];
                        IEnumerable<JToken> angajati;
                        List<DateTime> intrari = new List<DateTime>();
                        List<string> angajat = new List<string>();
                        List<DateTime> iesiri = new List<DateTime>();
                        List<string> ore = new List<string>();
                        int total = 0;
                        int count = 0;
                        for (int i = 0; i < jresponse.Count(); i++)
                        {
                            if (jresponse.ElementAt(i)["fields"]["Iesire"]["stringValue"].ToString() != "-1")
                            {
                                values = "{ \"structuredQuery\": {\"select\": {\"fields\": [{\"fieldPath\": \"nume\"}, {\"fieldPath\": \"prenume\"}]}, \"from\": [{\"collectionId\": \"Angajat\"}], \"where\": {\"fieldFilter\":{\"field\":{\"fieldPath\": \"__name__\"}, \"op\": \"EQUAL\", \"value\": {\"referenceValue\":\"" + jresponse.ElementAt(i)["fields"]["id"]["referenceValue"] + "\"}}}}}";
                                json = JObject.Parse(values);
                                content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                                response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                                angajati = JArray.Parse(response).Children()["document"];
                                angajat.Add(angajati.First()["fields"]["nume"]["stringValue"] + " " + angajati.First()["fields"]["prenume"]["stringValue"]);
                                intrari.Add(DateTime.ParseExact(jresponse.ElementAt(i)["fields"]["Intrare"]["stringValue"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture));
                                iesiri.Add(DateTime.ParseExact(jresponse.ElementAt(i)["fields"]["Iesire"]["stringValue"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture));
                                ore.Add(Convert.ToInt32(Math.Floor((iesiri[count] - intrari[count]).TotalMinutes / 60)).ToString() + ":" + Convert.ToInt32(Math.Floor((iesiri[count] - intrari[count]).TotalMinutes % 60)).ToString());
                                total += Convert.ToInt32((iesiri[count] - intrari[count]).TotalHours);
                                count++;
                            }
                        }
                        ViewBag.Angajat = angajat;
                        ViewBag.Count = count;
                        ViewBag.Total = total;
                        ViewBag.Intrari = intrari;
                        ViewBag.Iesiri = iesiri;
                        ViewBag.Ore = ore;
                        return View("Administrator_Menu_1");
                    case "2":
                        values = "{ \"structuredQuery\": {\"select\": {\"fields\": [{\"fieldPath\": \"locuri_parcare_max\"}]}, \"from\": [{\"collectionId\": \"Cladire\"}]}}";
                        json = JObject.Parse(values);
                        content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                        response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                        jresponse = JArray.Parse(response).Children()["document"];
                        ViewBag.Parcare = jresponse.First()["fields"]["locuri_parcare_max"]["integerValue"].ToString();
                        values = "{ \"structuredQuery\": {\"select\": {\"fields\": [{\"fieldPath\": \"index\"}]}, \"from\": [{\"collectionId\": \"Etaj\"}], \"orderBy\": [{\"field\": {\"fieldPath\": \"index\"}, \"direction\": \"ASCENDING\"}]}}";
                        json = JObject.Parse(values);
                        content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                        response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Cladire/Cladire_SmartView:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                        jresponse = JArray.Parse(response).Children()["document"];
                        ViewBag.Etaje = jresponse.Count();
                        ViewBag.Pereti = null;
                        ViewBag.Mese = null;
                        if (jresponse.Count() > 0)
                        {
                            values = "{ \"structuredQuery\": {\"select\": {\"fields\": [{\"fieldPath\": \"pereti\"}, {\"fieldPath\": \"mese\"}]}, \"from\": [{\"collectionId\": \"Etaj\"}], \"where\": {\"fieldFilter\":{\"field\":{\"fieldPath\": \"__name__\"}, \"op\":\"EQUAL\", \"value\":{\"referenceValue\": \"" + jresponse.First()["name"] + "\"}}}}}";
                            json = JObject.Parse(values);
                            content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                            response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents/Cladire/Cladire_SmartView:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                            jresponse = JArray.Parse(response).Children()["document"];
                            ViewBag.Pereti = jresponse.First()["fields"]["pereti"]["stringValue"].ToString().Split(' ');
                            ViewBag.Mese = jresponse.First()["fields"]["mese"]["stringValue"].ToString().Split(' ');
                        }


                        //jresponse = JArray.Parse(response).Children()["document"];
                        /*List<string> etaje = new List<string>();
                        List<string> etaje_ids = new List<string>();
                        List<string> birouri = new List<string>();
                        List<string> birouri_ids = new List<string>();
                        List<string> locuri = new List<string>();
                        for (int i = 0; i < jresponse.Count(); i++)
                        {
                            etaje.Add(jresponse.ElementAt(i)["fields"]["index"]["integerValue"].ToString());
                            etaje_ids.Add(jresponse.ElementAt(i)["name"].ToString());
                        }

                        ViewBag.Etaje = etaje;
                        ViewBag.Etaje_ids = etaje_ids;
                        ViewBag.Birouri = birouri;
                        ViewBag.Birouri_ids = birouri_ids;
                        ViewBag.Locuri = locuri;
                        //return View();*/
                        return View("Administrator_Menu_2");
                    case "3":
                        values = "{ \"structuredQuery\": {\"select\": {\"fields\": [{\"fieldPath\": \"cnp\"}, {\"fieldPath\": \"email\"}, {\"fieldPath\": \"email_firma\"}, {\"fieldPath\": \"nume\"}, {\"fieldPath\": \"prenume\"}, {\"fieldPath\": \"numar_inmatriculare\"}, {\"fieldPath\": \"departament\"}, {\"fieldPath\": \"etaj\"}, {\"fieldPath\": \"birou\"}, {\"fieldPath\": \"loc\"}]}, \"from\": [{\"collectionId\": \"Angajat\"}]}}";
                        json = JObject.Parse(values);
                        content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                        response = SendHttpRequest("https://firestore.googleapis.com/v1/projects/smartviewacces/databases/(default)/documents:runQuery/?key=AIzaSyAfTvf08m4ZPebBTzN3wW_xyEQ61OqF8EA", content, "POST");
                        List<string> ids = new List<string>();
                        List<string> cnp = new List<string>();
                        List<string> email = new List<string>();
                        List<string> email_firma = new List<string>();
                        List<string> nume = new List<string>();
                        List<string> prenume = new List<string>();
                        List<string> numar_inmatriculare = new List<string>();
                        List<string> departament = new List<string>();
                        List<string> etaj = new List<string>();
                        List<string> birou = new List<string>();
                        List<string> loc = new List<string>();
                        jresponse = JArray.Parse(response).Children()["document"];
                        for (int i = 0; i < jresponse.Count(); i++)
                        {
                            ids.Add(jresponse.ElementAt(i)["name"].ToString().Substring(62));
                            cnp.Add(jresponse.ElementAt(i)["fields"]["cnp"]["stringValue"].ToString());
                            email.Add(jresponse.ElementAt(i)["fields"]["email"]["stringValue"].ToString());
                            email_firma.Add(jresponse.ElementAt(i)["fields"]["email_firma"]["stringValue"].ToString());
                            nume.Add(jresponse.ElementAt(i)["fields"]["nume"]["stringValue"].ToString());
                            prenume.Add(jresponse.ElementAt(i)["fields"]["prenume"]["stringValue"].ToString());
                            numar_inmatriculare.Add(jresponse.ElementAt(i)["fields"]["numar_inmatriculare"]["stringValue"].ToString());
                            departament.Add(jresponse.ElementAt(i)["fields"]["departament"]["stringValue"].ToString());
                            etaj.Add(jresponse.ElementAt(i)["fields"]["etaj"]["integerValue"].ToString());
                            birou.Add(jresponse.ElementAt(i)["fields"]["birou"]["integerValue"].ToString());
                            loc.Add(jresponse.ElementAt(i)["fields"]["loc"]["integerValue"].ToString());

                        }
                        ViewBag.Count = jresponse.Count();
                        ViewBag.Ids = ids;
                        ViewBag.CNP = cnp;
                        ViewBag.Email = email;
                        ViewBag.Email_firma = email_firma;
                        ViewBag.Nume = nume;
                        ViewBag.Prenume = prenume;
                        ViewBag.Numar_inmatriculare = numar_inmatriculare;
                        ViewBag.Departament = departament;
                        ViewBag.Etaj = etaj;
                        ViewBag.Birou = birou;
                        ViewBag.Loc = loc;
                        return View("Administrator_Menu_3");
                    case "4": return View("Administrator_Menu_4");
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
