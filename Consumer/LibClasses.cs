using System;
using System.Threading.Tasks;
using MassTransit;
using NordPoolMessage;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace Consumer
{
    public class TeamMember
    {
        public int Id;
        public string Name;
    }

    public interface ITeamMemberRepository
    {
        void Save(TeamMember name);
    }

    public class TeamMemberRepository : ITeamMemberRepository
    {
        public void Save(TeamMember name)
        {
            Console.WriteLine(string.Concat("The team member repository called for ", name.Name));
        }
    }

    public class RegisterTeamMemberConsumer : IConsumer<IRegisterTeamMember>
    {
        private readonly ITeamMemberRepository _nameRepository;

        public RegisterTeamMemberConsumer(ITeamMemberRepository nameRepository)
        {
            _nameRepository = nameRepository ?? throw new ArgumentNullException("couldn't instantiate team member repository");
        }

        public Task Consume(ConsumeContext<IRegisterTeamMember> context)
        {
            IRegisterTeamMember newName = context.Message;
            Console.WriteLine("Registering new team member ");
            TeamMember tmObj = DownloadJsonData(newName.MockyURL);
            _nameRepository.Save(tmObj);
            context.Publish<ITeamMemberRegistered>(new { Id = tmObj.Id, Name = tmObj.Name });
            return Task.FromResult(context.Message);
        }

        private static TeamMember DownloadJsonData(string url) 
        {
            // Using Webclient
            //using (var w = new WebClient())
            //{
            //    var json_data = string.Empty;

            //    TeamMember tmObj = new TeamMember();

            //    json_data = w.DownloadString(url);

            //    if (!string.IsNullOrEmpty(json_data))
            //        tmObj = JsonConvert.DeserializeObject<TeamMember>(json_data);
            //    return tmObj;
            //}

            string html = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            TeamMember tmObj = new TeamMember();
            if (!string.IsNullOrEmpty(html))
                tmObj = JsonConvert.DeserializeObject<TeamMember>(html);
            return tmObj;
        }

    }
}
