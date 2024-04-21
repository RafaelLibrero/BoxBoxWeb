using BoxBoxClient.Filters;
using BoxBoxClient.Services;
using BoxBoxModels;
using Microsoft.AspNetCore.Mvc;

namespace BoxBox.Controllers
{
    public class TopicsController : Controller
    {
        private ServiceApiBoxBox service;

        public TopicsController(ServiceApiBoxBox service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<VTopic> topics = await this.service.GetVTopicsAsync();
            
            List<Post> lastMessages = new List<Post>();
            List<User> usuarios = new List<User>();

            foreach (var topic in topics)
            {
                Post lastMessage = await this.service.FindPostAsync(topic.LastMessage);
                if (lastMessage != null)
                {
                    lastMessages.Add(lastMessage);
                }
            }

            foreach (var lastMessage in lastMessages)
            {
                User usuario = await this.service.FindUserAsync(lastMessage.UserId);
                if (usuario != null)
                {
                    usuarios.Add(usuario);
                }  
            }
            
            ViewData["LastMessages"] = lastMessages;
            ViewData["Usuarios"] = usuarios;
            return View(topics);
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        public IActionResult Create()
        {
            return View();
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult>Create(Topic topic)
        {
            await this.service.CreateTopicAsync(topic);
            return RedirectToAction("Index");
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        public async Task<IActionResult> Edit(int topicId)
        {
            Topic topic = await this.service.FindTopicAsync(topicId);
            return View(topic);
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Edit(Topic topic)
        {
            await this.service.UpdateTopicAsync(topic);
            return RedirectToAction("Index");
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Delete(int topicId)
        {
            await this.service.DeleteTopicAsync(topicId);
            return RedirectToAction("Index");
        }

    }
}
