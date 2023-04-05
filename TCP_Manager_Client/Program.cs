using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using TCP_Manager_Client;

var ip = IPAddress.Loopback;
var port = 27001;

var client = new TcpClient();
client.Connect(ip, port);
var stream = client.GetStream();
var br = new BinaryReader(stream);
var bw = new BinaryWriter(stream);
Command command = null;
string responce = null;

while (true)
{
    Console.WriteLine("Write command or HELP: ");
    var str = Console.ReadLine().ToUpper();
    if (str == "HELP")
    {
        Console.WriteLine();
        Console.WriteLine("Command list:");
        Console.WriteLine(Command.ProccessList);
        Console.WriteLine($"{Command.Run} <proccess_name>");
        Console.WriteLine($"{Command.Kill} <proccess_name>");
        Console.WriteLine($"HELP");
        Console.ReadKey();
        Console.Clear();
        continue;
    }
    var input = str.Split(' ');
    switch (input[0])
    {
        case Command.ProccessList:
            command = new Command { Text = input[0] };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var proccessList = JsonSerializer.Deserialize<string[]>(responce);
            foreach (var proccessName in proccessList)
            {
                Console.WriteLine($"    {proccessName}");
            }
            Console.ReadKey();
            Console.Clear();
            break;

        case Command.Kill:
            command = new Command { Text = input[0], Param = input[1] };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var answer = JsonSerializer.Deserialize<string>(responce);
            Console.WriteLine(answer);
            Console.ReadKey();
            Console.Clear();
            break;
        case Command.Run:
            command = new Command { Text = input[0], Param = input[1] };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var answer2 = JsonSerializer.Deserialize<string>(responce);
            Console.WriteLine(answer2);
            Console.ReadKey();
            Console.Clear();
            break;

    }


}

