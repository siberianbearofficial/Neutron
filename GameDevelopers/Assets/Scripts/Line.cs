using System;
using UnityEngine;

public class Line
{
    public double x1, y1, x2, y2, k, b;
    public bool paral = false;
    private static void print(string x) { Enemy.print_from_other_classes(x);}
    public Line((double, double) first, (double, double) second)
    {
        x1 = first.Item1; y1 = first.Item2; x2 = second.Item1; y2 = second.Item2;
        if ((x1 - x2) == 0)
        {
            // Прямая параллельная оси y, для нее нельзя записать параметрическое уравнение, так как угловой коэффициент не существует
            // Надо дополнить класс для просчета и этого случая, либо переписать все через канонические уравнения
            paral = true;
        }
        else
        {
            k = (y1 - y2) / (x1 - x2);
            b = y1 - k * x1;
        }
    }
    public static (bool, (double, double)) Intersects(Line line1, Line line2)
    {
        double x_cross, y_cross;
        bool on_x1, on_x2, on_y1, on_y2;
        if (line1.paral && line2.paral) return (false, (double.NegativeInfinity, double.PositiveInfinity));
        if (line1.paral)
        {
            x_cross = line1.x1;
            y_cross = line2.k * x_cross + line2.b;
        }
        else if (line2.paral)
        {
            x_cross = line2.x1;
            y_cross = line1.k * x_cross + line1.b;
        }
        else
        {
            x_cross = (line2.b - line1.b) / (line1.k - line2.k);
            y_cross = line1.k * x_cross + line1.b;
        }
        double a1x, b1x, a2x, b2x, a1y, b1y, a2y, b2y;
        a1x = Min(line1.x1, line1.x2);
        b1x = Max(line1.x1, line1.x2);
        a2x = Min(line2.x1, line2.x2);
        b2x = Max(line2.x1, line2.x2);
        a1y = Min(line1.y1, line1.y2);
        b1y = Max(line1.y1, line1.y2);
        a2y = Min(line2.y1, line2.y2);
        b2y = Max(line2.y1, line2.y2);
        on_x1 = (x_cross >= a1x) && (x_cross <= b1x);
        on_y1 = (y_cross >= a1y) && (y_cross <= b1y);
        on_x2 = (x_cross >= a2x) && (x_cross <= b2x);
        on_y2 = (y_cross >= a2y) && (y_cross <= b2y);
        if (on_x1 && on_y1 && on_x2 && on_y2)
        {
            return (true, (x_cross, y_cross));
        }
        return (false, (x_cross, y_cross));
    }

    public static (double, double) Center(Line AB)
    {
        return ((AB.x2 - AB.x1) / 2 + AB.x1, (AB.y2 - AB.y1) / 2 + AB.y1);
    }

    public static double Angle(Line line1, Line line2)
    {
        double angle;
        if (line1.paral && line2.paral)
        {
            angle = 0d;
        }
        else if (line1.paral)
        {
            double angle1 = Mathf.PI / 2;
            double angle2 = Mathf.Abs(Mathf.Atan((float)line2.k));
            angle = angle1 - angle2;
        }
        else if (line2.paral)
        {
            double angle1 = Mathf.Abs(Mathf.Atan((float)line1.k));
            double angle2 = Mathf.PI / 2;
            angle = angle1 - angle2;
        }
        else
        {
            double angle1 = Mathf.Atan((float)line1.k);
            double angle2 = Mathf.Atan((float)line2.k);
            //print("Angle1: " + angle1.ToString());
            //print("Angle2: " + angle2.ToString());
            //print("Angle: " + (angle1 - angle2).ToString());
            //if ((angle1 * angle2) > 0)
            //{
            //    angle = angle1 - angle2;
            //} else
            //{
            //    angle = Mathf.Abs((float)angle1) + Mathf.Abs((float)angle2);
            //}
            angle = angle1 - angle2;
        }
        return angle;
    }
    private static double Min(double x, double y)
    {
        if (x > y) return y; return x;
    }
    private static double Max(double x, double y)
    {
        if (x > y) return x; return y;
    }
}