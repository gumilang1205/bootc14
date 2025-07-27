using System.Security.Cryptography.X509Certificates;

namespace ObjectOrientationProgramming.AbstractionInterface
{
    public interface IShape
    {
        double GetArea();
    }
    public class Circle : IShape
    {
        public double Radius { get; set; }
        public Circle(double radius)
        {
            Radius = radius;
        }
        public double GetArea()
        {
            return Math.PI * Radius * Radius;
        }
    }
    public class Square : IShape
    {
        public double Side { get; set; }
        public Square(double side)
        {
            Side = side;
        }
        public double GetArea()
        {
            return Side * Side;
        }
    }
}