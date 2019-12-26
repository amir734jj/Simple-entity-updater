using System;
using AutoFixture;
using Core.Tests.Models;
using SimpleEntityUpdater;
using SimpleEntityUpdater.Interfaces;
using Xunit;

namespace Core.Tests
{
    public class EntityUpdaterTest
    {
        private readonly IFixture _fixture;

        private readonly ISimpleEntityUpdaterMapper _mapper;

        public EntityUpdaterTest()
        {
            _fixture = new Fixture();

            _mapper = EntityUpdater.New(x => x.Assembly(typeof(EntityUpdaterTest).Assembly));
        }
        
        [Fact]
        public void Test__Basic()
        {
            // Arrange
            var source = _fixture.Create<DummyClass>();
            var destination = _fixture.Create<DummyClass>();

            // Act
            _mapper.Map(source, destination);

            // Assert
            Console.WriteLine(destination);
        }
    }
}