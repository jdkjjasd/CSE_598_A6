using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.IO;
using System.Net.Http;


namespace A5
{
    /// <summary>
    /// WeatherService allows a user to enter a zip code and get the weather for the next 5 days at that zip
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WeatherService : WebService
    {
        private const string NWSBaseUrl = "https://graphical.weather.gov/xml/SOAP_server/ndfdXMLserver.php";

        [WebMethod]
        public string[] Weather5day(string zipcode)
        {
            try
            {
                // Get Latitude and Longitude from ZIP code
                string latLon = GetLatLonFromZip(zipcode);
                if (string.IsNullOrEmpty(latLon))
                    throw new Exception("Failed to retrieve latitude and longitude for the provided ZIP code.");

                // Fetch the 5 day forecast using the latitude and longitude
                string[] forecast = Get5DayForecast(latLon);

                return forecast;
            }
            catch (Exception ex)
            {
                return new string[] { "Error: " + ex.Message };
            }
        }

        // Function to retrieve the latitude and longitude from zip code
        private string GetLatLonFromZip(string zipcode)
        {
            string soapEnvelope = $@"<soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/' xmlns:web='https://digital.weather.gov/xml/wsdl/ndfdXML.wsdl'>
                               <soap:Header/>
                               <soap:Body>
                                  <web:LatLonListZipCode>
                                     <web:zipCodeList>{zipcode}</web:zipCodeList>
                                     <web:XMLformat>None</web:XMLformat>
                                     <web:listType>latLonList</web:listType>
                                  </web:LatLonListZipCode>
                               </soap:Body>
                            </soap:Envelope>";

            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://digital.weather.gov/xml/SOAP_server/ndfdXMLserver.php")
                {
                    Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml")
                };
                request.Headers.Add("SOAPAction", "https://digital.weather.gov/xml/wsdl/ndfdXML.wsdl#LatLonListZipCode");

                HttpResponseMessage response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    string xmlResult = response.Content.ReadAsStringAsync().Result;

                    // Log the raw response
                    System.Diagnostics.Debug.WriteLine("Raw Response: " + xmlResult);

                    // Load the SOAP response
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlResult);

                    // Extract the <listLatLonOut> content
                    XmlNode listLatLonOutNode = xmlDoc.SelectSingleNode("//listLatLonOut");
                    if (listLatLonOutNode == null)
                    {
                        throw new Exception("Could not find listLatLonOut node in the response.");
                    }

                    string encodedInnerXml = listLatLonOutNode.InnerText;

                    // Decode the inner XML
                    XmlDocument innerXmlDoc = new XmlDocument();
                    innerXmlDoc.LoadXml(System.Net.WebUtility.HtmlDecode(encodedInnerXml));

                    // Extract the <latLonList> value
                    XmlNode latLonListNode = innerXmlDoc.SelectSingleNode("//latLonList");
                    if (latLonListNode == null)
                    {
                        throw new Exception("Could not find latLonList node in the inner XML.");
                    }

                    return latLonListNode.InnerText; // Example: "39.3789,-104.851"
                }
                else
                {
                    throw new Exception($"Error fetching latitude and longitude: {response.StatusCode}");
                }
            }
        }


        // Function to get the 5 day weather forecast of a zip code
        private string[] Get5DayForecast(string latLon)
        {
            // Debugging latitude and longitude
            if (string.IsNullOrEmpty(latLon) || !latLon.Contains(","))
                throw new Exception("Invalid latitude and longitude format.");

            var coords = latLon.Split(',');

            // Debugging latitude and longitude
            if (coords.Length != 2 || string.IsNullOrEmpty(coords[0]) || string.IsNullOrEmpty(coords[1]))
                throw new Exception("Invalid latitude and longitude data.");

            decimal latitude;
            decimal longitude;

            // Parse the latitude and longitude
            if (!decimal.TryParse(coords[0], out latitude) || !decimal.TryParse(coords[1], out longitude))
                throw new Exception("Failed to parse latitude or longitude.");

            // Current date and end date (5 days ahead)
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddDays(5);

            // SOAP request to fetch the 5 day weather forecast
            string soapEnvelope = $@"<soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/' xmlns:web='https://digital.weather.gov/xml/wsdl/ndfdXML.wsdl'>
                               <soap:Header/>
                               <soap:Body>
                                  <web:NDFDgen>
                                     <web:latitude>{latitude}</web:latitude>
                                     <web:longitude>{longitude}</web:longitude>
                                     <web:product>time-series</web:product>
                                     <web:XMLformat>24 hourly</web:XMLformat>
                                     <web:startTime>{startTime:yyyy-MM-ddTHH:mm:ss}</web:startTime>
                                     <web:endTime>{endTime:yyyy-MM-ddTHH:mm:ss}</web:endTime>
                                     <web:Unit>e</web:Unit>
                                     <web:weatherParameters>
                                        <web:maxt>true</web:maxt> <!-- Max temperature -->
                                        <web:mint>true</web:mint> <!-- Min temperature -->
                                        <web:pop12>true</web:pop12> <!-- Precipitation -->
                                     </web:weatherParameters>
                                  </web:NDFDgen>
                               </soap:Body>
                            </soap:Envelope>";

            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://digital.weather.gov/xml/SOAP_server/ndfdXMLserver.php")
                {
                    Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml")
                };
                request.Headers.Add("SOAPAction", "https://digital.weather.gov/xml/wsdl/ndfdXML.wsdl#NDFDgen");

                HttpResponseMessage response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    string xmlResult = response.Content.ReadAsStringAsync().Result;

                    // Log raw response for debugging
                    System.Diagnostics.Debug.WriteLine("Raw NDFDgen Response: " + xmlResult);

                    // Parse the weather forecast from the XML response
                    return ParseForecastFromXml(xmlResult);
                }
                else
                {
                    throw new Exception($"Error fetching 5-day weather forecast: {response.StatusCode}");
                }
            }
        }

        // Function to parse the retrieved weather xml
        private string[] ParseForecastFromXml(string xmlResult)
        {
            try
            {
                // Load the outer SOAP response
                XmlDocument outerXmlDoc = new XmlDocument();
                outerXmlDoc.LoadXml(xmlResult);

                // Extract the <XMLOut> node content
                XmlNode xMLOutNode = outerXmlDoc.SelectSingleNode("//XMLOut");
                if (xMLOutNode == null)
                {
                    throw new Exception("XMLOut node is missing in the response.");
                }

                // Decode the inner XML
                string nestedXml = System.Net.WebUtility.HtmlDecode(xMLOutNode.InnerText).Trim();
                nestedXml = nestedXml.Replace("&", "&amp;"); // Re-escape any problematic ampersands


                // Log the nested XML
                System.Diagnostics.Debug.WriteLine("Nested XML: " + nestedXml);

                // Load the nested XML into a new XmlDocument
                XmlDocument nestedXmlDoc = new XmlDocument();
                nestedXmlDoc.LoadXml(nestedXml);

                // Extract forecast data from the nested XML
                XmlNodeList maxTempNodes = nestedXmlDoc.SelectNodes("//temperature[@type='maximum']/value");
                XmlNodeList minTempNodes = nestedXmlDoc.SelectNodes("//temperature[@type='minimum']/value");
                XmlNodeList precipitationNodes = nestedXmlDoc.SelectNodes("//probability-of-precipitation/value");

                if (maxTempNodes == null || minTempNodes == null || precipitationNodes == null)
                {
                    throw new Exception("Missing weather parameter nodes in the nested XML.");
                }

                // Prepare the forecast data
                string[] forecast = new string[5];
                for (int i = 0; i < 5; i++)
                {
                    string maxTemp = maxTempNodes?.Item(i)?.InnerText ?? "N/A";
                    string minTemp = minTempNodes?.Item(i)?.InnerText ?? "N/A";
                    string precipitation = precipitationNodes?.Item(i)?.InnerText ?? "N/A";

                    forecast[i] = $"Day {i + 1}: Max Temp {maxTemp}°F, Min Temp {minTemp}°F, Precipitation {precipitation}%";

                    // Log each day's forecast
                    System.Diagnostics.Debug.WriteLine($"Forecast for Day {i + 1}: {forecast[i]}");
                }

                return forecast;
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                throw new Exception("Failed to parse weather data. Check the XML structure.", ex);
            }
        }

    }
}
