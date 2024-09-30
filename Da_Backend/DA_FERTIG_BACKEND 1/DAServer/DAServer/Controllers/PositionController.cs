using DAServer.Dtos;
using DAServer.Services;
using MathNet.Spatial.Euclidean;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class PositionController : ControllerBase
{
    private readonly MapService _mapService;
    private readonly PositionService _posService;



    public PositionController(MapService mapService, PositionService posService)
    {
        _mapService = mapService;
        _posService = posService;

    }
    [HttpGet("spf")]
    public List<int> GetShortestPath(int id1, int id2)
  => _posService.GetShortestPath(id1, id2);

    [HttpPost("getPosition")]
    public string CalculateCenterPoint([FromBody] Dictionary<string, string> requestBody)
    {
        //Dictionarry wurde gewählt da manche APs mehere signale abgeben welche gelesen werden jedoch selbe position und mac haben
        string parametersString = requestBody["parameters"];
        int triedAPCount = 3;
        string[] keyValuePairs = parametersString.Split(',');

        Dictionary<string, double> nearbyAPs = new Dictionary<string, double>();

        foreach (string pair in keyValuePairs)
        {
            string[] parts = pair.Split(';');

            if (parts.Length == 2)
            {
                string key = parts[0].Trim();
                if (double.TryParse(parts[1], out double value))
                {
                    nearbyAPs[key] = value;
                }
            }
        }
        var debug = nearbyAPs;

        var allAccessPoints = _mapService.AccessPoints;

        List<KeyValuePair<string, double>> sortedAPs = nearbyAPs.OrderBy(ap => ap.Value).ToList();

        List<CombinedAccessPoint> combinedAPs = sortedAPs
            .Select(ap =>
            {
                var matchingAccessPoint = allAccessPoints.FirstOrDefault(accessPoint => accessPoint.MacAddress == ap.Key);

                return new CombinedAccessPoint
                {
                    Id = matchingAccessPoint?.Id ?? 0,
                    Left = matchingAccessPoint?.Left ?? 0.0,
                    Top = matchingAccessPoint?.Top ?? 0.0,
                    MacAddress = ap.Key,
                    Distance = ap.Value
                };
            })
            .ToList();

        Point2D center1 = new Point2D(combinedAPs[0].Left, combinedAPs[0].Top);
        double radius1 = combinedAPs[0].Distance;

        Point2D center2 = new Point2D(combinedAPs[1].Left, combinedAPs[1].Top);
        double radius2 = combinedAPs[1].Distance;

        Point2D center3 = new Point2D(combinedAPs[2].Left, combinedAPs[2].Top);
        double radius3 = combinedAPs[2].Distance;

        bool circlesChanged = true;

        while (circlesChanged)
        {
            circlesChanged = false;
            if (triedAPCount-1>combinedAPs.Count)
            {
                throw new InvalidOperationException("Position can not be calculated");
            }

            if (AreCirclesEncapsulated(center1, radius1, center2, radius2))
            {
                center1 = new Point2D(combinedAPs[triedAPCount].Left, combinedAPs[triedAPCount].Top);
                radius1 = combinedAPs[0].Distance;
                triedAPCount++;
                circlesChanged = true;
            }

            if (AreCirclesEncapsulated(center3, radius3, center2, radius2))
            {
                center2 = new Point2D(combinedAPs[triedAPCount].Left, combinedAPs[triedAPCount].Top);
                radius2 = combinedAPs[0].Distance;
                triedAPCount++;
                circlesChanged = true;
            }

            if (AreCirclesEncapsulated(center1, radius1, center3, radius3))
            {
                center3 = new Point2D(combinedAPs[triedAPCount].Left, combinedAPs[triedAPCount].Top);
                radius3 = combinedAPs[0].Distance;
                triedAPCount++;
                circlesChanged = true;
            }
        }
        double factor = 1;
        double distance12 = center1.DistanceTo(center2);
        double distance23 = center2.DistanceTo(center3);
        double distance13 = center1.DistanceTo(center3);
        if (radius1 <=0)
        {
            radius1 = 0.1;
        }
        if (radius2 <= 0)
        {
            radius2 = 0.1;
        }
        if (radius3 <= 0)
        {
            radius3 = 0.1;
        }
        while ((distance12>radius1+radius2)||(distance13>radius2+radius3)||(distance23>radius2+radius3))
        {
            factor *= 1.1;
            radius1 *= factor;
            radius2 *= factor;
            radius3 *= factor;   
        }

        Point2D[] intersectionPoints12 = CalculateCircleIntersections(center1, radius1, center2, radius2,factor);
        Point2D[] intersectionPoints23 = CalculateCircleIntersections(center2, radius2, center3, radius3,factor);
        Point2D[] intersectionPoints13 = CalculateCircleIntersections(center1, radius1, center3, radius3,factor);

        Point2D triangleCenter = CalculateTriangleCenter(intersectionPoints12[0], intersectionPoints23[0], intersectionPoints13[1]);

        return triangleCenter.ToString();

    }

    private Point2D[] CalculateCircleIntersections(Point2D center1, double radius1, Point2D center2, double radius2,double factor)
    {
        double distance = center1.DistanceTo(center2);



        double a = (radius1 * radius1 - radius2 * radius2 + distance * distance) / (2 * distance);

        double h = radius1 * radius1 - (a * a);//makes a positive so not to return NAN
        if (h<0)
        {
            h = h * -1;
        }
        h = Math.Sqrt(h);
        Point2D p = center1 + a * (center2 - center1) / distance;

        double x3 = p.X + h * (center2.Y - center1.Y) / distance;
        double y3 = p.Y - h * (center2.X - center1.X) / distance;
        double x4 = p.X - h * (center2.Y - center1.Y) / distance;
        double y4 = p.Y + h * (center2.X - center1.X) / distance;

        return new Point2D[] { new Point2D(x3, y3), new Point2D(x4, y4) };
    }

    public bool AreCirclesEncapsulated(Point2D center1, double radius1, Point2D center2, double radius2)
    {
        double distanceBetweenCenters = Math.Sqrt(Math.Pow(center2.X - center1.X, 2) + Math.Pow(center2.Y - center1.Y, 2));
        bool isCircle1Encapsulated = distanceBetweenCenters + radius1 <= radius2;
        bool isCircle2Encapsulated = distanceBetweenCenters + radius2 <= radius1;
        return isCircle1Encapsulated || isCircle2Encapsulated;
    }
    private Point2D CalculateTriangleCenter(Point2D point1, Point2D point2, Point2D point3)
    {
        double centerX = (point1.X + point2.X + point3.X) / 3;
        double centerY = (point1.Y + point2.Y + point3.Y) / 3;

        return new Point2D(centerX, centerY);
    }
}