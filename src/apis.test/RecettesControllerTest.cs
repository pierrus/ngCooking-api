using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace apis
{
    public class RecettesControllerTest
    {
        private Mock<apis.Models.IRepository<apis.Models.Recette>> _repositoryMock;
        private Mock<apis.Models.IRepository<apis.Models.Ingredient>> _repositoryIngredientMock;
        private Mock<IHostingEnvironment> _hostingEnvironment;

        private apis.Controllers.RecettesController _recettesController;
        
        public RecettesControllerTest()
        {
            _repositoryMock = new Mock<apis.Models.IRepository<apis.Models.Recette>>();
            _repositoryIngredientMock = new Mock<apis.Models.IRepository<apis.Models.Ingredient>>();
            _hostingEnvironment = new Mock<IHostingEnvironment>();

            List<Models.Recette> recettes = new List<Models.Recette>();
            recettes.Add(new Models.Recette { Calories = 100, Id = "recette-1", IsAvailable = true, IngredientsRecettes = new List<Models.IngredientRecette>() });
            
            _repositoryMock.Setup(x => x.Get())
                .Returns(recettes.AsQueryable);

            _recettesController = new apis.Controllers.RecettesController(_repositoryMock.Object, _repositoryIngredientMock.Object, _hostingEnvironment.Object);
        }

        [Fact]
        public void SingleGet()
        {
            Task<dynamic> task = _recettesController.Get("recette-1");
            Assert.IsType<Models.Recette>(task.Result);

            Models.Recette recette = (Models.Recette)task.Result;
            Assert.Equal("recette-1", recette.Id);
        }
    }
}