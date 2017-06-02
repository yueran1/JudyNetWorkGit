using BusinessLogic;
using JudyDou.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Xml;

namespace JudyDou.Helper
{
    public static class CREAHelper
    {
        private static HttpWebRequest httpWebRequest;
        private static CredentialCache requestCredentialCache = new CredentialCache();
        private static string RetsUrl = "http://data.crea.ca";
        private static ICredentials requestCredentials = new NetworkCredential("g5R7s6SfUXeSaHcjBpGu7MzC", "PNrUckpbJMkBgRCOV5gP01va");
        private static CookieContainer cookieJar = new CookieContainer();
        private static string ImagePath = "/Images/Listing/";


        /// <summary>
        /// STEP 1. Login
        /// STEP 2. Search Properties by LastUpdated Query (This returns a list of properties that are new or updated)
        /// STEP 3. Search Properties by All to return a complete master list to validate against ex: (ID=*) 
        ///         a. Delete Properties that are in your DB but not in the complete list
        ///         b. If the Property exists but the lastupdated is different call Search Property By ID ex: (ID=123456,789012)
        /// STEP 4. Logout
        /// </summary>
        /// <remarks>http://data.crea.ca/Documentation/</remarks>
        public static void GetUpdated(UnitOfWork unitOfWork, HttpServerUtilityBase server, DateTime date)
        {
            if ((DateTime.Now - date).TotalHours > 1)
            {
                LoginTransaction();
                ProcessModel(unitOfWork, server, SearchTransaction("Property", "Property", "DMQL2", "(LastUpdated=" + date.ToString("yyyy-MM-ddThh:mm:ssZ") + ")", 1, "100"));
            }
        }

        public static void GetAll(UnitOfWork unitOfWork, HttpServerUtilityBase server)
        {
            LoginTransaction();
            ProcessModel(unitOfWork, server, SearchTransaction("Property", "Property", "DMQL2", "(ID=*)"));
        }

        private static void ProcessModel(UnitOfWork unitOfWork, HttpServerUtilityBase server, List<ListingModel> model)
        {
            //List<Listing> entities = unitOfWork.ListingRepository.Get(0, int.MaxValue, (int)Constants.ListingStatus.Active);

            foreach (var item in model)
            {
                if (item.Photo == null || item.Photo.Count == 0)
                {
                    item.Property.Image = ImagePath + "default.jpg";
                }

                //foreach (var entity in entities)
                //{
                //    if (entity.Id == item.Property.Id)
                //    {
                //        entities.Remove(entity);
                //    }
                //}

                unitOfWork.ListingRepository.Update(item.Property);
            }

            //foreach (var entity in entities)
            //{
            //    entity.Status = (int)Constants.ListingStatus.Expired;
            //    unitOfWork.ListingRepository.Update(entity);
            //}

            new Thread(new ThreadStart(() =>
            {
                foreach (var item in model)
                {
                    if (item.Photo != null)
                    {
                        foreach (var sequence in item.Photo)
                        {
                            //GetObject("Agent", "Photo");
                            //GetObject("Office", "ThumbnailPhoto");
                            GetObject(server, "Property", item.Property.Id, sequence, "LargePhoto");
                        }
                    }
                }

                LogoutTransaction();
            })).Start();
        }

        /// <summary>
        /// A client MUST issue a login request prior to proceeding with any other request. 
        /// The Login transaction verifies all login information provided by the user and begins a RETS session. 
        /// Subsequent session control may be mediated by HTTP cookies or any other method, though clients are 
        /// required to support at least session control via HTTP cookies.
        /// </summary>
        /// <remarks>Service End Point - /Login.svc/Login</remarks>
        public static void LoginTransaction()
        {
            string service = RetsUrl + "/Login.svc/Login";

            CookieContainer loginCookie = new CookieContainer();

            httpWebRequest = (HttpWebRequest)WebRequest.Create(service);
            httpWebRequest.CookieContainer = loginCookie; //STORE THE REQUEST COOKIE
            httpWebRequest.Credentials = requestCredentials;

            try
            {
                using (HttpWebResponse httpResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    Stream stream = httpResponse.GetResponseStream();
                    // READ THE RESPONSE STREAM USING XMLTEXTREADER
                    XmlTextReader reader = new XmlTextReader(stream);

                    while (reader.Read())
                    {
                        if (reader.Name == "RETS")
                        {
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "ReplyCode" & reader.Value == "0")
                                    {
                                        //STORE THE COOKIES RECEIVED FOR FUTURE RETRIEVAL
                                        //FOR STATELESS APPLICATION PROCESSING, STORE THE COOKIES RECEIVED IN THE SESSION STATE FOR FUTURE RETRIEVAL BY THIS SESSION.
                                        loginCookie.Add(httpResponse.Cookies);
                                        cookieJar = loginCookie;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// The Logout transaction terminates a session.  Clients should send a Logout transaction at the end of every session.
        /// </summary>
        /// <remarks>Service End Point - /Logout.svc/Logout</remarks>
        public static void LogoutTransaction()
        {
            string service = RetsUrl + "/Logout.svc/Logout";

            httpWebRequest = (HttpWebRequest)WebRequest.Create(service);
            httpWebRequest.CookieContainer = cookieJar; //GRAB THE COOKIE
            httpWebRequest.Credentials = requestCredentials; //PASS CREDENTIALS

            try
            {
                using (HttpWebResponse httpResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    Stream stream = httpResponse.GetResponseStream();
                    // READ THE RESPONSE STREAM USING XMLTEXTREADER
                    XmlTextReader reader = new XmlTextReader(stream);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Used to search Office, Agent or Property
        /// </summary>
        /// <param name="SearchType">Selects the Resource type to be returned</param>
        /// <param name="Class">Represents the class of the data within the SearchType</param>
        /// <param name="QueryType">An enumeration giving the language in which the query is presented</param>
        /// <param name="Query">The query to be executed by the server</param>
        /// <param name="Count">Controls whether the server response includes a record count</param>
        /// <param name="Limit">Request the server to apply a limit on the number of records returned in the search.</param>
        /// <param name="Offset">Request the server start at other than the first record in the set of matching records</param>
        /// <param name="Culture">Results localization</param>
        /// <param name="Format">Selects the supported data return format for the query response</param>
        /// <remarks>Service End Point - /Search.svc/Search</remarks>
        public static List<ListingModel> SearchTransaction(string SearchType, string Class, string QueryType, string Query, int Count = 1, string Limit = "None", int Offset = 1, string Culture = "en-CA", string Format = "STANDARD-XML")
        {
            string requestArguments = "?Format=" + Format + "&SearchType=" + SearchType + "&Class=" + Class + "&QueryType=" + QueryType + "&Query=" + Query + "&Count=" + Count + "&Limit=" + Limit + "&Offset=" + Offset + "&Culture=" + Culture;
            string searchService = RetsUrl + "/Search.svc/Search" + requestArguments;
            List<ListingModel> result = new List<ListingModel>();

            httpWebRequest = (HttpWebRequest)WebRequest.Create(searchService);
            httpWebRequest.CookieContainer = cookieJar; //GRAB THE COOKIE
            httpWebRequest.Credentials = requestCredentials; //PASS CREDENTIALS

            try
            {
                using (HttpWebResponse httpResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    Stream stream = httpResponse.GetResponseStream();
                    // READ THE RESPONSE STREAM USING XMLTEXTREADER
                    XmlTextReader reader = new XmlTextReader(stream);
                    ListingModel model = null;

                    while (reader.Read())
                    {
                        switch (reader.Name)
                        {
                            case "PropertyDetails":
                                if (reader.NodeType == XmlNodeType.EndElement)
                                {
                                    result.Add(model);
                                }
                                else if (reader.HasAttributes)
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name == "ID")
                                        {
                                            model = new ListingModel();
                                            model.Property = new Listing();
                                            model.Property.Id = int.Parse(reader.Value);
                                            model.Property.Status = (int)Constants.ListingStatus.Active;
                                        }
                                    }
                                }
                                break;

                            case "Photo":
                                model.Photo = ReadXMLElement(reader, "Photo", "PropertyPhoto", "SequenceId");
                                if (model.Photo.Count > 0)
                                    model.Property.Image = ImagePath + model.Property.Id + "/" + model.Photo[0] + ".jpg";
                                break;

                            case "PublicRemarks":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.Description = ReadXMLElement(reader);
                                break;

                            case "Features":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.Features = ReadXMLElement(reader);
                                break;

                            case "StreetAddress":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.Address = ReadXMLElement(reader);
                                break;

                            case "City":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.City = ReadXMLElement(reader);
                                break;

                            case "Province":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.Province = ReadXMLElement(reader);
                                break;

                            case "PostalCode":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.PostalCode = ReadXMLElement(reader);
                                break;

                            case "ListingID":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.MLSNumber = ReadXMLElement(reader);
                                break;

                            case "Price":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.Price = "$" + ReadXMLElement(reader);
                                break;

                            case "BedroomsTotal":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.Bedrooms = int.Parse(ReadXMLElement(reader));
                                break;

                            case "BathroomTotal":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.Bathrooms = int.Parse(ReadXMLElement(reader));
                                break;

                            case "SizeTotal":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.LotArea = ReadXMLElement(reader);
                                break;

                            case "SizeInterior":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.FloorArea = ReadXMLElement(reader);
                                break;

                            case "ConstructedDate":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.Age = int.Parse(ReadXMLElement(reader));
                                break;

                            case "MoreInformationLink":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    model.Property.MoreInfoLink = ReadXMLElement(reader);
                                break;
                        }
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        public static string ReadXMLElement(XmlTextReader reader, bool nested = false)
        {
            if (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.Read();

                if (nested)
                {
                    do
                    {
                        if (reader.HasValue)
                        {
                            return reader.Value;
                        }
                        else if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            break;
                        }
                    } while (reader.Read());
                }
                else if (reader.HasValue)
                {
                    return reader.Value;
                }
            }

            return null;
        }

        public static List<string> ReadXMLElement(XmlTextReader reader, string parent, params string[] children)
        {
            List<string> result = new List<string>();

            do
            {
                if (reader.Name.Equals(parent) && reader.NodeType == XmlNodeType.EndElement)
                {
                    break;
                }
                else
                {
                    for (int i = 0; i < children.Length; i++)
                    {
                        do
                        {
                            string what = children[i];
                            bool what1 = reader.Name == children[i];
                            bool what2 = reader.Name.Equals(children[i]);

                            if (reader.NodeType != XmlNodeType.EndElement)
                            {
                                if (reader.Name.Equals(children[i]))
                                {
                                    if (i < children.Length - 1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        reader.Read();
                                        if (reader.HasValue)
                                        {
                                            result.Add(reader.Value);
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (reader.Name.Equals(children[0]))
                            {
                                i = children.Length;
                                break;
                            }
                        } while (reader.Read());
                    }
                }
            } while (reader.Read());

            return result;
        }

        public static void GetObject(HttpServerUtilityBase server, string strResource, int id, string sequenceId, string strType)
        {
            string requestArguments = "?Resource=" + strResource + "&ID=" + id + ":" + sequenceId + "&Type=" + strType;
            string searchService = RetsUrl + "/Object.svc/GetObject" + requestArguments;
            string path = server.MapPath("~" + ImagePath + id);
            string file = path + "/" + sequenceId + ".jpg";

            if (!File.Exists(file))
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(searchService);
                httpWebRequest.CookieContainer = cookieJar; //GRAB THE COOKIE
                httpWebRequest.Credentials = requestCredentials; //PASS CREDENTIALS

                try
                {
                    using (HttpWebResponse httpResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                    {
                        // READ THE RESPONSE STREAM USING XMLTEXTREADER
                        if (httpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            Stream stream = httpResponse.GetResponseStream(); // READ THE RESPONSE STREAM USING STREAMREADER
                            StreamReader reader = new StreamReader(stream);

                            Directory.CreateDirectory(path);

                            Image image = Image.FromStream(stream);
                            image.Save(file, ImageFormat.Jpeg);
                        }
                    }
                }
                catch
                {
                }
            }
        }
    }
}
