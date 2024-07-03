using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Protocol.Core.Types;
using ZombieParty.Controllers;
using ZombieParty.Models;
using ZombieParty.Models.Data;
using ZombieParty.Services;
using ZombieParty.Utility;
using ZombieParty.ViewModels;

namespace ZombieParty_Tests
{
    public class ZombieTypeController_Tests
    {
        private Mock<IZombieTypeService> _zombieTypeService_Mock;
        private Mock<IZombieService> _zombieService_Mock;
        private ZombieTypeController _zombieTypeController;

        public ZombieTypeController_Tests()
        {
            _zombieTypeService_Mock = new Mock<IZombieTypeService>();
            _zombieService_Mock = new Mock<IZombieService>();
            _zombieTypeController = new ZombieTypeController(_zombieTypeService_Mock.Object, _zombieService_Mock.Object);
        }

        [Fact]
        public async Task Index_VerifyGetAllInvoked()
        {
            // Arrange
            // Constructeur
           //var _zombieTypeController = new ZombieTypeController(_zombieTypeService_Mock.Object, _zombieService_Mock.Object);


            // Act
            var result = await _zombieTypeController.Index();

            // Assert
                // GetAllAsync appelé une seule fois
            _zombieTypeService_Mock.Verify(x => x.GetAllAsync(), Times.Once);
                // Le result est de type ViewResult
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task Create_ModelStateInvalid_ReturnView()
        {
            // Arrange
            // Constructeur          
            //_zombieTypeController = new ZombieTypeController(_zombieTypeService_Mock.Object, _zombieService_Mock.Object);

            // Act
            _zombieTypeController.ModelState.AddModelError("Error", "Error"); 
            var result = await _zombieTypeController.Create(new ZombieType());

            // Assert
                // La vue retournée est Create
            ViewResult viewResult = result as ViewResult;
            Assert.Equal("Create", viewResult.ViewName);

        }

        [Fact]
        public async Task Create_ModelStateValid_RedirectToView()
        {
            // Arrange
            // Constructeur
            var zombieType_One = new ZombieType()
            {
                Id = 11,
                TypeName = "Rampant",
                Point = 3,
                IsDisponible = true
            };
            _zombieTypeService_Mock.Setup(x => x.CreateAsync(It.IsAny<ZombieType>())).ReturnsAsync(new ZombieType());
            //_zombieTypeController = new ZombieTypeController(_zombieTypeService_Mock.Object, _zombieService_Mock.Object);


            // Act
            var result = await _zombieTypeController.Create(zombieType_One);

            // Assert
            _zombieTypeService_Mock.Verify(x => x.CreateAsync(It.IsAny<ZombieType>()), Times.Once());

            Assert.IsType<RedirectToActionResult>(result);
            // même vérification 
           // Assert.IsAssignableFrom<RedirectToActionResult>(result);
            
            var viewResult = result as RedirectToActionResult;
            Assert.Equal("Index", viewResult.ActionName);
            Assert.Null(viewResult.ControllerName);
            
        }

        [Fact]
        public async Task Edit_GetValidId_ReturnView()
        {
            // Arrange
            // Constructeur          
            //  _studyRoomBookingService.Setup(x => x.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
            _zombieTypeService_Mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new ZombieType()
            {
                Id = 11,
                TypeName = "Rampant",
                Point = 3,
                IsDisponible = true
            });
        

            // Act
            var result = await _zombieTypeController.Edit(11);

            // Assert
            // La vue retournée est Edit
            ViewResult viewResult = result as ViewResult;
            Assert.Equal("Edit", viewResult.ViewName);
            
            Assert.NotNull(viewResult.ViewData);
            Assert.IsType<ZombieType>(viewResult.Model);
            var model = viewResult.ViewData.Model as ZombieType;

            Assert.Equal(11, model.Id);
        }

        public async Task Delete_GetValidId_ReturnView()
        {

        }

        public async Task Delete_GetInvalidId_ReturnNotFound()
        {

        }

        public async Task Delete_Post_ReturnView()
        {

        }
    }
}