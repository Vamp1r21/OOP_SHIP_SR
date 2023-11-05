using System;
using System.Collections.Generic;
using System.IO;

namespace OOP_SR
{
    using ShipGroup = List<Ship>;
    using WorldMap = List<Port>;
    using Routes = List<Route>;
    struct Ship
    {
        public string name;
        public double weight;
        public double speed;
        public double range;

        public Ship(string name, double weight, double speed, double range)
        {
            this.name = name;
            this.weight = weight;
            this.speed = speed;
            this.range = range;
        }
    }

    struct Port
    {
        public string name;
        public Coordinate coordinates;
        public List<string> namesShips;
        //public ShipGroup ships;

        public Port(string name, Coordinate coordinates, List<string> namesShips)
        {
            this.name = name;
            this.coordinates = coordinates;
            this.namesShips = namesShips;
        }
    }

    struct Coordinate
    {
        public double x;
        public double y;

        public Coordinate(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    struct Route
    {
        public List<string> rout;
        public double length;
    }

    struct Flight
    {
        public string nameFirsPort;
        public string nameSecondPort;
        public string nameShip;
        public Route route;
        public int time;
    }
    class Program
    {
        static void Main(string[] args)
        {
            WorldMap worldMap = new WorldMap();
            ShipGroup shipGroup = new ShipGroup();
            worldMap = ReadWorldMapFromFile("../../../ShipTask/WorldMap.txt");
            shipGroup = ReadShipGropFromFile("../../../ShipTask/ShipGroup.txt");
            Routes routes = new Routes();
            routes = PreliminaryRoutePlanning(worldMap, shipGroup);
            List<Route> route = new List<Route>();
            //route = RouteAdjustment(routes);
        }

        static WorldMap ReadWorldMapFromFile(string line)
        {
            WorldMap worldsMap = new WorldMap();
            string namePort = "";
            double x=0, y=0;
            List<string> nameShips = new List<string>();
            var file = File.ReadAllLines(line);
            for (int i = 0; i < file.Length; i++)
            {
                string[] delimeters_0 = { "  ", " ", "\t" };
                string[] mas = file[i].Split(delimeters_0, StringSplitOptions.RemoveEmptyEntries);
                if(namePort!=mas[0] && i!=0)
                {
                    worldsMap.Add(ReadPortFromFile(namePort, nameShips, x, y));
                    nameShips.Clear();
                }
                nameShips.Add(mas[1]);
                namePort = mas[0];
                x = double.Parse(mas[2]);
                y = double.Parse(mas[3]);
            }
            worldsMap.Add(ReadPortFromFile(namePort, nameShips, x, y));
            return worldsMap;

        }

        static Port ReadPortFromFile(string namePort, List<string> nameShip, double x, double y)
        {
            return new Port(namePort, new Coordinate(x, y), new List<string>(nameShip));
        }
        static ShipGroup ReadShipGropFromFile(string line)
        {
            ShipGroup shipGroup = new ShipGroup();
            var file = File.ReadAllLines(line);
            for (int i = 0; i < file.Length; i++)
            {
                string[] delimeters_0 = { "  ", " ", "\t" };
                string[] mas = file[i].Split(delimeters_0, StringSplitOptions.RemoveEmptyEntries);
                shipGroup.Add(ReadShipFromFile(mas));
            }
            return shipGroup;
        }
        static Ship ReadShipFromFile(string[] ships)
        {
            return new Ship(ships[0], double.Parse(ships[1]), double.Parse(ships[2]), double.Parse(ships[3]));
        }

        static Routes PreliminaryRoutePlanning(WorldMap worldMap,ShipGroup shipGroup)
        {
            Routes routes = new Routes();
            double weight = ReadWeightFromKeyboard();
            string nameFirstPort = ReadNamePortFromKeyboard("Введите начальный порт");
            string nameSecondPort = ReadNamePortFromKeyboard("Введите конечный порт");
            ShipGroup shipsGroup = new ShipGroup();
            shipsGroup = ArrangingGroupShipsForTransportation(worldMap.Find(n => n.name == nameFirstPort), shipGroup, weight);
            foreach(var i in shipsGroup)
            {
                routes.Add(CalculationRoutFromShip(nameFirstPort, nameSecondPort, worldMap, i));
            }
            return routes;
        }

        static double ReadWeightFromKeyboard()
        {
            Console.WriteLine("Введите вес груза");
            return double.Parse(Console.ReadLine());
        }

        static string ReadNamePortFromKeyboard(string text)
        {
            Console.WriteLine($"{text}");
            return Console.ReadLine();
        }

        static ShipGroup ArrangingGroupShipsForTransportation(Port port, ShipGroup shipGroup, double weight)
        {
            ShipGroup shipsGroup = new ShipGroup();
            foreach(var i in port.namesShips)
            {
                Ship ship = shipGroup.Find(n => n.name == i);
                if(CheckShip(weight, ship)==true)
                {
                    shipsGroup.Add(ship);
                }
            }
            return shipsGroup;
        }
        static bool CheckShip(double weight, Ship ship)
        {
            if(ship.weight >=weight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static Route CalculationRoutFromShip(string nameFirstPort, string nameSecondPort, WorldMap worldMap, Ship ship)
        {
            Route route = new Route();
            List<string> port = new List<string>();
            port.Add(nameFirstPort);
            double length = 0;
            Port firstPort = worldMap.Find(n => n.name == nameFirstPort);
            Port secondPort = worldMap.Find(n => n.name == nameSecondPort);
            length = CalculationLength(firstPort, secondPort);
            if(length>ship.range)
            {
                length = 0;
                CalculationRout(nameFirstPort, nameSecondPort, port, worldMap, ship);
                route.rout = port;
                for (int i = 0; i < port.Count; i++)
                {
                    if (i <= port.Count-2)
                    {
                        firstPort = worldMap.Find(n => n.name == port[i]);
                        secondPort = worldMap.Find(n => n.name == port[i + 1]);
                        length += CalculationLength(firstPort, secondPort);
                        route.length = length;
                    }
                }
            }
            else
            {
                port.Add(nameSecondPort);
                route.length = length;
                route.rout = port;
            }
            return route;
        }

        static void CalculationRout(string namePort, string nameSecondPort, List<string> port, WorldMap worldMap, Ship ship)
        {
            string namePorts = "";
            foreach(var i in worldMap)
            {
                if(!port.Contains(i.name))
                {
                    double length= CalculationLength(worldMap.Find(n => n.name == namePort), i);
                    if(ship.range>=length)
                    {
                        if(i.name==nameSecondPort)
                        {
                            port.Add(i.name);
                            break;
                        }
                        else
                        {
                            namePorts = i.name;
                        }
                    }
                    else
                    {
                        port.Add(namePorts);
                        CalculationRout(namePorts, nameSecondPort, port, worldMap, ship);
                        break;
                    }
                }
            }
        }

        static double CalculationLength(Port firstPort, Port secondPort)
        {
            double lenght = Math.Sqrt(Math.Pow(secondPort.coordinates.x - firstPort.coordinates.x, 2) +
                Math.Pow(secondPort.coordinates.y - firstPort.coordinates.y, 2));
            return lenght;
        }
    }
}
