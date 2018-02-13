# C# Windows Forms Application. Communication with Ardunio via serial port

This example project telling about how to communicate with a device via serial port on C#.
 The device may be a Arduino.
 This project sending a start command to device via serial port and then listening to port for feedbacks. 
 When port sends feedback "success", "error" or "continue", C# throws an event to form.
 All processes are async.
