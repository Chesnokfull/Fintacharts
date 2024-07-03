using Fintacharts_Data_Collection.Database;
using Fintacharts_Data_Collection.Models;
using Fintacharts_Data_Collection.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Fintacharts_Data_Collection.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WebSocketController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Authentication _authentication;
        private readonly ApplicationContext _context;
        public WebSocketController(IConfiguration configuration, Authentication authentication, ApplicationContext applicationContext)
        {
            _configuration = configuration;
            _authentication = authentication;
            _context = applicationContext;
        }
        [HttpGet(Name = "Request connection to WebSocket")]
        async public Task<IActionResult> RequestConnection()
        {
            try
            {
                await Connect();
                return Ok("Connection established");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet(Name = "Get data collected using WebSocket")]
        public ActionResult<List<WebSocketMessage>> Get(Guid guid)
        {
            var result = _context.Instruments.Where(x => x.id == guid).Include(x => x.webSocketMessages).ToList();
            return Ok(result);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        async private Task Connect()
        {
            var ws = new ClientWebSocket();
            var tocken = await _authentication.Access_Tocken();
            string uri = _configuration["Web:URI_WSS"] + "/api/streaming/ws/v1/realtime?token=" + tocken;

            Console.WriteLine("Connecting to server");
            await ws.ConnectAsync(new Uri(uri), CancellationToken.None);
            Console.WriteLine("Connected");

            var receiveTask = Task.Run(async () =>
            {
                using (var ms = new MemoryStream())
                {
                    while (ws.State == WebSocketState.Open)
                    {
                        WebSocketReceiveResult result;
                        do
                        {
                            var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
                            result = await ws.ReceiveAsync(messageBuffer, CancellationToken.None);
                            ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            var msgString = Encoding.UTF8.GetString(ms.ToArray());

                            var document = JsonConvert.DeserializeObject<WebSocketMessage>(msgString);

                            document.instrument =
                            _context.Instruments.Where(x => x.id == new Guid(document.id)).FirstOrDefault();
                            _context.WebSocketMessages.Add(document);

                            _context.SaveChanges();
                        }
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.Position = 0;
                    }
                }
            });
        }
    }
}
