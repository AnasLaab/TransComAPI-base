TransComAPI Documentation

Overview
TransComAPI is an Application Programming Interface (API) designed to facilitate communication between client applications and TRAN devices via a COM port. The API enables sending commands to TRAN devices, receiving responses, and managing the connection efficiently.
Architecture
The API is built using ASP.NET Core and follows a RESTful architecture to facilitate integration with various clients, including desktop applications, web applications, and other systems requiring interaction with TRAN devices.
Key Components
•	CommandsController: The main controller that exposes endpoints for sending commands to the TRAN device, auto-detecting the COM port, and managing the connection state.
•	SerialService: A service encapsulating the serial communication logic, including opening the connection, sending commands, and reading responses.
•	CommandRequest & CommandResponse: Data models to structure command requests and responses received from the device.

Features
Automatically Detecting the COM Port
The API provides functionality to auto-detect the COM port to which the TRAN device is connected. This feature simplifies the connection process by eliminating the need to manually specify the COM port.
Sending Commands and Receiving Responses
Users can send specific commands to the TRAN device and receive responses through dedicated endpoints. The API supports a variety of commands and handles the serialization of commands and deserialization of responses.
Managing Connection State
The API allows users to check the connection state (open or closed) and explicitly close the connection when necessary.




Endpoints
POST /commands/send
Send a command to the TRAN device.
•	Request Body: {"command": "<Command>"}
•	Response: A CommandResponse object containing the device's response.
GET /commands/autoDetectAndOpen
Auto-detect the TRAN device's COM port and open the connection.
•	Response: Information about the detected COM port and the initial response from the device.
GET /commands/status
Get the connection state.
•	Response: Connection status (open or closed).
POST /commands/close
Close the connection with the TRAN device.
•	Response: Confirmation of the connection closure.
Usage Example
1.	Detect the COM port and open the connection:
GET /commands/autoDetectAndOpen
2.	Send the command $PING?:
POST /commands/send with body {"command": "$PING?\r\n"}
3.	Close the connection:
POST /commands/close

