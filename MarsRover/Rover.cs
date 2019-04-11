using System.Collections.Generic;

namespace MarsRover
{
    public class Rover
    {
        private readonly IMotors _motors;

        public Rover(Direction direction, IMotors motors)
        {
            _motors = motors;
            Direction = direction;
        }

        public Direction Direction { get; private set; }

        public void TurnLeft()
        {
            if(_motors.RotateAntiClockwise()) Rotate(false);
        }

        public void TurnRight()
        {
            if(_motors.RotateClockwise()) Rotate(true);
        }

        private void Rotate(bool clockwise)
        {
            var offset = clockwise ? 1 : -1;
            var compass = new List<Direction> {Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.NORTH};

            var changedIndex = (compass.IndexOf(Direction) + offset) % 4;
            if (changedIndex < 0) changedIndex = 3;

            Direction = compass[changedIndex];
        }
    }
}