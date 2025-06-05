

using Microsoft.Extensions.Logging;
using SimpleSOAPClient.Exceptions;
using SimpleSOAPClient.Handlers;
using SimpleSOAPClient.Models;
using SimpleSOAPClient;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using SimpleSOAPClient.Helpers;
using Wesky.Net.OpenTools.HttpExtensions;
using System.Reflection;
using System.Xml.Linq;

namespace AOPDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [Log]
        public IEnumerable<WeatherForecast> Get(string str)
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("WebMethod1")]
        public async Task Get1()
        {
            var url = "http://192.168.30.226:9010/WebService1.asmx?wsdl";
            var webservice = new WebserviceHelper();
            var r = webservice.CallWebservice(url, "Test", parameters: "88889");
            var res =webservice.ExtractCustomerValueFromXml<TestResult>(r.Result, "TestResult", OpenWebserviceInfo.OpenWebservice.FirstOrDefault(x => x.WebserviceUrl == url&&x.OperationName=="Test").Namespace);
        }

        //private T ExtractCustomerValueFromXml1<T>(string xml, string rootNode, XNamespace ns) where T : new()
        //{
        //    XDocument xDocument = XDocument.Parse(xml);
        //    T val = new T();
        //    XElement xElement = xDocument.Descendants(ns + rootNode).FirstOrDefault();
        //    if (xElement == null)
        //    {
        //        throw new InvalidOperationException("Root node '" + rootNode + "' not found in XML.");
        //    }

        //    PropertyInfo[] properties = typeof(T).GetProperties();
        //    foreach (PropertyInfo propertyInfo in properties)
        //    {
        //        XElement xElement2 = xElement.Element(ns + propertyInfo.Name);
        //        if (xElement2 == null)
        //        {
        //            continue;
        //        }

        //        if (propertyInfo.PropertyType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
        //        {
        //            Type type = propertyInfo.PropertyType.GetGenericArguments()[0];
        //            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
        //            foreach (XElement item in xElement2.Elements())
        //            {
        //                list.Add(ParseXmlElement(type, item, ns));
        //            }

        //            propertyInfo.SetValue(val, list);
        //        }
        //        else if (!propertyInfo.PropertyType.IsPrimitive && propertyInfo.PropertyType != typeof(string))
        //        {
        //            object value = ParseXmlElement(propertyInfo.PropertyType, xElement2, ns);
        //            propertyInfo.SetValue(val, value);
        //        }
        //        else
        //        {
        //            try
        //            {
        //                object value2 = Convert.ChangeType(xElement2.Value, propertyInfo.PropertyType);
        //                propertyInfo.SetValue(val, value2);
        //            }
        //            catch (Exception innerException)
        //            {
        //                throw new InvalidOperationException("Error setting property " + propertyInfo.Name + " from XML. See inner exception for details.", innerException);
        //            }
        //        }
        //    }

        //    return val;
        //}

        //private object ParseXmlElement(Type type, XElement element, XNamespace ns)
        //{
        //    if (type == typeof(string))
        //    {
        //        return element.Value;
        //    }

        //    object obj = Activator.CreateInstance(type);
        //    PropertyInfo[] properties = type.GetProperties();
        //    foreach (PropertyInfo propertyInfo in properties)
        //    {
        //        XElement xElement = element.Element(ns + propertyInfo.Name);
        //        if (xElement == null)
        //        {
        //            continue;
        //        }

        //        if (propertyInfo.PropertyType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
        //        {
        //            Type type2 = propertyInfo.PropertyType.GetGenericArguments()[0];
        //            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type2));
        //            foreach (XElement item in xElement.Elements())
        //            {
        //                list.Add(ParseXmlElement(type2, item, ns));
        //            }

        //            propertyInfo.SetValue(obj, list);
        //        }
        //        else if (!propertyInfo.PropertyType.IsPrimitive && propertyInfo.PropertyType != typeof(string))
        //        {
        //            object value = ParseXmlElement(propertyInfo.PropertyType, xElement, ns);
        //            propertyInfo.SetValue(obj, value);
        //        }
        //        else
        //        {
        //            object value2 = ((!(propertyInfo.PropertyType == typeof(string))) ? Convert.ChangeType(xElement.Value, propertyInfo.PropertyType) : xElement.Value);
        //            propertyInfo.SetValue(obj, value2);
        //        }
        //    }

        //    return obj;
        //}
        private string CreateSoapRequest(int inputValue)
        {
            // SOAP 请求内容
            var soapRequest = @"
        <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
            <soap:Body>
                <!-- 你的 SOAP 请求体 -->
            </soap:Body>
        </soap:Envelope>";
            return soapRequest;
        }

    }
}
