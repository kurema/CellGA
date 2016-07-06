using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kurema.CellGA.Geometry
{
    public struct Point2d
    {
        public double X;
        public double Y;

        public Point2d(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public double DistanceTo(Point2d to)
        {
            return Functions.Distance(X, to.X, Y, to.Y);
        }
    }

    public struct Vector2d
    {
        public double X;
        public double Y;

        public Vector2d(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
    public struct Vector3d
    {
        public double X;
        public double Y;
        public double Z;

        public Vector3d(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
    }


    public struct Point3d
    {
        public double X;
        public double Y;
        public double Z;

        public Point3d(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
    }

    public class Polyline
    {
        private List<Point3d> Contents = new List<Point3d>();
        public Polyline()
        {
        }

        public void Add(double x,double y,double z)
        {
            Contents.Add(new Point3d(x, y, z));
        }

    }

}
