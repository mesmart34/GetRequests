using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GetRequests
{
    public struct Student
    {
        public string LastName;
        public string FirstName;
        public string MiddleName;
    }

    public struct Team
    {
        public string Name;
        public List<Student> Students;
    }


    public class RequestsManager
    {
        private const string m_url = "https://xn--e1aajagmjdbheh6azd.xn--p1ai";
        private readonly string m_sessionId;

        public RequestsManager(string sessionId)
        {
            m_sessionId = sessionId;
        }

        public string GetEventName()
        {
            var response = CreateWebRequest("api/v1/Event");
            dynamic events = JArray.Parse(response);
            dynamic currentEvent = events[0];
            string name = currentEvent.name;
            return name;
        }

        public Team GetEventData()
        {
            var request = CreateWebRequest("api/v1/User");

            dynamic eventData = JsonConvert.DeserializeObject(request);
            
            dynamic users = eventData.team.users;
            var team = new Team();
            team.Name = (string)eventData.team.project.name;
            team.Students = new List<Student>();
            foreach(var user in users)
            {
                string lastName = user.lastName;
                string firstName = user.firstName;
                string middleName = user.middleName;
                team.Students.Add(new Student()
                {
                    LastName = lastName,
                    FirstName = firstName,
                    MiddleName = middleName
                });
            }


            team.Students.TrimExcess();

            return team;
        }

        private string CreateWebRequest(string command)
        {
            var request = (HttpWebRequest)WebRequest.Create(m_url + "/" + command);
            request.Headers["Cookie"] = "SID=" + m_sessionId;
            request.AutomaticDecompression = DecompressionMethods.GZip;
            var response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            var result = reader.ReadToEnd();
            return result;
        }
    }
}
