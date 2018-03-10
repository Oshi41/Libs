using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProbabiltyMath
{
    public class FunctionBuilder
    {
        //"y = a * Math.Sqrt(x) + b * x + c"

        public void GetSircleCenter(Point p1, Point p2, Point p3)
        {
            
        }
        //public static Pt getCircleCenter(Pt a, Pt b, Pt c)
        //{
        //    double ax = a.getX();
        //    double ay = a.getY();
        //    double bx = b.getX();
        //    double by = b.getY();
        //    double cx = c.getX();
        //    double cy = c.getY();

        //    double A = bx - ax;
        //    double B = by - ay;
        //    double C = cx - ax;
        //    double D = cy - ay;

        //    double E = A * (ax + bx) + B * (ay + by);
        //    double F = C * (ax + cx) + D * (ay + cy);

        //    double G = 2 * (A * (cy - by) - B * (cx - bx));
        //    if (G == 0.0)
        //        return null; // a, b, c must be collinear

        //    double px = (D * E - B * F) / G;
        //    double py = (A * F - C * E) / G;
        //    return new Pt(px, py);
        //}
    }
}
