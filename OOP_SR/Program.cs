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
            Route route = new Route();
            route = RouteAdjustment(routes);
        }

        static WorldMap ReadWorldMapFromFile(string line)
        {
            WorldMap worlsMap = new WorldMap();
            var file = File.ReadAllLines(line);
            for (int i = 0; i < file.Length; i++)
            {
                string lines = file[i];
                string[] delimeters_0 = { "  ", " ", "\t" };
            }

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

    }
}
