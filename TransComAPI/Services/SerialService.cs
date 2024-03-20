using System;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TransComAPI.Models;
using Newtonsoft.Json;

namespace TransComAPI.Services
{
    public class SerialService
    {
        private SerialPort _serialPort;
        private readonly ILogger<SerialService> _logger;
        private readonly ParsingService _parsingService;

        public SerialService(ILogger<SerialService> logger, ParsingService parsingService)
        {
            _logger = logger;
            _parsingService = parsingService;
        }

        public async Task<(bool IsDetected, string PortName, string PingResponse)> AutoDetectAndOpenTranComPortAsync()
        {
            string[] ports = SerialPort.GetPortNames();

            foreach (var port in ports)
            {
                try
                {
                    _serialPort = new SerialPort(port, 115200, Parity.None, 8, StopBits.One)
                    {
                        ReadTimeout = 5000,
                        WriteTimeout = 5000
                    };
                    _serialPort.Open();
                    _logger.LogInformation($"Trying {port}...");

                    var commandResponse = await SendCommandForTextResponseAsync(new CommandRequest { Command = "$PING?" });

                    if (commandResponse.Success && !string.IsNullOrEmpty(commandResponse.Response))
                    {
                        _logger.LogInformation($"TRAN device detected on {port} with response: {commandResponse.Response}");
                        return (true, port, commandResponse.Response);
                    }

                    _serialPort.Close();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error while trying {port}: {ex.Message}");
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                    }
                }
            }

            return (false, null, null);
        }

        public async Task<CommandResponse> SendCommandForBinaryResponseAsync(CommandRequest commandRequest)
        {
            if (_serialPort == null || !_serialPort.IsOpen)
            {
                _logger.LogWarning("Attempted to send command with the port not open.");
                return new CommandResponse { Success = false, ErrorMessage = "Port not open." };
            }

            var sendTimestamp = DateTime.Now; // Capturer l'heure d'envoi

            try
            {
                _serialPort.WriteLine(commandRequest.Command + "\r\n");
                _logger.LogInformation($"Command sent: {commandRequest.Command}");

                var buffer = await ReadResponseAsync();
                var receiveTimestamp = DateTime.Now; // Capturer l'heure de réception

                try
                {
                    // Utiliser le ParsingService pour parser les données reçues
                    var parsedData = _parsingService.Parse(buffer);

                    // Convertir parsedData en une représentation JSON
                    var jsonResponse = JsonConvert.SerializeObject(parsedData, Formatting.Indented,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                    _logger.LogInformation($"Response received and parsed: {jsonResponse}");
                    return new CommandResponse
                    {
                        Success = true,
                        Response = jsonResponse,
                        SendTimestamp = sendTimestamp.ToString("HH:mm:ss.fff"),
                        ReceiveTimestamp = receiveTimestamp.ToString("HH:mm:ss.fff"),
                        Interval = (receiveTimestamp - sendTimestamp).TotalMilliseconds
                    };
                }
                catch (Exception parseEx)
                {
                    _logger.LogWarning($"Parsing binary response failed: {parseEx.Message}, attempting text response.");
                    // En cas d'échec du parsing, tentez de lire comme une réponse textuelle
                    return await SendCommandForTextResponseAsync(commandRequest);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending command: {ex.Message}");
                return new CommandResponse { Success = false, ErrorMessage = ex.Message };
            }
        }


        public async Task<CommandResponse> SendCommandForTextResponseAsync(CommandRequest commandRequest)
        {
            if (_serialPort == null || !_serialPort.IsOpen)
            {
                _logger.LogWarning("Attempted to send command with the port not open.");
                return new CommandResponse { Success = false, ErrorMessage = "Port not open." };
            }

            var sendTimestamp = DateTime.Now; // Capturer l'heure d'envoi

            try
            {
                _serialPort.WriteLine(commandRequest.Command + "\r\n");
                _logger.LogInformation($"Command sent: {commandRequest.Command}");

                var response = await ReadTextResponseAsync();
                var receiveTimestamp = DateTime.Now; // Capturer l'heure de réception

                _logger.LogInformation($"Response received: {response}");
                return new CommandResponse
                {
                    Success = true,
                    Response = response,
                    SendTimestamp = sendTimestamp.ToString("HH:mm:ss.fff"),
                    ReceiveTimestamp = receiveTimestamp.ToString("HH:mm:ss.fff"),
                    Interval = (receiveTimestamp - sendTimestamp).TotalMilliseconds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending command: {ex.Message}");
                return new CommandResponse { Success = false, ErrorMessage = ex.Message };
            }
        }

        private async Task<byte[]> ReadResponseAsync()
        {
            return await Task.Run(() =>
            {
                var response = new byte[1024]; // Taille arbitraire pour l'exemple
                try
                {
                    int bytesRead = _serialPort.Read(response, 0, response.Length);
                    Array.Resize(ref response, bytesRead); // Ajustez la taille du tableau à la quantité de données lues
                }
                catch (TimeoutException ex)
                {
                    _logger.LogWarning($"Read timeout: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during read: {ex.Message}");
                    response = Array.Empty<byte>(); // En cas d'erreur, retournez un tableau vide
                }
                return response;
            });
        }

        private Task<string> ReadTextResponseAsync()
        {
            return Task.Run(() =>
            {
                StringBuilder response = new StringBuilder();
                try
                {
                    do
                    {
                        string line = _serialPort.ReadLine();
                        response.AppendLine(line);
                    } while (_serialPort.BytesToRead > 0);
                }
                catch (TimeoutException ex)
                {
                    _logger.LogWarning($"Read timeout: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during read: {ex.Message}");
                }
                return response.ToString().Trim();
            });
        }

        public void CloseConnection()
        {
            if (_serialPort?.IsOpen ?? false)
            {
                _serialPort.Close();
                _logger.LogInformation("Serial port closed.");
            }
        }

        public bool IsOpen()
        {
            return _serialPort?.IsOpen ?? false;
        }
    }
}

