﻿using ArquiteturaDesafio.Application.UseCases.Commands.User.CreateUser;
using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using Bogus.DataSets;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ArquiteturaDesafio.Tests.Application.Handlers
{
    public class CreateUserHandlerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _tokenService;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _tokenService = Substitute.For<IJwtTokenService>();
            _handler = new CreateUserHandler(_unitOfWork, _userRepository, _mapper, _tokenService);
        }

        [Fact]
        public async Task Handle_Should_CreateUser_When_RequestIsValid()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                Firstname = "Test", 
                Lastname = "User",
                Address = new Core.Application.UseCases.DTOs.AddressDto() { City = "City", Geolocation = new Core.Application.UseCases.DTOs.GeolocationDto() { Lat = "1.0", Long = "2.0" }, Number = 1, Street = "Street", Zipcode = "606060660" },
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };
            var _user = new User(
                "testuser",
                "test@example.com",
                "Password123", "Test", "User",
                new Core.Domain.ValueObjects.Address("City", "Street", 1, "606060660",
                new Core.Domain.ValueObjects.Geolocation("1.0", "2.0")), "11111",
                UserStatus.Active, UserRole.Admin, _tokenService);

            _mapper.Map<User>(request).Returns(_user);
            _userRepository.GetByLoginAsync(request.Username).Returns(Task.FromResult<User>(null));
            _userRepository.GetByEmailAsync(request.Email).Returns(Task.FromResult<User>(null));

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            await _unitOfWork.Received().Commit(CancellationToken.None);
            _userRepository.Received().Create(Arg.Any<User>());
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_UsernameAlreadyTaken()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                Firstname = "Test", 
                Lastname = "User",
                Address = new Core.Application.UseCases.DTOs.AddressDto() { City = "City", Geolocation = new Core.Application.UseCases.DTOs.GeolocationDto() { Lat = "1.0", Long = "2.0" }, Number = 1, Street = "Street", Zipcode = "606060660" },
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };
            var _user = new User(
                "testuser2",
                "test@example.com",
                "Password123", "Test", "User",
                new Core.Domain.ValueObjects.Address("City", "Street", 1, "606060660",
                new Core.Domain.ValueObjects.Geolocation("1.0", "2.0")), "11111",
                UserStatus.Active, UserRole.Admin, _tokenService);

            _mapper.Map<User>(request).Returns(_user);
            _userRepository.GetByLoginAsync(request.Username).Returns(Task.FromResult(_user));
            _userRepository.GetByEmailAsync(request.Email).Returns(Task.FromResult(_user));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_EmailAlreadyTaken()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                Firstname = "Test",
                Lastname = "User",
                Address = new Core.Application.UseCases.DTOs.AddressDto() { City = "City", Geolocation = new Core.Application.UseCases.DTOs.GeolocationDto() { Lat = "1.0", Long = "2.0" }, Number = 1, Street = "Street", Zipcode = "606060660" },
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };
            var _user = new User(
                "testuser",
                "test@example.com",
                "Password123", "Test", "User",
                new Core.Domain.ValueObjects.Address("City", "Street", 1, "606060660",
                new Core.Domain.ValueObjects.Geolocation("1.0", "2.0")), "11111",
                UserStatus.Active, UserRole.Admin, _tokenService);

            _mapper.Map<User>(request).Returns(_user);
            _userRepository.GetByLoginAsync(request.Username).Returns(Task.FromResult(_user));
            _userRepository.GetByEmailAsync(request.Email).Returns(Task.FromResult(_user));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}

