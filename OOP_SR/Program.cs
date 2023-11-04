using System;
using System.Collections.Generic;

namespace OOP_SR
{
    using ShipGroup = List<Ship>;
    using WorldMap = List<Port>;
    struct Ship
    {
        public string name;
        public int weight;
        public double speed;
        public double range;
    }

    struct Port
    {
        public string name;
        public Coordinate coordinates;
        public List<Ship> ships;
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

        }
    }
}
