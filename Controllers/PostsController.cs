using BoxBoxClient.Filters;
using BoxBoxClient.Services;
using BoxBoxModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BoxBox.Controllers
{
    public class PostsController : Controller
    {
        private ServiceApiBoxBox service;
        private readonly AzureBlobStorageService _blobService;

        public PostsController(ServiceApiBoxBox service, AzureBlobStorageService blobService)
        {
            this.service = service;
            _blobService = blobService;
        }

        public async Task<IActionResult>Index(int? posicion, int conversationId)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            PostsPaginado posts = await this.service.GetPostsConversationAsync(posicion.Value, conversationId);
            ViewData["REGISTROS"] = posts.Registros;
            int siguiente = posicion.Value + 1;
            if (siguiente > posts.Registros)
            {
                
                siguiente = posts.Registros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = posts.Registros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            ViewData["CONVERSATIONID"] = conversationId;
            List<Driver> drivers = await this.service.GetDriversAsync();
            List<Team> teams = await this.service.GetTeamsAsync();
            Conversation conversation = await this.service.FindConversationAsync(conversationId);
            ViewData["DRIVERS"] = drivers;
            ViewData["TEAMS"] = teams;
            ViewData["Title"] = conversation.Title;
            List<User> users = new List<User>();

            foreach (var post in posts.Posts)
            {
                User usuario = await this.service.FindUserAsync(post.UserId);
                usuario.ProfilePicture = _blobService.GetBlobUrl(usuario.ProfilePicture);
                users.Add(usuario);
            }
            ViewData["Usuarios"] = users;

            if (HttpContext.Session.GetString("fromConversations") == "true")
            {
                await this.service.UpdateEntryCount(conversationId);
                HttpContext.Session.Remove("fromConversations"); 
            }

            return View(posts.Posts);
        }

        [AuthorizeUsers]
        public IActionResult Create(int conversationId)
        {
            ViewData["ConversationId"] = conversationId;
            return View();
        }

        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            await this.service.CreatePostAsync(post);

            return RedirectToAction("Index", new { conversationId = post.ConversationId });
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Delete(int postId)
        {
            Post post = await this.service.FindPostAsync(postId);
            await this.service.DeleteConversationAsync(postId);
            return RedirectToAction("Index", new { conversationId = post.ConversationId });
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        public async Task<IActionResult> ReportedPosts()
        {
            List<Post> posts = await this.service.GetReportedPosts();
            return View(posts);
        }

        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> ReportPost(int postId)
        {
            await this.service.ReportPostAsync(postId);
            Post post = await this.service.FindPostAsync(postId);
            return RedirectToAction("Index", new { conversationId = post.ConversationId });
        }
    }
}
