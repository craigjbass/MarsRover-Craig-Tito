using FluentAssertions;
using NUnit.Framework;

namespace MarsRover
{
    public class RoverTests
    {
        private Rover _rover;
        private MotorSpy _motorSpy;

        class MotorSpy : IMotors
        {
            public bool RotateClockwiseCalled { get; private set; }
            public bool RotateAntiClockwiseCalled { get; private set; }
            public bool FailToTurnOnNextCommand { private get; set; }

            public MotorSpy()
            {
                FailToTurnOnNextCommand = false;
                RotateClockwiseCalled = false;
                RotateAntiClockwiseCalled = false;
            }

            public bool RotateClockwise()
            {
                RotateClockwiseCalled = true;
                return !FailToTurnOnNextCommand;
            }

            public bool RotateAntiClockwise()
            {
                RotateAntiClockwiseCalled = true;
                return !FailToTurnOnNextCommand;
            }    
        }

        private Direction AfterTurningLeftTheDirection()
        {
            _rover.TurnLeft();
            var roverDirection = _rover.Direction;
            return roverDirection;
        }

        private Direction AfterTurningRightTheDirection()
        {
            _rover.TurnRight();

            Direction roverDirection = _rover.Direction;
            return roverDirection;
        }

        [SetUp]
        public void Setup()
        {
            _motorSpy = new MotorSpy();
            _rover = new Rover(Direction.NORTH, _motorSpy);
        }

        [Test]
        public void ShouldHaveStartingDirection()
        {
            _rover = new Rover(Direction.WEST, _motorSpy);
            _rover.Direction.Should().Be(Direction.WEST);
        }

        [Test]
        public void ShouldTurnLeft()
        {
            AfterTurningLeftTheDirection().Should().Be(Direction.WEST);
            AfterTurningLeftTheDirection().Should().Be(Direction.SOUTH);
            AfterTurningLeftTheDirection().Should().Be(Direction.EAST);
            AfterTurningLeftTheDirection().Should().Be(Direction.NORTH);
        }

        [Test]
        public void ShouldTurnRight()
        {
            AfterTurningRightTheDirection().Should().Be(Direction.EAST);
            AfterTurningRightTheDirection().Should().Be(Direction.SOUTH);
            AfterTurningRightTheDirection().Should().Be(Direction.WEST);
            AfterTurningRightTheDirection().Should().Be(Direction.NORTH);
        }

        [Test]
        public void ShouldActivateMotorsWhenTurningLeft()
        {
            _rover.TurnLeft();
            _motorSpy.RotateAntiClockwiseCalled.Should().BeTrue();
        }

        [Test]
        public void ShouldActivateMotorsWhenTurningRight()
        {
            _rover.TurnRight();
            _motorSpy.RotateClockwiseCalled.Should().BeTrue();
        }

        [Test]
        public void ShouldNotUpdateDirectionAfterFailingToTurn()
        {
            _motorSpy.FailToTurnOnNextCommand = true;
            AfterTurningLeftTheDirection().Should().Be(Direction.NORTH);
            AfterTurningRightTheDirection().Should().Be(Direction.NORTH);
        }
    }
}