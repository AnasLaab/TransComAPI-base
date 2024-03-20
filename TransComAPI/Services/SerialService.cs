using System;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TransComAPI.Models;

namespace TransComAPI.Services
{
    public class SerialService
    {
        private SerialPort _serialPort;
        private readonly ILogger<SerialService> _logger;

        public SerialService(ILogger<SerialService> logger)
        {
            _logger = logger;
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

                    var commandResponse = await SendCommandAsync(new CommandRequest { Command = "$PING?" });

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

        public async Task<CommandResponse> SendCommandAsync(CommandRequest commandRequest)
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

                var response = await ReadResponseAsync();
                var receiveTimestamp = DateTime.Now; // Capturer l'heure de réception

                _logger.LogInformation($"Response received: {response}");
                return new CommandResponse
                {
                    Success = true,
                    Response = response,
                    SendTimestamp = sendTimestamp.ToString("HH:mm:ss.fff"), // Convertir au format souhaité
                    ReceiveTimestamp = receiveTimestamp.ToString("HH:mm:ss.fff"), // Convertir au format souhaité
                    Interval = (receiveTimestamp - sendTimestamp).TotalMilliseconds // Calcul de l'intervalle
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending command: {ex.Message}");
                return new CommandResponse { Success = false, ErrorMessage = ex.Message };
            }
        }



        private Task<string> ReadResponseAsync()
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
