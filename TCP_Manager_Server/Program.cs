using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using TCP_Manager_Server;

var ip = IPAddress.Loopback;
var port = 27001;

var listener = new TcpListener(ip, port);

listener.Start();

while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var br = new BinaryReader(stream);
    var bw  = new BinaryWriter(stream);

    while (true)
    {
        var input = br.ReadString();
        var command = JsonSerializer.Deserialize<Command>(input);

        if (command == null) continue;

        Console.WriteLine(command.Text);
        Console.WriteLine(command.Param);
        switch (command.Text)
        {
            case Command.ProccessList:
                var proccesses = Process.GetProcesses();
                var proccessNames = JsonSerializer.Serialize(proccesses.Select(p => p.ProcessName));
                bw.Write(proccessNames);
                break;
            case Command.Kill:
                var proccess = command.Param;
                Process[] processes = Process.GetProcessesByName(proccess);
                foreach (var proc in processes) { proc.Kill(); }
                bw.Write(JsonSerializer.Serialize(new String($"Proccess: {proccess} killed! ")));
                break;

            case Command.Run:
                var run = command.Param;

                try
                {
                    Process.Start(run);
                    
                    bw.Write(JsonSerializer.Serialize(new String($"Proccess: {run} started! ")));


                    
                    
                }
                catch (Exception ex)
                {

                    bw.Write(JsonSerializer.Serialize(new String(ex.Message)));
                };
                

                break;
            default:
                break;
        }

    }
}
