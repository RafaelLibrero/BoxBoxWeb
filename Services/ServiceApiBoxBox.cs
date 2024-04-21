using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using BoxBoxModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BoxBoxClient.Services
{
    public class ServiceApiBoxBox
    {
        private string UrlApiBoxBox;
        private MediaTypeWithQualityHeaderValue Header;
        private IHttpContextAccessor httpContextAccessor;

        public ServiceApiBoxBox
            (IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.UrlApiBoxBox =
                configuration.GetValue<string>("ApiUrls:ApiBoxBox");
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<string> GetTokenAsync(string email
            , string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/auth/login";
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                var formData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password)
                });
                HttpResponseMessage response = await
                    client.PostAsync(request, formData);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject keys = JObject.Parse(data);
                    string token = keys.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        private async Task<T> CallApiAsync<T>
            (string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        #region Users

        public async Task<User> Register(User user)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/users";
            string jsonContent = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            string token =
                this.httpContextAccessor.HttpContext.User
                .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/users";
            List<User> users = await 
                this.CallApiAsync<List<User>>(request, token);
            return users;
        }

        public async Task<User> FindUserAsync(int id)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/users/" + id;
            User user = await 
                this.CallApiAsync<User>(request, token);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/users";
            string jsonContent = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/users/" + id;
            
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.DeleteAsync(request);
            }
        }

        public async Task<User> GetUserProfileAsync()
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/users/profile";
            User user = await
                this.CallApiAsync<User>(request);
            return user;
        }

        #endregion

        #region Topics

        public async Task<List<VTopic>> GetVTopicsAsync()
        {
            string request = "api/topics";
            List <VTopic> topics = await
                this.CallApiAsync<List<VTopic>>(request);
            return topics;
        }
        public async Task<Topic> FindTopicAsync(int topicId)
        {
            string request = "api/topics/" + topicId;
            Topic topic = await
                this.CallApiAsync<Topic>(request);
            return topic;
        }
        public async Task CreateTopicAsync(Topic tema)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/topics";
            string jsonContent = JsonConvert.SerializeObject(tema);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
        public async Task UpdateTopicAsync(Topic tema)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/topics";
            string jsonContent = JsonConvert.SerializeObject(tema);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }
        public async Task DeleteTopicAsync(int topicId)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/topics/" + topicId;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.DeleteAsync(request);
            }
        }

        #endregion

        #region Conversations
        public async Task<ConversationsPaginado> GetVConversationsTopicAsync(int posicion, int topicId)
        {
            string request = "api/conversations/get/" + posicion + "/" + topicId;
            ConversationsPaginado conversations = await
                this.CallApiAsync<ConversationsPaginado>(request);
            return conversations;
        }
        public async Task<Conversation> FindConversationAsync(int conversationId)
        {
            string request = "api/conversations/" + conversationId;
            Conversation conversation = await
                this.CallApiAsync<Conversation>(request);
            return conversation;
        }
        public async Task<Conversation> CreateConversationAsync(Conversation conversacion)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/conversations";
            string jsonContent = JsonConvert.SerializeObject(conversacion);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    Conversation conversation = await
                        response.Content.ReadAsAsync<Conversation>();
                    return conversation;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task UpdateConversationAsync(Conversation conversacion)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/conversations";
            string jsonContent = JsonConvert.SerializeObject(conversacion);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }
        public async Task DeleteConversationAsync(int conversationId)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/conversations/" + conversationId;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.DeleteAsync(request);
            }
        }
        public async Task UpdateEntryCount(int conversationId)
        {
            string request = "api/conversations/updateentrycount/" + conversationId;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage response =
                    await client.PutAsync(request, null);
            }
        }

        #endregion

        #region Posts

        public async Task<PostsPaginado> GetPostsConversationAsync(int posicion, int conversationId)
        {
            string request = "api/posts/get/" + posicion + "/" + conversationId;
            PostsPaginado posts = await
                this.CallApiAsync<PostsPaginado>(request);
            return posts;
        }
        public async Task<Post> FindPostAsync(int postId)
        {
            string request = "api/posts/" + postId;
            Post post = await
                this.CallApiAsync<Post>(request);
            return post;
        }
        public async Task CreatePostAsync(Post posteo)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/posts";
            string jsonContent = JsonConvert.SerializeObject(posteo);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
        public async Task UpdatePostAsync(Post posteo)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/posts";
            string jsonContent = JsonConvert.SerializeObject(posteo);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }
        public async Task DeletePostAsync(int postId)
        {
            string token =
                this.httpContextAccessor.HttpContext.User
                .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/posts/" + postId;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.DeleteAsync(request);
            }

        }
        public async Task<List<Post>> GetReportedPosts()
        {
            string token =
                this.httpContextAccessor.HttpContext.User
                .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/posts/getreported";
            List<Post> posts = await
                this.CallApiAsync<List<Post>>(request);
            return posts;
        }
        public async Task ReportPostAsync(int postId)
        {
            string token =
                this.httpContextAccessor.HttpContext.User
                .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/posts/report" + postId;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PutAsync(request, null);
            }
        }

        public async Task UnreportPostAsync(int postId)
        {
            string token =
                this.httpContextAccessor.HttpContext.User
                .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/posts/unreport" + postId;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PutAsync(request, null);
            }
        }

        #endregion

        #region Driver
        public async Task<List<Driver>> GetDriversAsync()
        {
            string request = "api/drivers";
            List<Driver> drivers = await
                this.CallApiAsync<List<Driver>>(request);
            return drivers;
        }
        public async Task<Driver> FindDriverAsync(int driverId)
        {
            string request = "api/drivers/" + driverId;
            Driver driver = await
                this.CallApiAsync<Driver>(request);
            return driver;
        }
        public async Task CreateDriverAsync(Driver conductor)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/drivers";
            string jsonContent = JsonConvert.SerializeObject(conductor);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
        public async Task UpdateDriverAsync(Driver conductor)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/drivers";
            string jsonContent = JsonConvert.SerializeObject(conductor);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }
        public async Task DeleteDriverAsync(int driverId)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/drivers/" + driverId;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.DeleteAsync(request);
            }
        }

        #endregion

        #region Teams

        public async Task<List<Team>> GetTeamsAsync()
        {
            string request = "api/teams";
            List<Team> teams = await
                this.CallApiAsync<List<Team>>(request);
            return teams;
        }
        public async Task<Team> FindTeamAsync(int teamId)
        {
            string request = "api/teams/" + teamId;
            Team team = await
                this.CallApiAsync<Team>(request);
            return team;
        }
        public async Task CreateTeamAsync(Team equipo)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/teams";
            string jsonContent = JsonConvert.SerializeObject(equipo);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
        public async Task UpdateTeamAsync(Team equipo)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/teams";
            string jsonContent = JsonConvert.SerializeObject(equipo);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }
        public async Task DeleteTeamAsync(int teamId)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/teams/" + teamId;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.DeleteAsync(request);
            }
        }

        #endregion

        #region Races
        public async Task<List<Race>> GetRacesAsync()
        {
            string request = "api/races";
            List<Race> races = await
                this.CallApiAsync<List<Race>>(request);
            return races;
        }
        public async Task<Race> FindRaceAsync(int raceId)
        {
            string request = "api/races/" + raceId;
            Race race = await
                this.CallApiAsync<Race>(request);
            return race;
        }
        public async Task CreateRaceAsync(Race carrera)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/races";
            string jsonContent = JsonConvert.SerializeObject(carrera);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
        public async Task UpdateRaceAsync(Race carrera)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/races";
            string jsonContent = JsonConvert.SerializeObject(carrera);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }
        public async Task DeleteRaceAsync(int raceId)
        {
            string token =
               this.httpContextAccessor.HttpContext.User
               .FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/races/" + raceId;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiBoxBox);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.DeleteAsync(request);
            }
        }

        #endregion
    }
}
