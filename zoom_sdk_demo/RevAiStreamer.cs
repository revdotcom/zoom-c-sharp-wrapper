using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace zoom_sdk_demo
{
    public class RevAiStreamer
    {
        public RevAiStreamer(
            string speaker,
            ZoomCaptioner captioner
            )
        {
            _speaker = speaker;
            _captioner = captioner;
            _socket = new ClientWebSocket();
            ByteChannel = Channel.CreateBounded<byte[]>(new BoundedChannelOptions(30) {
                FullMode = BoundedChannelFullMode.Wait
            });
        }

        public bool IsClosed()
        {
            return _socket.State != WebSocketState.Open;
        }

        public void SetNameIfNeeded(
            string name
            )
        {
            if (name == _speaker)
                return;
            _speaker = name;
        }

        public async Task StartAsync()
        {
            var url = new Uri($"{Url}?access_token={AccessToken}&content_type=audio/x-raw;layout=interleaved;rate=32000;format=S16LE;channels=1&max_segment_duration_seconds=10");
            await _socket.ConnectAsync(url, default).ConfigureAwait(false);
            try
            {
                var id = await ConnectHandshakeAsync().ConfigureAwait(false);
                Console.WriteLine("Connected to rev.ai");
                _ = StartReceiveLoopAsync().ConfigureAwait(false);
                _ = StartSendLoopAsync().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to connect to rev.ai - {ex.Message}");
            }
        }

        private async Task StartSendLoopAsync()
        {
            try
            {
                var arrays = new List<byte[]>();
                var count = 0;
                while (_socket.State == WebSocketState.Open)
                {
                    var bytes = await ByteChannel.Reader.ReadAsync();
                    arrays.Add(bytes);
                    count += 1;

                    if (count > 10)
                    {
                        var final = arrays.SelectMany(x => x).ToArray();

                        await _socket.SendAsync(new ArraySegment<byte>(final), WebSocketMessageType.Binary, true, default).ConfigureAwait(false);
                        count = 0;
                        arrays.Clear();
                    }
                }
                ByteChannel.Writer.TryComplete();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task SendDataAsync(
            byte[] data
            )
        {
            if (_socket.State == WebSocketState.Open)
            {
                try
                {
                    Console.WriteLine("Sending data");
                    await _socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, default)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        internal async Task StartReceiveLoopAsync()
        {
            while (Extensions.ReadingStates.Contains(_socket.State))
            {
                try
                {
                    switch (await _socket.ReceiveFullMessageAsync(default).ConfigureAwait(false))
                    {
                        case null:
                            Console.WriteLine("Socket in closed state");
                            return;
                        case var received when received.Value.type == WebSocketMessageType.Text:
                            var message = ParseMessage<RevAiMessage>(received.Value.data);
                            if (message.Type == "final")
                            {
                                if (message.Elements.Any(x => x.Type == "text"))
                                {
                                    var textTranscript = $"{_speaker}: {String.Join("", message.Elements.Select(e => e.Value))}";
                                    await _captioner.SendCaptionAsync(textTranscript).ConfigureAwait(false);
                                }
                            }
                            continue;
                        case var close when close.Value.type == WebSocketMessageType.Close:
                            Console.WriteLine("Close received");
                            return;
                        default:
                            Console.WriteLine("Unexpected data received");
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    continue;
                }
            }
        }

        private async Task<string> ConnectHandshakeAsync()
        {
            switch (await _socket.ReceiveFullMessageAsync(default).ConfigureAwait(false))
            {
                case var received when received.Value.type == WebSocketMessageType.Text:
                    var message = ParseMessage<ConnectedMessage>(received.Value.data);
                    if (message?.Type != "connected")
                    {
                        throw new Exception("Unexpected format of the connected message");
                    }
                    return message.Id;
                case null:
                    throw new Exception("Unexpected data received");
                case var close when close.Value.type == WebSocketMessageType.Close:
                    throw new Exception($"WebSocket closed with {(int)_socket.CloseStatus} {_socket.CloseStatus} during connection handshake. {_socket.CloseStatusDescription}");
                default:
                    throw new Exception("Unexpected data received");
            }
        }

        private T ParseMessage<T>(
            ArraySegment<byte> data
            )
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data.Array));
        }

        public Channel<byte[]> ByteChannel { get; set; }


        private string _speaker;
        private readonly ClientWebSocket _socket;
        private readonly ZoomCaptioner _captioner;

        private const string Url = "wss://api.rev.ai/speechtotext/v1/stream";
        private const string AccessToken = "02kQoXp3YkWGIg5lkvV3EtdNB3vOsOYSfq0MUnlQioSLD54awVYU_1Lt9viYHnX5yhhXXZuHAzkmIeNqJI0pi2dGVQWSA";
    }
}
