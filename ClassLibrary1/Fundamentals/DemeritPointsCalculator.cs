using System;

namespace ClassLibrary1.Fundamentals
{
    public class DemeritPointsCalculator
    {
        private const int SpeedLimit = 65;
        private const int KmPerDemeritPoint = 5;
        private const int MaxSpeed = 300;

        public int CalculateDemeritPoints(int speed)
        {
            if (speed < 0 || speed > MaxSpeed)
                throw new ArgumentOutOfRangeException();

            if (speed < SpeedLimit)
                return 0;

            return (speed - SpeedLimit) / KmPerDemeritPoint;
        }
    }
}